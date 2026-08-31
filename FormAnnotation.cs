using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Capillume
{
    public partial class FormAnnotation : Form
    {
        private Icon? _appIcon;
        private Font _font = new("Segoe UI", 24);
        private Color _fontColor = Color.White;
        private Color? _fontBackgroundColor;
        private int _annotationSelectionStart;
        private int _annotationSelectionLength;
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
            ("Black", Color.Black),
            ("White", Color.White),
            ("Gray", Color.Gray),
            ("DarkGray", Color.DarkGray),
            ("LightGray", Color.LightGray),

            ("Red", Color.Red),
            ("DarkRed", Color.DarkRed),
            ("Orange", Color.Orange),
            ("Yellow", Color.Yellow),
            ("Goldenrod", Color.Goldenrod),

            ("Green", Color.Green),
            ("DarkGreen", Color.DarkGreen),
            ("LightGreen", Color.LightGreen),
            ("MediumSeaGreen", Color.MediumSeaGreen),

            ("Blue", Color.Blue),
            ("DarkBlue", Color.DarkBlue),
            ("LightBlue", Color.LightBlue),
            ("CornflowerBlue", Color.CornflowerBlue),
            ("RoyalBlue", Color.RoyalBlue),
            ("SteelBlue", Color.SteelBlue),

            ("Purple", Color.Purple),
            ("Magenta", Color.Magenta),
            ("Fuchsia", Color.Fuchsia),
            ("Lavender", Color.Lavender),

            ("Cyan", Color.Cyan),
            ("Aqua", Color.Aqua),
            ("MintCream", Color.MintCream),
            ("Beige", Color.Beige),
            ("LightYellow", Color.LightYellow),
            ("LightPink", Color.LightPink),
            ("LightCoral", Color.LightCoral)
        ];

        public AnnotationSettings _settings { get; }
        private bool _isUpdatingUi;
        public FormAnnotation(AnnotationSettings settings)
        {
            _settings = new AnnotationSettings
            {
                //Enabled = settings.Enabled,
                UseAnnotation = settings.UseAnnotation,
                AnnotationFormat = settings.AnnotationFormat,
                AnnotationFontFamily = settings.AnnotationFontFamily,
                AnnotationFontSize = settings.AnnotationFontSize,
                AnnotationFontStyle = settings.AnnotationFontStyle,
                AnnotationFontColorArgb = settings.AnnotationFontColorArgb,
                AnnotationBackgroundColorArgb = settings.AnnotationBackgroundColorArgb,
                AnnotationOpacity = settings.AnnotationOpacity
            };
            components = new Container();
            InitializeComponent();
            InitializeUI();
        }

        private void ButtonAnnotationBackgroundColor_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip(components);
            var noColorItem = new ToolStripMenuItem("No color");
            noColorItem.Click += (_, _) => SetAnnotationBackgroundColor(null);

            menu.Items.Add(noColorItem);
            menu.Items.Add(new ToolStripSeparator());

            foreach ((string name, Color color) in AnnotationBackgroundColors)
            {
                double luminance = (
                    0.2126 * color.R /*_fontBackgroundColor.Value.R*/ +
                    0.7152 * color.G /*_fontBackgroundColor.Value.G*/ +
                    0.0722 * color.B /* _fontBackgroundColor.Value.B*/) / 255.0;

                var colorItem = new ToolStripMenuItem(name)
                {
                    BackColor = color,
                    ForeColor = luminance < 0.5 ? Color.White : Color.Black
                    //ForeColor = color.GetBrightness() < 0.5f ? Color.White : Color.Black
                };
                colorItem.Click += (_, _) => SetAnnotationBackgroundColor(color);
                menu.Items.Add(colorItem);
            }

            menu.Show(buttonAnnotationBackgroundColor, buttonAnnotationBackgroundColor.Width, 0);
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (toggleUseAnnotation.Checked && string.IsNullOrWhiteSpace(comboBoxAnnotationFormat.Text))
            {
                MessageBox.Show("Enter annotation text.", "Annotation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                comboBoxAnnotationFormat.Focus();
                return;
            }

            //_settings.Enabled = true;
            _settings.UseAnnotation = toggleUseAnnotation.Checked;
            _settings.AnnotationFormat = comboBoxAnnotationFormat.Text.Trim();
            _settings.AnnotationFontFamily = _font.FontFamily.Name;
            _settings.AnnotationFontSize = _font.SizeInPoints;
            _settings.AnnotationFontStyle = _font.Style;
            _settings.AnnotationFontColorArgb = _fontColor.ToArgb();
            _settings.AnnotationBackgroundColorArgb = _fontBackgroundColor?.ToArgb();
            _settings.AnnotationOpacity = trackBarOpacity.Value;
            DialogResult = DialogResult.OK;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonAnnotationFont_Click(object sender, EventArgs e)
        {
            using var dialog = new FontDialog
            {
                Font = _font,
                Color = _fontColor,
                ShowColor = true,
                AllowVectorFonts = true,
                FontMustExist = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _font.Dispose();
                _font = (Font)dialog.Font.Clone();
                _fontColor = dialog.Color;
                //labelAnnotationSample.Font = _font;

                UpdateAnnotationSample();
                UpdateAnnotationFontButton();
                UpdateSaveButtonState();
            }
        }

        private void ButtonAnnotationFields_Click(object sender, EventArgs e)
        {
            _annotationSelectionStart = comboBoxAnnotationFormat.SelectionStart;
            _annotationSelectionLength = comboBoxAnnotationFormat.SelectionLength;

            var menu = new ContextMenuStrip(components);
            foreach (string field in AnnotationFields)
            {
                var menuItem = new ToolStripMenuItem(field);
                menuItem.Click += (_, _) => InsertAnnotationField(field);
                menu.Items.Add(menuItem);
            }

            menu.Show(buttonAnnotationFields, buttonAnnotationFields.Width, 0);
        }

        private void TrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
            UpdateSaveButtonState();
        }

        private void ToggleUseAnnotation_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlState();
            UpdateSaveButtonState();
        }

        private void ComboBoxAnnotationFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateAnnotationSample();
            UpdateSaveButtonState();
        }

        private void InitializeUI()
        {
            var assembly = Assembly.GetExecutingAssembly();
            // Load icon for the system tray and form
            try
            {
                using (var stream = assembly.GetManifestResourceStream("Capillume.icon.ico"))
                {
                    if (null != stream)
                    {
                        _appIcon = new Icon(stream);
                        this.Icon = _appIcon;
                    }
                    else
                    {
                        // Fallback to generated icon
                        _appIcon = FallbackIcon.CreateAppIconAdvanced();
                        this.Icon = _appIcon;
                    }
                }
            }
            catch
            {
                // Fallback to generated icon
                _appIcon = FallbackIcon.CreateAppIconAdvanced();
                this.Icon = _appIcon;
            }

            // Load settings into UI
            _font.Dispose();
            _font = new Font(_settings.AnnotationFontFamily, _settings.AnnotationFontSize, _settings.AnnotationFontStyle);
            _fontColor = Color.FromArgb(_settings.AnnotationFontColorArgb);
            _fontBackgroundColor = _settings.AnnotationBackgroundColorArgb.HasValue
                ? Color.FromArgb(_settings.AnnotationBackgroundColorArgb.Value)
                : null;

            toggleUseAnnotation.Checked = _settings.UseAnnotation;
            comboBoxAnnotationFormat.BeginUpdate();
            comboBoxAnnotationFormat.Items.AddRange(AnnotationFormats);
            if (!string.IsNullOrWhiteSpace(_settings.AnnotationFormat)
                && !comboBoxAnnotationFormat.Items.Contains(_settings.AnnotationFormat))
            {
                comboBoxAnnotationFormat.Items.Add(_settings.AnnotationFormat);
            }
            comboBoxAnnotationFormat.Text = _settings.AnnotationFormat;
            comboBoxAnnotationFormat.EndUpdate();
            UpdateAnnotationFontButton();
            UpdateAnnotationBackgroundButton();
            labelAnnotationSample.Text = $"{_font.Name}, {_font.SizeInPoints:0.#} pt";
            trackBarOpacity.Value = Math.Clamp(_settings.AnnotationOpacity, Constants.AnnotationOpacityMin, Constants.AnnotationOpacityMax);
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
            // buttonAnnotationFields.Click += ButtonAnnotationFields_Click;
            // buttonAnnotationFont.Click += ButtonAnnotationFont_Click;
            // buttonAnnotationBackgroundColor1.Click += ButtonAnnotationBackgroundColor_Click;
            UpdateAnnotationSample();
            UpdateControlState();
            UpdateSaveButtonState();
        }

        private void InsertAnnotationField(string field)
        {
            comboBoxAnnotationFormat.Text = comboBoxAnnotationFormat.Text.Remove(
                _annotationSelectionStart,
                _annotationSelectionLength).Insert(_annotationSelectionStart, field);
            comboBoxAnnotationFormat.SelectionStart = _annotationSelectionStart + field.Length;
            comboBoxAnnotationFormat.SelectionLength = 0;
            comboBoxAnnotationFormat.Focus();
            UpdateSaveButtonState();
        }

        private void SetAnnotationBackgroundColor(Color? color)
        {
            _fontBackgroundColor = color;
            UpdateAnnotationBackgroundButton();
            UpdateAnnotationSample();
            UpdateSaveButtonState();
        }

        private void UpdateAnnotationBackgroundButton()
        {
            //buttonAnnotationBackgroundColor.Text = _fontBackgroundColor.HasValue ? "Background" : "No background";
            if (_fontBackgroundColor != null)
            {
                double luminance = (
                    0.2126 * _fontBackgroundColor.Value.R +
                    0.7152 * _fontBackgroundColor.Value.G +
                    0.0722 * _fontBackgroundColor.Value.B) / 255.0;

                buttonAnnotationBackgroundColor.ForeColor = luminance < 0.5 ? Color.White : Color.Black;
                buttonAnnotationBackgroundColor.BackColor = _fontBackgroundColor ?? Color.Transparent; // SystemColors.Control;
                //buttonAnnotationBackgroundColor.ForeColor = _fontBackgroundColor.HasValue && _fontBackgroundColor.Value.GetBrightness() < 0.5f
                //    ? Color.White
                //    : Color.Black;
            }
        }

        private void UpdateAnnotationFontButton()
        {
            //buttonAnnotationFont.Text = $"{_font.Name}, {_font.SizeInPoints:0.#} pt";
            //buttonAnnotationFont.BackColor = _fontColor;
            //buttonAnnotationFont.ForeColor = _fontColor;
            //buttonAnnotationFont.ForeColor = _fontColor.GetBrightness() < 0.5f ? Color.White : Color.Black;
        }

        private void UpdateControlState()
        {
            labelAnnotationFormat.Enabled = toggleUseAnnotation.Checked;
            comboBoxAnnotationFormat.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationFields.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationFont.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationBackgroundColor.Enabled = toggleUseAnnotation.Checked;
            labelAnnotationSample.Enabled = toggleUseAnnotation.Checked;
            labelOpacity.Enabled = toggleUseAnnotation.Checked;
            trackBarOpacity.Enabled = toggleUseAnnotation.Checked;
            labelOpacityValue.Enabled = toggleUseAnnotation.Checked;
        }

        /// <summary>
        /// Update the state of the Ok/Save button so that they are enabled only when there are unsaved changes.
        /// </summary>
        private void UpdateSaveButtonState()
        {
            if (_isUpdatingUi)
            {
                return;
            }

            bool hasChanges = HasUnsavedChanges();
            buttonOk.Enabled = hasChanges;
        }

        private bool HasUnsavedChanges()
        {
            return toggleUseAnnotation.Checked != _settings.UseAnnotation
                || comboBoxAnnotationFormat.Text != _settings.AnnotationFormat
                || _font.FontFamily.Name != _settings.AnnotationFontFamily
                || _font.SizeInPoints != _settings.AnnotationFontSize
                || _font.Style != _settings.AnnotationFontStyle
                || _fontColor.ToArgb() != _settings.AnnotationFontColorArgb
                || _fontBackgroundColor?.ToArgb() != _settings.AnnotationBackgroundColorArgb
                || trackBarOpacity.Value != _settings.AnnotationOpacity;
        }

        private void UpdateAnnotationSample()
        {
            string format = string.IsNullOrWhiteSpace(comboBoxAnnotationFormat.Text)
                ? Constants.AnnotationFormatDefault
                : comboBoxAnnotationFormat.Text;
            labelAnnotationSample.Text = WatermarkRenderer.ResolveAnnotation(format);

            // Keep the original font size
            float originalSize = labelAnnotationSample.Font.Size;
            labelAnnotationSample.BackColor = _fontBackgroundColor ?? Color.Transparent; // SystemColors.Control;
            labelAnnotationSample.ForeColor = _fontColor;
            labelAnnotationSample.Font = new Font(_font.FontFamily, originalSize, _font.Style);
        }
    }
}
