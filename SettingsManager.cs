using Microsoft.VisualBasic;
using System.Text.Json;

// TODO
// Would you like me to also show you how to auto‑detect the user’s DPI scaling so the watermark
// font size adjusts proportionally to their Windows display settings? That would make your
// watermark look consistent across different monitors.

namespace Capillume
{
    public enum DownscaleResizeMode
    {
        TargetHeight,
        Percentage,
        MaxWidth,
        BoundingBox
    }

    public enum DownscaleQuality
    {
        HighQuality,
        Balanced,
        Fast
    }

    public enum ImageProcessColorMode
    {
        FullColor,
        Grayscale,
        Monochrome1Bit,
        Color16,
        Color256,
        AdaptivePalette
    }

    public enum ImageProcessColorTempMode
    {
        Neutral,
        Warm,
        Cool
    }

    public enum DedupeCompareMode
    {
        PerceptualThenSsim,
        ChecksumThenSsim,
        PixelDifferenceOnly,
        SsimOnly
    }

    public static class DedupeCompareModeHelper
    {
        // Unified mapping: enum + display label
        public static readonly (DedupeCompareMode Mode, string Label)[] Items =
        [
            (DedupeCompareMode.PerceptualThenSsim, "Perceptual hash + SSIM"),
            (DedupeCompareMode.ChecksumThenSsim, "Checksum + SSIM"),
            (DedupeCompareMode.PixelDifferenceOnly, "Pixel difference only"),
            (DedupeCompareMode.SsimOnly, "SSIM only")
        ];

        // Convert enum → display string
        public static string ToModeString(DedupeCompareMode mode)
        {
            foreach (var item in Items)
                if (item.Mode == mode)
                    return item.Label;

            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        // Convert display string → enum
        public static DedupeCompareMode ToModeCode(string label)
        {
            foreach (var item in Items)
                if (item.Label == label)
                    return item.Mode;

            // Label not found → return default
            return Constants.DedupeCompareModeDefault;
            //throw new ArgumentException($"Invalid dynamic region mode label: {label}");
        }

        // Convenience: get all labels for ComboBox
        public static string[] Labels => Items.Select(i => i.Label).ToArray();
    }

    public enum DedupeDynamicRegionMode
    {
        IncludeAll,
        ExcludeTaskbars,
        ExcludeTaskbarsAndManualRegions,
        ManualRegionsOnly
    }

    public static class DedupeDynamicRegionModeHelper
    {
        // Unified mapping: enum + display label
        public static readonly (DedupeDynamicRegionMode Mode, string Label)[] Items =
        [
            (DedupeDynamicRegionMode.IncludeAll, "Include all screen areas"),
            (DedupeDynamicRegionMode.ExcludeTaskbars, "Exclude Windows taskbars"),
            (DedupeDynamicRegionMode.ExcludeTaskbarsAndManualRegions, "Exclude taskbars and manual regions"),
            (DedupeDynamicRegionMode.ManualRegionsOnly, "Exclude manual regions only")
        ];

        // Convert enum → display string
        public static string ToModeString(DedupeDynamicRegionMode mode)
        {
            foreach (var item in Items)
                if (item.Mode == mode)
                    return item.Label;

            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        // Convert display string → enum
        public static DedupeDynamicRegionMode ToModeCode(string label)
        {
            foreach (var item in Items)
                if (item.Label == label)
                    return item.Mode;

            // Label not found → return default
            return Constants.DedupeDynamicRegionModeDefault;
            //throw new ArgumentException($"Invalid dynamic region mode label: {label}");
        }

        // Convenience: get all labels for ComboBox
        public static string[] Labels => Items.Select(i => i.Label).ToArray();
    }

    public enum RetentionAction
    {
        MoveToRecycleBin,
        DeletePermanently,
        BackupThenDelete
    }

