using System.Reflection;

namespace Capillume
{
    public partial class FormSettings : Form
    {
        private const string WatermarkPlaceholderText = "Enter text here";

        private static readonly string[] AnnotationFields =
        [
            "{{TIME}}",
            "{{DATE}}",
            "{{DATETIME}}",
            "{{UTC}}",
            "{{TIMEZONE}}",
            "{{OFFSET}}",
            "{{MILLISECONDS}}",
            "{{PCNAME}}",
            "{{USER}}",
            "{{OS}}",
            "{{APP}}",
            "{{VERSION}}",
            "{{PID}}"
        ];

        private static readonly string[] AnnotationFormats =
        [
            Constants.AnnotationFormatDefault,
            "{{DATETIME}}",
            "{{UTC}}",
            "{{APP}} {{VERSION}} | {{DATETIME}}",
            "{{PCNAME}} / {{USER}} | {{DATETIME}}",
            "{{DATE}} {{TIME}} | {{PCNAME}} / {{USER}}"
        ];

        private static readonly (string Name, Color Color)[] AnnotationBackgroundColors =
        [
            // --- Grays ---
            ("Black", Color.Black),
            ("DarkGray", Color.DarkGray),
            ("Gray", Color.Gray),
            ("LightGray", Color.LightGray),
            ("White", Color.White),

            // --- Reds / Oranges / Yellows ---
            ("DarkRed", Color.DarkRed),
            ("Red", Color.Red),
            ("Orange", Color.Orange),
            ("Goldenrod", Color.Goldenrod),
            ("Yellow", Color.Yellow),
            ("LightYellow", Color.LightYellow),
            ("LightCoral", Color.LightCoral),
            ("LightPink", Color.LightPink),

            // --- Greens (Windows 11 adds soft greens) ---
            ("DarkGreen", Color.DarkGreen),
            ("Green", Color.Green),
            ("MediumSeaGreen", Color.MediumSeaGreen),
            ("LightGreen", Color.LightGreen),
            ("MintCream", Color.MintCream),
            ("Honeydew", Color.Honeydew),              // Windows 11 soft green
            ("PaleGreen", Color.PaleGreen),            // gentle pastel green

            // --- Blues (Windows 11 uses calm blues) ---
            ("DarkBlue", Color.DarkBlue),
            ("Blue", Color.Blue),
            ("RoyalBlue", Color.RoyalBlue),
            ("SteelBlue", Color.SteelBlue),
            ("CornflowerBlue", Color.CornflowerBlue),
            ("LightBlue", Color.LightBlue),
            ("Aqua", Color.Aqua),
            ("LightSteelBlue", Color.LightSteelBlue),  // Windows 11 soft blue
            ("AliceBlue", Color.AliceBlue),            // very light pastel blue
            ("Azure", Color.Azure),                    // modern UI tone

            // --- Purples / Magentas (Windows 11 uses soft purples) ---
            ("Purple", Color.Purple),
            ("DeepPink", Color.DeepPink),
            ("Magenta", Color.Magenta),
            ("MediumOrchid", Color.MediumOrchid),      // modern purple
            ("Orchid", Color.Orchid),
            ("MediumPurple", Color.MediumPurple),
            ("Lavender", Color.Lavender),
            ("Thistle", Color.Thistle),                // soft pastel purple
            ("GhostWhite", Color.GhostWhite),          // Windows 11 subtle purple-white

            // --- Neutrals / Soft Warm Tones ---
            ("Teal", Color.Teal),
            ("Beige", Color.Beige),
            ("AntiqueWhite", Color.AntiqueWhite),      // warm modern neutral
            ("FloralWhite", Color.FloralWhite),        // soft warm white
            ("Seashell", Color.SeaShell),              // Windows 11 warm pastel
            ("OldLace", Color.OldLace)                 // elegant warm tone
        ];

        private readonly WatermarkSettings _originalWatermarkSettings;
        private readonly AnnotationSettings _originalAnnotationSettings;

        private readonly WatermarkSettings _watermarkSettings;
        private readonly AnnotationSettings _annotationSettings;

        private Icon? _appIcon;
        private Font _watermarkFont = new("Segoe UI", 24);
        private Font _annotationFont = new("Segoe UI", 24);
        private Color _annotationFontColor = Color.White;
        private Color? _annotationBackgroundColor;
        private int _annotationSelectionStart;
        private int _annotationSelectionLength;

