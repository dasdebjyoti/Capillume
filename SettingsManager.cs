using System.Text.Json;

namespace Capillume
{
    public class AppSettings
    {
        public bool IsEnabled { get; set; } = false;
        public bool ShowNotifications { get; set; } = false;
        public bool StartWithWindows { get; set; } = false;
        public int IntervalMinutes { get; set; } = 10;
        public bool CaptureFullScreen { get; set; } = true;
        public string SaveFolder { get; set; } = string.Empty;
        public string ImageFormat { get; set; } = "PNG";
        public int ImageQuality { get; set; } = 90;
        public bool AutoStartWithWindows { get; set; } = false;
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
                    "Capillume Screenshots"
                )
            };
        }

        private static void ValidateSettings(AppSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.SaveFolder))
            {
                settings.SaveFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    "Capillume Screenshots"
                );
            }

            if (settings.IntervalMinutes < 1)
            {
                settings.IntervalMinutes = 1;
            }

            if (settings.ImageQuality < 1 || settings.ImageQuality > 100)
            {
                settings.ImageQuality = 70;
            }

            var validFormats = new[] { "PNG", "JPG", "BMP", "WEBP" };
            if (!validFormats.Contains(settings.ImageFormat.ToUpper()))
            {
                settings.ImageFormat = "PNG";
            }
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
