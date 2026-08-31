using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Capillume
{
    public partial class FormWatermark : Form
    {
        private Icon? _appIcon;
        private Font _font = new("Segoe UI", 24);
        private const string PlaceholderText = "Enter text here";
        public WatermarkSettings _settings { get; }
        private bool _isUpdatingUi;

        public FormWatermark(WatermarkSettings settings)
        {
            _settings = new WatermarkSettings
            {
                Enabled = settings.Enabled,
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
                WatermarkRotation = settings.WatermarkRotation,
            };
            InitializeComponent();
            InitializeUI();
        }
        private void FormWatermark_FormClosing(object? sender, FormClosingEventArgs e)
        {
        }

        private void ButtonBrowseWatermarkImage_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog { Filter = "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif|All files|*.*" };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                textBoxWatermarkImagePath.Text = dialog.FileName;
                UpdateSaveButtonState();
            }
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (toggleUseText.Checked && (
                string.IsNullOrWhiteSpace(textBoxWatermarkText.Text) || textBoxWatermarkText.Text == PlaceholderText))
            {
                MessageBox.Show("Enter watermark text.", "Watermark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                textBoxWatermarkText.Focus();
                return;
            }

            if (toggleUseImage.Checked && !File.Exists(textBoxWatermarkImagePath.Text))
            {
                MessageBox.Show("Select a valid watermark image.", "Watermark", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }

            _settings.Enabled = true;
            _settings.UseText = toggleUseText.Checked;
            _settings.UseImage = toggleUseImage.Checked;
            _settings.WatermarkText = textBoxWatermarkText.Text.Trim();
            _settings.WatermarkTextFontFamily = _font.FontFamily.Name;
            _settings.WatermarkTextFontSize = _font.SizeInPoints;
            _settings.WatermarkTextFontStyle = _font.Style;
            _settings.WatermarkImagePath = textBoxWatermarkImagePath.Text;
            _settings.WatermarkImageScale = trackBarWatermarkImageScale.Value;
            _settings.WatermarkOpacity = trackBarOpacity.Value;
            _settings.WatermarkPosition = comboBoxWatermarkPosition.SelectedItem?.ToString() ?? Constants.WatermarkPositionDefault;
            _settings.WatermarkRotation = comboBoxWatermarkRotation.SelectedIndex * 90;
            DialogResult = DialogResult.OK;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonChooseFont_Click(object sender, EventArgs e)
        {
            using var dialog = new FontDialog { Font = _font, ShowColor = false, AllowVectorFonts = true, FontMustExist = true };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _font.Dispose();
                _font = (Font)dialog.Font.Clone();
                labelFontDescription.Text = $"{_font.Name}, {_font.SizeInPoints:0.#} pt";
                //labelFontDescription.Font = _font;

                // Keep the original size
                float originalSize = labelFontDescription.Font.Size;
                labelFontDescription.Font = new Font(_font.FontFamily, originalSize, _font.Style);
                UpdateSaveButtonState();
            }
        }

        private void ComboBoxWatermarkRotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void ComboBoxWatermarkPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void TrackBarOpacity_Scroll(object sender, EventArgs e)
        {
            //labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
        }

        private void TrackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
            UpdateSaveButtonState();
        }

        private void TrackBarWatermarkImageScale_ValueChanged(object sender, EventArgs e)
        {
            labelWatermarkImageScaleValue.Text = $"{trackBarWatermarkImageScale.Value}%";
            UpdateSaveButtonState();
        }
        private void ToggleUseText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlState();
            UpdateSaveButtonState();
        }

        private void ToggleUseImage_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlState();
            UpdateSaveButtonState();
        }

        private void TextBoxWatermarkImagePath_TextChanged(object sender, EventArgs e)
        {
            Image? image = null;

            try
            {
                if (File.Exists(textBoxWatermarkImagePath.Text))
                {
                    using var loadedImage = Image.FromFile(textBoxWatermarkImagePath.Text);
                    image = new Bitmap(loadedImage);
                }
            }
            catch
            {
                image = null;
            }

            pictureBoxWatermarkImage.Image?.Dispose();

            if (image != null)
            {
                pictureBoxWatermarkImage.Image = image;
                return;
            }

            var invalidImage = new Bitmap(
                Math.Max(1, pictureBoxWatermarkImage.ClientSize.Width),
                Math.Max(1, pictureBoxWatermarkImage.ClientSize.Height));

            using (var graphics = Graphics.FromImage(invalidImage))
            using (var font = new Font("Segoe UI", 10F))
            using (var brush = new SolidBrush(Color.Gray))
            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                graphics.Clear(Color.White);
                var bounds = new RectangleF(0, 0, invalidImage.Width, invalidImage.Height);
                graphics.DrawString("Invalid image", font, brush, bounds, format);
            }

            pictureBoxWatermarkImage.Image = invalidImage;
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
            toggleUseText.Checked = _settings.UseText;
            toggleUseImage.Checked = _settings.UseImage;
            textBoxWatermarkText.Text = _settings.WatermarkText;
            textBoxWatermarkImagePath.Text = _settings.WatermarkImagePath;
            _font.Dispose();
            _font = new Font(_settings.WatermarkTextFontFamily, _settings.WatermarkTextFontSize, _settings.WatermarkTextFontStyle);
            labelFontDescription.Text = $"{_font.Name}, {_font.SizeInPoints:0.#} pt";
            trackBarWatermarkImageScale.Value = Math.Clamp(_settings.WatermarkImageScale, Constants.WatermarkImageScaleMin, Constants.WatermarkImageScaleMax);
            labelWatermarkImageScaleValue.Text = $"{trackBarWatermarkImageScale.Value}%";
            trackBarOpacity.Value = Math.Clamp(_settings.WatermarkOpacity, Constants.WatermarkOpacityMin, Constants.WatermarkOpacityMax);
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
            comboBoxWatermarkPosition.SelectedItem = _settings.WatermarkPosition;
            if (comboBoxWatermarkPosition.SelectedIndex < 0) comboBoxWatermarkPosition.SelectedItem = Constants.WatermarkPositionDefault;
            comboBoxWatermarkRotation.SelectedIndex = _settings.WatermarkRotation / 90;
            SetupTextboxPlaceholder(textBoxWatermarkText);
            UpdateControlState();
            UpdateSaveButtonState();
        }

        private void UpdateControlState()
        {
            textBoxWatermarkText.Enabled = toggleUseText.Checked;
            buttonChooseFont.Enabled = toggleUseText.Checked;
            labelFontDescription.Enabled = toggleUseText.Checked;
            textBoxWatermarkImagePath.Enabled = toggleUseImage.Checked;
            buttonBrowseWatermarkImage.Enabled = toggleUseImage.Checked;
            trackBarWatermarkImageScale.Enabled = toggleUseImage.Checked;
            labelWatermarkImageScaleValue.Enabled = toggleUseImage.Checked;
            labelOpacityValue.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            labelOpacityValue.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            trackBarOpacity.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            labelWatermarkPosition.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            comboBoxWatermarkPosition.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            labelWatermarkRotation.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
            comboBoxWatermarkRotation.Enabled = toggleUseText.Checked || toggleUseImage.Checked;
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
            return toggleUseText.Checked != _settings.UseText
                || toggleUseImage.Checked != _settings.UseImage
                || (textBoxWatermarkText.Text != PlaceholderText && textBoxWatermarkText.Text != _settings.WatermarkText)
                || textBoxWatermarkImagePath.Text != _settings.WatermarkImagePath
                || _font.FontFamily.Name != _settings.WatermarkTextFontFamily
                || _font.SizeInPoints != _settings.WatermarkTextFontSize
                || _font.Style != _settings.WatermarkTextFontStyle
                || trackBarWatermarkImageScale.Value != _settings.WatermarkImageScale
                || trackBarOpacity.Value != _settings.WatermarkOpacity
                || !string.Equals(comboBoxWatermarkPosition.SelectedItem?.ToString(), _settings.WatermarkPosition)
                || comboBoxWatermarkRotation.SelectedIndex * 90 != _settings.WatermarkRotation;
        }

        private void SetupTextboxPlaceholder(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Font = new Font(textBox.Font, FontStyle.Italic); // italic placeholder
                textBox.Text = PlaceholderText;
            }

            textBox.GotFocus += (s, e) =>
            {
                if (textBox.Text == PlaceholderText)
                {
                    textBox.Text = "";
                    textBox.ForeColor = SystemColors.WindowText;
                    textBox.Font = new Font(textBox.Font, FontStyle.Regular); // back to normal
                }
                UpdateSaveButtonState();
            };

            textBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = PlaceholderText;
                    textBox.ForeColor = Color.Gray;
                    textBox.Font = new Font(textBox.Font, FontStyle.Italic); // italic placeholder
                }
                UpdateSaveButtonState();
            };

            textBox.TextChanged += (s, e) =>
            {
                // Ignore placeholder text changes
                if (textBox.Focused && textBox.Text == PlaceholderText)
                    return;
                UpdateSaveButtonState();
            };
        }
    }
}