        public WatermarkSettings WatermarkSettings => _watermarkSettings;
        public AnnotationSettings AnnotationSettings => _annotationSettings;

        public bool WatermarkSettingsChanged => !AreEqual(_originalWatermarkSettings, _watermarkSettings);
        public bool AnnotationSettingsChanged => !AreEqual(_originalAnnotationSettings, _annotationSettings);

        public FormSettings(WatermarkSettings watermarkSettings, AnnotationSettings annotationSettings)
        {
            InitializeComponent();

            _originalWatermarkSettings = Clone(watermarkSettings);
            _originalAnnotationSettings = Clone(annotationSettings);
            _watermarkSettings = Clone(watermarkSettings);
            _annotationSettings = Clone(annotationSettings);

            InitializeIcon();
            InitializeWatermarkTab();
            InitializeAnnotationTab();
        }

        private void InitializeIcon()
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Load icon for the system tray and form
            try
            {
                using var stream = assembly.GetManifestResourceStream("Capillume.icon.ico");
                if (stream != null)
                {
                    _appIcon = new Icon(stream);
                    Icon = _appIcon;
                    return;
                }
            }
            catch
            {
            }

            _appIcon = FallbackIcon.CreateAppIconAdvanced();
            Icon = _appIcon;
        }

        private void InitializeWatermarkTab()
        {
            // Load settings into UI
            _watermarkFont.Dispose();
            _watermarkFont = new Font(
                _watermarkSettings.WatermarkTextFontFamily,
                _watermarkSettings.WatermarkTextFontSize,
                _watermarkSettings.WatermarkTextFontStyle);

            wmToggleUseText.Checked = _watermarkSettings.UseText;
            wmToggleUseImage.Checked = _watermarkSettings.UseImage;
            wmTextBoxWatermarkText.Text = _watermarkSettings.WatermarkText;
            wmTextBoxWatermarkImagePath.Text = _watermarkSettings.WatermarkImagePath;

            wmLabelFontDescription.Text = $"{_watermarkFont.Name}, {_watermarkFont.SizeInPoints:0.#} pt";

            wmTrackBarWatermarkImageScale.Value = Math.Clamp(
                _watermarkSettings.WatermarkImageScale,
                Constants.WatermarkImageScaleMin,
                Constants.WatermarkImageScaleMax);
            wmLabelWatermarkImageScaleValue.Text = $"{wmTrackBarWatermarkImageScale.Value}%";

            wmTrackBarOpacity.Value = Math.Clamp(
                _watermarkSettings.WatermarkOpacity,
                Constants.WatermarkOpacityMin,
                Constants.WatermarkOpacityMax);
            wmLabelOpacityValue.Text = $"{wmTrackBarOpacity.Value}%";

            wmComboBoxWatermarkPosition.SelectedItem = _watermarkSettings.WatermarkPosition;
            if (wmComboBoxWatermarkPosition.SelectedIndex < 0)
            {
                wmComboBoxWatermarkPosition.SelectedItem = Constants.WatermarkPositionDefault;
            }

            wmComboBoxWatermarkRotation.SelectedIndex = Math.Clamp(_watermarkSettings.WatermarkRotation / 90, 0, 3);

            SetupWatermarkTextboxPlaceholder(wmTextBoxWatermarkText);
            UpdateWatermarkImagePreview();
            UpdateWatermarkControlState();
        }

