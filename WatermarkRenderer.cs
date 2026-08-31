using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;

namespace Capillume
{
    public static class WatermarkRenderer
    {
        public static void Apply(Bitmap screenshot, WatermarkSettings watermarkSettings, AnnotationSettings annotationSettings)
        {
            bool drawWatermark = watermarkSettings.Enabled && (watermarkSettings.UseText || watermarkSettings.UseImage);
            bool drawAnnotation = /*annotationSettings.Enabled &&*/ annotationSettings.UseAnnotation;
            if (!drawWatermark && !drawAnnotation)
            {
                return;
            }

            using var targetGraphics = Graphics.FromImage(screenshot);
            targetGraphics.CompositingMode = CompositingMode.SourceOver;

            if (drawWatermark)
            {
                DrawWatermark(targetGraphics, screenshot, watermarkSettings);
            }

            if (drawAnnotation)
            {
                DrawAnnotation(targetGraphics, screenshot, annotationSettings);
            }
        }

        private static void DrawWatermark(Graphics targetGraphics, Bitmap screenshot, WatermarkSettings settings)
        {
            const int padding = 16;
            const int gap = 12;

            using var sourceImage = settings.UseImage ? LoadImage(settings.WatermarkImagePath) : null;
            using var font = settings.UseText
                ? new Font(settings.WatermarkTextFontFamily, settings.WatermarkTextFontSize, settings.WatermarkTextFontStyle)
                : null;

            SizeF textSize = font is not null
                ? targetGraphics.MeasureString(settings.WatermarkText, font)
                : SizeF.Empty;
            Size imageSize = GetImageSize(sourceImage, screenshot.Size, settings.WatermarkImageScale);
            int width = (int)Math.Ceiling(Math.Max(textSize.Width, imageSize.Width)) + padding * 2;
            int height = (int)Math.Ceiling(textSize.Height + (font is not null && sourceImage is not null ? gap : 0) + imageSize.Height) + padding * 2;

            using var watermark = new Bitmap(Math.Max(width, 1), Math.Max(height, 1), PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(watermark))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                int y = padding;
                if (sourceImage is not null)
                {
                    using var attributes = new ImageAttributes();
                    var matrix = new ColorMatrix { Matrix33 = GetAlpha(settings.WatermarkOpacity) / 255f };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    var imageRect = new Rectangle((watermark.Width - imageSize.Width) / 2, y, imageSize.Width, imageSize.Height);
                    graphics.DrawImage(sourceImage, imageRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);
                    y += imageSize.Height + (font is not null ? gap : 0);
                }

                if (font is not null)
                {
                    using var brush = new SolidBrush(Color.FromArgb(GetAlpha(settings.WatermarkOpacity), Color.White));
                    var textPoint = new PointF((watermark.Width - textSize.Width) / 2, y);
                    graphics.DrawString(settings.WatermarkText, font, brush, textPoint);
                }
            }

            switch (settings.WatermarkRotation)
            {
                case 90:
                    watermark.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 180:
                    watermark.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 270:
                    watermark.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }

            Point location = GetLocation(screenshot.Size, watermark.Size, settings.WatermarkPosition);
            targetGraphics.DrawImageUnscaled(watermark, location);
        }

