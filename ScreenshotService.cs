using System.Drawing;
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

    public class ScreenshotService : IDisposable
    {
        private System.Windows.Forms.Timer? _timer;
        private AppSettings _settings;
        private readonly IntPtr _capillumeWindowHandle;
        private bool _disposed;
        private bool _isPaused;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

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

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const int SW_HIDE = 0;
        private const int SW_SHOWNOACTIVATE = 4;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        //public event EventHandler<string>? ScreenshotCaptured;
        public event EventHandler<ScreenshotCapturedEventArgs>? ScreenshotCaptured;
        public event EventHandler<string>? ErrorOccurred;

        public ScreenshotService(AppSettings settings, IntPtr capillumeWindowHandle)
        {
            _settings = settings;
            _capillumeWindowHandle = capillumeWindowHandle;
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
                        screenshot = CaptureActiveWindow(captureWindowBelowCapillume);
                    }

                    if (screenshot == null)
                    {
                        ErrorOccurred?.Invoke(this, "Failed to capture screenshot.");
                        return;
                    }

                    WatermarkRenderer.Apply(screenshot, _settings.Watermark, _settings.Annotation);

                    string filePath = Path.Combine(_settings.SaveFolder, GenerateFileName());

                    SaveScreenshot(screenshot, filePath);
                    sw.Stop();
                    long elapsedMs = sw.ElapsedMilliseconds;
                    ScreenshotCaptured?.Invoke(this, new ScreenshotCapturedEventArgs(filePath, sw.ElapsedMilliseconds));
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

            List<IntPtr> hiddenWindows = _settings.IncludeCapillume
                ? new List<IntPtr>()
                : HideCapillumeWindows();

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

        private Bitmap? CaptureActiveWindow(bool captureWindowBelowCapillume)
        {
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            bool hideCapillume = false;
            if (!_settings.IncludeCapillume && captureWindowBelowCapillume)
            {
                handle = FindWindowBelow(_capillumeWindowHandle);
                hideCapillume = true;
            }
            else if (!_settings.IncludeCapillume && IsCapillumeWindow(handle))
            {
                handle = FindWindowBelow(handle);
                hideCapillume = true;
                if (handle == IntPtr.Zero)
                {
                    return null;
                }
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
                // Default to JPG if the format is unrecognized
                ConvertAndSaveJpg(bitmap, filePath);
            }
        }

        private void ConvertAndSavePng(Bitmap bitmap, string filePath)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Png, 100);
        }

        private void ConvertAndSaveJpg(Bitmap bitmap, string filePath)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Jpeg, _settings.ImageQuality);
        }

        private void ConvertAndSaveWebp(Bitmap bitmap, string filePath)
        {
            EncodeAndSave(bitmap, filePath, SKEncodedImageFormat.Webp, _settings.ImageQuality);
        }

        private static void EncodeAndSave(
            Bitmap bitmap,
            string filePath,
            SKEncodedImageFormat format,
            int quality)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            ms.Position = 0;

            using var skBitmap = SKBitmap.Decode(ms)
                ?? throw new InvalidOperationException("SkiaSharp could not decode the captured bitmap.");
            using var image = SKImage.FromBitmap(skBitmap);
            using var encodedData = image.Encode(format, quality);

            if (encodedData == null)
            {
                throw new InvalidOperationException($"SkiaSharp could not encode the image as {format}.");
            }

            using var output = File.Create(filePath);
            encodedData.SaveTo(output);
        }

        private string GenerateFileName()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
            string extension = _settings.ImageFormat.ToLower();

            if (extension == "jpg")
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
                _disposed = true;
            }
        }
    }
}
