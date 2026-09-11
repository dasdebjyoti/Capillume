using System.Reflection;
using System.Text;
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
            ("Gainsboro", Color.Gainsboro),
            ("White", Color.White),

            // --- Reds / Oranges / Yellows ---
            ("DarkRed", Color.DarkRed),
            ("Red", Color.Red),
            ("Orange", Color.Orange),
            ("Goldenrod", Color.Goldenrod),
            ("Yellow", Color.Yellow),
            ("LightYellow", Color.LightYellow),
            ("LemonChiffon", Color.LemonChiffon),
            ("LightCoral", Color.LightCoral),
            ("LightPink", Color.LightPink),
            ("MistyRose", Color.MistyRose),

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
            ("OldLace", Color.OldLace),                // elegant warm tone
            ("NavajoWhite", Color.NavajoWhite)
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

        private readonly string _saveFolder;
        private readonly WatermarkSettings _originalWatermarkSettings;
        private readonly AnnotationSettings _originalAnnotationSettings;
        private readonly DownscaleSettings _originalDownscaleSettings;
        private readonly ImageProcessingSettings _originalImageProcessingSettings;
        private readonly RetentionSettings _originalRetentionSettings;

        private readonly WatermarkSettings _watermarkSettings;
        private readonly AnnotationSettings _annotationSettings;
        private readonly DownscaleSettings _downscaleSettings;
        private readonly ImageProcessingSettings _imageProcessingSettings;
        private readonly RetentionSettings _retentionSettings;

        private Icon? _appIcon;
        private Font _watermarkFont = new("Segoe UI", 24);
        private Font _annotationFont = new("Segoe UI", 24);
        private Color _annotationFontColor = Color.White;
        private Color? _annotationBackgroundColor;
        private int _annotationSelectionStart;
        private int _annotationSelectionLength;
        private bool _isUpdatingDownscaleUi;
        private bool _isRetentionOperationRunning;

        private readonly Label dsLabelDefaultSize1 = new();
        public WatermarkSettings WatermarkSettings => _watermarkSettings;
        public AnnotationSettings AnnotationSettings => _annotationSettings;
        public DownscaleSettings DownscaleSettings => _downscaleSettings;
        public ImageProcessingSettings ImageProcessingSettings => _imageProcessingSettings;
        public RetentionSettings RetentionSettings => _retentionSettings;

        public bool WatermarkSettingsChanged => !AreEqual(_originalWatermarkSettings, _watermarkSettings);
        public bool AnnotationSettingsChanged => !AreEqual(_originalAnnotationSettings, _annotationSettings);
        public bool DownscaleSettingsChanged => !AreEqual(_originalDownscaleSettings, _downscaleSettings);
        public bool ImageProcessingSettingsChanged => !AreEqual(_originalImageProcessingSettings, _imageProcessingSettings);
        public bool RetentionSettingsChanged => !AreEqual(_originalRetentionSettings, _retentionSettings);

        public FormSettings(
            WatermarkSettings watermarkSettings,
            AnnotationSettings annotationSettings,
            DownscaleSettings downscaleSettings,
            ImageProcessingSettings imageProcessingSettings,
            RetentionSettings retentionSettings,
            string saveFolder)
        {
            InitializeComponent();

            _saveFolder = saveFolder;
            _originalWatermarkSettings = Clone(watermarkSettings);
            _originalAnnotationSettings = Clone(annotationSettings);
            _originalDownscaleSettings = Clone(downscaleSettings);
            _originalImageProcessingSettings = Clone(imageProcessingSettings);
            _originalRetentionSettings = Clone(retentionSettings);
            _watermarkSettings = Clone(watermarkSettings);
            _annotationSettings = Clone(annotationSettings);
            _downscaleSettings = Clone(downscaleSettings);
            _imageProcessingSettings = Clone(imageProcessingSettings);
            _retentionSettings = Clone(retentionSettings);

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(dsLabelQuality, "Controls how the image is resized.\nHigher‑quality methods produce smoother results.");
            toolTip.SetToolTip(dsCheckBoxSharpen, "Adds a light sharpening pass to improve clarity after resizing.");
            toolTip.SetToolTip(dsCheckBoxSkipSmaller, "Avoids resizing when the screenshot is already smaller than the target size.");
            toolTip.SetToolTip(rtToggleSwitchAutoCleanup, "Runs screenshot file cleanup after each screenshot is saved.");
            toolTip.SetToolTip(rtCheckBoxDryRun, "Preview what would be removed without actually deleting any files.");
            toolTip.SetToolTip(rtCheckBoxIncludeSubfolders, "Also evaluate screenshots inside subfolders of the save location.");
            toolTip.SetToolTip(rtCheckBoxPerSessionSubfolder, "Organize each cleanup backup in a separate timestamped folder.");

            InitializeIcon();
            InitializeTabWatermark();
            InitializeTabAnnotation();
            InitializeTabDownscale();
            InitializeTabImageProcessing();
            InitializeTabRetention();
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

            dsComboBoxTargetHeight.Items.AddRange(DownscaleHeightPresets.Select(p => p.Label).Append("Custom").ToArray());
            dsNumericTargetHeight.Minimum = Constants.DownscaleTargetHeightMin;
            dsNumericTargetHeight.Maximum = Constants.DownscaleTargetHeightMax;
            dsComboBoxPercentage.Items.AddRange(DownscalePercentagePresets.Select(p => $"{p}%").Append("Custom").ToArray());
            dsNumericPercentage.Maximum = Constants.DownscalePercentageMax;
            dsNumericPercentage.Minimum = Constants.DownscalePercentageMin;
            dsComboBoxMaxWidth.Items.AddRange(DownscaleWidthPresets.Select(p => p.ToString()).Append("Custom").ToArray());
            dsNumericMaxWidth.Maximum = Constants.DownscaleMaxWidthMax;
            dsNumericMaxWidth.Minimum = Constants.DownscaleMaxWidthMin;
            dsComboBoxBoundingBox.Items.AddRange(DownscaleBoundingBoxPresets.Select(p => p.Label).Append("Custom").ToArray());
            dsNumericBoundingWidth.Maximum = Constants.DownscaleMaxWidthMax;
            dsNumericBoundingWidth.Minimum = Constants.DownscaleMaxWidthMin;
            dsNumericBoundingHeight.Maximum = Constants.DownscaleTargetHeightMax;
            dsNumericBoundingHeight.Minimum = Constants.DownscaleTargetHeightMin;

            dsComboBoxQuality.Items.AddRange(
            [
                "High Quality (Bicubic)",
                "Balanced (Bilinear)",
                "Fast (Nearest Neighbor)"
            ]);

            dsComboBoxQuality.SelectedIndexChanged += (_, _) => UpdateDownscaleSummary();

            dsCheckBoxSharpen.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxSkipSmaller.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxFullScreenOnly.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxLossyOnly.CheckedChanged += (_, _) => UpdateDownscaleSummary();

            _isUpdatingDownscaleUi = true;
            dsToggleEnable.Checked = _downscaleSettings.Enabled;
            dsNumericTargetHeight.Value = _downscaleSettings.TargetHeight;
            dsNumericPercentage.Value = _downscaleSettings.ResizePercentage;
            dsNumericMaxWidth.Value = _downscaleSettings.MaxWidth;
            dsNumericBoundingWidth.Value = _downscaleSettings.BoundingBoxWidth;
            dsNumericBoundingHeight.Value = _downscaleSettings.BoundingBoxHeight;
            dsComboBoxQuality.SelectedIndex = (int)_downscaleSettings.Quality;
            dsCheckBoxSharpen.Checked = _downscaleSettings.SharpenAfterResize;
            dsCheckBoxSkipSmaller.Checked = _downscaleSettings.SkipSmallerImages;
            dsCheckBoxFullScreenOnly.Checked = _downscaleSettings.FullScreenOnly;
            dsCheckBoxLossyOnly.Checked = _downscaleSettings.LossyFormatsOnly;
            SelectDownscaleMode(_downscaleSettings.Mode);
            SyncDownscalePresetSelections();
            _isUpdatingDownscaleUi = false;

            UpdateDownscaleControlState();
            UpdateDownscaleSummary();
            tabPageDownscale.ResumeLayout(false);
        }

        private void InitializeTabImageProcessing()
        {
            SelectImageColorMode(_imageProcessingSettings.ColorMode);
            ipCheckBoxHighContrast.Checked = _imageProcessingSettings.HighContrast;
            ipCheckBoxNoiseReduction.Checked = _imageProcessingSettings.NoiseReduction;
            ipComboBoxColorTemperature.SelectedIndex = (int)_imageProcessingSettings.ColorTemperature;

            if (ipComboBoxColorTemperature.SelectedIndex < 0)
            {
                ipComboBoxColorTemperature.SelectedIndex = (int)ColorTemperatureMode.Neutral;
            }
        }

        private void InitializeTabRetention()
        {
            rtToggleSwitchAutoCleanup.Checked = _retentionSettings.AutoCleanupEnabled;
            rtCheckBoxMaxDays.Checked = _retentionSettings.MaxDaysEnabled;
            rtNumericMaxDays.Value = Math.Clamp(
                _retentionSettings.MaxDaysToRetain,
                Constants.RetentionDaysMin,
                Constants.RetentionDaysMax);
            rtCheckBoxMaxFiles.Checked = _retentionSettings.MaxFilesEnabled;
            rtNumericMaxFiles.Value = Math.Clamp(
                _retentionSettings.MaxFilesToRetain,
                Constants.RetentionFileCountMin,
                Constants.RetentionFileCountMax);
            rtComboBoxAction.SelectedIndex = (int)_retentionSettings.Action;
            rtTextBoxBackupFolder.Text = _retentionSettings.BackupFolder;
            rtCheckBoxPerSessionSubfolder.Checked = _retentionSettings.PerSessionSubfolder;
            rtCheckBoxDryRun.Checked = _retentionSettings.DryRunMode;
            rtCheckBoxIncludeSubfolders.Checked = _retentionSettings.IncludeSubfolders;

            if (rtComboBoxAction.SelectedIndex < 0)
            {
                rtComboBoxAction.SelectedIndex = (int)RetentionAction.MoveToRecycleBin;
            }

            rtToggleSwitchAutoCleanup.CheckedChanged += RetentionSettingChanged;
            rtCheckBoxMaxDays.CheckedChanged += RetentionSettingChanged;
            rtNumericMaxDays.ValueChanged += RetentionSettingChanged;
            rtCheckBoxMaxFiles.CheckedChanged += RetentionSettingChanged;
            rtNumericMaxFiles.ValueChanged += RetentionSettingChanged;
            rtComboBoxAction.SelectedIndexChanged += RtComboBoxAction_SelectedIndexChanged;
            rtTextBoxBackupFolder.TextChanged += RetentionSettingChanged;
            rtCheckBoxPerSessionSubfolder.CheckedChanged += RetentionSettingChanged;
            rtCheckBoxDryRun.CheckedChanged += RetentionSettingChanged;
            rtCheckBoxIncludeSubfolders.CheckedChanged += RetentionSettingChanged;

            UpdateRetentionControlState();
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

            tabControlSettings.SelectedTab = tabPageImageProcessing;
            if (!TryApplyImageProcessingSettings())
            {
                return;
            }

            tabControlSettings.SelectedTab = tabPageRetention;
            if (!TryApplyRetentionSettings())
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
            _downscaleSettings.Enabled = dsToggleEnable.Checked;
            _downscaleSettings.Mode = GetSelectedDownscaleMode();
            _downscaleSettings.TargetHeight = (int)dsNumericTargetHeight.Value;
            _downscaleSettings.ResizePercentage = (int)dsNumericPercentage.Value;
            _downscaleSettings.MaxWidth = (int)dsNumericMaxWidth.Value;
            _downscaleSettings.BoundingBoxWidth = (int)dsNumericBoundingWidth.Value;
            _downscaleSettings.BoundingBoxHeight = (int)dsNumericBoundingHeight.Value;
            _downscaleSettings.Quality = (DownscaleQuality)dsComboBoxQuality.SelectedIndex;
            _downscaleSettings.SharpenAfterResize = dsCheckBoxSharpen.Checked;
            _downscaleSettings.SkipSmallerImages = dsCheckBoxSkipSmaller.Checked;
            _downscaleSettings.FullScreenOnly = dsCheckBoxFullScreenOnly.Checked;
            _downscaleSettings.LossyFormatsOnly = dsCheckBoxLossyOnly.Checked;
            return true;
        }

        private bool TryApplyImageProcessingSettings()
        {
            _imageProcessingSettings.ColorMode = GetSelectedImageColorMode();
            _imageProcessingSettings.HighContrast = ipCheckBoxHighContrast.Checked;
            _imageProcessingSettings.NoiseReduction = ipCheckBoxNoiseReduction.Checked;
            _imageProcessingSettings.ColorTemperature = (ColorTemperatureMode)Math.Clamp(
                ipComboBoxColorTemperature.SelectedIndex,
                (int)ColorTemperatureMode.Neutral,
                (int)ColorTemperatureMode.Cool);
            return true;
        }

        private bool TryApplyRetentionSettings()
        {
            if ((RetentionAction)rtComboBoxAction.SelectedIndex == RetentionAction.BackupThenDelete
                && string.IsNullOrWhiteSpace(rtTextBoxBackupFolder.Text))
            {
                MessageBox.Show("Select a backup folder before using backup retention action.", "Retention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtTextBoxBackupFolder.Focus();
                return false;
            }

            _retentionSettings.AutoCleanupEnabled = rtToggleSwitchAutoCleanup.Checked;
            _retentionSettings.MaxDaysEnabled = rtCheckBoxMaxDays.Checked;
            _retentionSettings.MaxDaysToRetain = (int)rtNumericMaxDays.Value;
            _retentionSettings.MaxFilesEnabled = rtCheckBoxMaxFiles.Checked;
            _retentionSettings.MaxFilesToRetain = (int)rtNumericMaxFiles.Value;
            _retentionSettings.Action = (RetentionAction)Math.Clamp(
                rtComboBoxAction.SelectedIndex,
                (int)RetentionAction.MoveToRecycleBin,
                (int)RetentionAction.BackupThenDelete);
            _retentionSettings.BackupFolder = rtTextBoxBackupFolder.Text.Trim();
            _retentionSettings.PerSessionSubfolder = rtCheckBoxPerSessionSubfolder.Checked;
            _retentionSettings.DryRunMode = rtCheckBoxDryRun.Checked;
            _retentionSettings.IncludeSubfolders = rtCheckBoxIncludeSubfolders.Checked;
            return true;
        }

        private void SelectImageColorMode(ImageColorMode mode)
        {
            ipRadioFullColor.Checked = mode == ImageColorMode.FullColor;
            ipRadioGrayscale.Checked = mode == ImageColorMode.Grayscale;
            ipRadioMonochrome1Bit.Checked = mode == ImageColorMode.Monochrome1Bit;
            ipRadioColor16.Checked = mode == ImageColorMode.Color16;
            ipRadioColor256.Checked = mode == ImageColorMode.Color256;
            ipRadioAdaptivePalette.Checked = mode == ImageColorMode.AdaptivePalette;
        }

        private ImageColorMode GetSelectedImageColorMode()
        {
            if (ipRadioGrayscale.Checked)
            {
                return ImageColorMode.Grayscale;
            }

            if (ipRadioMonochrome1Bit.Checked)
            {
                return ImageColorMode.Monochrome1Bit;
            }

            if (ipRadioColor16.Checked)
            {
                return ImageColorMode.Color16;
            }

            if (ipRadioColor256.Checked)
            {
                return ImageColorMode.Color256;
            }

            if (ipRadioAdaptivePalette.Checked)
            {
                return ImageColorMode.AdaptivePalette;
            }

            return ImageColorMode.FullColor;
        }

        private void ImageProcessingColorModeChanged(object? sender, EventArgs e)
        {
        }

        private void ImageProcessingSettingChanged(object? sender, EventArgs e)
        {
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

            if (dsComboBoxTargetHeight.SelectedIndex >= 0 && dsComboBoxTargetHeight.SelectedIndex < DownscaleHeightPresets.Length)
            {
                dsNumericTargetHeight.Value = DownscaleHeightPresets[dsComboBoxTargetHeight.SelectedIndex].Value;
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

            if (dsComboBoxPercentage.SelectedIndex >= 0 && dsComboBoxPercentage.SelectedIndex < DownscalePercentagePresets.Length)
            {
                dsNumericPercentage.Value = DownscalePercentagePresets[dsComboBoxPercentage.SelectedIndex];
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

            if (dsComboBoxMaxWidth.SelectedIndex >= 0 && dsComboBoxMaxWidth.SelectedIndex < DownscaleWidthPresets.Length)
            {
                dsNumericMaxWidth.Value = DownscaleWidthPresets[dsComboBoxMaxWidth.SelectedIndex];
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

            if (dsComboBoxBoundingBox.SelectedIndex >= 0 && dsComboBoxBoundingBox.SelectedIndex < DownscaleBoundingBoxPresets.Length)
            {
                var preset = DownscaleBoundingBoxPresets[dsComboBoxBoundingBox.SelectedIndex];
                dsNumericBoundingWidth.Value = preset.Width;
                dsNumericBoundingHeight.Value = preset.Height;
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
                dsComboBoxTargetHeight,
                Array.FindIndex(DownscaleHeightPresets, preset => preset.Value == (int)dsNumericTargetHeight.Value),
                DownscaleHeightPresets.Length);
        }

        private void SyncPercentagePresetSelection()
        {
            SetComboSelection(
                dsComboBoxPercentage,
                Array.FindIndex(DownscalePercentagePresets, preset => preset == (int)dsNumericPercentage.Value),
                DownscalePercentagePresets.Length);
        }

        private void SyncMaxWidthPresetSelection()
        {
            SetComboSelection(
                dsComboBoxMaxWidth,
                Array.FindIndex(DownscaleWidthPresets, preset => preset == (int)dsNumericMaxWidth.Value),
                DownscaleWidthPresets.Length);
        }

        private void SyncBoundingBoxPresetSelection()
        {
            SetComboSelection(
                dsComboBoxBoundingBox,
                Array.FindIndex(
                    DownscaleBoundingBoxPresets,
                    preset => preset.Width == (int)dsNumericBoundingWidth.Value && preset.Height == (int)dsNumericBoundingHeight.Value),
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
            dsRadioTargetHeight.Checked = mode == DownscaleMode.TargetHeight;
            dsRadioPercentage.Checked = mode == DownscaleMode.Percentage;
            dsRadioMaxWidth.Checked = mode == DownscaleMode.MaxWidth;
            dsRadioBoundingBox.Checked = mode == DownscaleMode.BoundingBox;
            _isUpdatingDownscaleUi = previousState;
        }

        private DownscaleMode GetSelectedDownscaleMode()
        {
            if (dsRadioPercentage.Checked)
            {
                return DownscaleMode.Percentage;
            }

            if (dsRadioMaxWidth.Checked)
            {
                return DownscaleMode.MaxWidth;
            }

            if (dsRadioBoundingBox.Checked)
            {
                return DownscaleMode.BoundingBox;
            }

            return DownscaleMode.TargetHeight;
        }

        private void UpdateDownscaleControlState()
        {
            bool enabled = dsToggleEnable.Checked;
            dsGroupBoxModes.Enabled = enabled;
            dsGroupBoxProcessing.Enabled = enabled;

            DownscaleMode selectedMode = GetSelectedDownscaleMode();
            dsComboBoxTargetHeight.Enabled = enabled && selectedMode == DownscaleMode.TargetHeight;
            dsNumericTargetHeight.Enabled = enabled && selectedMode == DownscaleMode.TargetHeight;
            dsComboBoxPercentage.Enabled = enabled && selectedMode == DownscaleMode.Percentage;
            dsNumericPercentage.Enabled = enabled && selectedMode == DownscaleMode.Percentage;
            dsComboBoxMaxWidth.Enabled = enabled && selectedMode == DownscaleMode.MaxWidth;
            dsNumericMaxWidth.Enabled = enabled && selectedMode == DownscaleMode.MaxWidth;
            dsComboBoxBoundingBox.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
            dsNumericBoundingWidth.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
            dsNumericBoundingHeight.Enabled = enabled && selectedMode == DownscaleMode.BoundingBox;
        }

        private void UpdateDownscaleSummary()
        {
            if (!dsToggleEnable.Checked)
            {
                dsLabelSummary.Text = "Downscaling is currently off. Screenshots will be saved at their original size.";
                return;
            }

            string resizeDescription = GetSelectedDownscaleMode() switch
            {
                DownscaleMode.TargetHeight => $"Resize to {dsNumericTargetHeight.Value:N0}px height.",
                DownscaleMode.Percentage => $"Resize to {dsNumericPercentage.Value}% of the captured size.",
                DownscaleMode.MaxWidth => $"Reduce to {dsNumericMaxWidth.Value:N0}px width.",
                DownscaleMode.BoundingBox => $"Fit within {dsNumericBoundingWidth.Value:N0} × {dsNumericBoundingHeight.Value:N0}.",
                _ => string.Empty
            };

            string scopeDescription = dsCheckBoxFullScreenOnly.Checked
                ? "Applies only to full-screen captures."
                : "Applies to full-screen and active-window captures.";

            string formatDescription = dsCheckBoxLossyOnly.Checked
                ? "Only JPG and WEBP saves will be downscaled."
                : "All save formats can be downscaled.";

            string skipDescription = dsCheckBoxSkipSmaller.Checked
                ? "Smaller images are not upscaled."
                : "Smaller images may still be resized if the chosen target is larger.";

            string sharpenDescription = dsCheckBoxSharpen.Checked
                ? "A light sharpen pass runs after resize."
                : "No sharpen pass is applied.";

            dsLabelSummary.Text = $"{resizeDescription} Quality: {dsComboBoxQuality.SelectedItem}. {scopeDescription} {formatDescription} {skipDescription} {sharpenDescription}";
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

        private void RetentionSettingChanged(object? sender, EventArgs e)
        {
            UpdateRetentionControlState();
        }

        private void RtComboBoxAction_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateRetentionControlState();
        }

        private void UpdateRetentionControlState()
        {
            bool backupSelected = GetSelectedRetentionAction() == RetentionAction.BackupThenDelete;
            bool hasSaveFolder = !string.IsNullOrWhiteSpace(_saveFolder);
            bool controlsEnabled = !_isRetentionOperationRunning;

            rtToggleSwitchAutoCleanup.Enabled = controlsEnabled;
            rtCheckBoxMaxDays.Enabled = controlsEnabled;
            rtNumericMaxDays.Enabled = controlsEnabled && rtCheckBoxMaxDays.Checked;
            rtLabelDaysSuffix.Enabled = controlsEnabled && rtCheckBoxMaxDays.Checked;
            rtCheckBoxMaxFiles.Enabled = controlsEnabled;
            rtNumericMaxFiles.Enabled = controlsEnabled && rtCheckBoxMaxFiles.Checked;
            rtLabelFilesSuffix.Enabled = controlsEnabled && rtCheckBoxMaxFiles.Checked;
            rtLabelAction.Enabled = controlsEnabled;
            rtComboBoxAction.Enabled = controlsEnabled;
            rtLabelBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtTextBoxBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtButtonBrowseBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtCheckBoxPerSessionSubfolder.Enabled = controlsEnabled && backupSelected;
            rtCheckBoxDryRun.Enabled = controlsEnabled;
            rtCheckBoxIncludeSubfolders.Enabled = controlsEnabled;
            rtButtonCleanupNow.Enabled = controlsEnabled && hasSaveFolder;

            if (!hasSaveFolder && !_isRetentionOperationRunning)
            {
                rtTextBoxCleanupResult.Text = "Select a screenshot save folder on the main window before running cleanup.";
            }
        }

        private RetentionAction GetSelectedRetentionAction()
        {
            return (RetentionAction)Math.Clamp(
                rtComboBoxAction.SelectedIndex,
                (int)RetentionAction.MoveToRecycleBin,
                (int)RetentionAction.BackupThenDelete);
        }

        private async void RtButtonCleanupNow_Click(object sender, EventArgs e)
        {
            if (_isRetentionOperationRunning)
            {
                return;
            }

            if (!TryApplyRetentionSettings())
            {
                return;
            }

            _isRetentionOperationRunning = true;
            rtTextBoxCleanupResult.Text = "Generating cleanup preview...";
            UpdateRetentionControlState();

            try
            {
                using var retentionService = new ScreenshotRetentionService();
                var retentionSettings = Clone(_retentionSettings);
                var preview = await retentionService.PreviewCleanupAsync(_saveFolder, retentionSettings);
                rtLabelPreviewSummary.Text = FormatPreviewSummary(preview, retentionSettings);

                if (preview.TotalCandidateFiles == 0)
                {
                    rtTextBoxCleanupResult.Text = retentionSettings.DryRunMode
                        ? "Dry run completed. No screenshots match the current retention rules."
                        : "No screenshots match the current retention rules.";
                    return;
                }

                string confirmButtonLabel = retentionSettings.DryRunMode ? "run the dry run" : "continue with cleanup";
                DialogResult confirmation = MessageBox.Show(
                    this,
                    $"{FormatPreviewSummary(preview, retentionSettings)}\n\nSelect OK to {confirmButtonLabel}.",
                    "Preview Cleanup Summary",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                if (confirmation != DialogResult.OK)
                {
                    rtTextBoxCleanupResult.Text = "Cleanup canceled after preview.";
                    return;
                }

                rtTextBoxCleanupResult.Text = retentionSettings.DryRunMode
                    ? "Dry run in progress..."
                    : "Cleanup in progress...";

                var result = await retentionService.ExecuteCleanupAsync(_saveFolder, retentionSettings);
                rtTextBoxCleanupResult.Text = FormatCleanupResult(result);

                if (result.Errors.Count > 0)
                {
                    string errorPreview = string.Join(Environment.NewLine, result.Errors.Take(5));
                    MessageBox.Show(
                        this,
                        $"Some files could not be processed:\n\n{errorPreview}",
                        "Retention Cleanup",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                rtTextBoxCleanupResult.Text = $"Cleanup failed: {ex.Message}";
                MessageBox.Show(this, ex.Message, "Retention Cleanup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isRetentionOperationRunning = false;
                UpdateRetentionControlState();
            }
        }

        private void RtButtonBrowseBackupFolder_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select a backup folder (local or network shared path)",
                SelectedPath = Directory.Exists(rtTextBoxBackupFolder.Text) ? rtTextBoxBackupFolder.Text : _saveFolder,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                rtTextBoxBackupFolder.Text = dialog.SelectedPath;
            }
        }

        private static string FormatPreviewSummary(ScreenshotCleanupPreview preview, RetentionSettings settings)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Older than {settings.MaxDaysToRetain:N0} days: {(settings.MaxDaysEnabled ? preview.FilesOlderThanMaxDays : 0):N0} file(s)");
            builder.AppendLine($"Exceeding {settings.MaxFilesToRetain:N0} retained files: {(settings.MaxFilesEnabled ? preview.FilesExceedingCountLimit : 0):N0} file(s)");
            builder.AppendLine($"Total files to process: {preview.TotalCandidateFiles:N0}");
            builder.AppendLine($"Estimated space to free: {ScreenshotRetentionService.FormatSize(preview.TotalBytesToFree)}");
            builder.Append($"Action: {DescribeRetentionAction(settings)}");
            return builder.ToString();
        }

        private static string FormatCleanupResult(ScreenshotCleanupResult result)
        {
            var builder = new StringBuilder();
            builder.AppendLine(result.DryRunMode ? "Dry run completed." : "Cleanup completed.");
            builder.AppendLine($"Matched files: {result.Preview.TotalCandidateFiles:N0}");
            builder.AppendLine($"Moved to Recycle Bin: {result.RecycledFiles:N0}");
            builder.AppendLine($"Deleted permanently: {result.DeletedFiles:N0}");
            builder.AppendLine($"Backed up first: {result.BackedUpFiles:N0}");
            builder.AppendLine($"Failed: {result.FailedFiles:N0}");
            builder.Append($"Estimated space affected: {ScreenshotRetentionService.FormatSize(result.Preview.TotalBytesToFree)}");

            if (!string.IsNullOrWhiteSpace(result.BackupFolderUsed))
            {
                builder.AppendLine();
                builder.Append($"Backup folder: {result.BackupFolderUsed}");
            }

            return builder.ToString();
        }

        private static string DescribeRetentionAction(RetentionSettings settings)
        {
            string action = settings.Action switch
            {
                RetentionAction.MoveToRecycleBin => "Move to Recycle Bin",
                RetentionAction.DeletePermanently => "Delete permanently",
                RetentionAction.BackupThenDelete => $"Backup to folder then delete ({settings.BackupFolder})",
                _ => "Move to Recycle Bin"
            };

            if (settings.DryRunMode)
            {
                action += " [dry run]";
            }

            return action;
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
            using var picker = new AnnotationColorPalette(_annotationBackgroundColor);
            picker.StartPosition = FormStartPosition.Manual;

            // If true, the right of Color Picker will be aligned to the right of the Background Color
            // button. Else, the left of Color Picker will be aligned to the left of the button.
            // Aigning the right sides will help prevent the Color Picker from going outside the Settings dialog.
            bool alignPickerToRight = true;
            Point location;

            if (alignPickerToRight)
            {
                Point buttonBottomRight = anButtonAnnotationBackgroundColor.PointToScreen(
                    new Point(anButtonAnnotationBackgroundColor.Width,
                    anButtonAnnotationBackgroundColor.Height));

                location = new(buttonBottomRight.X - picker.Width, buttonBottomRight.Y);
            }
            else
            {
                location = anButtonAnnotationBackgroundColor.PointToScreen(
                    new Point(0, anButtonAnnotationBackgroundColor.Height));
            }


            Rectangle workingArea = Screen.FromControl(anButtonAnnotationBackgroundColor).WorkingArea;
            location.X = Math.Clamp(location.X, workingArea.Left, workingArea.Right - picker.Width);
            location.Y = Math.Clamp(location.Y, workingArea.Top, workingArea.Bottom - picker.Height);
            picker.Location = location;

            if (picker.ShowDialog(this) == DialogResult.OK)
            {
                SetAnnotationBackgroundColor(picker.SelectedColor);
            }
        }

        private sealed class AnnotationColorPalette : Form
        {
            private readonly ToolTip _toolTip = new();

            public Color? SelectedColor { get; private set; }

            public AnnotationColorPalette(Color? selectedColor)
            {
                const int FormClientWidth = 348;
                const int FormClientHeight = 800;
                const int FormPadding = 16;
                const int ControlSpacing = 3;

                SelectedColor = selectedColor;

                AutoScaleMode = AutoScaleMode.Dpi;
                AutoScaleDimensions = new SizeF(96F, 96F);
                BackColor = Color.White;
                ClientSize = new Size(FormClientWidth, FormClientHeight);
                ControlBox = true;
                FormBorderStyle = FormBorderStyle.FixedSingle;
                MaximizeBox = false;
                MinimizeBox = false;
                Padding = new Padding(FormPadding);
                ShowIcon = false;
                ShowInTaskbar = false;
                Text = "Highlight color";
                KeyPreview = true;

                int contentWidth = ClientSize.Width - Padding.Left - Padding.Right;


                //var title = new Label
                //{
                //    AutoSize = true,
                //    Font = new Font("Segoe UI Semibold", 10F),
                //    ForeColor = Color.FromArgb(32, 32, 32),
                //    Text = "Choose highlight color",
                //    Location = new Point(Padding.Left, Padding.Top),
                //    BackColor = Color.Aqua
                //};

                //var description = new Label
                //{
                //    AutoSize = true,
                //    Font = new Font("Segoe UI", 8.5F),
                //    ForeColor = Color.FromArgb(100, 100, 100),
                //    Text = "Select a color for the annotation background.",
                //    Location = new Point(16, 40)
                //};

                var noColorButton = new Button
                {
                    AccessibleName = "No color",
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(Padding.Left, Padding.Top),
                    Size = new Size(contentWidth /*344*/, 40),
                    Text = "No color",
                    UseVisualStyleBackColor = false
                };
                noColorButton.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
                noColorButton.Click += (_, _) => SelectColor(null);

                var palette = new FlowLayoutPanel
                {
                    //BackColor = Color.Black,
                    FlowDirection = FlowDirection.LeftToRight,
                    Location = new Point(Padding.Left, noColorButton.Bottom + ControlSpacing /*112*/),
                    Size = new Size(contentWidth, 292),
                    WrapContents = true
                };

                foreach ((string name, Color color) in AnnotationBackgroundColors)
                {
                    var colorButton = new Button
                    {
                        AccessibleName = name,
                        BackColor = color,
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(ControlSpacing),
                        Size = new Size(39, 30),
                        TabStop = true,
                        UseVisualStyleBackColor = false
                    };
                    colorButton.FlatAppearance.BorderColor = Color.FromArgb(190, 190, 190);
                    colorButton.FlatAppearance.MouseOverBackColor = color;
                    colorButton.Tag = color.ToArgb() == selectedColor?.ToArgb();
                    colorButton.Paint += PaintColorSelection;
                    colorButton.Click += (_, _) => SelectColor(color);
                    _toolTip.SetToolTip(colorButton, name);
                    palette.Controls.Add(colorButton);
                }

                var customButton = new Button
                {
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(Padding.Left, palette.Bottom + ControlSpacing /*414*/),
                    Size = new Size(contentWidth, 40),
                    Text = "Custom color...",
                    UseVisualStyleBackColor = false
                };
                customButton.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
                customButton.Click += ChooseCustomColor;

                var cancelButton = new Button
                {
                    AccessibleName = "Cancel",
                    BackColor = Color.White,
                    DialogResult = DialogResult.Cancel,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F),
                    Location = new Point(Padding.Left, customButton.Bottom + ControlSpacing),
                    Size = new Size(contentWidth, 40),
                    Text = "Cancel",
                    UseVisualStyleBackColor = false
                };
                cancelButton.FlatAppearance.BorderColor = Color.FromArgb(210, 210, 210);
                CancelButton = cancelButton;
                //Height = cancelButton.Top + cancelButton.Height + FormPadding;
                //ClientSize = new Size(ClientSize.Width, cancelButton.Top + cancelButton.Height + FormPadding);

                //Controls.Add(title);
                //Controls.Add(description);
                Controls.Add(noColorButton);
                Controls.Add(palette);
                Controls.Add(customButton);
                Controls.Add(cancelButton);

                ClientSize = new Size(ClientSize.Width, cancelButton.Bottom + Padding.Bottom);

                Shown += (_, _) => ActiveControl = null;
                Deactivate += (_, _) =>
                {
                    if (!IsHandleCreated || IsDisposed || Disposing)
                    {
                        return;
                    }

                    BeginInvoke(() =>
                    {
                        if (IsDisposed || Disposing || ContainsFocus)
                        {
                            return;
                        }

                        foreach (Form ownedForm in OwnedForms)
                        {
                            if (ownedForm.ContainsFocus)
                            {
                                return;
                            }
                        }

                        DialogResult = DialogResult.Cancel;
                    });
                };
                KeyDown += (_, args) =>
                {
                    if (args.KeyCode == Keys.Escape)
                    {
                        DialogResult = DialogResult.Cancel;
                    }
                };
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    _toolTip.Dispose();
                }

                base.Dispose(disposing);
            }

            private void SelectColor(Color? color)
            {
                SelectedColor = color;
                DialogResult = DialogResult.OK;
            }

            private void ChooseCustomColor(object? sender, EventArgs e)
            {
                using var dialog = new ColorDialog
                {
                    AnyColor = true,
                    Color = SelectedColor ?? Color.White,
                    FullOpen = true
                };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    SelectColor(dialog.Color);
                }
            }

            private void PaintColorSelection(object? sender, PaintEventArgs e)
            {
                if (sender is not Button { Tag: true } button)
                {
                    return;
                }

                using var pen = new Pen(Color.FromArgb(32, 32, 32), 2);
                Rectangle bounds = new(1, 1, button.ClientSize.Width - 3, button.ClientSize.Height - 3);
                e.Graphics.DrawRectangle(pen, bounds);
            }
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

        private static ImageProcessingSettings Clone(ImageProcessingSettings settings)
        {
            return new ImageProcessingSettings
            {
                ColorMode = settings.ColorMode,
                HighContrast = settings.HighContrast,
                NoiseReduction = settings.NoiseReduction,
                ColorTemperature = settings.ColorTemperature
            };
        }

        private static RetentionSettings Clone(RetentionSettings settings)
        {
            return new RetentionSettings
            {
                AutoCleanupEnabled = settings.AutoCleanupEnabled,
                MaxDaysEnabled = settings.MaxDaysEnabled,
                MaxDaysToRetain = settings.MaxDaysToRetain,
                MaxFilesEnabled = settings.MaxFilesEnabled,
                MaxFilesToRetain = settings.MaxFilesToRetain,
                Action = settings.Action,
                BackupFolder = settings.BackupFolder,
                PerSessionSubfolder = settings.PerSessionSubfolder,
                DryRunMode = settings.DryRunMode,
                IncludeSubfolders = settings.IncludeSubfolders
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

        private static bool AreEqual(ImageProcessingSettings left, ImageProcessingSettings right)
        {
            return left.ColorMode == right.ColorMode
                && left.HighContrast == right.HighContrast
                && left.NoiseReduction == right.NoiseReduction
                && left.ColorTemperature == right.ColorTemperature;
        }

        private static bool AreEqual(RetentionSettings left, RetentionSettings right)
        {
            return left.AutoCleanupEnabled == right.AutoCleanupEnabled
                && left.MaxDaysEnabled == right.MaxDaysEnabled
                && left.MaxDaysToRetain == right.MaxDaysToRetain
                && left.MaxFilesEnabled == right.MaxFilesEnabled
                && left.MaxFilesToRetain == right.MaxFilesToRetain
                && left.Action == right.Action
                && string.Equals(left.BackupFolder, right.BackupFolder, StringComparison.Ordinal)
                && left.DryRunMode == right.DryRunMode
                && left.IncludeSubfolders == right.IncludeSubfolders;
        }
    }
}
