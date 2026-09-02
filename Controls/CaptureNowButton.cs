using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Capillume
{
    public class CaptureNowButton : Control
    {
        private bool _isHovered;
        private bool _isPressed;
        private float _pressProgress;
        private System.Windows.Forms.Timer? _pressAnimationTimer;
        private Image? _image;
        private Color _backColor = Color.FromArgb(0, 120, 212);
        private Color _hoverBackColor = Color.FromArgb(16, 110, 190);
        private Color _pressedBackColor = Color.FromArgb(0, 92, 158);
        private Color _textColor = Color.White;
        private int _cornerRadius = 10;

        public CaptureNowButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.Selectable, true);

            TabStop = true;
            Cursor = Cursors.Hand;
            AccessibleRole = AccessibleRole.PushButton;
            AccessibleName = "Capture screenshot now";
            Text = "Capture Now";
            Font = new Font("Segoe UI Semibold", 10F);
            Size = new Size(190, 54);
            Image = FallbackIcon.CreateAppImageAdvanced();
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Image? Image
        {
            get => _image;
            set
            {
                if (ReferenceEquals(_image, value))
                {
                    return;
                }

                _image?.Dispose();
                _image = value;
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color ButtonBackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color HoverBackColor
        {
            get => _hoverBackColor;
            set
            {
                _hoverBackColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color PressedBackColor
        {
            get => _pressedBackColor;
            set
            {
                _pressedBackColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color ButtonTextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public int CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = Math.Max(0, value);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle bounds = new(1, 1, Width - 2, Height - 2);
            int radius = Math.Min(_cornerRadius, Math.Min(bounds.Width, bounds.Height) / 2);

            using GraphicsPath path = CreateRoundedPath(bounds, radius);
            Color background = !Enabled
                ? Color.FromArgb(210, 210, 210)
                : _isHovered ? _hoverBackColor : _backColor;
            if (Enabled && _pressProgress > 0)
            {
                background = BlendColors(background, _pressedBackColor, _pressProgress);
            }

            using (var backgroundBrush = new SolidBrush(background))
            {
                e.Graphics.FillPath(backgroundBrush, path);
            }

            int imageSize = Math.Max(1, Math.Min(Width, Height) / 2);
            string displayText = GetDisplayText();
            bool hasText = !string.IsNullOrEmpty(displayText);
            int contentWidth = imageSize + (hasText ? 10 + (int)e.Graphics.MeasureString(displayText, Font).Width : 0);
            int contentX = Math.Max(8, (Width - contentWidth) / 2);
            Rectangle imageBounds = new(contentX, (Height - imageSize) / 2, imageSize, imageSize);

            if (_image is not null)
            {
                Rectangle imageBackdrop = Rectangle.Inflate(imageBounds, 3, 3);
                using (var imageBackdropBrush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillEllipse(imageBackdropBrush, imageBackdrop);
                }

                using var attributes = new ImageAttributes();
                if (!Enabled)
                {
                    float[][] matrix =
                    {
                        new[] { 0.7f, 0.0f, 0.0f, 0.0f, 0.0f },
                        new[] { 0.0f, 0.7f, 0.0f, 0.0f, 0.0f },
                        new[] { 0.0f, 0.0f, 0.7f, 0.0f, 0.0f },
                        new[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
                        new[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
                    };
                    attributes.SetColorMatrix(new ColorMatrix(matrix));
                    e.Graphics.DrawImage(_image, imageBounds, 0, 0, _image.Width, _image.Height, GraphicsUnit.Pixel, attributes);
                }
                else
                {
                    e.Graphics.DrawImage(_image, imageBounds);
                }
            }

            if (hasText)
            {
                int textX = _image is null ? contentX : imageBounds.Right + 10;
                Rectangle textBounds = new(textX, 0, Width - textX - contentX, Height);
                Color textColor = Enabled ? _textColor : Color.FromArgb(120, 120, 120);
                TextFormatFlags textFlags = TextFormatFlags.Left |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPadding |
                    TextFormatFlags.EndEllipsis;

                if (!ShowKeyboardCues)
                {
                    textFlags |= TextFormatFlags.NoPrefix;
                }

                string renderedText = ShowKeyboardCues ? Text : displayText;
                TextRenderer.DrawText(e.Graphics, renderedText, Font, textBounds, textColor, textFlags);
            }

            if (Focused && ShowFocusCues)
            {
                Rectangle focusBounds = Rectangle.Inflate(bounds, -5, -5);
                using var focusPen = new Pen(Color.FromArgb(180, Color.White), 1) { DashStyle = DashStyle.Dot };
                using GraphicsPath focusPath = CreateRoundedPath(focusBounds, Math.Max(0, radius - 4));
                e.Graphics.DrawPath(focusPen, focusPath);
            }
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            if (CanSelect && IsMnemonic(charCode, Text))
            {
                Focus();
                OnClick(EventArgs.Empty);
                return true;
            }

            return base.ProcessMnemonic(charCode);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            EndPressAnimation();
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                BeginPressAnimation();
                Focus();
                Invalidate();
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            EndPressAnimation();
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Enter or Keys.Space)
            {
                BeginPressAnimation();
                Invalidate();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode is Keys.Enter or Keys.Space)
            {
                EndPressAnimation();
                Invalidate();
                OnClick(EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            base.OnKeyUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            EndPressAnimation();
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pressAnimationTimer?.Dispose();
                _image?.Dispose();
                _pressAnimationTimer = null;
                _image = null;
            }

            base.Dispose(disposing);
        }

        private void BeginPressAnimation()
        {
            _isPressed = true;
            AnimatePressTo(1F);
        }

        private void EndPressAnimation()
        {
            _isPressed = false;
            AnimatePressTo(0F);
        }

        private void AnimatePressTo(float target)
        {
            _pressAnimationTimer?.Stop();
            _pressAnimationTimer?.Dispose();

            _pressAnimationTimer = new System.Windows.Forms.Timer { Interval = 15 };
            _pressAnimationTimer.Tick += (sender, e) =>
            {
                float difference = target - _pressProgress;
                if (Math.Abs(difference) < 0.05F)
                {
                    _pressProgress = target;
                    _pressAnimationTimer?.Stop();
                }
                else
                {
                    _pressProgress += difference * 0.35F;
                }

                Invalidate();
            };
            _pressAnimationTimer.Start();
        }

        private static Color BlendColors(Color first, Color second, float amount)
        {
            amount = Math.Clamp(amount, 0F, 1F);
            return Color.FromArgb(
                (int)(first.R + ((second.R - first.R) * amount)),
                (int)(first.G + ((second.G - first.G) * amount)),
                (int)(first.B + ((second.B - first.B) * amount)));
        }

        private string GetDisplayText()
        {
            return Text.Replace("&&", "\0").Replace("&", string.Empty).Replace('\0', '&');
        }

        private static GraphicsPath CreateRoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int diameter = radius * 2;
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