    public static class RetentionActionHelper
    {
        // Unified mapping: enum + display label
        public static readonly (RetentionAction Mode, string Label)[] Items =
        [
            (RetentionAction.MoveToRecycleBin, "Move to Recycle Bin"),
            (RetentionAction.DeletePermanently, "Delete permanently"),
            (RetentionAction.BackupThenDelete, "Backup to folder, then delete")
        ];

        // Convert enum → display string
        public static string ToModeString(RetentionAction mode)
        {
            foreach (var item in Items)
                if (item.Mode == mode)
                    return item.Label;

            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        // Convert display string → enum
        public static RetentionAction ToModeCode(string label)
        {
            foreach (var item in Items)
                if (item.Label == label)
                    return item.Mode;

            // Label not found → return default
            return Constants.RetentionActionDefault;
            //throw new ArgumentException($"Invalid dynamic region mode label: {label}");
        }

        // Convenience: get all labels for ComboBox
        public static string[] Labels => Items.Select(i => i.Label).ToArray();
    }

    public static class Constants
    {
        // App-wide constants and defaults
        public const int TooltipAutoPopDelay = 8000;
        public const int TooltipInitialDelay = 500;
        public const int TooltipReshowDelay = 200;
        public const bool TooltipShowAlways = false;

        // Main window constants and default
        public const string ScreenshotsFoldername = "Capillume Screenshots";
        public const string MainStatusReady = "Ready.";
        public const string MainStatusSettingsUnsaved = "Unsaved changes - click Save Settings.";
        public const string MainStatusSettingsSaved = "Settings saved successfully.";
        public const bool MainScreenshotEnabledDefault = true;
        public const bool MainShowNotificationsDefault = false;
        public const bool MainStartWithWindowsDefault = false;
        public const int MainIntervalMinutesDefault = 10;
        public const bool MainCaptureFullScreenDefault = true;
        public const bool MainExcludeCapillumeDefault = true;
        public const int ScreenshotImageQualityMin = 1;
        public const int ScreenshotImageQualityMax = 100;
        public const int ScreenshotImageQualityDefault = 70;
        public const bool ScreenshotAutoStartWithWindowsDefault = false;

        // Watermark constants and defaults
        public static readonly string WatermarkTextDefault = "Capillume"; //string.Empty;
        public static readonly string WatermarkTextFontFamilyDefault = SystemFonts.DefaultFont.FontFamily.Name; //"Segoe UI";
        public static readonly float WatermarkTextFontSizeDefault = (float)Math.Ceiling(SystemFonts.DefaultFont.Size + 10);
        public const FontStyle WatermarkTextFontStyleDefault = FontStyle.Regular;
        public static readonly string WatermarkImagePathDefault = string.Empty;
        public const int WatermarkImageScaleMin = 1;
        public const int WatermarkImageScaleMax = 100;
        public const int WatermarkImageScaleDefault = 50;
        public const int WatermarkOpacityMin = 1;
        public const int WatermarkOpacityMax = 100;
        public const int WatermarkOpacityDefault = 50;
        public const string WatermarkPositionDefault = "Top Right";
        public const int WatermarkRotationDefault = 0; // 0, 90, 180, 270

        // Annotation constants and defaults
        public const string AnnotationFormatDefault = "{{OS}} | {{DATETIME}}";
        public static readonly string AnnotationFontFamilyDefault = SystemFonts.DefaultFont.FontFamily.Name; //"Segoe UI";
        public static readonly float AnnotationFontSizeDefault = (float)Math.Ceiling(SystemFonts.DefaultFont.Size + 8); //10;
        public const FontStyle AnnotationFontStyleDefault = FontStyle.Regular;
        public static readonly int AnnotationFontColorArgbDefault = Color.Black.ToArgb();
        public static readonly int AnnotationBackgroundColorArgbDefault = Color.White.ToArgb();
        public const int AnnotationOpacityDefault = 80;
        public const int AnnotationOpacityMin = 1;
        public const int AnnotationOpacityMax = 100;

