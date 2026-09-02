using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Capillume
{
    internal static class FallbackIcon
    {
        public static Icon CreateAppIcon()
        {
            // Create a simple icon (camera-like representation)
            var bitmap = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw a simple camera shape
                using (var brush = new SolidBrush(Color.FromArgb(0, 120, 215)))
                {
                    g.FillRectangle(brush, 6, 10, 20, 14);
                    g.FillEllipse(brush, 12, 12, 8, 8);
                }

                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(pen, 13, 13, 6, 6);
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            return Icon.FromHandle(hIcon);
        }

        public static Icon CreateAppIconAdvanced()
        {
            using var bmp = CreateAppImageAdvanced();
            return Icon.FromHandle(bmp.GetHicon());
        }

        public static Bitmap CreateAppImageAdvanced()
        {
            var bmp = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Colors
                Color bodyColor = Color.FromArgb(0, 120, 215);
                Color lensColor = Color.FromArgb(30, 30, 30);
                Color highlightColor = Color.White;

                // --- Camera body ---
                using (var bodyBrush = new SolidBrush(bodyColor))
                {
                    // Main rectangle
                    //g.FillRoundedRectangle(bodyBrush, new Rectangle(4, 10, 24, 16), 4);
                    // Top bump (viewfinder)
                    //g.FillRoundedRectangle(bodyBrush, new Rectangle(10, 6, 12, 6), 3);

                    g.FillRoundedRectangle(bodyBrush, new Rectangle(4, 10, 24, 16), new Size(4, 4));
                    g.FillRoundedRectangle(bodyBrush, new Rectangle(10, 6, 12, 6), new Size(3, 3));

                }

                // --- Lens ---
                using (var lensBrush = new SolidBrush(lensColor))
                {
                    g.FillEllipse(lensBrush, 11, 12, 10, 10);
                }

                // --- Lens ring ---
                using (var ringPen = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(ringPen, 12, 13, 8, 8);
                }

                // --- Lens highlight ---
                using (var highlightBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255)))
                {
                    g.FillEllipse(highlightBrush, 14, 14, 3, 3);
                }
            }

            return bmp;
        }

    }

    public static class GraphicsExtensions
    {
        public static void FillRoundedRectangle(this Graphics g, Brush brush, Rectangle rect, Size radius)
        {
            using (var path = new GraphicsPath())
            {
                int rX = radius.Width;
                int rY = radius.Height;

                path.AddArc(rect.X, rect.Y, rX, rY, 180, 90);
                path.AddArc(rect.Right - rX, rect.Y, rX, rY, 270, 90);
                path.AddArc(rect.Right - rX, rect.Bottom - rY, rX, rY, 0, 90);
                path.AddArc(rect.X, rect.Bottom - rY, rX, rY, 90, 90);
                path.CloseFigure();

                g.FillPath(brush, path);
            }
        }
    }
}
