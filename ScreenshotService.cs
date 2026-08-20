using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Webp;

namespace Capilume
{
    public class ScreenshotService : IDisposable
    {
        private System.Windows.Forms.Timer? _timer;
        private AppSettings _settings;
        private bool _disposed;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public event EventHandler<string>? ScreenshotCaptured;
        public event EventHandler<string>? ErrorOccurred;

        public ScreenshotService(AppSettings settings)
        {
            _settings = settings;
        }

        public void Start()
        {
            if (_timer != null)
            {
                Stop();
            }

            _timer = new System.Windows.Forms.Timer
            {
                Interval = _settings.IntervalMinutes * 60 * 1000
            };
            _timer.Tick += OnTimerTick;
            _timer.Start();

            // Take first screenshot immediately if enabled
            if (_settings.IsEnabled)
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
        }

        public void UpdateSettings(AppSettings settings)
        {
            _settings = settings;

            if (_timer != null)
            {
                _timer.Interval = _settings.IntervalMinutes * 60 * 1000;
            }
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            if (_settings.IsEnabled)
            {
                CaptureScreenshot();
            }
        }

        public void CaptureScreenshot()
        {
            try
            {
                if (!Directory.Exists(_settings.SaveFolder))
                {
                    Directory.CreateDirectory(_settings.SaveFolder);
                }

                Bitmap? screenshot = null;
                try
                {
                    if (_settings.CaptureFullScreen)
                    {
                        screenshot = CaptureFullScreen();
                    }
                    else
                    {
                        screenshot = CaptureActiveWindow();
                    }

                    if (screenshot == null)
                    {
                        ErrorOccurred?.Invoke(this, "Failed to capture screenshot.");
                        return;
                    }

                    string fileName = GenerateFileName();
                    string filePath = Path.Combine(_settings.SaveFolder, fileName);

                    SaveScreenshot(screenshot, filePath);

                    ScreenshotCaptured?.Invoke(this, filePath);
                }
                finally
                {
                    screenshot?.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error capturing screenshot: {ex.Message}");
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

            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(minX, minY, 0, 0, new System.Drawing.Size(width, height));
            }

            return bitmap;
        }

        private Bitmap? CaptureActiveWindow()
        {
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            if (!GetWindowRect(handle, out RECT rect))
            {
                return null;
            }

            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (width <= 0 || height <= 0)
            {
                return null;
            }

            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0, new System.Drawing.Size(width, height));
            }

            return bitmap;
        }

        private void SaveScreenshot(Bitmap bitmap, string filePath)
        {
            string format = _settings.ImageFormat.ToUpper();

            if (format == "PNG")
            {
                ConvertAndSavePng(bitmap, filePath);
            }
            else if (format == "JPG")
            {
                ConvertAndSaveJpg(bitmap, filePath);
            }
            else if (format == "BMP")
            {
                bitmap.Save(filePath, ImageFormat.Bmp);
            }
            else if (format == "WEBP")
            {
                ConvertAndSaveWebp(bitmap, filePath);
            }
            else
            {
                // Default to PNG
                ConvertAndSavePng(bitmap, filePath);
            }
        }

        private void ConvertAndSavePng(Bitmap bitmap, string filePath)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            using var image = SixLabors.ImageSharp.Image.Load(ms);
            var encoder = new PngEncoder();
            image.Save(filePath, encoder);
        }

        private void ConvertAndSaveJpg(Bitmap bitmap, string filePath)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            using var image = SixLabors.ImageSharp.Image.Load(ms);
            var encoder = new JpegEncoder { Quality = _settings.ImageQuality };
            image.Save(filePath, encoder);
        }

        private void ConvertAndSaveWebp(Bitmap bitmap, string filePath)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            using var image = SixLabors.ImageSharp.Image.Load(ms);
            var encoder = new WebpEncoder { Quality = _settings.ImageQuality };
            image.Save(filePath, encoder);
        }

        private string GenerateFileName()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string extension = _settings.ImageFormat.ToLower();

            if (extension == "jpg")
            {
                extension = "jpg";
            }

            string captureType = _settings.CaptureFullScreen ? "fullscreen" : "window";
            return $"screenshot_{captureType}_{timestamp}.{extension}";
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}
