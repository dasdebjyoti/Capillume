using Microsoft.VisualBasic.FileIO;

namespace Capillume
{
    [Flags]
    public enum CleanupReason
    {
        None = 0,
        OlderThanMaxDays = 1,
        ExceedsFileCountLimit = 2
    }

    public sealed class ScreenshotCleanupPreview
    {
        public int FilesOlderThanMaxDays { get; init; }
        public int FilesExceedingCountLimit { get; init; }
        public int TotalCandidateFiles { get; init; }
        public long TotalBytesToFree { get; init; }
    }

    public sealed class ScreenshotCleanupResult
    {
        public ScreenshotCleanupPreview Preview { get; init; } = new();
        public bool DryRunMode { get; init; }
        public int DeletedFiles { get; init; }
        public int RecycledFiles { get; init; }
        public int BackedUpFiles { get; init; }
        public int FailedFiles { get; init; }
        public string? BackupFolderUsed { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
    }

    internal sealed class ScreenshotCleanupCandidate
    {
        public required FileInfo File { get; init; }
        public CleanupReason Reason { get; set; }
    }

    public sealed class ScreenshotRetentionService : IDisposable
    {
        private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".webp"
        };

        private readonly SemaphoreSlim _executionLock = new(1, 1);
        private readonly CancellationTokenSource _disposeCancellation = new();
        private int _pendingAutoCleanup;
        private int _autoCleanupWorkerActive;
        private bool _disposed;

        public Task<ScreenshotCleanupPreview> PreviewCleanupAsync(string saveFolder, RetentionSettings settings, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => BuildPreview(saveFolder, Clone(settings), cancellationToken), cancellationToken);
        }

