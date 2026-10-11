using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Numerics;
using SkiaSharp;

namespace Capillume
{
    public enum ScreenshotDedupeDecision
    {
        Save,
        SkipDuplicate,
        Indeterminate
    }

    public sealed class ScreenshotDedupeResult
    {
        public ScreenshotDedupeDecision Decision { get; init; }
        public string Reason { get; init; } = string.Empty;
        public int PerceptualHashDistance { get; init; }
        public double PixelDifferencePercent { get; init; }
        public double SsimScore { get; init; }
        internal DedupeFrame? Candidate { get; init; }
    }

    public sealed class ScreenshotDedupeRequest
    {
        public required Bitmap Bitmap { get; init; }
        public required string BaselineKey { get; init; }
        public Rectangle ComparisonBounds { get; init; }
        public IReadOnlyCollection<Rectangle> ExcludedRegions { get; init; } = Array.Empty<Rectangle>();
    }

    internal sealed class DedupeFrame
    {
        public required string Key { get; init; }
        public required byte[] Checksum { get; init; }
        public required byte[] Pixels { get; init; }
        public required bool[] ExcludedPixels { get; init; }
        public required int Width { get; init; }
        public required int Height { get; init; }
        public required ulong PerceptualHash { get; init; }
    }

    public sealed class ScreenshotDedupeService : IDisposable
    {
        private const int AnalysisMaximumDimension = 512;
        private readonly Dictionary<string, DedupeFrame> _baselines = new(StringComparer.Ordinal);
        private bool _disposed;

        public ScreenshotDedupeResult Compare(ScreenshotDedupeRequest request, DedupeSettings settings, CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            cancellationToken.ThrowIfCancellationRequested();

            DedupeFrame candidate = CreateFrame(request, cancellationToken);
            if (!_baselines.TryGetValue(request.BaselineKey, out DedupeFrame? baseline) ||
                baseline.Width != candidate.Width || baseline.Height != candidate.Height ||
                !MasksMatch(baseline.ExcludedPixels, candidate.ExcludedPixels))
            {
                return Save(candidate, "No compatible saved reference is available.");
            }

            if (CryptographicOperations.FixedTimeEquals(baseline.Checksum, candidate.Checksum))
            {
                return Skip(candidate, "The capture exactly matches the last saved reference.", 0, 0);
            }

            int hashDistance = BitOperations.PopCount(baseline.PerceptualHash ^ candidate.PerceptualHash);
            double differencePercent = CalculateDifferencePercent(baseline, candidate, settings.Sensitivity, cancellationToken);
            if (settings.CompareMode == DedupeCompareMode.PixelDifferenceOnly)
            {
                double threshold = GetDifferenceThreshold(settings.Sensitivity);
                return differencePercent > threshold
                    ? Save(candidate, $"Pixel difference {differencePercent:F3}% exceeds the {threshold:F3}% threshold.", hashDistance, differencePercent)
                    : Skip(candidate, $"Pixel difference {differencePercent:F3}% is within the {threshold:F3}% threshold.", hashDistance, differencePercent);
            }

            int hashThreshold = Math.Clamp(16 - settings.Sensitivity * 16 / 100, 2, 16);
            if (settings.CompareMode == DedupeCompareMode.PerceptualThenSsim && hashDistance >= hashThreshold)
            {
                return Save(candidate, $"Perceptual hash distance {hashDistance} exceeds the {hashThreshold} threshold.", hashDistance, differencePercent);
            }

            double ssim = CalculateSsim(baseline, candidate, cancellationToken);
            double ssimThreshold = GetSsimThreshold(settings.Sensitivity);
            return ssim < ssimThreshold
                ? Save(candidate, $"SSIM {ssim:F6} is below the {ssimThreshold:F6} threshold.", hashDistance, differencePercent, ssim)
                : Skip(candidate, $"SSIM {ssim:F6} is within the {ssimThreshold:F6} threshold.", hashDistance, differencePercent, ssim);
        }

        public void Commit(ScreenshotDedupeResult result)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (result.Candidate != null)
            {
                _baselines[result.Candidate.Key] = result.Candidate;
            }
        }

        public void Clear()
        {
            _baselines.Clear();
        }

        private static ScreenshotDedupeResult Save(DedupeFrame candidate, string reason, int hashDistance = 0, double differencePercent = 0, double ssim = 1)
        {
            return new ScreenshotDedupeResult
            {
                Decision = ScreenshotDedupeDecision.Save,
                Reason = reason,
                PerceptualHashDistance = hashDistance,
                PixelDifferencePercent = differencePercent,
                SsimScore = ssim,
                Candidate = candidate
            };
        }