        // Downscale constants and defaults
        public const DownscaleResizeMode DownscaleResizeModeDefault = DownscaleResizeMode.TargetHeight;
        public const int DownscaleResizeTargetHeightMin = 100;
        public const int DownscaleResizeTargetHeightMax = 10000;
        public const int DownscaleResizeTargetHeightDefault = 1080;
        public const int DownscaleResizePercentageMin = 1;
        public const int DownscaleResizePercentageMax = 100;
        public const int DownscaleResizePercentageDefault = 75;
        public const int DownscaleResizeMaxWidthMin = 100;
        public const int DownscaleResizeMaxWidthMax = 10000;
        public const int DownscaleResizeMaxWidthDefault = 1920;
        public const int DownscaleResizeBoundingWidthDefault = 1920;
        public const int DownscaleResizeBoundingHeightDefault = 1080;
        public const DownscaleQuality DownscaleQualityDefault = DownscaleQuality.HighQuality;
        public const bool DownscaleSharpenAfterResizeDefault = false;
        public const bool DownscaleSkipSmallerImagesDefault = true;
        public const bool DownscaleFullScreenOnlyDefault = false;
        public const bool DownscaleLossyFormatsOnlyDefault = false;

        // Image Processing constants and defaults
        public const ImageProcessColorMode ImageProcColorModeDefault = ImageProcessColorMode.FullColor;
        public const bool ImageProcHighContrastDefault = false;
        public const bool ImageProcNoiseReductionDefault = false;
        public const ImageProcessColorTempMode ImageProcColorTemperatureDefault = ImageProcessColorTempMode.Neutral;

        // Dedupe/Duplicate constants and defaults
        public const DedupeCompareMode DedupeCompareModeDefault = DedupeCompareMode.PerceptualThenSsim;
        public const int DedupeSensitivityDefault = 50;
        public const int DedupeTimeoutMillisecondsDefault = 250;
        public const DedupeDynamicRegionMode DedupeDynamicRegionModeDefault = DedupeDynamicRegionMode.ExcludeTaskbars;
        public static readonly List<Rectangle> DedupeExcludeRegionsDefault = [];
        public const bool DedupeComparePerActiveWindowDefault = true;
        public const bool DedupeUpdateReferenceOnlyOnSaveDefault = true;
        public const int DedupeMinimumMinutesBetweenSavesDefault = 0;
        public const bool DedupeDryRunDefault = false;
        public const bool DedupeShowSkippedNotificationsDefault = false;
        public const bool DedupeEnableDiagnosticsDefault = false;

        // Retention constants and defaults
        public const bool RetentionMaxDaysEnabledDefault = true;
        public const int RetentionDaysMin = 1;
        public const int RetentionDaysMax = 3650;
        public const int RetentionDaysDefault = 30;
        public const bool RetentionMaxFilesEnabledDefault = false;
        public const int RetentionFileCountMin = 1;
        public const int RetentionFileCountMax = 1000000;
        public const int RetentionFileCountDefault = 500;
        public const bool RetentionIncludeSubfoldersDefault = false;
        public const RetentionAction RetentionActionDefault = RetentionAction.MoveToRecycleBin;
        public static readonly string RetentionBackupFolderDefault = string.Empty;
        public const bool RetentionPerSessionSubfolderDefault = false;
        public const bool RetentionDryRunModeDefault = true;

        // Hotkey constants and defaults
        public static readonly Keys HotkeyModifiersDefault = Keys.Control | Keys.Shift;
        public static readonly Keys HotkeyKeyDefault = Keys.F12;
        public const string HotKeyMsgNoModifier = "Choose a shortcut with at least one modifier and a key.";
        public const string HotKeyMsgConflict =
            "Your selected hotkey is already in use by another application.\n" +
            "Please choose another hotkey in Advanced Settings → Hotkeys.\n\n" +
            "You previous hotkey (if any) has been restored.";
        public const string HotKeyMsgConflictNoRestore =
            "Your selected hotkey is already in use by another application.\n" +
            "Please choose another hotkey in Advanced Settings → Hotkeys.\n\n" +
            "No previous hotkey was restored. Hotkey disabled.";
    }

