using System.Reflection;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

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

        private static readonly (string Label, int Value)[] DownscaleHeightPresets =
        [
            ("2160p (4K)", 2160),
            ("1440p (QHD)", 1440),
            ("1080p (Full HD)", 1080),
            ("720p (HD)", 720)
        ];

        private static readonly int[] DownscalePercentagePresets = [75, 50, 25];
        private static readonly int[] DownscaleWidthPresets = [1920, 1600, 1366, 1280, 1024];
        private static readonly (string Label, int Width, int Height)[] DownscaleBoundingBoxPresets =
        [
            ("1920 × 1080", 1920, 1080),
            ("1280 × 720", 1280, 720),
            ("800 × 600", 800, 600)
        ];

        private readonly WatermarkSettings _originalWatermarkSettings;
        private readonly AnnotationSettings _originalAnnotationSettings;
        private readonly DownscaleSettings _originalDownscaleSettings;

        private readonly WatermarkSettings _watermarkSettings;
        private readonly AnnotationSettings _annotationSettings;
        private readonly DownscaleSettings _downscaleSettings;

        private Icon? _appIcon;
        private Font _watermarkFont = new("Segoe UI", 24);
        private Font _annotationFont = new("Segoe UI", 24);
        private Color _annotationFontColor = Color.White;
        private Color? _annotationBackgroundColor;
        private int _annotationSelectionStart;
        private int _annotationSelectionLength;
        private bool _isUpdatingDownscaleUi;

        private readonly Label dsLabelDefaultSize1 = new();
        public WatermarkSettings WatermarkSettings => _watermarkSettings;
        public AnnotationSettings AnnotationSettings => _annotationSettings;
        public DownscaleSettings DownscaleSettings => _downscaleSettings;

        public bool WatermarkSettingsChanged => !AreEqual(_originalWatermarkSettings, _watermarkSettings);
        public bool AnnotationSettingsChanged => !AreEqual(_originalAnnotationSettings, _annotationSettings);
        public bool DownscaleSettingsChanged => !AreEqual(_originalDownscaleSettings, _downscaleSettings);

        public FormSettings(
            WatermarkSettings watermarkSettings,
            AnnotationSettings annotationSettings,
            DownscaleSettings downscaleSettings)
        {
            InitializeComponent();

            _originalWatermarkSettings = Clone(watermarkSettings);
            _originalAnnotationSettings = Clone(annotationSettings);
            _originalDownscaleSettings = Clone(downscaleSettings);
            _watermarkSettings = Clone(watermarkSettings);
            _annotationSettings = Clone(annotationSettings);
            _downscaleSettings = Clone(downscaleSettings);

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(dsLabelQuality1, "Controls how the image is resized.\nHigher‑quality methods produce smoother results.");
            toolTip.SetToolTip(dsCheckBoxSharpen1, "Adds a light sharpening pass to improve clarity after resizing.");
            toolTip.SetToolTip(dsCheckBoxSkipSmaller1, "Avoids resizing when the screenshot is already smaller than the target size.");

            InitializeIcon();
            InitializeTabWatermark();
            InitializeTabAnnotation();
            InitializeTabDownscale();
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

        private void InitializeTabWatermark()
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

        private void InitializeTabAnnotation()
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

        private void InitializeTabDownscale()
        {
            tabPageDownscale.SuspendLayout();
            //tabPageDownscale.Controls.Clear();
            tabPageDownscale.AutoScroll = true;

            dsLabelDefaultSize.Text = GetDefaultCaptureSizeDescription();

            //dsToggleEnable.AutoSize = true;
            //dsToggleEnable.Location = new Point(16, 72);
            //dsToggleEnable.Name = nameof(dsToggleEnable);
            //dsToggleEnable.Text = "Enable screenshot downscale";
            //dsToggleEnable.CheckedChanged += DownscaleSettingChanged;

            int radioLeft = 24;
            int presetLabelLeft = 560;
            int presetComboLeft = 670;
            int valueLabelLeft = 920;
            int valueControlLeft = 1035;
            int rowTop = 44;
            int rowSpacing = 64;

            dsComboBoxTargetHeight1.Items.AddRange(DownscaleHeightPresets.Select(p => p.Label).Append("Custom").ToArray());
            dsNumericTargetHeight1.Minimum = Constants.DownscaleTargetHeightMin;
            dsNumericTargetHeight1.Maximum = Constants.DownscaleTargetHeightMax;
            dsComboBoxPercentage1.Items.AddRange(DownscalePercentagePresets.Select(p => $"{p}%").Append("Custom").ToArray());
            dsNumericPercentage1.Maximum = Constants.DownscalePercentageMax;
            dsNumericPercentage1.Minimum = Constants.DownscalePercentageMin;
            dsComboBoxMaxWidth1.Items.AddRange(DownscaleWidthPresets.Select(p => p.ToString()).Append("Custom").ToArray());
            dsNumericMaxWidth1.Maximum = Constants.DownscaleMaxWidthMax;
            dsNumericMaxWidth1.Minimum = Constants.DownscaleMaxWidthMin;
            dsComboBoxBoundingBox1.Items.AddRange(DownscaleBoundingBoxPresets.Select(p => p.Label).Append("Custom").ToArray());
            dsNumericBoundingWidth1.Maximum = Constants.DownscaleMaxWidthMax;
            dsNumericBoundingWidth1.Minimum = Constants.DownscaleMaxWidthMin;
            dsNumericBoundingHeight1.Maximum = Constants.DownscaleTargetHeightMax;
            dsNumericBoundingHeight1.Minimum = Constants.DownscaleTargetHeightMin;

            dsComboBoxQuality1.Items.AddRange(
            [
                "High Quality (Bicubic)",
                "Balanced (Bilinear)",
                "Fast (Nearest Neighbor)"
            ]);

            dsComboBoxQuality1.SelectedIndexChanged += (_, _) => UpdateDownscaleSummary();

            dsCheckBoxSharpen1.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxSkipSmaller1.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxFullScreenOnly1.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxLossyOnly1.CheckedChanged += (_, _) => UpdateDownscaleSummary();

            _isUpdatingDownscaleUi = true;
            dsToggleEnable1.Checked = _downscaleSettings.Enabled;
            dsNumericTargetHeight1.Value = _downscaleSettings.TargetHeight;
            dsNumericPercentage1.Value = _downscaleSettings.ResizePercentage;
            dsNumericMaxWidth1.Value = _downscaleSettings.MaxWidth;
            dsNumericBoundingWidth1.Value = _downscaleSettings.BoundingBoxWidth;
            dsNumericBoundingHeight1.Value = _downscaleSettings.BoundingBoxHeight;
            dsComboBoxQuality1.SelectedIndex = (int)_downscaleSettings.Quality;
            dsCheckBoxSharpen1.Checked = _downscaleSettings.SharpenAfterResize;
            dsCheckBoxSkipSmaller1.Checked = _downscaleSettings.SkipSmallerImages;
            dsCheckBoxFullScreenOnly1.Checked = _downscaleSettings.FullScreenOnly;
            dsCheckBoxLossyOnly1.Checked = _downscaleSettings.LossyFormatsOnly;
            SelectDownscaleMode(_downscaleSettings.Mode);
            SyncDownscalePresetSelections();
            _isUpdatingDownscaleUi = false;

            UpdateDownscaleControlState();
            UpdateDownscaleSummary();
            tabPageDownscale.ResumeLayout(false);
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

            tabControlSettings.SelectedTab = tabPageDownscale;
            if (!TryApplyDownscaleSettings())
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

        private bool TryApplyDownscaleSettings()
        {
            _downscaleSettings.Enabled = dsToggleEnable1.Checked;
            _downscaleSettings.Mode = GetSelectedDownscaleMode();
            _downscaleSettings.TargetHeight = (int)dsNumericTargetHeight1.Value;
            _downscaleSettings.ResizePercentage = (int)dsNumericPercentage1.Value;
            _downscaleSettings.MaxWidth = (int)dsNumericMaxWidth1.Value;
            _downscaleSettings.BoundingBoxWidth = (int)dsNumericBoundingWidth1.Value;
            _downscaleSettings.BoundingBoxHeight = (int)dsNumericBoundingHeight1.Value;
            _downscaleSettings.Quality = (DownscaleQuality)dsComboBoxQuality1.SelectedIndex;
            _downscaleSettings.SharpenAfterResize = dsCheckBoxSharpen1.Checked;
            _downscaleSettings.SkipSmallerImages = dsCheckBoxSkipSmaller1.Checked;
            _downscaleSettings.FullScreenOnly = dsCheckBoxFullScreenOnly1.Checked;
            _downscaleSettings.LossyFormatsOnly = dsCheckBoxLossyOnly1.Checked;
            return true;
        }

        private void DownscaleModeChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            DownscaleSettingChanged(sender, e);
            UpdateDownscaleControlState();
        }

        private void DownscaleSettingChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            UpdateDownscaleControlState();
            UpdateDownscaleSummary();
        }

        private void DsComboBoxTargetHeight_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            if (dsComboBoxTargetHeight1.SelectedIndex >= 0 && dsComboBoxTargetHeight1.SelectedIndex < DownscaleHeightPresets.Length)
            {
                dsNumericTargetHeight1.Value = DownscaleHeightPresets[dsComboBoxTargetHeight1.SelectedIndex].Value;
            }

            UpdateDownscaleSummary();
        }

        private void DsNumericTargetHeight_ValueChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            SyncTargetHeightPresetSelection();
        }

        private void DsComboBoxPercentage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            if (dsComboBoxPercentage1.SelectedIndex >= 0 && dsComboBoxPercentage1.SelectedIndex < DownscalePercentagePresets.Length)
            {
                dsNumericPercentage1.Value = DownscalePercentagePresets[dsComboBoxPercentage1.SelectedIndex];
            }

            UpdateDownscaleSummary();
        }

        private void DsNumericPercentage_ValueChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            SyncPercentagePresetSelection();
        }

        private void DsComboBoxMaxWidth_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            if (dsComboBoxMaxWidth1.SelectedIndex >= 0 && dsComboBoxMaxWidth1.SelectedIndex < DownscaleWidthPresets.Length)
            {
                dsNumericMaxWidth1.Value = DownscaleWidthPresets[dsComboBoxMaxWidth1.SelectedIndex];
            }

            UpdateDownscaleSummary();
        }

        private void DsNumericMaxWidth_ValueChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            SyncMaxWidthPresetSelection();
        }

        private void DsComboBoxBoundingBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            if (dsComboBoxBoundingBox1.SelectedIndex >= 0 && dsComboBoxBoundingBox1.SelectedIndex < DownscaleBoundingBoxPresets.Length)
            {
                var preset = DownscaleBoundingBoxPresets[dsComboBoxBoundingBox1.SelectedIndex];
                dsNumericBoundingWidth1.Value = preset.Width;
                dsNumericBoundingHeight1.Value = preset.Height;
            }

            UpdateDownscaleSummary();
        }

        private void DsNumericBoundingBox_ValueChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingDownscaleUi)
            {
                return;
            }

            SyncBoundingBoxPresetSelection();
        }

        private void SyncDownscalePresetSelections()
        {
            SyncTargetHeightPresetSelection();
            SyncPercentagePresetSelection();
            SyncMaxWidthPresetSelection();
            SyncBoundingBoxPresetSelection();
        }

        private void SyncTargetHeightPresetSelection()
        {
            SetComboSelection(
                dsComboBoxTargetHeight1,
                Array.FindIndex(DownscaleHeightPresets, preset => preset.Value == (int)dsNumericTargetHeight1.Value),
                DownscaleHeightPresets.Length);
        }

        private void SyncPercentagePresetSelection()
        {
            SetComboSelection(
                dsComboBoxPercentage1,
                Array.FindIndex(DownscalePercentagePresets, preset => preset == (int)dsNumericPercentage1.Value),
                DownscalePercentagePresets.Length);
        }

        private void SyncMaxWidthPresetSelection()
        {
            SetComboSelection(
                dsComboBoxMaxWidth1,
                Array.FindIndex(DownscaleWidthPresets, preset => preset == (int)dsNumericMaxWidth1.Value),
                DownscaleWidthPresets.Length);
        }

        private void SyncBoundingBoxPresetSelection()
        {
            SetComboSelection(
                dsComboBoxBoundingBox1,
                Array.FindIndex(
                    DownscaleBoundingBoxPresets,
                    preset => preset.Width == (int)dsNumericBoundingWidth1.Value && preset.Height == (int)dsNumericBoundingHeight1.Value),
                DownscaleBoundingBoxPresets.Length);
        }

        private void SetComboSelection(ComboBox comboBox, int presetIndex, int customIndex)
        {
            bool previousState = _isUpdatingDownscaleUi;
            _isUpdatingDownscaleUi = true;
            comboBox.SelectedIndex = presetIndex >= 0 ? presetIndex : customIndex;
            _isUpdatingDownscaleUi = previousState;
        }

        private void SelectDownscaleMode(DownscaleMode mode)
        {
            bool previousState = _isUpdatingDownscaleUi;
            _isUpdatingDownscaleUi = true;
            dsRadioTargetHeight1.Checked = mode == DownscaleMode.TargetHeight;
            dsRadioPercentage1.Checked = mode == DownscaleMode.Percentage;
            dsRadioMaxWidth1.Checked = mode == DownscaleMode.MaxWidth;
            dsRadioBoundingBox1.Checked = mode == DownscaleMode.BoundingBox;
            _isUpdatingDownscaleUi = previousState;
        }

        private DownscaleMode GetSelectedDownscaleMode()
        {
            if (dsRadioPercentage1.Checked)
            {
                return DownscaleMode.Percentage;
            }

            if (dsRadioMaxWidth1.Checked)
            {
                return DownscaleMode.MaxWidth;
            }

            if (dsRadioBoundingBox1.Checked)
            {
                return DownscaleMode.BoundingBox;
            }

            return DownscaleMode.TargetHeight;
        }

        private void UpdateDownscaleControlState()
        {
            bool enabled = dsToggleEnable1.Checked;
            dsGroupBoxModes1.Enabled = enabled;
            dsGroupBoxProcessing1.Enabled = enabled;

            DownscaleMode selectedMode = GetSelectedDownscaleMode();
            dsComboBoxTargetHeight1.Enabled = enabled && selectedMode == DownscaleMode.TargetHeight;
            dsNumericTargetHeight1.Enabled = enabled && selectedMode == DownscaleMode.TargetHeight;
            dsComboBoxPercentage1.Enabled = enabled && selectedMode == DownscaleMode.Percentage;
            dsNumericPercentage1.Enabled = enabled && selectedMode == DownscaleMode.Percentage;
            dsComboBoxMaxWidth1.Enabled = enabled && selectedMode == DownscaleMode.MaxWidth;
            dsNumericMaxWidth1.Enabled = enabled && selectedMode == DownscaleMode.MaxWidth;
            dsComboBoxBoundingBox1.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
            dsNumericBoundingWidth1.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
            dsNumericBoundingHeight1.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
        }

        private void UpdateDownscaleSummary()
        {
            if (!dsToggleEnable1.Checked)
            {
                dsLabelSummary1.Text = "Downscaling is currently off. Screenshots will be saved at their original size.";
                return;
            }

            string resizeDescription = GetSelectedDownscaleMode() switch
            {
                DownscaleMode.TargetHeight => $"Resize to {dsNumericTargetHeight1.Value:N0}px height.",
                DownscaleMode.Percentage => $"Resize to {dsNumericPercentage1.Value}% of the captured size.",
                DownscaleMode.MaxWidth => $"Reduce to {dsNumericMaxWidth1.Value:N0}px width.",
                DownscaleMode.BoundingBox => $"Fit within {dsNumericBoundingWidth1.Value:N0} × {dsNumericBoundingHeight1.Value:N0}.",
                _ => string.Empty
            };

            string scopeDescription = dsCheckBoxFullScreenOnly1.Checked
                ? "Applies only to full-screen captures."
                : "Applies to full-screen and active-window captures.";

            string formatDescription = dsCheckBoxLossyOnly1.Checked
                ? "Only JPG and WEBP saves will be downscaled."
                : "All save formats can be downscaled.";

            string skipDescription = dsCheckBoxSkipSmaller1.Checked
                ? "Smaller images are not upscaled."
                : "Smaller images may still be resized if the chosen target is larger.";

            string sharpenDescription = dsCheckBoxSharpen1.Checked
                ? "A light sharpen pass runs after resize."
                : "No sharpen pass is applied.";

            dsLabelSummary1.Text = $"{resizeDescription} Quality: {dsComboBoxQuality1.SelectedItem}. {scopeDescription} {formatDescription} {skipDescription} {sharpenDescription}";
        }

        private static string GetDefaultCaptureSizeDescription()
        {
            int minX = Screen.AllScreens.Min(screen => screen.Bounds.Left);
            int minY = Screen.AllScreens.Min(screen => screen.Bounds.Top);
            int maxX = Screen.AllScreens.Max(screen => screen.Bounds.Right);
            int maxY = Screen.AllScreens.Max(screen => screen.Bounds.Bottom);
            int width = maxX - minX;
            int height = maxY - minY;

            return $"Detected default size: full-screen captures ({width:N0}×{height:N0}); active-window captures keep the original window size.";
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

        private static DownscaleSettings Clone(DownscaleSettings settings)
        {
            return new DownscaleSettings
            {
                Enabled = settings.Enabled,
                Mode = settings.Mode,
                TargetHeight = settings.TargetHeight,
                ResizePercentage = settings.ResizePercentage,
                MaxWidth = settings.MaxWidth,
                BoundingBoxWidth = settings.BoundingBoxWidth,
                BoundingBoxHeight = settings.BoundingBoxHeight,
                Quality = settings.Quality,
                SharpenAfterResize = settings.SharpenAfterResize,
                SkipSmallerImages = settings.SkipSmallerImages,
                FullScreenOnly = settings.FullScreenOnly,
                LossyFormatsOnly = settings.LossyFormatsOnly
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

        private static bool AreEqual(DownscaleSettings left, DownscaleSettings right)
        {
            return left.Enabled == right.Enabled
                && left.Mode == right.Mode
                && left.TargetHeight == right.TargetHeight
                && left.ResizePercentage == right.ResizePercentage
                && left.MaxWidth == right.MaxWidth
                && left.BoundingBoxWidth == right.BoundingBoxWidth
                && left.BoundingBoxHeight == right.BoundingBoxHeight
                && left.Quality == right.Quality
                && left.SharpenAfterResize == right.SharpenAfterResize
                && left.SkipSmallerImages == right.SkipSmallerImages
                && left.FullScreenOnly == right.FullScreenOnly
                && left.LossyFormatsOnly == right.LossyFormatsOnly;
        }
    }
}