        private static ScreenshotDedupeResult Skip(DedupeFrame candidate, string reason, int hashDistance, double differencePercent, double ssim = 1)
        {
            return new ScreenshotDedupeResult
            {
                Decision = ScreenshotDedupeDecision.SkipDuplicate,
                Reason = reason,
                PerceptualHashDistance = hashDistance,
                PixelDifferencePercent = differencePercent,
                SsimScore = ssim,
                Candidate = candidate
            };
        }

        private static double GetSsimThreshold(int sensitivity)
        {
            return 0.9995 - sensitivity * 0.00009;
        }

        private static double CalculateSsim(DedupeFrame baseline, DedupeFrame candidate, CancellationToken cancellationToken)
        {
            double sumX = 0;
            double sumY = 0;
            int count = 0;
            for (int index = 0; index < baseline.Pixels.Length; index++)
            {
                if ((index & 4095) == 0) cancellationToken.ThrowIfCancellationRequested();
                if (baseline.ExcludedPixels[index]) continue;
                sumX += baseline.Pixels[index];
                sumY += candidate.Pixels[index];
                count++;
            }

            if (count == 0) return 1;
            double meanX = sumX / count;
            double meanY = sumY / count;
            double varianceX = 0;
            double varianceY = 0;
            double covariance = 0;
            for (int index = 0; index < baseline.Pixels.Length; index++)
            {
                if ((index & 4095) == 0) cancellationToken.ThrowIfCancellationRequested();
                if (baseline.ExcludedPixels[index]) continue;
                double dx = baseline.Pixels[index] - meanX;
                double dy = candidate.Pixels[index] - meanY;
                varianceX += dx * dx;
                varianceY += dy * dy;
                covariance += dx * dy;
            }

            double divisor = Math.Max(1, count - 1);
            varianceX /= divisor;
            varianceY /= divisor;
            covariance /= divisor;
            const double c1 = 6.5025;
            const double c2 = 58.5225;
            return Math.Clamp(((2 * meanX * meanY + c1) * (2 * covariance + c2)) /
                ((meanX * meanX + meanY * meanY + c1) * (varianceX + varianceY + c2)), -1, 1);
        }

        private static DedupeFrame CreateFrame(ScreenshotDedupeRequest request, CancellationToken cancellationToken)
        {
            Rectangle bounds = request.ComparisonBounds;
            if (bounds == Rectangle.Empty)
            {
                bounds = new Rectangle(Point.Empty, request.Bitmap.Size);
            }

            bounds.Intersect(new Rectangle(Point.Empty, request.Bitmap.Size));
            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                throw new ArgumentException("The comparison bounds do not intersect the captured bitmap.", nameof(request));
            }