        private void InitializeAnnotationTab()
        {
            // Load settings into UI
            _annotationFont.Dispose();
            _annotationFont = new Font(
                _annotationSettings.AnnotationFontFamily,
                _annotationSettings.AnnotationFontSize,
                _annotationSettings.AnnotationFontStyle);

            _annotationFontColor = Color.FromArgb(_annotationSettings.AnnotationFontColorArgb);
            _annotationBackgroundColor = _annotationSettings.AnnotationBackgroundColorArgb.HasValue
                ? Color.FromArgb(_annotationSettings.AnnotationBackgroundColorArgb.Value)
                : null;

            anToggleUseAnnotation.Checked = _annotationSettings.UseAnnotation;
            anComboBoxAnnotationFormat.BeginUpdate();
            anComboBoxAnnotationFormat.Items.Clear();
            anComboBoxAnnotationFormat.Items.AddRange(AnnotationFormats);
            if (!string.IsNullOrWhiteSpace(_annotationSettings.AnnotationFormat)
                && !anComboBoxAnnotationFormat.Items.Contains(_annotationSettings.AnnotationFormat))
            {
                anComboBoxAnnotationFormat.Items.Add(_annotationSettings.AnnotationFormat);
            }

            anComboBoxAnnotationFormat.Text = _annotationSettings.AnnotationFormat;
            anComboBoxAnnotationFormat.EndUpdate();

            anTrackBarOpacity.Value = Math.Clamp(
                _annotationSettings.AnnotationOpacity,
                Constants.AnnotationOpacityMin,
                Constants.AnnotationOpacityMax);
            anLabelOpacityValue.Text = $"{anTrackBarOpacity.Value}%";

            UpdateAnnotationBackgroundButton();
            UpdateAnnotationPreview();
            UpdateAnnotationControlState();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            tabControlSettings.SelectedTab = tabPageWatermark;
            if (!TryApplyWatermarkSettings())
            {
                return;
            }

            tabControlSettings.SelectedTab = tabPageAnnotation;
            if (!TryApplyAnnotationSettings())
            {
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool TryApplyWatermarkSettings()
        {
            if (wmToggleUseText.Checked &&
                (string.IsNullOrWhiteSpace(wmTextBoxWatermarkText.Text) || wmTextBoxWatermarkText.Text == WatermarkPlaceholderText))
            {
                MessageBox.Show("Enter watermark text.", "Watermark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                wmTextBoxWatermarkText.Focus();
                return false;
            }

            if (wmToggleUseImage.Checked && !File.Exists(wmTextBoxWatermarkImagePath.Text))
            {
                MessageBox.Show("Select a valid watermark image.", "Watermark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            _watermarkSettings.UseText = wmToggleUseText.Checked;
            _watermarkSettings.UseImage = wmToggleUseImage.Checked;
            _watermarkSettings.WatermarkText = wmTextBoxWatermarkText.Text.Trim();
            _watermarkSettings.WatermarkTextFontFamily = _watermarkFont.FontFamily.Name;
            _watermarkSettings.WatermarkTextFontSize = _watermarkFont.SizeInPoints;
            _watermarkSettings.WatermarkTextFontStyle = _watermarkFont.Style;
            _watermarkSettings.WatermarkImagePath = wmTextBoxWatermarkImagePath.Text;
            _watermarkSettings.WatermarkImageScale = wmTrackBarWatermarkImageScale.Value;
            _watermarkSettings.WatermarkOpacity = wmTrackBarOpacity.Value;
            _watermarkSettings.WatermarkPosition = wmComboBoxWatermarkPosition.SelectedItem?.ToString() ?? Constants.WatermarkPositionDefault;
            _watermarkSettings.WatermarkRotation = wmComboBoxWatermarkRotation.SelectedIndex * 90;

            return true;
        }

        private bool TryApplyAnnotationSettings()
        {
            if (anToggleUseAnnotation.Checked && string.IsNullOrWhiteSpace(anComboBoxAnnotationFormat.Text))
            {
                MessageBox.Show("Enter annotation text.", "Annotation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                anComboBoxAnnotationFormat.Focus();
                return false;
            }

            _annotationSettings.UseAnnotation = anToggleUseAnnotation.Checked;
            _annotationSettings.AnnotationFormat = anComboBoxAnnotationFormat.Text.Trim();
            _annotationSettings.AnnotationFontFamily = _annotationFont.FontFamily.Name;
            _annotationSettings.AnnotationFontSize = _annotationFont.SizeInPoints;
            _annotationSettings.AnnotationFontStyle = _annotationFont.Style;
            _annotationSettings.AnnotationFontColorArgb = _annotationFontColor.ToArgb();
            _annotationSettings.AnnotationBackgroundColorArgb = _annotationBackgroundColor?.ToArgb();
            _annotationSettings.AnnotationOpacity = anTrackBarOpacity.Value;

            return true;
        }

        private void WmToggleUseText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWatermarkControlState();
        }

        private void WmToggleUseImage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWatermarkControlState();
        }

        private void WmButtonChooseFont_Click(object sender, EventArgs e)
        {
            using var dialog = new FontDialog { Font = _watermarkFont, ShowColor = false, AllowVectorFonts = true, FontMustExist = true };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _watermarkFont.Dispose();
                _watermarkFont = (Font)dialog.Font.Clone();
                wmLabelFontDescription.Text = $"{_watermarkFont.Name}, {_watermarkFont.SizeInPoints:0.#} pt";
                float originalSize = wmLabelFontDescription.Font.Size;
                wmLabelFontDescription.Font = new Font(_watermarkFont.FontFamily, originalSize, _watermarkFont.Style);
            }
        }

        private void WmButtonBrowseWatermarkImage_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog { Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files|*.*" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                wmTextBoxWatermarkImagePath.Text = dialog.FileName;
            }
        }

        private void WmTrackBarWatermarkImageScale_ValueChanged(object sender, EventArgs e)
        {
            wmLabelWatermarkImageScaleValue.Text = $"{wmTrackBarWatermarkImageScale.Value}%";
        }

        private void WmTrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            wmLabelOpacityValue.Text = $"{wmTrackBarOpacity.Value}%";
        }

        private void WmTextBoxWatermarkImagePath_TextChanged(object sender, EventArgs e)
        {
            UpdateWatermarkImagePreview();
        }

        private void AnToggleUseAnnotation_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAnnotationControlState();
        }

        private void AnComboBoxAnnotationFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateAnnotationPreview();
        }

        private void AnTrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            anLabelOpacityValue.Text = $"{anTrackBarOpacity.Value}%";
        }

        private void AnButtonAnnotationFont_Click(object sender, EventArgs e)
        {
            using var dialog = new FontDialog
            {
                Font = _annotationFont,
                Color = _annotationFontColor,
                ShowColor = true,
                AllowVectorFonts = true,
                FontMustExist = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _annotationFont.Dispose();
                _annotationFont = (Font)dialog.Font.Clone();
                _annotationFontColor = dialog.Color;
                UpdateAnnotationPreview();
            }
        }

        private void AnButtonAnnotationBackgroundColor_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip(components);
            var noColorItem = new ToolStripMenuItem("No color")
            {
                Tag = !_annotationBackgroundColor.HasValue
            };
            noColorItem.Paint += DrawCurrentColorBorder;
            noColorItem.Click += (_, _) => SetAnnotationBackgroundColor(null);
            menu.Items.Add(noColorItem);
            menu.Items.Add(new ToolStripSeparator());

            foreach ((string name, Color color) in AnnotationBackgroundColors)
            {
                double luminance = (0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) / 255.0;
                var colorItem = new ToolStripMenuItem(name)
                {
                    BackColor = color,
                    ForeColor = luminance < 0.5 ? Color.White : Color.Black,
                    Tag = _annotationBackgroundColor?.ToArgb() == color.ToArgb()
                };
                colorItem.Paint += DrawCurrentColorBorder;
                colorItem.Click += (_, _) => SetAnnotationBackgroundColor(color);
                menu.Items.Add(colorItem);
            }

            menu.Show(anButtonAnnotationBackgroundColor, anButtonAnnotationBackgroundColor.Width, 0);
        }

        private void AnButtonAnnotationFields_Click(object sender, EventArgs e)
        {
            _annotationSelectionStart = anComboBoxAnnotationFormat.SelectionStart;
            _annotationSelectionLength = anComboBoxAnnotationFormat.SelectionLength;

            var menu = new ContextMenuStrip(components);
            foreach (string field in AnnotationFields)
            {
                var menuItem = new ToolStripMenuItem(field);
                menuItem.Click += (_, _) => InsertAnnotationField(field);
                menu.Items.Add(menuItem);
            }

            menu.Show(anButtonAnnotationFields, anButtonAnnotationFields.Width, 0);
        }

        private static void DrawCurrentColorBorder(object? sender, PaintEventArgs e)
        {
            if (sender is ToolStripItem { Tag: true } item)
            {
                double luminance = (0.2126 * item.BackColor.R + 0.7152 * item.BackColor.G + 0.0722 * item.BackColor.B) / 255.0;
                Color borderColor = luminance < 0.5 ? Color.White : Color.Black;
                using var pen = new Pen(borderColor, 5);
                Rectangle borderBounds = new(1, 1, item.Width - 3, item.Height - 3);
                e.Graphics.DrawRectangle(pen, borderBounds);
            }
        }

        private void InsertAnnotationField(string field)
        {
            anComboBoxAnnotationFormat.Text = anComboBoxAnnotationFormat.Text.Remove(
                _annotationSelectionStart,
                _annotationSelectionLength).Insert(_annotationSelectionStart, field);

            anComboBoxAnnotationFormat.SelectionStart = _annotationSelectionStart + field.Length;
            anComboBoxAnnotationFormat.SelectionLength = 0;
            anComboBoxAnnotationFormat.Focus();
        }

        private void SetAnnotationBackgroundColor(Color? color)
        {
            _annotationBackgroundColor = color;
            UpdateAnnotationBackgroundButton();
            UpdateAnnotationPreview();
        }

        private void UpdateWatermarkControlState()
        {
            bool textEnabled = wmToggleUseText.Checked;
            bool imageEnabled = wmToggleUseImage.Checked;
            bool commonEnabled = textEnabled || imageEnabled;

            wmTextBoxWatermarkText.Enabled = textEnabled;
            wmButtonChooseFont.Enabled = textEnabled;
            wmLabelFontDescription.Enabled = textEnabled;
            wmTextBoxWatermarkImagePath.Enabled = imageEnabled;
            wmButtonBrowseWatermarkImage.Enabled = imageEnabled;
            wmTrackBarWatermarkImageScale.Enabled = imageEnabled;
            wmLabelWatermarkImageScaleValue.Enabled = imageEnabled;
            wmLabelOpacity.Enabled = commonEnabled;
            wmTrackBarOpacity.Enabled = commonEnabled;
            wmLabelOpacityValue.Enabled = commonEnabled;
            wmLabelWatermarkPosition.Enabled = commonEnabled;
            wmComboBoxWatermarkPosition.Enabled = commonEnabled;
            wmLabelWatermarkRotation.Enabled = commonEnabled;
            wmComboBoxWatermarkRotation.Enabled = commonEnabled;
        }

        private void UpdateAnnotationControlState()
        {
            bool enabled = anToggleUseAnnotation.Checked;
            anLabelAnnotationFormat.Enabled = enabled;
            anComboBoxAnnotationFormat.Enabled = enabled;
            anButtonAnnotationFields.Enabled = enabled;
            anButtonAnnotationFont.Enabled = enabled;
            anButtonAnnotationBackgroundColor.Enabled = enabled;
            anLabelAnnotationSample.Enabled = enabled;
            anLabelOpacity.Enabled = enabled;
            anTrackBarOpacity.Enabled = enabled;
            anLabelOpacityValue.Enabled = enabled;
        }

        private void UpdateAnnotationBackgroundButton()
        {
            if (_annotationBackgroundColor != null)
            {
                double luminance =
                    (0.2126 * _annotationBackgroundColor.Value.R +
                     0.7152 * _annotationBackgroundColor.Value.G +
                     0.0722 * _annotationBackgroundColor.Value.B) / 255.0;

                anButtonAnnotationBackgroundColor.ForeColor = luminance < 0.5 ? Color.White : Color.Black;
                anButtonAnnotationBackgroundColor.BackColor = _annotationBackgroundColor ?? Color.Transparent;
            }
            else
            {
                anButtonAnnotationBackgroundColor.ForeColor = SystemColors.ControlText;
                anButtonAnnotationBackgroundColor.BackColor = Color.White;
            }
        }

        private void UpdateAnnotationPreview()
        {
            string format = string.IsNullOrWhiteSpace(anComboBoxAnnotationFormat.Text)
                ? Constants.AnnotationFormatDefault
                : anComboBoxAnnotationFormat.Text;

            anLabelAnnotationSample.Text = WatermarkRenderer.ResolveAnnotation(format);
            float originalSize = anLabelAnnotationSample.Font.Size;
            anLabelAnnotationSample.BackColor = _annotationBackgroundColor ?? Color.Transparent;
            anLabelAnnotationSample.ForeColor = _annotationFontColor;
            anLabelAnnotationSample.Font = new Font(_annotationFont.FontFamily, originalSize, _annotationFont.Style);
        }

        private void SetupWatermarkTextboxPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Font = new Font(textBox.Font, FontStyle.Italic);
                textBox.Text = WatermarkPlaceholderText;
            }

            textBox.GotFocus += (_, _) =>
            {
                if (textBox.Text == WatermarkPlaceholderText)
                {
                    textBox.Text = string.Empty;
                    textBox.ForeColor = SystemColors.WindowText;
                    textBox.Font = new Font(textBox.Font, FontStyle.Regular);
                }
            };

            textBox.LostFocus += (_, _) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = WatermarkPlaceholderText;
                    textBox.ForeColor = Color.Gray;
                    textBox.Font = new Font(textBox.Font, FontStyle.Italic);
                }
            };
        }

        private void UpdateWatermarkImagePreview()
        {
            Image? image = null;

            try
            {
                if (File.Exists(wmTextBoxWatermarkImagePath.Text))
                {
                    using var loadedImage = Image.FromFile(wmTextBoxWatermarkImagePath.Text);
                    image = new Bitmap(loadedImage);
                }
            }
            catch
            {
                image = null;
            }

            wmPictureBoxWatermarkImage.Image?.Dispose();

            if (image != null)
            {
                wmPictureBoxWatermarkImage.Image = image;
                return;
            }

            var invalidImage = new Bitmap(
                Math.Max(1, wmPictureBoxWatermarkImage.ClientSize.Width),
                Math.Max(1, wmPictureBoxWatermarkImage.ClientSize.Height));

            using (var graphics = Graphics.FromImage(invalidImage))
            using (var font = new Font("Segoe UI", 10F))
            using (var brush = new SolidBrush(Color.Gray))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.White);
                var bounds = new RectangleF(0, 0, invalidImage.Width, invalidImage.Height);
                graphics.DrawString("Invalid image", font, brush, bounds, format);
            }

            wmPictureBoxWatermarkImage.Image = invalidImage;
        }

        private static WatermarkSettings Clone(WatermarkSettings settings)
        {
            return new WatermarkSettings
            {
                UseText = settings.UseText,
                UseImage = settings.UseImage,
                WatermarkText = settings.WatermarkText,
                WatermarkTextFontFamily = settings.WatermarkTextFontFamily,
                WatermarkTextFontSize = settings.WatermarkTextFontSize,
                WatermarkTextFontStyle = settings.WatermarkTextFontStyle,
                WatermarkImagePath = settings.WatermarkImagePath,
                WatermarkImageScale = settings.WatermarkImageScale,
                WatermarkOpacity = settings.WatermarkOpacity,
                WatermarkPosition = settings.WatermarkPosition,
                WatermarkRotation = settings.WatermarkRotation
            };
        }

        private static AnnotationSettings Clone(AnnotationSettings settings)
        {
            return new AnnotationSettings
            {
                UseAnnotation = settings.UseAnnotation,
                AnnotationFormat = settings.AnnotationFormat,
                AnnotationFontFamily = settings.AnnotationFontFamily,
                AnnotationFontSize = settings.AnnotationFontSize,
                AnnotationFontStyle = settings.AnnotationFontStyle,
                AnnotationFontColorArgb = settings.AnnotationFontColorArgb,
                AnnotationBackgroundColorArgb = settings.AnnotationBackgroundColorArgb,
                AnnotationOpacity = settings.AnnotationOpacity
            };
        }

        private static bool AreEqual(WatermarkSettings left, WatermarkSettings right)
        {
            return left.UseText == right.UseText
                && left.UseImage == right.UseImage
                && string.Equals(left.WatermarkText, right.WatermarkText, StringComparison.Ordinal)
                && string.Equals(left.WatermarkTextFontFamily, right.WatermarkTextFontFamily, StringComparison.Ordinal)
                && left.WatermarkTextFontSize == right.WatermarkTextFontSize
                && left.WatermarkTextFontStyle == right.WatermarkTextFontStyle
                && string.Equals(left.WatermarkImagePath, right.WatermarkImagePath, StringComparison.Ordinal)
                && left.WatermarkImageScale == right.WatermarkImageScale
                && left.WatermarkOpacity == right.WatermarkOpacity
                && string.Equals(left.WatermarkPosition, right.WatermarkPosition, StringComparison.Ordinal)
                && left.WatermarkRotation == right.WatermarkRotation;
        }

        private static bool AreEqual(AnnotationSettings left, AnnotationSettings right)
        {
            return left.UseAnnotation == right.UseAnnotation
                && string.Equals(left.AnnotationFormat, right.AnnotationFormat, StringComparison.Ordinal)
                && string.Equals(left.AnnotationFontFamily, right.AnnotationFontFamily, StringComparison.Ordinal)
                && left.AnnotationFontSize == right.AnnotationFontSize
                && left.AnnotationFontStyle == right.AnnotationFontStyle
                && left.AnnotationFontColorArgb == right.AnnotationFontColorArgb
                && left.AnnotationBackgroundColorArgb == right.AnnotationBackgroundColorArgb
                && left.AnnotationOpacity == right.AnnotationOpacity;
        }
    }
}