    public class AppSettings
    {
        public bool IsScreenshotEnabled { get; set; } = Constants.MainScreenshotEnabledDefault;
        public bool ShowNotifications { get; set; } = Constants.MainShowNotificationsDefault;
        public bool StartWithWindows { get; set; } = Constants.MainStartWithWindowsDefault;
        public int IntervalMinutes { get; set; } = Constants.MainIntervalMinutesDefault;
        public bool CaptureFullScreen { get; set; } = Constants.MainCaptureFullScreenDefault;
        public bool ExcludeCapillume { get; set; } = Constants.MainExcludeCapillumeDefault;
        public string SaveFolder { get; set; } = string.Empty;
        public string ImageFormat { get; set; } = "JPG";
        public int ImageQuality { get; set; } = Constants.ScreenshotImageQualityDefault; // 70;
        public bool AutoStartWithWindows { get; set; } = Constants.ScreenshotAutoStartWithWindowsDefault;
        public WatermarkSettings Watermark { get; set; } = new();
        public AnnotationSettings Annotation { get; set; } = new();
        public DownscaleSettings Downscale { get; set; } = new();
        public ImageProcessingSettings ImageProcessing { get; set; } = new();
        public RetentionSettings Retention { get; set; } = new();
        public HotkeySettings Hotkeys { get; set; } = new();
        public DedupeSettings Dedupe { get; set; } = new();
    }

    public class WatermarkSettings
    {
        public bool UseText { get; set; } = false;
        public bool UseImage { get; set; } = false;
        public string WatermarkText { get; set; } = Constants.WatermarkTextDefault;
        public string WatermarkTextFontFamily { get; set; } = Constants.WatermarkTextFontFamilyDefault;
        public float WatermarkTextFontSize { get; set; } = Constants.WatermarkTextFontSizeDefault; //24;
        public FontStyle WatermarkTextFontStyle { get; set; } = Constants.WatermarkTextFontStyleDefault;
        public string WatermarkImagePath { get; set; } = Constants.WatermarkImagePathDefault;
        public int WatermarkImageScale { get; set; } = Constants.WatermarkImageScaleDefault; // 50
        public int WatermarkOpacity { get; set; } = Constants.WatermarkOpacityDefault; // 50
        public string WatermarkPosition { get; set; } = Constants.WatermarkPositionDefault;
        public int WatermarkRotation { get; set; } = Constants.WatermarkRotationDefault; // 0, 90, 180, 270
    }

    public class AnnotationSettings
    {
        public bool UseAnnotation { get; set; } = false;
        public string AnnotationFormat { get; set; } = Constants.AnnotationFormatDefault;
        public string AnnotationFontFamily { get; set; } = Constants.AnnotationFontFamilyDefault;
        public float AnnotationFontSize { get; set; } = Constants.AnnotationFontSizeDefault;
        public FontStyle AnnotationFontStyle { get; set; } = Constants.AnnotationFontStyleDefault;
        public int AnnotationFontColorArgb { get; set; } = Constants.AnnotationFontColorArgbDefault;
        public int? AnnotationBackgroundColorArgb { get; set; } = Constants.AnnotationBackgroundColorArgbDefault;
        public int AnnotationOpacity { get; set; } = Constants.AnnotationOpacityDefault;
    }