        private static void DrawAnnotation(Graphics graphics, Bitmap screenshot, AnnotationSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.AnnotationFormat))
            {
                return;
            }

            string annotation = ResolveAnnotation(settings.AnnotationFormat);
            if (string.IsNullOrWhiteSpace(annotation))
            {
                return;
            }

            using var font = new Font(settings.AnnotationFontFamily, settings.AnnotationFontSize, settings.AnnotationFontStyle);
            using var brush = new SolidBrush(Color.FromArgb(GetAlpha(settings.AnnotationOpacity), Color.FromArgb(settings.AnnotationFontColorArgb)));
            using var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Far,
                FormatFlags = StringFormatFlags.NoWrap
            };

            const float margin = 24;
            var bounds = new RectangleF(0, 0, screenshot.Width, Math.Max(1, screenshot.Height - margin));
            if (settings.AnnotationBackgroundColorArgb.HasValue)
            {
                SizeF textSize = graphics.MeasureString(annotation, font, bounds.Size, format);
                const float horizontalPadding = 8;
                const float verticalPadding = 4;
                var backgroundBounds = new RectangleF(
                    (screenshot.Width - textSize.Width) / 2 - horizontalPadding,
                    screenshot.Height - margin - textSize.Height - verticalPadding,
                    textSize.Width + horizontalPadding * 2,
                    textSize.Height + verticalPadding * 2);
                using var backgroundBrush = new SolidBrush(Color.FromArgb(
                    GetAlpha(settings.AnnotationOpacity),
                    Color.FromArgb(settings.AnnotationBackgroundColorArgb.Value)));
                graphics.FillRectangle(backgroundBrush, backgroundBounds);
            }

            graphics.DrawString(annotation, font, brush, bounds, format);
        }

        public static string ResolveAnnotation(string format)
        {
            DateTimeOffset now = DateTimeOffset.Now;

            return format
                .Replace("{{DATE}}", now.ToString("yyyy-MM-dd"), StringComparison.Ordinal)
                .Replace("{{TIME}}", now.ToString("HH:mm:ss"), StringComparison.Ordinal)
                .Replace("{{DATETIME}}", now.ToString("yyyy-MM-dd HH:mm:ss"), StringComparison.Ordinal)
                .Replace("{{UTC}}", now.UtcDateTime.ToString("yyyy-MM-dd HH:mm:ss'Z'"), StringComparison.Ordinal)
                .Replace("{{TIMEZONE}}", TimeZoneInfo.Local.StandardName, StringComparison.Ordinal)
                .Replace("{{OFFSET}}", now.ToString("zzz"), StringComparison.Ordinal)
                .Replace("{{MILLISECONDS}}", now.ToString("fff"), StringComparison.Ordinal)
                .Replace("{{PCNAME}}", Environment.MachineName, StringComparison.Ordinal)
                .Replace("{{USER}}", Environment.UserName, StringComparison.Ordinal)
                .Replace("{{OS}}", Environment.OSVersion.VersionString, StringComparison.Ordinal)
                .Replace("{{APP}}", Application.ProductName, StringComparison.Ordinal)
                //.Replace("{{VERSION}}", Application.ProductVersion, StringComparison.Ordinal)
                .Replace("{{VERSION}}", Assembly.GetExecutingAssembly().GetName().Version.ToString(), StringComparison.Ordinal)
                .Replace("{{PID}}", Environment.ProcessId.ToString(), StringComparison.Ordinal);
        }

        private static Image? LoadImage(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            using var source = Image.FromFile(path);
            return new Bitmap(source);
        }

        private static Size GetImageSize(Image? image, Size screenshotSize, int imageScale)
        {
            if (image == null)
            {
                return Size.Empty;
            }

            int maxWidth = Math.Max(1, screenshotSize.Width / 4);
            int maxHeight = Math.Max(1, screenshotSize.Height / 4);
            double requestedScale = Math.Clamp(imageScale, Constants.WatermarkImageScaleMin, Constants.WatermarkImageScaleMax) / 100d;
            double scale = requestedScale * Math.Min(1d, Math.Min((double)maxWidth / image.Width, (double)maxHeight / image.Height));
            return new Size(Math.Max(1, (int)(image.Width * scale)), Math.Max(1, (int)(image.Height * scale)));
        }

        private static int GetAlpha(int opacity) => (int)Math.Round(255 * Math.Clamp(opacity, 1, 100) / 100d);

        private static Point GetLocation(Size canvas, Size watermark, string position)
        {
            const int margin = 24;
            int x = position.Contains("Left", StringComparison.OrdinalIgnoreCase)
                ? margin
                : position.Contains("Right", StringComparison.OrdinalIgnoreCase)
                    ? canvas.Width - watermark.Width - margin
                    : (canvas.Width - watermark.Width) / 2;
            int y = position.StartsWith("Top", StringComparison.OrdinalIgnoreCase)
                ? margin
                : position.StartsWith("Bottom", StringComparison.OrdinalIgnoreCase)
                    ? canvas.Height - watermark.Height - margin
                    : (canvas.Height - watermark.Height) / 2;
            return new Point(Math.Max(0, x), Math.Max(0, y));
        }
    }
}