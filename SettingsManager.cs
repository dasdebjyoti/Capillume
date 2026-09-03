using Microsoft.VisualBasic;
using System.Text.Json;

// TODO
// Would you like me to also show you how to auto‑detect the user’s DPI scaling so the watermark
// font size adjusts proportionally to their Windows display settings? That would make your
// watermark look consistent across different monitors.

namespace Capillume
{
    public static class Constants
    {
        public const string ScreenshotsFoldername = "Capillume Screenshots";
        public const string StatusReady = "Ready.";
        public const int ScreenshotImageQualityMin = 1;
        public const int ScreenshotImageQualityMax = 100;
        public const int ScreenshotImageQualityDefault = 70;
        public const int WatermarkOpacityMin = 1;
        public const int WatermarkOpacityMax = 100;
        public const int WatermarkOpacityDefault = 50;
        public const int WatermarkImageScaleMin = 1;
        public const int WatermarkImageScaleMax = 100;
        public const int WatermarkImageScaleDefault = 50;
        public const string WatermarkPositionDefault = "Top Right";
        public const int AnnotationOpacityMin = 1;
        public const int AnnotationOpacityMax = 100;
        public const int AnnotationOpacityDefault = 80;
        public const string AnnotationFormatDefault = "{{OS}} | {{DATETIME}}";
    }

    public class AppSettings
    {
        public bool IsScreenshotEnabled { get; set; } = false;
        public bool ShowNotifications { get; set; } = false;
        public bool StartWithWindows { get; set; } = false;
        public int IntervalMinutes { get; set; } = 10;
        public bool CaptureFullScreen { get; set; } = true;
        public bool IncludeCapillume { get; set; } = false;
        public string SaveFolder { get; set; } = string.Empty;
        public string ImageFormat { get; set; } = "JPG";
        public int ImageQuality { get; set; } = Constants.ScreenshotImageQualityDefault; // 70;
        public bool AutoStartWithWindows { get; set; } = false;
        public WatermarkSettings Watermark { get; set; } = new();
        public AnnotationSettings Annotation { get; set; } = new();
    }

    public class WatermarkSettings
    {
        public bool Enabled { get; set; }
        public bool UseText { get; set; } = false;
        public bool UseImage { get; set; } = false;
        public string WatermarkText { get; set; } = "Capillume"; // string.Empty;
        public string WatermarkTextFontFamily { get; set; } = SystemFonts.DefaultFont.FontFamily.Name; //"Segoe UI";
        public float WatermarkTextFontSize { get; set; } = 24; // SystemFonts.DefaultFont.Size;
        public FontStyle WatermarkTextFontStyle { get; set; } = FontStyle.Regular;
        public string WatermarkImagePath { get; set; } = string.Empty;
        public int WatermarkImageScale { get; set; } = Constants.WatermarkImageScaleDefault; // 50
        public int WatermarkOpacity { get; set; } = Constants.WatermarkOpacityDefault; // 50
        public string WatermarkPosition { get; set; } = Constants.WatermarkPositionDefault;
        public int WatermarkRotation { get; set; } = 0; // 0, 90, 180, 270
    }

    public class AnnotationSettings
    {
        //public bool Enabled { get; set; }
        public bool UseAnnotation { get; set; } = false;
        public string AnnotationFormat { get; set; } = Constants.AnnotationFormatDefault;
        public string AnnotationFontFamily { get; set; } = SystemFonts.DefaultFont.FontFamily.Name; //"Segoe UI";
        public float AnnotationFontSize { get; set; } = (float)Math.Ceiling(SystemFonts.DefaultFont.Size + 8); //10;
        public FontStyle AnnotationFontStyle { get; set; } = FontStyle.Regular;
        public int AnnotationFontColorArgb { get; set; } = Color.Black.ToArgb();
        public int? AnnotationBackgroundColorArgb { get; set; } = Color.White.ToArgb();
        public int AnnotationOpacity { get; set; } = Constants.AnnotationOpacityDefault; // 80
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