    public class DownscaleSettings
    {
        public bool Enabled { get; set; } = false;
        public DownscaleResizeMode ResizeMode { get; set; } = Constants.DownscaleResizeModeDefault;
        public int ResizeTargetHeight { get; set; } = Constants.DownscaleResizeTargetHeightDefault;
        public int ResizePercentage { get; set; } = Constants.DownscaleResizePercentageDefault;
        public int ResizeMaxWidth { get; set; } = Constants.DownscaleResizeMaxWidthDefault;
        public int ResizeBoundingBoxWidth { get; set; } = Constants.DownscaleResizeBoundingWidthDefault;
        public int ResizeBoundingBoxHeight { get; set; } = Constants.DownscaleResizeBoundingHeightDefault;
        public DownscaleQuality Quality { get; set; } = Constants.DownscaleQualityDefault;
        public bool SharpenAfterResize { get; set; } = Constants.DownscaleSharpenAfterResizeDefault;
        public bool SkipSmallerImages { get; set; } = Constants.DownscaleSkipSmallerImagesDefault;
        public bool FullScreenOnly { get; set; } = Constants.DownscaleFullScreenOnlyDefault;
        public bool LossyFormatsOnly { get; set; } = Constants.DownscaleLossyFormatsOnlyDefault;
    }

    public class ImageProcessingSettings
    {
        public ImageProcessColorMode ColorMode { get; set; } = Constants.ImageProcColorModeDefault;
        public bool HighContrast { get; set; } = Constants.ImageProcHighContrastDefault;
        public bool NoiseReduction { get; set; } = Constants.ImageProcNoiseReductionDefault;
        public ImageProcessColorTempMode ColorTemperature { get; set; } = Constants.ImageProcColorTemperatureDefault;
    }

    public class DedupeSettings
    {
        public bool Enabled { get; set; } = true;
        public DedupeCompareMode CompareMode { get; set; } = Constants.DedupeCompareModeDefault;
        public int Sensitivity { get; set; } = Constants.DedupeSensitivityDefault;
        public int TimeoutMilliseconds { get; set; } = Constants.DedupeTimeoutMillisecondsDefault;
        public DedupeDynamicRegionMode DynamicRegionMode { get; set; } = Constants.DedupeDynamicRegionModeDefault;
        public List<Rectangle> ExcludeRegions { get; set; } = Constants.DedupeExcludeRegionsDefault;
        public bool ComparePerActiveWindow { get; set; } = Constants.DedupeComparePerActiveWindowDefault;
        public bool UpdateReferenceOnlyOnSave { get; set; } = Constants.DedupeUpdateReferenceOnlyOnSaveDefault;
        public int MinimumMinutesBetweenSaves { get; set; } = Constants.DedupeMinimumMinutesBetweenSavesDefault;
        public bool DryRun { get; set; } = Constants.DedupeDryRunDefault;
        public bool ShowSkippedNotifications { get; set; } = Constants.DedupeShowSkippedNotificationsDefault;
        public bool EnableDiagnostics { get; set; } = Constants.DedupeEnableDiagnosticsDefault;
    }

    public class RetentionSettings
    {
        public bool AutoCleanupEnabled { get; set; } = false;
        public bool MaxDaysEnabled { get; set; } = Constants.RetentionMaxDaysEnabledDefault;
        public int MaxDaysToRetain { get; set; } = Constants.RetentionDaysDefault;
        public bool MaxFilesEnabled { get; set; } = Constants.RetentionMaxFilesEnabledDefault;
        public int MaxFilesToRetain { get; set; } = Constants.RetentionFileCountDefault;
        public bool IncludeSubfolders { get; set; } = Constants.RetentionIncludeSubfoldersDefault;
        public RetentionAction Action { get; set; } = Constants.RetentionActionDefault;
        public string BackupFolder { get; set; } = Constants.RetentionBackupFolderDefault;
        public bool PerSessionSubfolder { get; set; } = Constants.RetentionPerSessionSubfolderDefault;
        public bool DryRunMode { get; set; } = Constants.RetentionDryRunModeDefault;
    }