        public async Task<ScreenshotCleanupResult> ExecuteCleanupAsync(
            string saveFolder,
            RetentionSettings settings,
            bool? dryRunOverride = null,
            CancellationToken cancellationToken = default)
        {
            using var linkedCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _disposeCancellation.Token);
            return await Task.Run(
                () => ExecuteCleanupCoreAsync(saveFolder, Clone(settings), dryRunOverride, linkedCancellation.Token),
                linkedCancellation.Token);
        }

        public void ScheduleAutoCleanup(string saveFolder, RetentionSettings settings, Action<string>? errorHandler = null)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (!settings.AutoCleanupEnabled)
            {
                return;
            }

            var settingsSnapshot = Clone(settings);
            Interlocked.Exchange(ref _pendingAutoCleanup, 1);

            if (Interlocked.CompareExchange(ref _autoCleanupWorkerActive, 1, 0) == 1)
            {
                return;
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    while (!_disposeCancellation.IsCancellationRequested)
                    {
                        if (Interlocked.Exchange(ref _pendingAutoCleanup, 0) != 1)
                        {
                            break;
                        }

                        await ExecuteCleanupCoreAsync(
                            saveFolder,
                            settingsSnapshot,
                            settingsSnapshot.DryRunMode,
                            _disposeCancellation.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                }
                catch (Exception ex)
                {
                    errorHandler?.Invoke($"Auto-cleanup failed: {ex.Message}");
                }
                finally
                {
                    Interlocked.Exchange(ref _autoCleanupWorkerActive, 0);
                    if (!_disposeCancellation.IsCancellationRequested
                        && Volatile.Read(ref _pendingAutoCleanup) == 1)
                    {
                        ScheduleAutoCleanup(saveFolder, settingsSnapshot, errorHandler);
                    }
                }
            }, _disposeCancellation.Token);
        }

        private async Task<ScreenshotCleanupResult> ExecuteCleanupCoreAsync(
            string saveFolder,
            RetentionSettings settings,
            bool? dryRunOverride,
            CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            await _executionLock.WaitAsync(cancellationToken);

            try
            {
                var candidateMap = BuildCandidateMap(saveFolder, settings, cancellationToken);
                var preview = BuildPreview(candidateMap);
                bool dryRunMode = dryRunOverride ?? settings.DryRunMode;

                if (dryRunMode || preview.TotalCandidateFiles == 0)
                {
                    return new ScreenshotCleanupResult
                    {
                        Preview = preview,
                        DryRunMode = dryRunMode
                    };
                }

                string? backupFolderUsed = null;
                if (settings.Action == RetentionAction.BackupThenDelete)
                {
                    if (string.IsNullOrWhiteSpace(settings.BackupFolder))
                    {
                        throw new InvalidOperationException("Select a backup folder before using the backup retention action.");
                    }

                    backupFolderUsed = settings.PerSessionSubfolder
                        ? CreateBackupSessionFolder(settings.BackupFolder)
                        : settings.BackupFolder;
                }

                int deletedFiles = 0;
                int recycledFiles = 0;
                int backedUpFiles = 0;
                int failedFiles = 0;
                List<string> errors = [];

                foreach (var candidate in candidateMap.Values.OrderBy(item => item.File.CreationTimeUtc).ThenBy(item => item.File.FullName, StringComparer.OrdinalIgnoreCase))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    try
                    {
                        switch (settings.Action)
                        {
                            case RetentionAction.MoveToRecycleBin:
                                FileSystem.DeleteFile(candidate.File.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                                recycledFiles++;
                                break;
                            case RetentionAction.DeletePermanently:
                                File.Delete(candidate.File.FullName);
                                deletedFiles++;
                                break;
                            case RetentionAction.BackupThenDelete:
                                string backupFilePath = GetBackupDestinationPath(saveFolder, backupFolderUsed!, candidate.File.FullName);
                                string? backupDirectory = Path.GetDirectoryName(backupFilePath);
                                if (!string.IsNullOrWhiteSpace(backupDirectory))
                                {
                                    Directory.CreateDirectory(backupDirectory);
                                }

                                File.Copy(candidate.File.FullName, backupFilePath, overwrite: false);
                                backedUpFiles++;
                                File.Delete(candidate.File.FullName);
                                deletedFiles++;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        failedFiles++;
                        errors.Add($"{Path.GetFileName(candidate.File.FullName)}: {ex.Message}");
                    }
                }

                return new ScreenshotCleanupResult
                {
                    Preview = preview,
                    DryRunMode = false,
                    DeletedFiles = deletedFiles,
                    RecycledFiles = recycledFiles,
                    BackedUpFiles = backedUpFiles,
                    FailedFiles = failedFiles,
                    BackupFolderUsed = backupFolderUsed,
                    Errors = errors.ToArray()
                };
            }
            finally
            {
                _executionLock.Release();
            }
        }

        private static ScreenshotCleanupPreview BuildPreview(string saveFolder, RetentionSettings settings, CancellationToken cancellationToken)
        {
            return BuildPreview(BuildCandidateMap(saveFolder, settings, cancellationToken));
        }

        private static ScreenshotCleanupPreview BuildPreview(IReadOnlyDictionary<string, ScreenshotCleanupCandidate> candidateMap)
        {
            int olderThanMaxDays = candidateMap.Values.Count(item => item.Reason.HasFlag(CleanupReason.OlderThanMaxDays));
            int exceedingCountLimit = candidateMap.Values.Count(item => item.Reason.HasFlag(CleanupReason.ExceedsFileCountLimit));
            long totalBytesToFree = candidateMap.Values.Sum(item => item.File.Length);

            return new ScreenshotCleanupPreview
            {
                FilesOlderThanMaxDays = olderThanMaxDays,
                FilesExceedingCountLimit = exceedingCountLimit,
                TotalCandidateFiles = candidateMap.Count,
                TotalBytesToFree = totalBytesToFree
            };
        }

        private static Dictionary<string, ScreenshotCleanupCandidate> BuildCandidateMap(
            string saveFolder,
            RetentionSettings settings,
            CancellationToken cancellationToken)
        {
            var candidateMap = new Dictionary<string, ScreenshotCleanupCandidate>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(saveFolder) || !Directory.Exists(saveFolder))
            {
                return candidateMap;
            }

            var files = EnumerateScreenshotFiles(saveFolder, settings)
                .Select(path => new FileInfo(path))
                .OrderByDescending(file => file.CreationTimeUtc)
                .ThenByDescending(file => file.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Sequential retention rules:
            // 1. Delete every file older than X days.
            // 2.From the files that remain, keep only the newest Y files.
            //
            // The second rule can retain files older than X days if they are among the newest Y files.
            //
            // Example
            // Assume:
            // Maximum age: 30 days
            // Maximum files: 500
            // 400 screenshots exist
            // 50 of them are older than 30 days
            //
            // Sequential rules:
            // 400 - 50 old files = 350 files retained
            // 350 files < 500 max files, so no additional files are deleted.
            //
            // Count - only rule:
            // All 400 files are retained because the total is below 500

            if (settings.MaxDaysEnabled)
            {
                DateTime cutoffUtc = DateTime.UtcNow.AddDays(-settings.MaxDaysToRetain);
                foreach (var file in files)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if (file.CreationTimeUtc >= cutoffUtc)
                    {
                        continue;
                    }

                    AddCandidate(candidateMap, file, CleanupReason.OlderThanMaxDays);
                }
            }

            if (settings.MaxFilesEnabled)
            {
                var remainingFiles = files
                    .Where(file => !candidateMap.ContainsKey(file.FullName))
                    .ToList();

                if (remainingFiles.Count <= settings.MaxFilesToRetain)
                {
                    return candidateMap;
                }

                foreach (var file in remainingFiles.Skip(settings.MaxFilesToRetain))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    AddCandidate(candidateMap, file, CleanupReason.ExceedsFileCountLimit);
                }
            }

            return candidateMap;
        }

        private static IEnumerable<string> EnumerateScreenshotFiles(string saveFolder, RetentionSettings settings)
        {
            System.IO.SearchOption searchOption = settings.IncludeSubfolders
                ? System.IO.SearchOption.AllDirectories
                : System.IO.SearchOption.TopDirectoryOnly;
            string? backupRoot = GetNormalizedPathOrNull(settings.Action == RetentionAction.BackupThenDelete ? settings.BackupFolder : null);

            foreach (var filePath in Directory.EnumerateFiles(saveFolder, "*.*", searchOption))
            {
                if (!SupportedExtensions.Contains(Path.GetExtension(filePath)))
                {
                    continue;
                }

                if (backupRoot != null)
                {
                    string normalizedFilePath = GetNormalizedPathOrNull(filePath) ?? filePath;
                    if (normalizedFilePath.StartsWith(backupRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                }

                yield return filePath;
            }
        }

        private static void AddCandidate(Dictionary<string, ScreenshotCleanupCandidate> candidateMap, FileInfo file, CleanupReason reason)
        {
            if (candidateMap.TryGetValue(file.FullName, out var existing))
            {
                existing.Reason |= reason;
                return;
            }

            candidateMap[file.FullName] = new ScreenshotCleanupCandidate
            {
                File = file,
                Reason = reason
            };
        }

        private static string CreateBackupSessionFolder(string backupFolder)
        {
            string sessionFolder = Path.Combine(backupFolder, $"Capillume Cleanup {DateTime.Now:yyyyMMdd_HHmmss}");
            Directory.CreateDirectory(sessionFolder);
            return sessionFolder;
        }

        private static string GetBackupDestinationPath(string saveFolder, string backupSessionFolder, string sourceFilePath)
        {
            string relativePath = Path.GetRelativePath(saveFolder, sourceFilePath);
            string targetPath = Path.Combine(backupSessionFolder, relativePath);
            if (!File.Exists(targetPath))
            {
                return targetPath;
            }

            string directory = Path.GetDirectoryName(targetPath) ?? backupSessionFolder;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(targetPath);
            string extension = Path.GetExtension(targetPath);

            for (int index = 1; index < 10000; index++)
            {
                string candidatePath = Path.Combine(directory, $"{fileNameWithoutExtension}_{index}{extension}");
                if (!File.Exists(candidatePath))
                {
                    return candidatePath;
                }
            }

            throw new IOException($"Unable to create a unique backup file name for '{Path.GetFileName(sourceFilePath)}'.");
        }

        private static string? GetNormalizedPathOrNull(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            return Path.GetFullPath(path)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                + Path.DirectorySeparatorChar;
        }

        public static string FormatSize(long bytes)
        {
            string[] units = ["B", "KB", "MB", "GB", "TB"];
            double size = bytes;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < units.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return $"{size:0.##} {units[unitIndex]}";
        }

        private static RetentionSettings Clone(RetentionSettings settings)
        {
            return new RetentionSettings
            {
                AutoCleanupEnabled = settings.AutoCleanupEnabled,
                MaxDaysEnabled = settings.MaxDaysEnabled,
                MaxDaysToRetain = settings.MaxDaysToRetain,
                MaxFilesEnabled = settings.MaxFilesEnabled,
                MaxFilesToRetain = settings.MaxFilesToRetain,
                Action = settings.Action,
                BackupFolder = settings.BackupFolder,
                DryRunMode = settings.DryRunMode,
                IncludeSubfolders = settings.IncludeSubfolders
            };
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposeCancellation.Cancel();
            _executionLock.Dispose();
            _disposeCancellation.Dispose();
        }
    }
}
