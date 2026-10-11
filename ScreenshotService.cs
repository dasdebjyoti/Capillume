using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace Capillume
{
    public class ScreenshotCapturedEventArgs : EventArgs
    {
        public string FilePath { get; }
        public long ElapsedMilliseconds { get; }

        public ScreenshotCapturedEventArgs(string filePath, long elapsedMs)
        {
            FilePath = filePath;
            ElapsedMilliseconds = elapsedMs;
        }
    }

    public class ScreenshotDedupeDiagnosticEventArgs : EventArgs
    {
        public ScreenshotDedupeResult Result { get; }

        public ScreenshotDedupeDiagnosticEventArgs(ScreenshotDedupeResult result)
        {
            Result = result;
        }
    }

    public class ScreenshotSkippedEventArgs : EventArgs
    {
        public string Reason { get; }
        public double PixelDifferencePercent { get; }

        public ScreenshotSkippedEventArgs(string reason, double pixelDifferencePercent)
        {
            Reason = reason;
            PixelDifferencePercent = pixelDifferencePercent;
        }
    }

    public class ScreenshotService : IDisposable
    {
        private System.Windows.Forms.Timer? _timer;
        private AppSettings _settings;
        private readonly IntPtr _capillumeWindowHandle;
        private readonly ScreenshotRetentionService _retentionService;
        private readonly ScreenshotDedupeService _dedupeService = new();
        private readonly SemaphoreSlim _dedupeGate = new(1, 1);
        private readonly Dictionary<string, DateTime> _lastSavedByBaseline = new(StringComparer.Ordinal);
        private bool _disposed;
        private bool _isPaused;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("dwmapi.dll")]
        private static extern int DwmGetWindowAttribute(
            IntPtr hWnd,
            int dwAttribute,
            out RECT pvAttribute,
            int cbAttribute);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const int SW_HIDE = 0;
        private const int SW_SHOWNOACTIVATE = 4;
        private const int DWMWA_EXTENDED_FRAME_BOUNDS = 9;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        //public event EventHandler<string>? ScreenshotCaptured;
        public event EventHandler<ScreenshotCapturedEventArgs>? ScreenshotCaptured;
        public event EventHandler<ScreenshotSkippedEventArgs>? ScreenshotSkipped;
        public event EventHandler<ScreenshotDedupeDiagnosticEventArgs>? DedupeDiagnostic;
        public event EventHandler<string>? ErrorOccurred;

        public ScreenshotService(AppSettings settings, IntPtr capillumeWindowHandle)
        {
            _settings = settings;
            _capillumeWindowHandle = capillumeWindowHandle;
            _retentionService = new ScreenshotRetentionService();
        }

        public void Start()
        {
            if (_timer != null)
            {
                Stop();
            }

            _isPaused = false;

            _timer = new System.Windows.Forms.Timer
            {
                Interval = _settings.IntervalMinutes * 60 * 1000
            };
            _timer.Tick += OnTimerTick;
            _timer.Start();

            // Take first screenshot immediately if enabled
            if (_settings.IsScreenshotEnabled)
            {
                CaptureScreenshot();
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= OnTimerTick;
                _timer.Dispose();
                _timer = null;
            }

            _isPaused = false;
        }

        public void Pause()
        {
            if (_timer == null || _isPaused)
            {
                return;
            }

            _isPaused = true;
            _timer.Stop();
        }

        public void Resume()
        {
            if (_timer == null || !_isPaused)
            {
                return;
            }

            _isPaused = false;
            _timer.Start();

            if (_settings.IsScreenshotEnabled)
            {
                CaptureScreenshot();
            }
        }

        public void UpdateSettings(AppSettings settings)
        {
            _settings = settings;
            _dedupeService.Clear();
            if (_timer != null) _timer.Interval = _settings.IntervalMinutes * 60 * 1000;
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (!_isPaused && _settings.IsScreenshotEnabled) CaptureScreenshot();
        }

        public void CaptureScreenshot(bool captureWindowBelowCapillume = false)
        {
            if (_isPaused)
            {
                return;
            }

            try
            {
                var sw = System.Diagnostics.Stopwatch.StartNew();

                string saveFolder = _settings.SaveFolder;
                string imageFormat = _settings.ImageFormat;
                int imageQuality = _settings.ImageQuality;

                Bitmap? screenshot;
                Rectangle comparisonBounds;
                string baselineKey;
                if (_settings.CaptureFullScreen)
                {
                    screenshot = CaptureFullScreen();
                    comparisonBounds = new Rectangle(Point.Empty, screenshot.Size);
                    baselineKey = $"fullscreen:{comparisonBounds.Size}";
                }
                else
                {
                    screenshot = CaptureActiveWindow(captureWindowBelowCapillume, out comparisonBounds, out IntPtr windowHandle);
                    baselineKey = _settings.Dedupe.ComparePerActiveWindow
                        ? $"window:{windowHandle.ToInt64()}:{comparisonBounds.Size}"
                        : "active-window";
                }

                if (screenshot == null)
                {
                    ErrorOccurred?.Invoke(this, "Failed to capture screenshot.");
                    return;
                }

                _ = ProcessCapturedScreenshotAsync(
                    screenshot,
                    _settings.CaptureFullScreen,
                    comparisonBounds,
                    baselineKey,
                    saveFolder,
                    imageFormat,
                    imageQuality,
                    sw);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error capturing screenshot: {ex.Message}");
            }
        }

        private async Task ProcessCapturedScreenshotAsync(
            Bitmap screenshot,
            bool isFullScreenCapture,
            Rectangle comparisonBounds,
            string baselineKey,
            string saveFolder,
            string imageFormat,
            int imageQuality,
            System.Diagnostics.Stopwatch stopwatch)
        {
            try
            {
                DedupeSettings dedupe = _settings.Dedupe;
                ScreenshotDedupeResult? dedupeResult = null;
                bool gateEntered = false;
                if (dedupe.Enabled)
                {
                    using var timeout = new CancellationTokenSource(dedupe.TimeoutMilliseconds);
                    try
                    {
                        gateEntered = await _dedupeGate.WaitAsync(dedupe.TimeoutMilliseconds, timeout.Token).ConfigureAwait(false);
                        if (gateEntered)
                        {
                            dedupeResult = await Task.Run(() => _dedupeService.Compare(
                                new ScreenshotDedupeRequest
                                {
                                    Bitmap = screenshot,
                                    BaselineKey = baselineKey,
                                    ComparisonBounds = comparisonBounds,
                                    ExcludedRegions = GetExcludedRegions(isFullScreenCapture, dedupe)
                                }, dedupe, timeout.Token), timeout.Token).ConfigureAwait(false);

                            if (dedupe.EnableDiagnostics)
                            {
                                DedupeDiagnostic?.Invoke(this, new ScreenshotDedupeDiagnosticEventArgs(dedupeResult));
                            }

                            bool forceSave = dedupe.MinimumMinutesBetweenSaves > 0 &&
                                (!_lastSavedByBaseline.TryGetValue(baselineKey, out DateTime lastSaved) ||
                                 DateTime.UtcNow - lastSaved >= TimeSpan.FromMinutes(dedupe.MinimumMinutesBetweenSaves));
                            if (dedupeResult.Decision == ScreenshotDedupeDecision.SkipDuplicate && !dedupe.DryRun && !forceSave)
                            {
                                if (!dedupe.UpdateReferenceOnlyOnSave)
                                {
                                    _dedupeService.Commit(dedupeResult);
                                }
                                ScreenshotSkipped?.Invoke(this, new ScreenshotSkippedEventArgs(dedupeResult.Reason, dedupeResult.PixelDifferencePercent));
                                return;
                            }

                            _dedupeService.Commit(dedupeResult);
                            _lastSavedByBaseline[baselineKey] = DateTime.UtcNow;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // A dedupe timeout fails open so the capture is not lost.
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke(this, $"Dedupe comparison failed; saving screenshot: {ex.Message}");
                    }
                    finally
                    {
                        if (gateEntered)
                        {
                            _dedupeGate.Release();
                        }
                    }
                }

                WatermarkRenderer.Apply(screenshot, _settings.Watermark, _settings.Annotation);
                string filePath = Path.Combine(saveFolder, GenerateFileName());
                Bitmap? processedScreenshot = null;
                Bitmap? imageProcessedScreenshot = null;
                try
                {
                    processedScreenshot = CreateDownscaledBitmapIfNeeded(screenshot, isFullScreenCapture);
                    imageProcessedScreenshot = ApplyImageProcessingIfNeeded(processedScreenshot ?? screenshot);
                    Bitmap bitmapToSave = imageProcessedScreenshot ?? processedScreenshot ?? screenshot;
                    if (imageProcessedScreenshot != null)
                    {
                        processedScreenshot?.Dispose();
                        screenshot.Dispose();
                    }
                    else if (processedScreenshot != null)
                    {
                        screenshot.Dispose();
                    }

                    imageProcessedScreenshot = null;
                    processedScreenshot = null;
                    screenshot = null!;
                    QueueSaveScreenshot(bitmapToSave, filePath, saveFolder, imageFormat, imageQuality, stopwatch);
                }
                finally
                {
                    imageProcessedScreenshot?.Dispose();
                    processedScreenshot?.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error processing screenshot: {ex.Message}");
            }
            finally
            {
                screenshot?.Dispose();
            }
        }

        private void QueueSaveScreenshot(
            Bitmap bitmap,
            string filePath,
            string saveFolder,
            string imageFormat,
            int imageQuality,
            System.Diagnostics.Stopwatch stopwatch)
        {
            try
            {
                _ = Task.Run(() =>
                {
                    try
                    {
                        Directory.CreateDirectory(saveFolder);
                        SaveScreenshot(bitmap, filePath, imageFormat, imageQuality);

                        stopwatch.Stop();
                        ScreenshotCaptured?.Invoke(
                            this,
                            new ScreenshotCapturedEventArgs(filePath, stopwatch.ElapsedMilliseconds));
                        _retentionService.ScheduleAutoCleanup(
                            saveFolder,
                            _settings.Retention,
                            error => ErrorOccurred?.Invoke(this, error));
                    }
                    catch (Exception ex)
                    {
                        ErrorOccurred?.Invoke(this, $"Error saving screenshot: {ex.Message}");
                    }
                    finally
                    {
                        bitmap.Dispose();
                    }
                });
            }
            catch
            {
                bitmap.Dispose();
                throw;
            }
        }

        private Bitmap CaptureFullScreen()
        {
            var bounds = Screen.PrimaryScreen?.Bounds ?? Screen.AllScreens[0].Bounds;

            // For multi-monitor, capture all screens
            int minX = Screen.AllScreens.Min(s => s.Bounds.X);
            int minY = Screen.AllScreens.Min(s => s.Bounds.Y);
            int maxX = Screen.AllScreens.Max(s => s.Bounds.Right);
            int maxY = Screen.AllScreens.Max(s => s.Bounds.Bottom);

            int width = maxX - minX;
            int height = maxY - minY;

            List<IntPtr> hiddenWindows = _settings.ExcludeCapillume
                ? HideCapillumeWindows()
                : new List<IntPtr>();

            try
            {
                var bitmap = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(minX, minY, 0, 0, new System.Drawing.Size(width, height));
                }

                return bitmap;
            }
            finally
            {
                RestoreWindows(hiddenWindows);
            }
        }

        private IReadOnlyCollection<Rectangle> GetExcludedRegions(bool isFullScreenCapture, DedupeSettings settings)
        {
            if (!isFullScreenCapture || settings.DynamicRegionMode == DedupeDynamicRegionMode.IncludeAll)
            {
                return Array.Empty<Rectangle>();
            }

            int minX = Screen.AllScreens.Min(screen => screen.Bounds.Left);
            int minY = Screen.AllScreens.Min(screen => screen.Bounds.Top);
            var regions = new List<Rectangle>();

            if (settings.DynamicRegionMode is DedupeDynamicRegionMode.ExcludeTaskbars or DedupeDynamicRegionMode.ExcludeTaskbarsAndManualRegions)
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    Rectangle bounds = screen.Bounds;
                    Rectangle working = screen.WorkingArea;
                    Rectangle taskbar = GetTaskbarRegion(bounds, working);
                    if (!taskbar.IsEmpty)
                    {
                        taskbar.Offset(-minX, -minY);
                        regions.Add(taskbar);
                    }
                }
            }

            if (settings.DynamicRegionMode is DedupeDynamicRegionMode.ExcludeTaskbarsAndManualRegions or DedupeDynamicRegionMode.ManualRegionsOnly)
            {
                foreach (Rectangle region in settings.ExcludeRegions)
                {
                    Rectangle translated = region;
                    translated.Offset(-minX, -minY);
                    regions.Add(translated);
                }
            }

            return regions;
        }

        private static Rectangle GetTaskbarRegion(Rectangle bounds, Rectangle workingArea)
        {
            if (workingArea.Top > bounds.Top)
            {
                return Rectangle.FromLTRB(bounds.Left, bounds.Top, bounds.Right, workingArea.Top);
            }

            if (workingArea.Bottom < bounds.Bottom)
            {
                return Rectangle.FromLTRB(bounds.Left, workingArea.Bottom, bounds.Right, bounds.Bottom);
            }

            if (workingArea.Left > bounds.Left)
            {
                return Rectangle.FromLTRB(bounds.Left, bounds.Top, workingArea.Left, bounds.Bottom);
            }

            return workingArea.Right < bounds.Right
                ? Rectangle.FromLTRB(workingArea.Right, bounds.Top, bounds.Right, bounds.Bottom)
                : Rectangle.Empty;
        }

        private Bitmap? CaptureActiveWindow(bool captureWindowBelowCapillume, out Rectangle comparisonBounds, out IntPtr capturedWindowHandle)
        {
            comparisonBounds = Rectangle.Empty;
            IntPtr handle = GetForegroundWindow();
            capturedWindowHandle = handle;
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            bool hideCapillume = false;
            if (_settings.ExcludeCapillume && captureWindowBelowCapillume)
            {
                handle = FindWindowBelow(_capillumeWindowHandle);
                hideCapillume = true;
            }
            else if (_settings.ExcludeCapillume && IsCapillumeWindow(handle))
            {
                handle = FindWindowBelow(handle);
                hideCapillume = true;
                if (handle == IntPtr.Zero)
                {
                    return null;
                }
            }

            if (!TryGetVisibleWindowRect(handle, out RECT rect) &&
                !GetWindowRect(handle, out rect))
            {
                return null;
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            List<IntPtr> hiddenWindows = hideCapillume
                ? HideCapillumeWindows()
                : new List<IntPtr>();

            try
            {
                var bitmap = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new System.Drawing.Size(width, height));
                }

                comparisonBounds = GetClientComparisonBounds(handle, rect, bitmap.Size);
                capturedWindowHandle = handle;

                return bitmap;
            }
            finally
            {
                RestoreWindows(hiddenWindows);
                if (hideCapillume && _capillumeWindowHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(_capillumeWindowHandle);
                }
            }
        }

        private static Rectangle GetClientComparisonBounds(IntPtr handle, RECT windowRect, Size bitmapSize)
        {
            if (!GetClientRect(handle, out RECT clientRect))
            {
                return new Rectangle(Point.Empty, bitmapSize);
            }

            var clientOrigin = new POINT { X = clientRect.Left, Y = clientRect.Top };
            if (!ClientToScreen(handle, ref clientOrigin))
            {
                return new Rectangle(Point.Empty, bitmapSize);
            }

            Rectangle result = new(
                clientOrigin.X - windowRect.Left,
                clientOrigin.Y - windowRect.Top,
                clientRect.Right - clientRect.Left,
                clientRect.Bottom - clientRect.Top);
            result.Intersect(new Rectangle(Point.Empty, bitmapSize));
            return result.IsEmpty ? new Rectangle(Point.Empty, bitmapSize) : result;
        }

        private static bool TryGetVisibleWindowRect(IntPtr handle, out RECT rect)
        {
            return DwmGetWindowAttribute(
                handle,
                DWMWA_EXTENDED_FRAME_BOUNDS,
                out rect,
                Marshal.SizeOf<RECT>()) == 0;
        }

        private IntPtr FindWindowBelow(IntPtr window)
        {
            bool foundWindow = false;
            IntPtr windowBelow = IntPtr.Zero;

            EnumWindows((handle, _) =>
            {
                if (!foundWindow)
                {
                    if (handle == window)
                    {
                        foundWindow = true;
                    }

                    return true;
                }

                if (IsWindowVisible(handle) && !IsIconic(handle) && !IsCapillumeWindow(handle))
                {
                    windowBelow = handle;
                    return false;
                }

                return true;
            }, IntPtr.Zero);

            return windowBelow;
        }

        private bool IsCapillumeWindow(IntPtr handle)
        {
            if (handle == _capillumeWindowHandle)
            {
                return true;
            }

            GetWindowThreadProcessId(handle, out uint processId);
            return processId == (uint)Environment.ProcessId;
        }

        private static List<IntPtr> HideCapillumeWindows()
        {
            var windows = new List<IntPtr>();
            uint processId = (uint)Environment.ProcessId;

            EnumWindows((handle, _) =>
            {
                GetWindowThreadProcessId(handle, out uint windowProcessId);
                if (windowProcessId == processId && IsWindowVisible(handle) && !IsIconic(handle))
                {
                    if (ShowWindow(handle, SW_HIDE))
                    {
                        windows.Add(handle);
                    }
                }

                return true;
            }, IntPtr.Zero);

            return windows;
        }

        private static void RestoreWindows(IEnumerable<IntPtr> windows)
        {
            foreach (IntPtr window in windows)
            {
                ShowWindow(window, SW_SHOWNOACTIVATE);
            }
        }

        private Bitmap? CreateDownscaledBitmapIfNeeded(Bitmap bitmap, bool isFullScreenCapture)
        {
            DownscaleSettings settings = _settings.Downscale;
            if (!settings.Enabled)
            {
                return null;
            }

            if (settings.FullScreenOnly && !isFullScreenCapture)
            {
                return null;
            }

            if (settings.LossyFormatsOnly && !IsLossyImageFormat(_settings.ImageFormat))
            {
                return null;
            }

            Size targetSize = CalculateTargetSize(bitmap.Size, settings);
            if (targetSize.Width <= 0 || targetSize.Height <= 0)
            {
                return null;
            }

            bool wouldUpscale = targetSize.Width > bitmap.Width || targetSize.Height > bitmap.Height;
            if (settings.SkipSmallerImages && wouldUpscale)
            {
                return null;
            }

            if (targetSize.Width == bitmap.Width && targetSize.Height == bitmap.Height)
            {
                return null;
            }

            Bitmap resized = ResizeBitmap(bitmap, targetSize, settings.Quality);
            if (settings.SharpenAfterResize && (targetSize.Width < bitmap.Width || targetSize.Height < bitmap.Height))
            {
                Bitmap sharpened = ApplySlightSharpen(resized);
                resized.Dispose();
                return sharpened;
            }

            return resized;
        }

        private static Size CalculateTargetSize(Size sourceSize, DownscaleSettings settings)
        {
            double scale = settings.ResizeMode switch
            {
                DownscaleResizeMode.TargetHeight => settings.ResizeTargetHeight / (double)sourceSize.Height,
                DownscaleResizeMode.Percentage => settings.ResizePercentage / 100.0,
                DownscaleResizeMode.MaxWidth => settings.ResizeMaxWidth / (double)sourceSize.Width,
                DownscaleResizeMode.BoundingBox => Math.Min(
                    settings.ResizeBoundingBoxWidth / (double)sourceSize.Width,
                    settings.ResizeBoundingBoxHeight / (double)sourceSize.Height),
                _ => 1.0
            };

            int width = Math.Max(1, (int)Math.Round(sourceSize.Width * scale));
            int height = Math.Max(1, (int)Math.Round(sourceSize.Height * scale));
            return new Size(width, height);
        }

        private static Bitmap ResizeBitmap(Bitmap source, Size targetSize, DownscaleQuality quality)
        {
            var resized = new Bitmap(targetSize.Width, targetSize.Height, PixelFormat.Format32bppArgb);
            resized.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using var graphics = Graphics.FromImage(resized);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = quality == DownscaleQuality.Fast
                ? CompositingQuality.HighSpeed
                : CompositingQuality.HighQuality;
            graphics.SmoothingMode = quality == DownscaleQuality.Fast
                ? SmoothingMode.None
                : SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = quality == DownscaleQuality.Fast
                ? PixelOffsetMode.Half
                : PixelOffsetMode.HighQuality;
            graphics.InterpolationMode = quality switch
            {
                DownscaleQuality.HighQuality => InterpolationMode.HighQualityBicubic,
                DownscaleQuality.Balanced => InterpolationMode.HighQualityBilinear,
                _ => InterpolationMode.NearestNeighbor
            };

            using var attributes = new ImageAttributes();
            attributes.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(
                source,
                new Rectangle(Point.Empty, targetSize),
                0,
                0,
                source.Width,
                source.Height,
                GraphicsUnit.Pixel,
                attributes);

            return resized;
        }

        private static Bitmap ApplySlightSharpen(Bitmap source)
        {
            Rectangle rect = new(0, 0, source.Width, source.Height);
            using var workingCopy = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(workingCopy))
            {
                graphics.DrawImageUnscaled(source, 0, 0);
            }

            var sharpened = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            var kernel = new double[]
            {
                0, -0.5, 0,
                -0.5, 3, -0.5,
                0, -0.5, 0
            };

            BitmapData sourceData = workingCopy.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData destinationData = sharpened.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            try
            {
                int stride = sourceData.Stride;
                int bytes = Math.Abs(stride) * source.Height;
                byte[] sourceBuffer = new byte[bytes];
                byte[] destinationBuffer = new byte[bytes];
                Marshal.Copy(sourceData.Scan0, sourceBuffer, 0, bytes);
                Buffer.BlockCopy(sourceBuffer, 0, destinationBuffer, 0, bytes);

                for (int y = 1; y < source.Height - 1; y++)
                {
                    for (int x = 1; x < source.Width - 1; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        for (int channel = 0; channel < 3; channel++)
                        {
                            double value = 0;
                            int kernelIndex = 0;
                            for (int ky = -1; ky <= 1; ky++)
                            {
                                for (int kx = -1; kx <= 1; kx++)
                                {
                                    int sampleIndex = (y + ky) * stride + (x + kx) * 4 + channel;
                                    value += sourceBuffer[sampleIndex] * kernel[kernelIndex++];
                                }
                            }

                            destinationBuffer[pixelIndex + channel] = (byte)Math.Clamp((int)Math.Round(value), 0, 255);
                        }

                        destinationBuffer[pixelIndex + 3] = sourceBuffer[pixelIndex + 3];
                    }
                }

                Marshal.Copy(destinationBuffer, 0, destinationData.Scan0, bytes);
            }
            finally
            {
                workingCopy.UnlockBits(sourceData);
                sharpened.UnlockBits(destinationData);
            }

            return sharpened;
        }

        private Bitmap? ApplyImageProcessingIfNeeded(Bitmap source)
        {
            ImageProcessingSettings settings = _settings.ImageProcessing ?? new ImageProcessingSettings();
            bool hasTransform = settings.ColorMode != ImageProcessColorMode.FullColor
                || settings.HighContrast
                || settings.NoiseReduction
                || settings.ColorTemperature != ImageProcessColorTempMode.Neutral;

            if (!hasTransform)
            {
                return null;
            }

            Bitmap processed = CreateArgbCopy(source);

            if (settings.NoiseReduction)
            {
                ApplyBoxBlur3x3InPlace(processed);
            }

            if (settings.ColorMode == ImageProcessColorMode.AdaptivePalette)
            {
                if (settings.HighContrast)
                {
                    ApplyHighContrastInPlace(processed);
                }

                if (settings.ColorTemperature != ImageProcessColorTempMode.Neutral)
                {
                    ApplyColorTemperatureInPlace(processed, settings.ColorTemperature);
                }

                ApplyAdaptivePaletteQuantizationInPlace(processed, 256);
            }
            else if (settings.HighContrast
                || settings.ColorTemperature != ImageProcessColorTempMode.Neutral
                || settings.ColorMode != ImageProcessColorMode.FullColor)
            {
                ApplyPixelTransformsInPlace(processed, settings);
            }

            return processed;
        }

        private static Bitmap CreateArgbCopy(Bitmap source)
        {
            var copy = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
            copy.SetResolution(source.HorizontalResolution, source.VerticalResolution);
            using var graphics = Graphics.FromImage(copy);
            graphics.DrawImageUnscaled(source, 0, 0);
            return copy;
        }

        private static void ApplyColorModeInPlace(Bitmap bitmap, ImageProcessColorMode mode)
        {
            switch (mode)
            {
                case ImageProcessColorMode.Grayscale:
                    ApplyGrayscaleInPlace(bitmap);
                    break;
                case ImageProcessColorMode.Monochrome1Bit:
                    ApplyMonochromeInPlace(bitmap);
                    break;
                case ImageProcessColorMode.Color16:
                    ApplyFixedPaletteQuantizationInPlace(bitmap, 2, 2, 4);
                    break;
                case ImageProcessColorMode.Color256:
                    ApplyFixedPaletteQuantizationInPlace(bitmap, 8, 8, 4);
                    break;
                case ImageProcessColorMode.AdaptivePalette:
                    ApplyAdaptivePaletteQuantizationInPlace(bitmap, 256);
                    break;
                default:
                    break;
            }
        }

        private static void ApplyPixelTransformsInPlace(Bitmap bitmap, ImageProcessingSettings settings)
        {
            (double redMultiplier, double greenMultiplier, double blueMultiplier) = settings.ColorTemperature switch
            {
                ImageProcessColorTempMode.Warm => (1.08, 1.0, 0.92),
                ImageProcessColorTempMode.Cool => (0.92, 1.0, 1.08),
                _ => (1.0, 1.0, 1.0)
            };

            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;

                        if (settings.HighContrast)
                        {
                            buffer[pixelIndex + 2] = AdjustContrast(buffer[pixelIndex + 2], 1.25, 128.0);
                            buffer[pixelIndex + 1] = AdjustContrast(buffer[pixelIndex + 1], 1.25, 128.0);
                            buffer[pixelIndex] = AdjustContrast(buffer[pixelIndex], 1.25, 128.0);
                        }

                        if (settings.ColorTemperature != ImageProcessColorTempMode.Neutral)
                        {
                            buffer[pixelIndex + 2] = (byte)Math.Clamp(
                                (int)Math.Round(buffer[pixelIndex + 2] * redMultiplier), 0, 255);
                            buffer[pixelIndex + 1] = (byte)Math.Clamp(
                                (int)Math.Round(buffer[pixelIndex + 1] * greenMultiplier), 0, 255);
                            buffer[pixelIndex] = (byte)Math.Clamp(
                                (int)Math.Round(buffer[pixelIndex] * blueMultiplier), 0, 255);
                        }

                        switch (settings.ColorMode)
                        {
                            case ImageProcessColorMode.Grayscale:
                            {
                                byte gray = (byte)((299 * buffer[pixelIndex + 2]
                                    + 587 * buffer[pixelIndex + 1]
                                    + 114 * buffer[pixelIndex] + 500) / 1000);
                                buffer[pixelIndex] = gray;
                                buffer[pixelIndex + 1] = gray;
                                buffer[pixelIndex + 2] = gray;
                                break;
                            }
                            case ImageProcessColorMode.Monochrome1Bit:
                            {
                                byte gray = (byte)((299 * buffer[pixelIndex + 2]
                                    + 587 * buffer[pixelIndex + 1]
                                    + 114 * buffer[pixelIndex] + 500) / 1000);
                                byte monochrome = gray >= 128 ? (byte)255 : (byte)0;
                                buffer[pixelIndex] = monochrome;
                                buffer[pixelIndex + 1] = monochrome;
                                buffer[pixelIndex + 2] = monochrome;
                                break;
                            }
                            case ImageProcessColorMode.Color16:
                                buffer[pixelIndex + 2] = QuantizeChannel(buffer[pixelIndex + 2], 2);
                                buffer[pixelIndex + 1] = QuantizeChannel(buffer[pixelIndex + 1], 2);
                                buffer[pixelIndex] = QuantizeChannel(buffer[pixelIndex], 4);
                                break;
                            case ImageProcessColorMode.Color256:
                                buffer[pixelIndex + 2] = QuantizeChannel(buffer[pixelIndex + 2], 8);
                                buffer[pixelIndex + 1] = QuantizeChannel(buffer[pixelIndex + 1], 8);
                                buffer[pixelIndex] = QuantizeChannel(buffer[pixelIndex], 4);
                                break;
                        }
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void ApplyGrayscaleInPlace(Bitmap bitmap)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        byte b = buffer[pixelIndex];
                        byte g = buffer[pixelIndex + 1];
                        byte r = buffer[pixelIndex + 2];
                        //byte gray = (byte)Math.Clamp((int)Math.Round(0.299 * r + 0.587 * g + 0.114 * b), 0, 255);
                        // Code optimmization: for grayscale conversion using integer math to avoid floating-point operations
                        byte gray = (byte)((299 * r + 587 * g + 114 * b + 500) / 1000);

                        buffer[pixelIndex] = gray;
                        buffer[pixelIndex + 1] = gray;
                        buffer[pixelIndex + 2] = gray;
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void ApplyMonochromeInPlace(Bitmap bitmap)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        byte b = buffer[pixelIndex];
                        byte g = buffer[pixelIndex + 1];
                        byte r = buffer[pixelIndex + 2];
                        //byte gray = (byte)Math.Clamp((int)Math.Round(0.299 * r + 0.587 * g + 0.114 * b), 0, 255);
                        // Code optimmization: for grayscale conversion using integer math to avoid floating-point operations
                        byte gray = (byte)((299 * r + 587 * g + 114 * b + 500) / 1000);
                        byte mono = gray >= 128 ? (byte)255 : (byte)0;

                        buffer[pixelIndex] = mono;
                        buffer[pixelIndex + 1] = mono;
                        buffer[pixelIndex + 2] = mono;
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void ApplyFixedPaletteQuantizationInPlace(Bitmap bitmap, int redLevels, int greenLevels, int blueLevels)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        buffer[pixelIndex + 2] = QuantizeChannel(buffer[pixelIndex + 2], redLevels);
                        buffer[pixelIndex + 1] = QuantizeChannel(buffer[pixelIndex + 1], greenLevels);
                        buffer[pixelIndex] = QuantizeChannel(buffer[pixelIndex], blueLevels);
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static byte QuantizeChannel(byte value, int levels)
        {
            if (levels <= 1)
            {
                return 0;
            }

            int bucket = (int)Math.Round(value * (levels - 1) / 255.0);
            int quantized = (int)Math.Round(bucket * 255.0 / (levels - 1));
            return (byte)Math.Clamp(quantized, 0, 255);
        }

        private static void ApplyAdaptivePaletteQuantizationInPlace(Bitmap bitmap, int maxColors)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                int[] counts = new int[32768];
                int[] redSums = new int[32768];
                int[] greenSums = new int[32768];
                int[] blueSums = new int[32768];

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        byte b = buffer[pixelIndex];
                        byte g = buffer[pixelIndex + 1];
                        byte r = buffer[pixelIndex + 2];
                        int key = ((r >> 3) << 10) | ((g >> 3) << 5) | (b >> 3);

                        counts[key]++;
                        redSums[key] += r;
                        greenSums[key] += g;
                        blueSums[key] += b;
                    }
                }

                int[] selectedKeys = Enumerable.Range(0, counts.Length)
                    .Where(i => counts[i] > 0)
                    .OrderByDescending(i => counts[i])
                    .Take(Math.Clamp(maxColors, 2, 256))
                    .ToArray();

                if (selectedKeys.Length == 0)
                {
                    return;
                }

                var palette = new (byte R, byte G, byte B)[selectedKeys.Length];
                for (int i = 0; i < selectedKeys.Length; i++)
                {
                    int key = selectedKeys[i];
                    int count = counts[key];
                    palette[i] = (
                        (byte)(redSums[key] / count),
                        (byte)(greenSums[key] / count),
                        (byte)(blueSums[key] / count));
                }

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        byte b = buffer[pixelIndex];
                        byte g = buffer[pixelIndex + 1];
                        byte r = buffer[pixelIndex + 2];

                        int bestIndex = 0;
                        int bestDistance = int.MaxValue;

                        for (int i = 0; i < palette.Length; i++)
                        {
                            int dr = r - palette[i].R;
                            int dg = g - palette[i].G;
                            int db = b - palette[i].B;
                            int distance = dr * dr + dg * dg + db * db;
                            if (distance < bestDistance)
                            {
                                bestDistance = distance;
                                bestIndex = i;
                            }
                        }

                        buffer[pixelIndex + 2] = palette[bestIndex].R;
                        buffer[pixelIndex + 1] = palette[bestIndex].G;
                        buffer[pixelIndex] = palette[bestIndex].B;
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void ApplyHighContrastInPlace(Bitmap bitmap)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                const double contrast = 1.25;
                const double midpoint = 128.0;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        buffer[pixelIndex + 2] = AdjustContrast(buffer[pixelIndex + 2], contrast, midpoint);
                        buffer[pixelIndex + 1] = AdjustContrast(buffer[pixelIndex + 1], contrast, midpoint);
                        buffer[pixelIndex] = AdjustContrast(buffer[pixelIndex], contrast, midpoint);
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static byte AdjustContrast(byte channel, double contrast, double midpoint)
        {
            int value = (int)Math.Round((channel - midpoint) * contrast + midpoint);
            return (byte)Math.Clamp(value, 0, 255);
        }

        private static void ApplyColorTemperatureInPlace(Bitmap bitmap, ImageProcessColorTempMode mode)
        {
            (double redMultiplier, double greenMultiplier, double blueMultiplier) = mode switch
            {
                ImageProcessColorTempMode.Warm => (1.08, 1.0, 0.92),
                ImageProcessColorTempMode.Cool => (0.92, 1.0, 1.08),
                _ => (1.0, 1.0, 1.0)
            };

            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, buffer, 0, bytes);

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int pixelIndex = y * stride + x * 4;
                        buffer[pixelIndex + 2] = (byte)Math.Clamp((int)Math.Round(buffer[pixelIndex + 2] * redMultiplier), 0, 255);
                        buffer[pixelIndex + 1] = (byte)Math.Clamp((int)Math.Round(buffer[pixelIndex + 1] * greenMultiplier), 0, 255);
                        buffer[pixelIndex] = (byte)Math.Clamp((int)Math.Round(buffer[pixelIndex] * blueMultiplier), 0, 255);
                    }
                }

                Marshal.Copy(buffer, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void ApplyBoxBlur3x3InPlace(Bitmap bitmap)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            try
            {
                int stride = bitmapData.Stride;
                int bytes = Math.Abs(stride) * bitmap.Height;
                byte[] source = new byte[bytes];
                byte[] destination = new byte[bytes];
                Marshal.Copy(bitmapData.Scan0, source, 0, bytes);
                Buffer.BlockCopy(source, 0, destination, 0, bytes);

                if (bitmap.Width < 3 || bitmap.Height < 3)
                {
                    Marshal.Copy(destination, 0, bitmapData.Scan0, bytes);
                    return;
                }

                int[] topRowSums = new int[bitmap.Width * 3];
                int[] middleRowSums = new int[bitmap.Width * 3];
                int[] bottomRowSums = new int[bitmap.Width * 3];
                for (int y = 1; y < bitmap.Height - 1; y++)
                {
                    FillHorizontalBlurSums(source, topRowSums, y - 1, stride, bitmap.Width);
                    FillHorizontalBlurSums(source, middleRowSums, y, stride, bitmap.Width);
                    FillHorizontalBlurSums(source, bottomRowSums, y + 1, stride, bitmap.Width);

                    for (int x = 1; x < bitmap.Width - 1; x++)
                    {
                        int pixelIndex = y * stride + x * 4;

                        int sumIndex = x * 3;
                        destination[pixelIndex] = (byte)((topRowSums[sumIndex]
                            + middleRowSums[sumIndex] + bottomRowSums[sumIndex]) / 9);
                        destination[pixelIndex + 1] = (byte)((topRowSums[sumIndex + 1]
                            + middleRowSums[sumIndex + 1] + bottomRowSums[sumIndex + 1]) / 9);
                        destination[pixelIndex + 2] = (byte)((topRowSums[sumIndex + 2]
                            + middleRowSums[sumIndex + 2] + bottomRowSums[sumIndex + 2]) / 9);

                        destination[pixelIndex + 3] = source[pixelIndex + 3];
                    }
                }

                Marshal.Copy(destination, 0, bitmapData.Scan0, bytes);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void FillHorizontalBlurSums(
            byte[] source,
            int[] rowSums,
            int row,
            int stride,
            int width)
        {
            int rowStart = row * stride;
            int firstPixel = rowStart;
            int secondPixel = rowStart + 4;
            int thirdPixel = rowStart + 8;

            int blueSum = source[firstPixel] + source[secondPixel] + source[thirdPixel];
            int greenSum = source[firstPixel + 1] + source[secondPixel + 1] + source[thirdPixel + 1];
            int redSum = source[firstPixel + 2] + source[secondPixel + 2] + source[thirdPixel + 2];

            for (int x = 1; x < width - 1; x++)
            {
                int sumIndex = x * 3;
                rowSums[sumIndex] = blueSum;
                rowSums[sumIndex + 1] = greenSum;
                rowSums[sumIndex + 2] = redSum;

                if (x < width - 2)
                {
                    int outgoingPixel = rowStart + (x - 1) * 4;
                    int incomingPixel = rowStart + (x + 2) * 4;
                    blueSum += source[incomingPixel] - source[outgoingPixel];
                    greenSum += source[incomingPixel + 1] - source[outgoingPixel + 1];
                    redSum += source[incomingPixel + 2] - source[outgoingPixel + 2];
                }
            }
        }

        private static bool IsLossyImageFormat(string imageFormat)
        {
            return imageFormat.Equals("JPG", StringComparison.OrdinalIgnoreCase)
                || imageFormat.Equals("JPEG", StringComparison.OrdinalIgnoreCase)
                || imageFormat.Equals("WEBP", StringComparison.OrdinalIgnoreCase);
        }

        private static void SaveScreenshot(Bitmap bitmap, string filePath, string imageFormat, int imageQuality)
        {
            string format = imageFormat.ToUpper();

            if (format == "PNG")
            {
                ConvertAndSavePng(bitmap, filePath);
            }
            else if (format == "JPG")
            {
                ConvertAndSaveJpg(bitmap, filePath, imageQuality);
            }
            else if (format == "BMP")
            {
                bitmap.Save(filePath, ImageFormat.Bmp);
            }
            else if (format == "WEBP")
            {
                ConvertAndSaveWebp(bitmap, filePath, imageQuality);
            }
            else
            {
                // Default to JPG if the format is unrecognized
                ConvertAndSaveJpg(bitmap, filePath, imageQuality);
            }
        }

        private static void ConvertAndSavePng(Bitmap bitmap, string filePath)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Png, 100);
        }

        private static void ConvertAndSaveJpg(Bitmap bitmap, string filePath, int imageQuality)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Jpeg, imageQuality);
        }

        private static void ConvertAndSaveWebp(Bitmap bitmap, string filePath, int imageQuality)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Webp, imageQuality);
        }

        private static void EncodeAndSave(
            Bitmap bitmap,
            string filePath,
            SKEncodedImageFormat format,
            int quality)
        {
            Rectangle rect = new(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(
                rect,
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            try
            {
                var imageInfo = new SKImageInfo(
                    bitmap.Width,
                    bitmap.Height,
                    SKColorType.Bgra8888,
                    SKAlphaType.Unpremul);
                using var image = SKImage.FromPixels(imageInfo, bitmapData.Scan0, bitmapData.Stride);
                using var encodedData = image?.Encode(format, quality)
                    ?? throw new InvalidOperationException($"SkiaSharp could not encode the image as {format}.");

                using var output = File.Create(filePath);
                encodedData.SaveTo(output);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private string GenerateFileName()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            string extension = _settings.ImageFormat.ToLowerInvariant();

            if (extension == "jpeg")
            {
                extension = "jpg";
            }

            // string captureType = _settings.CaptureFullScreen ? "fullscreen" : "window";
            // return $"screenshot_{captureType}_{timestamp}.{extension}";
            return $"Screenshot {timestamp}.{extension}";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _retentionService.Dispose();
                _dedupeService.Dispose();
                _dedupeGate.Dispose();
                _disposed = true;
            }
        }
    }
}