    public class HotkeySettings
    {
        public bool Enabled { get; set; } = true;
        public Keys Modifiers { get; set; } = Constants.HotkeyModifiersDefault;
        public Keys Key { get; set; } = Constants.HotkeyKeyDefault;
        public bool HasConflict { get; set; } = false;
    }

    public static class SettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Capillume",
            "settings.json"
        );

        public static AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    var settings = JsonSerializer.Deserialize<AppSettings>(json);
                    if (settings != null)
                    {
                        ValidateSettings(settings);
                        return settings;
                    }
                }
            }
            catch
            {
                // If loading fails, return default settings
            }

            return CreateDefaultSettings();
        }

        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                ValidateSettings(settings);

                string directory = Path.GetDirectoryName(SettingsPath)!;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static AppSettings CreateDefaultSettings()
        {
            return new AppSettings
            {
                SaveFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Constants.ScreenshotsFoldername
                )
            };
        }

        private static void ValidateSettings(AppSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.SaveFolder))
            {
                settings.SaveFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    Constants.ScreenshotsFoldername
                );
            }

            if (settings.IntervalMinutes < 1)
            {
                settings.IntervalMinutes = 1;
            }

            if (settings.ImageQuality < Constants.ScreenshotImageQualityMin || settings.ImageQuality > Constants.ScreenshotImageQualityMax)
            {
                settings.ImageQuality = Constants.ScreenshotImageQualityDefault; // 70;
            }

            var validFormats = new[] { "JPG", "PNG", "BMP", "WEBP" };
            if (!validFormats.Contains(settings.ImageFormat.ToUpper()))
            {
                settings.ImageFormat = "JPG";
            }

            settings.Watermark ??= new WatermarkSettings();
            settings.Watermark.WatermarkTextFontFamily ??= SystemFonts.DefaultFont.FontFamily.Name;
            settings.Watermark.WatermarkTextFontSize = Math.Clamp(settings.Watermark.WatermarkTextFontSize, 6, 200);
            settings.Watermark.WatermarkImageScale = Math.Clamp(settings.Watermark.WatermarkImageScale, Constants.WatermarkImageScaleMin, Constants.WatermarkImageScaleMax);
            settings.Watermark.WatermarkOpacity = Math.Clamp(settings.Watermark.WatermarkOpacity, Constants.WatermarkOpacityMin, Constants.WatermarkOpacityMax);
            if (settings.Watermark.WatermarkRotation is not (0 or 90 or 180 or 270))
            {
                settings.Watermark.WatermarkRotation = 0;
            }

            settings.Annotation ??= new AnnotationSettings();
            settings.Annotation.AnnotationFontFamily ??= SystemFonts.DefaultFont.FontFamily.Name;
            settings.Annotation.AnnotationFontSize = Math.Clamp(settings.Annotation.AnnotationFontSize, 6, 200);
            settings.Annotation.AnnotationFormat ??= string.Empty;
            settings.Annotation.AnnotationOpacity = Math.Clamp(settings.Annotation.AnnotationOpacity, Constants.AnnotationOpacityMin, Constants.AnnotationOpacityMax);

            settings.Downscale ??= new DownscaleSettings();
            if (!Enum.IsDefined(settings.Downscale.ResizeMode))
            {
                settings.Downscale.ResizeMode = DownscaleResizeMode.TargetHeight;
            }

            settings.Downscale.ResizeTargetHeight = Math.Clamp(
                settings.Downscale.ResizeTargetHeight,
                Constants.DownscaleResizeTargetHeightMin,
                Constants.DownscaleResizeTargetHeightMax);
            settings.Downscale.ResizePercentage = Math.Clamp(
                settings.Downscale.ResizePercentage,
                Constants.DownscaleResizePercentageMin,
                Constants.DownscaleResizePercentageMax);
            settings.Downscale.ResizeMaxWidth = Math.Clamp(
                settings.Downscale.ResizeMaxWidth,
                Constants.DownscaleResizeMaxWidthMin,
                Constants.DownscaleResizeMaxWidthMax);
            settings.Downscale.ResizeBoundingBoxWidth = Math.Clamp(
                settings.Downscale.ResizeBoundingBoxWidth,
                Constants.DownscaleResizeMaxWidthMin,
                Constants.DownscaleResizeMaxWidthMax);
            settings.Downscale.ResizeBoundingBoxHeight = Math.Clamp(
                settings.Downscale.ResizeBoundingBoxHeight,
                Constants.DownscaleResizeTargetHeightMin,
                Constants.DownscaleResizeTargetHeightMax);

            if (!Enum.IsDefined(settings.Downscale.Quality))
            {
                settings.Downscale.Quality = DownscaleQuality.HighQuality;
            }

            settings.Retention ??= new RetentionSettings();
            settings.Retention.MaxDaysToRetain = Math.Clamp(
                settings.Retention.MaxDaysToRetain,
                Constants.RetentionDaysMin,
                Constants.RetentionDaysMax);
            settings.Retention.MaxFilesToRetain = Math.Clamp(
                settings.Retention.MaxFilesToRetain,
                Constants.RetentionFileCountMin,
                Constants.RetentionFileCountMax);
            settings.Retention.BackupFolder ??= string.Empty;

            //if (!Enum.IsDefined(settings.Retention.Action))
            //{
            //    settings.Retention.Action = RetentionAction.MoveToRecycleBin;
            //}
            if (!RetentionActionHelper.Items.Any(i => i.Mode == settings.Retention.Action))
            {
                settings.Retention.Action = RetentionAction.MoveToRecycleBin;
            }


            settings.Hotkeys ??= new HotkeySettings();
            settings.Hotkeys.Modifiers &= Keys.Modifiers;
            settings.Hotkeys.Key &= Keys.KeyCode;
            if (!Enum.IsDefined(typeof(Keys), settings.Hotkeys.Modifiers))
            {
                settings.Hotkeys.Modifiers = Constants.HotkeyModifiersDefault;
            }

            if (!Enum.IsDefined(typeof(Keys), settings.Hotkeys.Key))
            {
                settings.Hotkeys.Key = Constants.HotkeyKeyDefault;
            }

            if (settings.Hotkeys.Modifiers == Keys.None || settings.Hotkeys.Key == Keys.None)
            {
                settings.Hotkeys.Enabled = false;
            }

            settings.Dedupe ??= new DedupeSettings();
            if (!Enum.IsDefined(settings.Dedupe.CompareMode))
            {
                settings.Dedupe.CompareMode = DedupeCompareMode.PerceptualThenSsim;
            }

            if (!Enum.IsDefined(settings.Dedupe.DynamicRegionMode))
            {
                settings.Dedupe.DynamicRegionMode = DedupeDynamicRegionMode.ExcludeTaskbars;
            }

            settings.Dedupe.Sensitivity = Math.Clamp(settings.Dedupe.Sensitivity, 0, 100);
            settings.Dedupe.TimeoutMilliseconds = Math.Clamp(settings.Dedupe.TimeoutMilliseconds, 50, 1000);
            settings.Dedupe.MinimumMinutesBetweenSaves = Math.Clamp(settings.Dedupe.MinimumMinutesBetweenSaves, 0, 1440);
            settings.Dedupe.ExcludeRegions ??= [];
            settings.Dedupe.ExcludeRegions = settings.Dedupe.ExcludeRegions
                .Where(region => region.Width > 0 && region.Height > 0)
                .Take(20)
                .ToList();
        }

        public static void SetAutoStart(bool enable)
        {
            try
            {
                string appName = "Capillume";
                string appPath = Application.ExecutablePath;

                using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

                if (enable)
                {
                    key?.SetValue(appName, $"\"{appPath}\" --autostart");
                }
                else
                {
                    key?.DeleteValue(appName, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update auto-start setting: {ex.Message}", 
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
