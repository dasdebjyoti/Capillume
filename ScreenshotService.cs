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

                    Bitmap? processedScreenshot = null;
                    try
                    {
                        processedScreenshot = CreateDownscaledBitmapIfNeeded(screenshot, _settings.CaptureFullScreen);
                        SaveScreenshot(processedScreenshot ?? screenshot, filePath);
                    }
                    finally
                    {
                        processedScreenshot?.Dispose();
                    }

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
            double scale = settings.Mode switch
            {
                DownscaleMode.TargetHeight => settings.TargetHeight / (double)sourceSize.Height,
                DownscaleMode.Percentage => settings.ResizePercentage / 100.0,
                DownscaleMode.MaxWidth => settings.MaxWidth / (double)sourceSize.Width,
                DownscaleMode.BoundingBox => Math.Min(
                    settings.BoundingBoxWidth / (double)sourceSize.Width,
                    settings.BoundingBoxHeight / (double)sourceSize.Height),
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

        private static bool IsLossyImageFormat(string imageFormat)
        {
            return imageFormat.Equals("JPG", StringComparison.OrdinalIgnoreCase)
                || imageFormat.Equals("JPEG", StringComparison.OrdinalIgnoreCase)
                || imageFormat.Equals("WEBP", StringComparison.OrdinalIgnoreCase);
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