            Size size = GetAnalysisSize(bounds.Size);
            using var source = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(source))
            {
                graphics.DrawImage(request.Bitmap, new Rectangle(Point.Empty, source.Size), bounds, GraphicsUnit.Pixel);
            }

            using SKBitmap sourceSkBitmap = ToSkBitmap(source);
            //using SKBitmap analysis = sourceSkBitmap.Resize(new SKImageInfo(size.Width, size.Height, SKColorType.Bgra8888, SKAlphaType.Premul), SKFilterQuality.Medium)
            using SKBitmap analysis = sourceSkBitmap.Resize(new SKImageInfo(size.Width, size.Height, SKColorType.Bgra8888, SKAlphaType.Premul), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear))
                ?? throw new InvalidOperationException("Unable to resize the screenshot for comparison.");

            byte[] pixels = new byte[size.Width * size.Height];
            bool[] excluded = BuildExclusionMask(request.ExcludedRegions, bounds, size);
            using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            int offset = 0;
            for (int y = 0; y < size.Height; y++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                for (int x = 0; x < size.Width; x++)
                {
                    SKColor color = analysis.GetPixel(x, y);
                    byte gray = (byte)((299 * color.Red + 587 * color.Green + 114 * color.Blue) / 1000);
                    pixels[offset] = gray;
                    hash.AppendData([excluded[offset] ? (byte)0 : gray]);
                    offset++;
                }
            }

            return new DedupeFrame
            {
                Key = request.BaselineKey,
                Checksum = hash.GetHashAndReset(),
                Pixels = pixels,
                ExcludedPixels = excluded,
                Width = size.Width,
                Height = size.Height,
                PerceptualHash = CalculatePerceptualHash(pixels, excluded, size.Width, size.Height)
            };
        }

        private static SKBitmap ToSkBitmap(Bitmap bitmap)
        {
            Rectangle bounds = new(Point.Empty, bitmap.Size);
            BitmapData data = bitmap.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            try
            {
                var result = new SKBitmap(new SKImageInfo(bitmap.Width, bitmap.Height, SKColorType.Bgra8888, SKAlphaType.Premul));
                int bytes = Math.Abs(data.Stride) * bitmap.Height;
                byte[] buffer = new byte[bytes];
                Marshal.Copy(data.Scan0, buffer, 0, bytes);
                Marshal.Copy(buffer, 0, result.GetPixels(), bytes);
                return result;
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
        }

        private static Size GetAnalysisSize(Size source)
        {
            int maximum = Math.Max(source.Width, source.Height);
            if (maximum <= AnalysisMaximumDimension)
            {
                return source;
            }

            double scale = AnalysisMaximumDimension / (double)maximum;
            return new Size(Math.Max(1, (int)Math.Round(source.Width * scale)), Math.Max(1, (int)Math.Round(source.Height * scale)));
        }

        private static bool[] BuildExclusionMask(IReadOnlyCollection<Rectangle> regions, Rectangle sourceBounds, Size targetSize)
        {
            var excluded = new bool[targetSize.Width * targetSize.Height];
            foreach (Rectangle sourceRegion in regions)
            {
                Rectangle region = Rectangle.Intersect(sourceRegion, sourceBounds);
                if (region.IsEmpty)
                {
                    continue;
                }

                int left = Math.Clamp((int)Math.Floor((region.Left - sourceBounds.Left) * targetSize.Width / (double)sourceBounds.Width), 0, targetSize.Width);
                int top = Math.Clamp((int)Math.Floor((region.Top - sourceBounds.Top) * targetSize.Height / (double)sourceBounds.Height), 0, targetSize.Height);
                int right = Math.Clamp((int)Math.Ceiling((region.Right - sourceBounds.Left) * targetSize.Width / (double)sourceBounds.Width), 0, targetSize.Width);
                int bottom = Math.Clamp((int)Math.Ceiling((region.Bottom - sourceBounds.Top) * targetSize.Height / (double)sourceBounds.Height), 0, targetSize.Height);

                for (int y = top; y < bottom; y++)
                {
                    Array.Fill(excluded, true, y * targetSize.Width + left, right - left);
                }
            }

            return excluded;
        }

        private static bool MasksMatch(bool[] first, bool[] second)
        {
            return first.Length == second.Length && first.AsSpan().SequenceEqual(second);
        }

        private static ulong CalculatePerceptualHash(byte[] pixels, bool[] excluded, int width, int height)
        {
            Span<byte> sample = stackalloc byte[64];
            for (int cellY = 0; cellY < 8; cellY++)
            {
                for (int cellX = 0; cellX < 8; cellX++)
                {
                    int left = cellX * width / 8;
                    int right = Math.Max(left + 1, (cellX + 1) * width / 8);
                    int top = cellY * height / 8;
                    int bottom = Math.Max(top + 1, (cellY + 1) * height / 8);
                    int total = 0;
                    int count = 0;
                    for (int y = top; y < bottom; y++)
                    {
                        for (int x = left; x < right; x++)
                        {
                            int index = y * width + x;
                            if (!excluded[index])
                            {
                                total += pixels[index];
                                count++;
                            }
                        }
                    }

                    sample[cellY * 8 + cellX] = count == 0 ? (byte)0 : (byte)(total / count);
                }
            }

            int average = (int)sample.ToArray().Average(value => (int)value);
            ulong result = 0;
            for (int index = 0; index < sample.Length; index++)
            {
                if (sample[index] >= average)
                {
                    result |= 1UL << index;
                }
            }

            return result;
        }

        private static double CalculateDifferencePercent(DedupeFrame baseline, DedupeFrame candidate, int sensitivity, CancellationToken cancellationToken)
        {
            int channelTolerance = Math.Clamp(18 - sensitivity / 10, 4, 18);
            int compared = 0;
            int different = 0;
            for (int index = 0; index < baseline.Pixels.Length; index++)
            {
                if ((index & 4095) == 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                if (baseline.ExcludedPixels[index])
                {
                    continue;
                }

                compared++;
                if (Math.Abs(baseline.Pixels[index] - candidate.Pixels[index]) > channelTolerance)
                {
                    different++;
                }
            }

            return compared == 0 ? 0 : different * 100d / compared;
        }

        private static double GetDifferenceThreshold(int sensitivity)
        {
            return 0.05 + (100 - sensitivity) * 0.009;
        }

        public void Dispose()
        {
            _disposed = true;
            _baselines.Clear();
        }
    }
}
