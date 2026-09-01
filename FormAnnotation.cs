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
            var noColorItem = new ToolStripMenuItem("No color")
            {
                Tag = !_fontBackgroundColor.HasValue
            };
            noColorItem.Paint += DrawCurrentColorBorder;
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
                    //ForeColor = color.GetBrightness() < 0.5f ? Color.White : Color.Black
                    ForeColor = luminance < 0.5 ? Color.White : Color.Black,
                    Tag = _fontBackgroundColor?.ToArgb() == color.ToArgb()
                };
                colorItem.Paint += DrawCurrentColorBorder;
                colorItem.Click += (_, _) => SetAnnotationBackgroundColor(color);
                menu.Items.Add(colorItem);
            }

            menu.Show(buttonAnnotationBackgroundColor, buttonAnnotationBackgroundColor.Width, 0);
        }

        private static void DrawCurrentColorBorder(object? sender, PaintEventArgs e)
        {
            if (sender is ToolStripItem { Tag: true } item)
            {
                double luminance = (
                    0.2126 * item.BackColor.R /*_fontBackgroundColor.Value.R*/ +
                    0.7152 * item.BackColor.G /*_fontBackgroundColor.Value.G*/ +
                    0.0722 * item.BackColor.B /* _fontBackgroundColor.Value.B*/) / 255.0;

                //Color borderColor = item.BackColor.GetBrightness() < 0.5f ? Color.White : Color.Black;
                Color borderColor = luminance < 0.5 ? Color.White : Color.Black;
                using var pen = new Pen(borderColor, 5);
                Rectangle borderBounds = new(1, 1, item.Width - 3, item.Height - 3);
                e.Graphics.DrawRectangle(pen, borderBounds);
            }
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
            else
            {
                buttonAnnotationBackgroundColor.ForeColor = SystemColors.ControlText;
                buttonAnnotationBackgroundColor.BackColor = Color.White; // SystemColors.Control;
            }
        }

        private void UpdateControlState()
        {
            labelAnnotationFormat.Enabled = toggleUseAnnotation.Checked;
            comboBoxAnnotationFormat.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationFields.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationFont.Enabled = toggleUseAnnotation.Checked;
            buttonAnnotationBackgroundColor.Enabled = toggleUseAnnotation.Checked;
            labelAnnotationSample.Enabled = toggleUseAnnotation.Checked;
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
