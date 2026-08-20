using System.Drawing.Drawing2D;

namespace Capilume
{
    public class ToggleSwitch : Control
    {
        private bool _checked = false;
        private Color _onColor = Color.FromArgb(0, 120, 212);
        private Color _offColor = Color.FromArgb(200, 200, 200);
        private Color _thumbColor = Color.White;
        private float _thumbPosition = 0;
        private System.Windows.Forms.Timer? _animationTimer;
        private const int AnimationStep = 5;

        public event EventHandler? CheckedChanged;

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.UserPaint | 
                     ControlStyles.ResizeRedraw | 
                     ControlStyles.OptimizedDoubleBuffer, true);

            Size = new Size(50, 25);
            Cursor = Cursors.Hand;
        }

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Gets or sets whether the toggle is checked")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked != value)
                {
                    _checked = value;
                    AnimateToggle();
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Color when toggle is ON")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color OnColor
        {
            get => _onColor;
            set
            {
                _onColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Color when toggle is OFF")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color OffColor
        {
            get => _offColor;
            set
            {
                _offColor = value;
                Invalidate();
            }
        }

        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Color of the sliding thumb")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public Color ThumbColor
        {
            get => _thumbColor;
            set
            {
                _thumbColor = value;
                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = !Checked;
        }

        private void AnimateToggle()
        {
            _animationTimer?.Stop();
            _animationTimer?.Dispose();

            _animationTimer = new System.Windows.Forms.Timer { Interval = 10 };
            _animationTimer.Tick += (s, e) =>
            {
                float targetPosition = _checked ? Width - Height + 4 : 4;
                float difference = targetPosition - _thumbPosition;

                if (Math.Abs(difference) < 1)
                {
                    _thumbPosition = targetPosition;
                    _animationTimer?.Stop();
                }
                else
                {
                    _thumbPosition += difference * 0.3f;
                }

                Invalidate();
            };

            _animationTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Initialize thumb position if needed
            if (_thumbPosition == 0 && !_checked)
            {
                _thumbPosition = 4;
            }
            else if (_thumbPosition == 0 && _checked)
            {
                _thumbPosition = Width - Height + 4;
            }

            // Draw background track
            Color currentColor = _checked ? _onColor : _offColor;
            using (var trackPath = GetRoundedRectPath(new RectangleF(0, 0, Width, Height), Height / 2f))
            using (var trackBrush = new SolidBrush(currentColor))
            {
                g.FillPath(trackBrush, trackPath);
            }

            // Draw thumb (circle)
            int thumbSize = Height - 8;
            float thumbY = 4;
            using (var thumbPath = GetRoundedRectPath(
                new RectangleF(_thumbPosition, thumbY, thumbSize, thumbSize), 
                thumbSize / 2f))
            using (var thumbBrush = new SolidBrush(_thumbColor))
            {
                // Draw shadow
                using (var shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                {
                    g.FillEllipse(shadowBrush, 
                        _thumbPosition + 1, thumbY + 1, thumbSize, thumbSize);
                }

                g.FillPath(thumbBrush, thumbPath);
            }
        }

        private GraphicsPath GetRoundedRectPath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Update thumb position when resized
            if (_checked)
            {
                _thumbPosition = Width - Height + 4;
            }
            else
            {
                _thumbPosition = 4;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer?.Stop();
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
