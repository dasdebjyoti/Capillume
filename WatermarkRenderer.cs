using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Capillume
{
    public static class WatermarkRenderer
    {
        public static void Apply(Bitmap screenshot, WatermarkSettings settings)
        {
            if (!settings.Enabled || (!settings.UseText && !settings.UseImage))
            {
                return;
            }

            using var font = new Font(settings.WatermarkTextFontFamily, settings.WatermarkTextFontSize, settings.WatermarkTextFontStyle);
            using var sourceImage = settings.UseImage ? LoadImage(settings.WatermarkImagePath) : null;
            using var measureGraphics = Graphics.FromImage(screenshot);

            const int padding = 16;
            const int gap = 12;
            SizeF textSize = settings.UseText
                ? measureGraphics.MeasureString(settings.WatermarkText, font)
                : SizeF.Empty;
            Size imageSize = GetImageSize(sourceImage, screenshot.Size, settings.WatermarkImageScale);
            int width = (int)Math.Ceiling(Math.Max(textSize.Width, imageSize.Width)) + padding * 2;
            int height = (int)Math.Ceiling(textSize.Height + (settings.UseText && settings.UseImage ? gap : 0) + imageSize.Height) + padding * 2;

            using var watermark = new Bitmap(Math.Max(width, 1), Math.Max(height, 1), PixelFormat.Format32bppArgb);
            using (var graphics = Graphics.FromImage(watermark))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                int y = padding;
                //if (settings.UseText)
                //{
                //    using var brush = new SolidBrush(Color.FromArgb(GetAlpha(settings.WatermarkOpacity), Color.White));
                //    var textPoint = new PointF((watermark.Width - textSize.Width) / 2, y);
                //    graphics.DrawString(settings.WatermarkText, font, brush, textPoint);
                //    y += (int)Math.Ceiling(textSize.Height) + (settings.UseImage ? gap : 0);
                //}

                //if (settings.UseImage && sourceImage != null)
                //{
                //    using var attributes = new ImageAttributes();
                //    var matrix = new ColorMatrix { Matrix33 = settings.WatermarkOpacity / 100f };
                //    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                //    var imageRect = new Rectangle((watermark.Width - imageSize.Width) / 2, y, imageSize.Width, imageSize.Height);
                //    graphics.DrawImage(sourceImage, imageRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);
                //}

                if (settings.UseImage && sourceImage != null)
                {
                    using var attributes = new ImageAttributes();
                    var matrix = new ColorMatrix { Matrix33 = settings.WatermarkOpacity / 100f };
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                    var imageRect = new Rectangle((watermark.Width - imageSize.Width) / 2, y, imageSize.Width, imageSize.Height);
                    graphics.DrawImage(sourceImage, imageRect, 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel, attributes);
                    y += (int)Math.Ceiling((float)imageSize.Height) + (settings.UseText ? gap : 0);
                }

                if (settings.UseText)
                {
                    using var brush = new SolidBrush(Color.FromArgb(GetAlpha(settings.WatermarkOpacity), Color.White));
                    var textPoint = new PointF((watermark.Width - textSize.Width) / 2, y);
                    graphics.DrawString(settings.WatermarkText, font, brush, textPoint);
                    //y += (int)Math.Ceiling(textSize.Height) + (settings.UseImage ? gap : 0);
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
            using var targetGraphics = Graphics.FromImage(screenshot);
            targetGraphics.CompositingMode = CompositingMode.SourceOver;
            targetGraphics.DrawImageUnscaled(watermark, location);
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