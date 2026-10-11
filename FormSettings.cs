using System.Drawing.Drawing2D;
using System.Reflection;
using System.Text;
using static System.Windows.Forms.DataFormats;
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
        private readonly HotkeySettings _originalHotkeySettings;
        private readonly DedupeSettings _originalDedupeSettings;

        private readonly WatermarkSettings _watermarkSettings;
        private readonly AnnotationSettings _annotationSettings;
        private readonly DownscaleSettings _downscaleSettings;
        private readonly ImageProcessingSettings _imageProcessingSettings;
        private readonly RetentionSettings _retentionSettings;
        private readonly HotkeySettings _hotkeySettings;
        private readonly DedupeSettings _dedupeSettings;
        private readonly GlobalHotkeyManager _globalHotkeyManager;

        private Icon? _appIcon;
        private Font _watermarkFont = new("Segoe UI", 24);
        private Font _annotationFont = new("Segoe UI", 24);
        private Color _annotationFontColor = Color.White;
        private Color? _annotationBackgroundColor;
        private int _annotationSelectionStart;
        private int _annotationSelectionLength;
        private bool _isUpdatingDownscaleUi;
        private bool _isUpdatingHotkeyList;
        private bool _isRetentionOperationRunning;
        private int _hotkeyListSortColumn;
        private SortOrder _hotkeyListSortOrder = SortOrder.Ascending;
        private readonly List<ToolStripDropDown> _ownedMenus = [];

        private readonly record struct HotkeyCandidate(Keys Modifiers, Keys Key);
        public WatermarkSettings WatermarkSettings => _watermarkSettings;
        public AnnotationSettings AnnotationSettings => _annotationSettings;
        public DownscaleSettings DownscaleSettings => _downscaleSettings;
        public ImageProcessingSettings ImageProcessingSettings => _imageProcessingSettings;
        public RetentionSettings RetentionSettings => _retentionSettings;
        public DedupeSettings DedupeSettings => _dedupeSettings;
        public HotkeySettings HotkeySettings => _hotkeySettings;

        public bool WatermarkSettingsChanged => !AreEqual(_originalWatermarkSettings, _watermarkSettings);
        public bool AnnotationSettingsChanged => !AreEqual(_originalAnnotationSettings, _annotationSettings);
        public bool DownscaleSettingsChanged => !AreEqual(_originalDownscaleSettings, _downscaleSettings);
        public bool ImageProcessingSettingsChanged => !AreEqual(_originalImageProcessingSettings, _imageProcessingSettings);
        public bool RetentionSettingsChanged => !AreEqual(_originalRetentionSettings, _retentionSettings);
        public bool DedupeSettingsChanged => !AreEqual(_originalDedupeSettings, _dedupeSettings);
        public bool HotkeySettingsChanged => !AreEqual(_originalHotkeySettings, _hotkeySettings);

        public FormSettings(
            WatermarkSettings watermarkSettings,
            AnnotationSettings annotationSettings,
            DownscaleSettings downscaleSettings,
            ImageProcessingSettings imageProcessingSettings,
            RetentionSettings retentionSettings,
            HotkeySettings hotkeySettings,
            DedupeSettings dedupeSettings,
            string saveFolder,
            GlobalHotkeyManager? globalHotkeyManager)
        {
            InitializeComponent();

            tabControlSettings.Padding = new Point(16, 8);
            tabControlSettings.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlSettings.DrawItem += TabControlSettings_DrawItem;

            _saveFolder = saveFolder;
            _originalWatermarkSettings = Clone(watermarkSettings);
            _originalAnnotationSettings = Clone(annotationSettings);
            _originalDownscaleSettings = Clone(downscaleSettings);
            _originalImageProcessingSettings = Clone(imageProcessingSettings);
            _originalRetentionSettings = Clone(retentionSettings);
            _originalHotkeySettings = Clone(hotkeySettings);
            _originalDedupeSettings = Clone(dedupeSettings);
            _watermarkSettings = Clone(watermarkSettings);
            _annotationSettings = Clone(annotationSettings);
            _downscaleSettings = Clone(downscaleSettings);
            _imageProcessingSettings = Clone(imageProcessingSettings);
            _retentionSettings = Clone(retentionSettings);
            _hotkeySettings = Clone(hotkeySettings);
            _dedupeSettings = Clone(dedupeSettings);
            _globalHotkeyManager = globalHotkeyManager
                ?? throw new ArgumentNullException(nameof(globalHotkeyManager));

            InitializeIcon();
            InitializeTabWatermark();
            InitializeTabAnnotation();
            InitializeTabDownscale();
            InitializeTabImageProcessing();
            InitializeTabDedupe();
            InitializeTabRetention();
            InitializeTabHotkeys();
            ApplyDefaultButtonIcon();

            ToolTip toolTip = new();
            ConfigureTooltips(toolTip);
        }

        bool FluentAvailable()
        {
            return FontFamily.Families.Any(f =>
                f.Name.Equals("Segoe Fluent Icons", StringComparison.OrdinalIgnoreCase));
        }

        void ApplyDefaultButtonIcon()
        {
            Font font;
            string glyph;

            if (FluentAvailable())
            {
                font = new Font("Segoe Fluent Icons", 12F);
                glyph = "\uE777"; // "\uE777"
            }
            else
            {
                // Fallback for Windows 10 MDL2
                font = new Font("Segoe MDL2 Assets", 12F);
                glyph = "\uE72C"; // "\uE72C"
            }

            wmButtonCommonDefault.Font = font;
            wmButtonUseImageDefault.Font = font;
            wmButtonUseTextDefault.Font = font;
            anButtonDefault.Font = font;
            dsButtonProcessingDefault.Font = font;
            dsButtonModesDefault.Font = font;
            ipButtonAdvProcessingDefault.Font = font;
            ipButtonColorModeDefault.Font = font;
            ddButtonDefault.Font = font;
            rtButtonActionDefault.Font = font;
            rtButtonActionDefault.Font = font;
            rtButtonSelectionDefault.Font = font;
            hkButtonDefault.Font = font;

            wmButtonCommonDefault.Text = glyph;
            wmButtonUseImageDefault.Text = glyph;
            wmButtonUseTextDefault.Text = glyph;
            anButtonDefault.Text = glyph;
            dsButtonProcessingDefault.Text = glyph;
            dsButtonModesDefault.Text = glyph;
            ipButtonAdvProcessingDefault.Text = glyph;
            ipButtonColorModeDefault.Text = glyph;
            ddButtonDefault.Text = glyph;
            rtButtonActionDefault.Text = glyph;
            rtButtonSelectionDefault.Text = glyph;
            hkButtonDefault.Text = glyph;
        }

        private void ConfigureTooltips(ToolTip toolTip)
        {
            toolTip.AutoPopDelay = Constants.TooltipAutoPopDelay;
            toolTip.InitialDelay = Constants.TooltipInitialDelay;
            toolTip.ReshowDelay = Constants.TooltipReshowDelay;
            toolTip.ShowAlways = Constants.TooltipShowAlways;

            // Tooltips for Watermark tab
            toolTip.SetToolTip(wmButtonUseTextDefault, "Reset watermark text settings to their default values.");
            toolTip.SetToolTip(wmButtonUseImageDefault, "Reset watermark image settings to their default values.");
            toolTip.SetToolTip(wmButtonCommonDefault, "Reset watermark common settings to their default values.");

            // Tooltips for Annotation tab
            toolTip.SetToolTip(anButtonDefault, "Reset annotation settings to their default values.");

            // Tooltips for Downscale tab
            toolTip.SetToolTip(dsButtonModesDefault, "Reset resize settings to their default values.");
            toolTip.SetToolTip(dsButtonProcessingDefault, "Reset image processing settings to their default values.");
            toolTip.SetToolTip(dsLabelQuality, "Controls how the image is resized.\nHigher‑quality methods produce smoother results.");
            toolTip.SetToolTip(dsCheckBoxSharpen, "Adds a light sharpening pass to improve clarity after resizing.");
            toolTip.SetToolTip(dsCheckBoxSkipSmaller, "Avoids resizing when the screenshot is already smaller than the target size.");

            // Tooltips for Image Processing tab
            toolTip.SetToolTip(ipButtonColorModeDefault, "Reset color mode to its default value.");
            toolTip.SetToolTip(ipButtonAdvProcessingDefault, "Reset advanced processing settings to their default values.");

            // Tooltips for Duplicates tab
            toolTip.SetToolTip(ddButtonDefault, "Reset duplicate settings to their default values.");
            toolTip.SetToolTip(ddLabelEnable, "Enables or disables duplicate detection. When disabled, every capture is saved.");
            toolTip.SetToolTip(ddToggleEnable, toolTip.GetToolTip(ddLabelEnable));
            toolTip.SetToolTip(ddLabelSensitivity,
                "Controls how much visual change is required before a capture is considered meaningfully different.");
            toolTip.SetToolTip(ddTrackBarSensitivity,
                "Move right for higher sensitivity: smaller visual changes are more likely to be saved.\n" +
                "Move left to ignore more minor changes such as clocks, cursors, and animations.");
            toolTip.SetToolTip(ddLabelDynamicRegions,
                "Choose which screen areas are ignored during full-screen comparison.");
            toolTip.SetToolTip(ddComboBoxDynamicRegions,
                "Include all screen areas compares everything.\n" +
                "Exclude Windows taskbars ignores taskbar and clock changes on every monitor.\n" +
                "The manual options also apply the rectangles which you enter in the Excluded regions box.");
            toolTip.SetToolTip(ddCheckBoxPerActiveWindow,
                "When selected, each active window gets its own comparison reference.\n" +
                "This prevents switching between applications from making unrelated windows appear duplicated.");
            toolTip.SetToolTip(ddLabelTimeout,
                "Limits how long Dedupe may wait while comparing a capture.\n" +
                "If the timeout expires, Capillume saves the screenshow file anyway.");
            toolTip.SetToolTip(ddNumericTimeout,
                "Comparison timeout in milliseconds. A shorter value keeps capture responsive;\n" +
                "if the timeout is reached, Capillume fails open and saves the screenshot.");
            toolTip.SetToolTip(ddCheckBoxDryRun,
                "Reports duplicates but never skips a save.\n" +
                "Use this to tune sensitivity safely before enabling suppression.");
            toolTip.SetToolTip(ddCheckBoxShowNotifications,
                "Displays a tray notification when Dedupe skips a capture because no meaningful change was detected.");
            toolTip.SetToolTip(ddLabelExcludedRegions,
                "Enter screen rectangles to ignore during full-screen comparison.\n" +
                "Coordinates use the virtual desktop origin.");
            toolTip.SetToolTip(ddTextBoxExcludedRegions,
                "Enter one region per line in the format x,y,width,height (for example: 0,1040,1920,40).\n" +
                "These areas are ignored when a manual‑region mode is enabled, so any changes inside them\n" +
                "won’t trigger a new screenshot file.");
            toolTip.SetToolTip(ddComboBoxCompareMode,
                "Perceptual hash + SSIM is the balanced default.\n" +
                "Checksum + SSIM quickly rejects exact matches.\n" +
                "Pixel difference only is fastest and simplest.\n" +
                "SSIM only provides the most structural comparison but uses more processing time.");
            toolTip.SetToolTip(ddNumericMinimumSaveInterval,
                "Optional maximum time between saved captures. Set to 0 to disable.\n" +
                "When set, Capillume saves at least one capture after this many minutes even if the screen is unchanged.");
            toolTip.SetToolTip(ddCheckBoxUpdateReferenceOnSave,
                "Recommended: keep enabled so a skipped duplicate does not become the new reference.\n" +
                "Disable only if you want every capture to advance the comparison reference.");
            toolTip.SetToolTip(ddCheckBoxDiagnostics,
                "Writes Dedupe decision details such as hash distance, SSIM score, pixel difference,\n" +
                "and the reason to the Visual Studio Debug output.");

            // Tooltips for Retention tab
            toolTip.SetToolTip(rtButtonSelectionDefault, "Reset selection settings to their default values.");
            toolTip.SetToolTip(rtButtonActionDefault, "Reset action settings to their default values.");
            toolTip.SetToolTip(rtToggleSwitchAutoCleanup, "Runs screenshot file cleanup after each screenshot is saved.");
            toolTip.SetToolTip(rtCheckBoxDryRun, "Preview what would be removed without actually deleting any files.");
            toolTip.SetToolTip(rtCheckBoxIncludeSubfolders, "Also evaluate screenshots inside subfolders of the save location.");
            toolTip.SetToolTip(rtCheckBoxPerSessionSubfolder, "Organize each cleanup backup in a separate timestamped folder.");

            // Tooltips for Hotkeys tab
            toolTip.SetToolTip(hkButtonDefault, "Reset hotkey settings to their default values.");
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

        private enum WatermarkResetDefaultScope { None, Text, Image, Common, All }

        private void InitializeTabWatermark(WatermarkResetDefaultScope resetDefaultScope = WatermarkResetDefaultScope.None)
        {
            tabPageWatermark.SuspendLayout();
            if (resetDefaultScope != WatermarkResetDefaultScope.None)
            {
                // Reset settings object to defaults
                if (resetDefaultScope == WatermarkResetDefaultScope.Text || resetDefaultScope == WatermarkResetDefaultScope.All)
                {
                    //_watermarkSettings.UseText = false;
                    _watermarkSettings.WatermarkText = Constants.WatermarkTextDefault;
                    _watermarkSettings.WatermarkTextFontFamily = Constants.WatermarkTextFontFamilyDefault;
                    _watermarkSettings.WatermarkTextFontSize = Constants.WatermarkTextFontSizeDefault;
                    _watermarkSettings.WatermarkTextFontStyle = Constants.WatermarkTextFontStyleDefault;
                }
                if (resetDefaultScope == WatermarkResetDefaultScope.Image || resetDefaultScope == WatermarkResetDefaultScope.All)
                {
                    //_watermarkSettings.UseImage = false;
                    _watermarkSettings.WatermarkImagePath = Constants.WatermarkImagePathDefault;
                    _watermarkSettings.WatermarkImageScale = Constants.WatermarkImageScaleDefault;
                }
                if (resetDefaultScope == WatermarkResetDefaultScope.Common || resetDefaultScope == WatermarkResetDefaultScope.All)
                {
                    _watermarkSettings.WatermarkOpacity = Constants.WatermarkOpacityDefault;
                    _watermarkSettings.WatermarkPosition = Constants.WatermarkPositionDefault;
                    _watermarkSettings.WatermarkRotation = Constants.WatermarkRotationDefault;
                }
            }

            // Load settings into UI
            wmToggleUseImage.Checked = _watermarkSettings.UseImage;

            if (resetDefaultScope == WatermarkResetDefaultScope.None || resetDefaultScope == WatermarkResetDefaultScope.Text)
            {
                _watermarkFont.Dispose();
                _watermarkFont = new Font(
                    _watermarkSettings.WatermarkTextFontFamily,
                    _watermarkSettings.WatermarkTextFontSize,
                    _watermarkSettings.WatermarkTextFontStyle);

                wmToggleUseText.Checked = _watermarkSettings.UseText;
                wmTextBoxWatermarkText.Text = _watermarkSettings.WatermarkText;
                SetWatermarkTextDescription();
            }

            if (resetDefaultScope == WatermarkResetDefaultScope.None || resetDefaultScope == WatermarkResetDefaultScope.Image)
            {
                wmTextBoxWatermarkImagePath.Text = _watermarkSettings.WatermarkImagePath;
                wmTrackBarWatermarkImageScale.Value = Math.Clamp(
                    _watermarkSettings.WatermarkImageScale,
                    Constants.WatermarkImageScaleMin,
                    Constants.WatermarkImageScaleMax);
                wmLabelWatermarkImageScaleValue.Text = $"{wmTrackBarWatermarkImageScale.Value}%";
                UpdateWatermarkImagePreview();
            }

            if (resetDefaultScope == WatermarkResetDefaultScope.None || resetDefaultScope == WatermarkResetDefaultScope.Common)
            {
                wmTrackBarOpacity.Value = Math.Clamp(
                    value: _watermarkSettings.WatermarkOpacity,
                    min: Constants.WatermarkOpacityMin,
                    max: Constants.WatermarkOpacityMax);

                wmLabelOpacityValue.Text = $"{wmTrackBarOpacity.Value}%";

                wmComboBoxWatermarkPosition.SelectedItem = _watermarkSettings.WatermarkPosition;
                if (wmComboBoxWatermarkPosition.SelectedIndex < 0)
                {
                    wmComboBoxWatermarkPosition.SelectedItem = Constants.WatermarkPositionDefault;
                }

                wmComboBoxWatermarkRotation.SelectedIndex = Math.Clamp(_watermarkSettings.WatermarkRotation / 90, 0, 3);
            }

            SetupWatermarkTextboxPlaceholder(wmTextBoxWatermarkText);
            UpdateWatermarkControlState();
            tabPageWatermark.ResumeLayout(false);
        }

        private void InitializeTabAnnotation(bool resetToDefaults = false)
        {
            tabPageAnnotation.SuspendLayout();
            if (resetToDefaults)
            {
                // Reset settings object to defaults
                _annotationSettings.AnnotationFormat = Constants.AnnotationFormatDefault;
                _annotationSettings.AnnotationFontFamily = Constants.AnnotationFontFamilyDefault;
                _annotationSettings.AnnotationFontSize = Constants.AnnotationFontSizeDefault;
                _annotationSettings.AnnotationFontStyle = Constants.AnnotationFontStyleDefault;
                _annotationSettings.AnnotationFontColorArgb = Constants.AnnotationFontColorArgbDefault;
                _annotationSettings.AnnotationBackgroundColorArgb = Constants.AnnotationBackgroundColorArgbDefault;
                _annotationSettings.AnnotationOpacity = Constants.AnnotationOpacityDefault;
            }

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
            tabPageAnnotation.ResumeLayout(false);
        }

        private enum DownscaleResetDefaultScope { None, Resize, Processing, All }

        private void InitializeTabDownscale(DownscaleResetDefaultScope resetDefaultScope = DownscaleResetDefaultScope.None)
        {
            tabPageDownscale.SuspendLayout();
            tabPageDownscale.AutoScroll = true;

            if (resetDefaultScope != DownscaleResetDefaultScope.None)
            {
                // Reset settings object to defaults
                if (resetDefaultScope == DownscaleResetDefaultScope.Resize || resetDefaultScope == DownscaleResetDefaultScope.All)
                {
                    _downscaleSettings.ResizeMode = Constants.DownscaleResizeModeDefault;
                    _downscaleSettings.ResizeTargetHeight = Constants.DownscaleResizeTargetHeightDefault;
                    _downscaleSettings.ResizePercentage = Constants.DownscaleResizePercentageDefault;
                    _downscaleSettings.ResizeMaxWidth = Constants.DownscaleResizeMaxWidthDefault;
                    _downscaleSettings.ResizeBoundingBoxWidth = Constants.DownscaleResizeBoundingWidthDefault;
                    _downscaleSettings.ResizeBoundingBoxHeight = Constants.DownscaleResizeBoundingHeightDefault;
                }
                if (resetDefaultScope == DownscaleResetDefaultScope.Processing || resetDefaultScope == DownscaleResetDefaultScope.All)
                {
                    _downscaleSettings.Quality = Constants.DownscaleQualityDefault;
                    _downscaleSettings.SharpenAfterResize = Constants.DownscaleSharpenAfterResizeDefault;
                    _downscaleSettings.SkipSmallerImages = Constants.DownscaleSkipSmallerImagesDefault;
                    _downscaleSettings.FullScreenOnly = Constants.DownscaleFullScreenOnlyDefault;
                    _downscaleSettings.LossyFormatsOnly = Constants.DownscaleLossyFormatsOnlyDefault;
                }
            }

            dsComboBoxQuality.SelectedIndexChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxSharpen.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxSkipSmaller.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxFullScreenOnly.CheckedChanged += (_, _) => UpdateDownscaleSummary();
            dsCheckBoxLossyOnly.CheckedChanged += (_, _) => UpdateDownscaleSummary();

            _isUpdatingDownscaleUi = true;
            dsToggleEnable.Checked = _downscaleSettings.Enabled;

            if (resetDefaultScope == DownscaleResetDefaultScope.None || resetDefaultScope == DownscaleResetDefaultScope.Resize)
            {
                dsComboBoxTargetHeight.Items.Clear();
                dsComboBoxTargetHeight.Items.AddRange(DownscaleHeightPresets.Select(p => p.Label).Append("Custom").ToArray());
                dsNumericTargetHeight.Minimum = Constants.DownscaleResizeTargetHeightMin;
                dsNumericTargetHeight.Maximum = Constants.DownscaleResizeTargetHeightMax;
                dsComboBoxPercentage.Items.Clear();
                dsComboBoxPercentage.Items.AddRange(DownscalePercentagePresets.Select(p => $"{p}%").Append("Custom").ToArray());
                dsNumericPercentage.Maximum = Constants.DownscaleResizePercentageMax;
                dsNumericPercentage.Minimum = Constants.DownscaleResizePercentageMin;
                dsComboBoxMaxWidth.Items.Clear();
                dsComboBoxMaxWidth.Items.AddRange(DownscaleWidthPresets.Select(p => p.ToString()).Append("Custom").ToArray());
                dsNumericMaxWidth.Maximum = Constants.DownscaleResizeMaxWidthMax;
                dsNumericMaxWidth.Minimum = Constants.DownscaleResizeMaxWidthMin;
                dsComboBoxBoundingBox.Items.Clear();
                dsComboBoxBoundingBox.Items.AddRange(DownscaleBoundingBoxPresets.Select(p => p.Label).Append("Custom").ToArray());
                dsNumericBoundingWidth.Maximum = Constants.DownscaleResizeMaxWidthMax;
                dsNumericBoundingWidth.Minimum = Constants.DownscaleResizeMaxWidthMin;
                dsNumericBoundingHeight.Maximum = Constants.DownscaleResizeTargetHeightMax;
                dsNumericBoundingHeight.Minimum = Constants.DownscaleResizeTargetHeightMin;

                dsNumericTargetHeight.Value = _downscaleSettings.ResizeTargetHeight;
                dsNumericPercentage.Value = _downscaleSettings.ResizePercentage;
                dsNumericMaxWidth.Value = _downscaleSettings.ResizeMaxWidth;
                dsNumericBoundingWidth.Value = _downscaleSettings.ResizeBoundingBoxWidth;
                dsNumericBoundingHeight.Value = _downscaleSettings.ResizeBoundingBoxHeight;
                SelectDownscaleMode(_downscaleSettings.ResizeMode);
                SyncDownscalePresetSelections();
            }

            if (resetDefaultScope == DownscaleResetDefaultScope.None || resetDefaultScope == DownscaleResetDefaultScope.Processing)
            {
                dsComboBoxQuality.Items.Clear();
                dsComboBoxQuality.Items.AddRange(
                [
                    "High Quality (Bicubic)",
                    "Balanced (Bilinear)",
                    "Fast (Nearest Neighbor)"
                ]);

                dsComboBoxQuality.SelectedIndex = (int)_downscaleSettings.Quality;
                dsCheckBoxSharpen.Checked = _downscaleSettings.SharpenAfterResize;
                dsCheckBoxSkipSmaller.Checked = _downscaleSettings.SkipSmallerImages;
                dsCheckBoxFullScreenOnly.Checked = _downscaleSettings.FullScreenOnly;
                dsCheckBoxLossyOnly.Checked = _downscaleSettings.LossyFormatsOnly;
            }
            _isUpdatingDownscaleUi = false;

            dsLabelDefaultSize.Text = GetDefaultCaptureSizeDescription();
            UpdateDownscaleSummary();
            UpdateDownscaleControlState();
            tabPageDownscale.ResumeLayout(false);
        }

        private enum ImageProcessingResetDefaultScope { None, ColorMode, AdvProcessing, All }

        private void InitializeTabImageProcessing(ImageProcessingResetDefaultScope resetDefaultScope = ImageProcessingResetDefaultScope.None)
        {
            tabPageImageProcessing.SuspendLayout();

            if (resetDefaultScope != ImageProcessingResetDefaultScope.None)
            {
                // Reset settings object to defaults
                if (resetDefaultScope == ImageProcessingResetDefaultScope.ColorMode || resetDefaultScope == ImageProcessingResetDefaultScope.All)
                {
                    _imageProcessingSettings.ColorMode = Constants.ImageProcColorModeDefault;
                }
                if (resetDefaultScope == ImageProcessingResetDefaultScope.AdvProcessing || resetDefaultScope == ImageProcessingResetDefaultScope.All)
                {
                    _imageProcessingSettings.HighContrast = Constants.ImageProcHighContrastDefault;
                    _imageProcessingSettings.NoiseReduction = Constants.ImageProcNoiseReductionDefault;
                    _imageProcessingSettings.ColorTemperature = Constants.ImageProcColorTemperatureDefault;
                }
            }

            if (resetDefaultScope == ImageProcessingResetDefaultScope.None || resetDefaultScope == ImageProcessingResetDefaultScope.ColorMode)
            {
                SelectImageColorMode(_imageProcessingSettings.ColorMode);
            }

            if (resetDefaultScope == ImageProcessingResetDefaultScope.None || resetDefaultScope == ImageProcessingResetDefaultScope.AdvProcessing)
            {
                ipCheckBoxHighContrast.Checked = _imageProcessingSettings.HighContrast;
                ipCheckBoxNoiseReduction.Checked = _imageProcessingSettings.NoiseReduction;
                ipComboBoxColorTemperature.SelectedIndex = (int)_imageProcessingSettings.ColorTemperature;

                if (ipComboBoxColorTemperature.SelectedIndex < 0)
                {
                    ipComboBoxColorTemperature.SelectedIndex = (int)ImageProcessColorTempMode.Neutral;
                }
            }

            tabPageImageProcessing.ResumeLayout(false);
        }

        private void InitializeTabDedupe(bool resetToDefaults = false)
        {
            tabPageDedupe.SuspendLayout();

            ddComboBoxDynamicRegions.Items.Clear();
            ddComboBoxDynamicRegions.Items.AddRange(DedupeDynamicRegionModeHelper.Labels);

            ddComboBoxCompareMode.Items.Clear();
            ddComboBoxCompareMode.Items.AddRange(DedupeCompareModeHelper.Labels);

            if (resetToDefaults)
            {
                // Reset settings object to defaults
                _dedupeSettings.CompareMode = Constants.DedupeCompareModeDefault;
                _dedupeSettings.Sensitivity = Constants.DedupeSensitivityDefault;
                _dedupeSettings.TimeoutMilliseconds = Constants.DedupeTimeoutMillisecondsDefault;
                _dedupeSettings.DynamicRegionMode = Constants.DedupeDynamicRegionModeDefault;
                _dedupeSettings.ExcludeRegions = Constants.DedupeExcludeRegionsDefault;
                _dedupeSettings.ComparePerActiveWindow = Constants.DedupeComparePerActiveWindowDefault;
                _dedupeSettings.UpdateReferenceOnlyOnSave = Constants.DedupeUpdateReferenceOnlyOnSaveDefault;
                _dedupeSettings.MinimumMinutesBetweenSaves = Constants.DedupeMinimumMinutesBetweenSavesDefault;
                _dedupeSettings.DryRun = Constants.DedupeDryRunDefault;
                _dedupeSettings.ShowSkippedNotifications = Constants.DedupeShowSkippedNotificationsDefault;
                _dedupeSettings.EnableDiagnostics = Constants.DedupeEnableDiagnosticsDefault;
            }

            ddToggleEnable.Checked = _dedupeSettings.Enabled;
            //ddComboBoxCompareMode.SelectedIndex = (int)_dedupeSettings.CompareMode;
            ddComboBoxCompareMode.SelectedIndex = Array.FindIndex(DedupeCompareModeHelper.Items, item => item.Mode == _dedupeSettings.CompareMode);
            ddTrackBarSensitivity.Value = _dedupeSettings.Sensitivity;
            ddLabelSensitivityValue.Text = $"{_dedupeSettings.Sensitivity}%";
            //ddComboBoxDynamicRegions.SelectedIndex = (int)_dedupeSettings.DynamicRegionMode;
            ddComboBoxDynamicRegions.SelectedIndex = Array.FindIndex(DedupeDynamicRegionModeHelper.Items, item => item.Mode == _dedupeSettings.DynamicRegionMode);
            ddCheckBoxPerActiveWindow.Checked = _dedupeSettings.ComparePerActiveWindow;
            ddNumericTimeout.Value = _dedupeSettings.TimeoutMilliseconds;
            ddCheckBoxDryRun.Checked = _dedupeSettings.DryRun;
            ddCheckBoxShowNotifications.Checked = _dedupeSettings.ShowSkippedNotifications;
            ddNumericMinimumSaveInterval.Value = _dedupeSettings.MinimumMinutesBetweenSaves;
            ddCheckBoxUpdateReferenceOnSave.Checked = _dedupeSettings.UpdateReferenceOnlyOnSave;
            ddCheckBoxDiagnostics.Checked = _dedupeSettings.EnableDiagnostics;
            ddTextBoxExcludedRegions.Text = string.Join(
                Environment.NewLine,
                _dedupeSettings.ExcludeRegions.Select(region => $"{region.X},{region.Y},{region.Width},{region.Height}"));
            UpdateDedupeControlState();
            tabPageDedupe.ResumeLayout(false);
        }

        private enum RetentionResetDefaultScope { None, Selection, Action, All }

        private void InitializeTabRetention(RetentionResetDefaultScope resetDefaultScope = RetentionResetDefaultScope.None)
        {
            tabPageRetention.SuspendLayout();

            if (resetDefaultScope != RetentionResetDefaultScope.None)
            {
                // Reset settings object to defaults
                if (resetDefaultScope == RetentionResetDefaultScope.Selection || resetDefaultScope == RetentionResetDefaultScope.All)
                {
                    _retentionSettings.MaxDaysEnabled = Constants.RetentionMaxDaysEnabledDefault;
                    _retentionSettings.MaxDaysToRetain = Constants.RetentionDaysDefault;
                    _retentionSettings.MaxFilesEnabled = Constants.RetentionMaxFilesEnabledDefault;
                    _retentionSettings.MaxFilesToRetain = Constants.RetentionFileCountDefault;
                    _retentionSettings.IncludeSubfolders = Constants.RetentionIncludeSubfoldersDefault;
                }
                if (resetDefaultScope == RetentionResetDefaultScope.Action || resetDefaultScope == RetentionResetDefaultScope.All)
                {
                    _retentionSettings.Action = Constants.RetentionActionDefault;
                    //_retentionSettings.BackupFolder = Constants.RetentionBackupFolderDefault;
                    _retentionSettings.PerSessionSubfolder = Constants.RetentionPerSessionSubfolderDefault;
                    // DryRunMode is not part of the Action tab group, so don't reset.
                    // It is not part of any GroupBox.
                    //_retentionSettings.DryRunMode = Constants.RetentionDryRunModeDefault;
                }
            }

            rtToggleSwitchAutoCleanup.Checked = _retentionSettings.AutoCleanupEnabled;
            rtCheckBoxDryRun.Checked = _retentionSettings.DryRunMode;

            if (resetDefaultScope == RetentionResetDefaultScope.None || resetDefaultScope == RetentionResetDefaultScope.Selection)
            {
                rtCheckBoxMaxDays.Checked = _retentionSettings.MaxDaysEnabled;
                rtNumericMaxDays.Value = Math.Clamp(_retentionSettings.MaxDaysToRetain, Constants.RetentionDaysMin, Constants.RetentionDaysMax);
                rtCheckBoxMaxFiles.Checked = _retentionSettings.MaxFilesEnabled;
                rtNumericMaxFiles.Value = Math.Clamp(_retentionSettings.MaxFilesToRetain, Constants.RetentionFileCountMin, Constants.RetentionFileCountMax);
                rtCheckBoxIncludeSubfolders.Checked = _retentionSettings.IncludeSubfolders;
            }

            if (resetDefaultScope == RetentionResetDefaultScope.None || resetDefaultScope == RetentionResetDefaultScope.Action)
            {
                rtComboBoxAction.Items.Clear();
                rtComboBoxAction.Items.AddRange(RetentionActionHelper.Labels);
                //rtComboBoxAction.SelectedIndex = (int)_retentionSettings.Action;
                rtComboBoxAction.SelectedIndex = Array.FindIndex(RetentionActionHelper.Items, item => item.Mode == _retentionSettings.Action);
                rtTextBoxBackupFolder.Text = _retentionSettings.BackupFolder;
                rtCheckBoxPerSessionSubfolder.Checked = _retentionSettings.PerSessionSubfolder;
            }

            UpdateRetentionControlState();
            tabPageRetention.ResumeLayout(false);
        }

        private void InitializeTabHotkeys(bool resetToDefaults = false)
        {
            tabPageHotkeys.SuspendLayout();

            //if (_hotkeySettings.HasConflict)
            //{
            //    MessageBox.Show(
            //        "The current hotkey combination is already in use by another application.\n" +
            //        "Please choose a different combination.",
            //        "Hotkey Conflict",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Warning);
            //}

            if (resetToDefaults)
            {
                // Reset settings object to defaults
                _hotkeySettings.Modifiers = Constants.HotkeyModifiersDefault;
                _hotkeySettings.Key = Constants.HotkeyKeyDefault;
                hkToggleEnable.Checked = true;
            }
            else
            {
                hkToggleEnable.Checked = _hotkeySettings.Enabled;
            }

            if (_hotkeySettings.Modifiers == Keys.None)
            {
                _hotkeySettings.Modifiers = Constants.HotkeyModifiersDefault;
            }

            if (_hotkeySettings.Key == Keys.None)
            {
                _hotkeySettings.Key = Constants.HotkeyKeyDefault;
            }

            hkTextBoxShortcut.Text = FormatHotkey(_hotkeySettings.Modifiers, _hotkeySettings.Key);
            PopulateAvailableHotkeys();
            UpdateHotkeyControlState();

            tabPageHotkeys.ResumeLayout(false);
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

            tabControlSettings.SelectedTab = tabPageHotkeys;
            if (!TryApplyHotkeySettings())
            {
                return;
            }

            tabControlSettings.SelectedTab = tabPageDedupe;
            if (!TryApplyDedupeSettings())
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

        private void DdSettingChanged(object? sender, EventArgs e)
        {
            ddLabelSensitivityValue.Text = $"{ddTrackBarSensitivity.Value}%";
            UpdateDedupeControlState();
        }

        private void UpdateDedupeControlState()
        {
            bool enabled = ddToggleEnable.Checked;
            ddLabelSensitivity.Enabled = enabled;
            ddTrackBarSensitivity.Enabled = enabled;
            ddLabelSensitivityValue.Enabled = enabled;
            ddLabelDynamicRegions.Enabled = enabled;
            ddComboBoxDynamicRegions.Enabled = enabled;
            ddCheckBoxPerActiveWindow.Enabled = enabled;
            ddLabelTimeout.Enabled = enabled;
            ddLabelTimeoutUnit.Enabled = enabled;
            ddNumericTimeout.Enabled = enabled;
            ddCheckBoxDryRun.Enabled = enabled;
            ddCheckBoxShowNotifications.Enabled = enabled;
            ddLabelExcludedRegions.Enabled = enabled;
            ddTextBoxExcludedRegions.Enabled = enabled;
            ddLabelCompareMode.Enabled = enabled;
            ddComboBoxCompareMode.Enabled = enabled;
            ddLabelMinimumSaveInterval.Enabled = enabled;
            ddLabelMinimumSaveIntervalUnit.Enabled = enabled;
            ddNumericMinimumSaveInterval.Enabled = enabled;
            ddCheckBoxUpdateReferenceOnSave.Enabled = enabled;
            ddCheckBoxDiagnostics.Enabled = enabled;
        }

        private bool TryApplyDedupeSettings()
        {
            if (!TryParseExcludedRegions(ddTextBoxExcludedRegions.Text, out List<Rectangle> regions, out string? error))
            {
                MessageBox.Show(error, "Dedupe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ddTextBoxExcludedRegions.Focus();
                return false;
            }

            _dedupeSettings.Enabled = ddToggleEnable.Checked;
            _dedupeSettings.Sensitivity = ddTrackBarSensitivity.Value;
            //_dedupeSettings.CompareMode = (DedupeCompareMode)Math.Max(0, ddComboBoxCompareMode.SelectedIndex);
            _dedupeSettings.CompareMode = DedupeCompareModeHelper.ToModeCode(ddComboBoxCompareMode.Text);
            //_dedupeSettings.DynamicRegionMode = (DedupeDynamicRegionMode)Math.Max(0, ddComboBoxDynamicRegions.SelectedIndex);
            _dedupeSettings.DynamicRegionMode = DedupeDynamicRegionModeHelper.ToModeCode(ddComboBoxDynamicRegions.Text);
            _dedupeSettings.ComparePerActiveWindow = ddCheckBoxPerActiveWindow.Checked;
            _dedupeSettings.TimeoutMilliseconds = (int)ddNumericTimeout.Value;
            _dedupeSettings.DryRun = ddCheckBoxDryRun.Checked;
            _dedupeSettings.ShowSkippedNotifications = ddCheckBoxShowNotifications.Checked;
            _dedupeSettings.MinimumMinutesBetweenSaves = (int)ddNumericMinimumSaveInterval.Value;
            _dedupeSettings.UpdateReferenceOnlyOnSave = ddCheckBoxUpdateReferenceOnSave.Checked;
            _dedupeSettings.EnableDiagnostics = ddCheckBoxDiagnostics.Checked;
            _dedupeSettings.ExcludeRegions = regions;
            return true;
        }

        private static bool TryParseExcludedRegions(string text, out List<Rectangle> regions, out string? error)
        {
            regions = [];
            error = null;
            foreach (string line in text.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                string[] values = line.Split(',', StringSplitOptions.TrimEntries);
                if (values.Length != 4 || !int.TryParse(values[0], out int x) || !int.TryParse(values[1], out int y) ||
                    !int.TryParse(values[2], out int width) || !int.TryParse(values[3], out int height) || width <= 0 || height <= 0)
                {
                    error = $"Invalid excluded region '{line}'. Use x,y,width,height on each line.";
                    return false;
                }

                regions.Add(new Rectangle(x, y, width, height));
            }

            return true;
        }

        private bool TryApplyHotkeySettings()
        {
            if (hkToggleEnable.Checked &&
                (_hotkeySettings.Modifiers == Keys.None || _hotkeySettings.Key == Keys.None))
            {
                MessageBox.Show(
                    Constants.HotKeyMsgNoModifier,
                    "Hotkeys",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                hkTextBoxShortcut.Focus();
                return false;
            }

            _hotkeySettings.Enabled = hkToggleEnable.Checked;
            return true;
        }

        private void HkToggleEnable_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateHotkeyControlState();
        }

        private void HkTextBoxShortcut_KeyDown(object? sender, KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            if (key is Keys.ControlKey or Keys.ShiftKey or Keys.Menu or Keys.LWin or Keys.RWin)
            {
                e.SuppressKeyPress = true;
                return;
            }

            Keys modifiers = e.Modifiers & Keys.Modifiers;
            if (modifiers == Keys.None)
            {
                hkLabelStatus.Text = "Use Ctrl, Alt, Shift, or Windows with a key.";
                e.SuppressKeyPress = true;
                return;
            }

            _hotkeySettings.Modifiers = modifiers;
            _hotkeySettings.Key = key;
            hkTextBoxShortcut.Text = FormatHotkey(modifiers, key);
            PopulateAvailableHotkeys();
            UpdateHotkeyControlState();
            e.SuppressKeyPress = true;
        }

        private void HkButtonClear_Click(object? sender, EventArgs e)
        {
            _hotkeySettings.Modifiers = Keys.None;
            _hotkeySettings.Key = Keys.None;
            hkToggleEnable.Checked = false;
            hkTextBoxShortcut.Clear();
            hkLabelStatus.Text = "Capture hotkey is disabled.";
        }

        private void UpdateHotkeyControlState()
        {
            if (HotkeySettings.HasConflict)
            {
                hkLabelWarning.Visible = true;
            }
            else
            {
                hkLabelWarning.Visible = false;
            }

            hkLabelShortcut.Enabled = hkToggleEnable.Checked;
            hkTextBoxShortcut.Enabled = hkToggleEnable.Checked;
            hkButtonClear.Enabled = hkToggleEnable.Checked || _hotkeySettings.Key != Keys.None;
            hkListViewAvailable.Enabled = true;
            if (!hkToggleEnable.Checked)
            {
                hkLabelStatus.Text = "Capture hotkey is disabled.";
            }
            else if (GetCurrentHotkeyAvailability() == "In use")
            {
                hkLabelStatus.Text = "Shortcut is already taken. Choose another shortcut";
            }
            else
            {
                hkLabelStatus.Text = "Shortcut ready to save.";
            }
        }

        private void HkListViewAvailable_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingHotkeyList || hkListViewAvailable.SelectedItems.Count == 0)
            {
                return;
            }

            if (hkListViewAvailable.SelectedItems[0].Tag is not HotkeyCandidate candidate)
            {
                return;
            }

            bool available = hkListViewAvailable.SelectedItems[0].SubItems[1].Text == "Available";
            _hotkeySettings.Modifiers = candidate.Modifiers;
            _hotkeySettings.Key = candidate.Key;
            hkTextBoxShortcut.Text = FormatHotkey(candidate.Modifiers, candidate.Key);
            hkToggleEnable.Checked = available;
            hkLabelStatus.Text = available
                ? "Available shortcut selected."
                : "Shortcut is already taken.";
        }

        private string? GetCurrentHotkeyAvailability()
        {
            foreach (ListViewItem item in hkListViewAvailable.Items)
            {
                if (item.Tag is HotkeyCandidate candidate
                    && candidate.Modifiers == _hotkeySettings.Modifiers
                    && candidate.Key == _hotkeySettings.Key)
                {
                    return item.SubItems[1].Text;
                }
            }

            return null;
        }

        private void HkListViewAvailable_ColumnClick(object? sender, ColumnClickEventArgs e)
        {
            if (_hotkeyListSortColumn == e.Column)
            {
                _hotkeyListSortOrder = _hotkeyListSortOrder == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                _hotkeyListSortColumn = e.Column;
                _hotkeyListSortOrder = SortOrder.Ascending;
            }

            hkListViewAvailable.ListViewItemSorter = new HotkeyListViewItemComparer(
                _hotkeyListSortColumn,
                _hotkeyListSortOrder);
            hkListViewAvailable.Sort();
        }

        private void PopulateAvailableHotkeys()
        {
            _isUpdatingHotkeyList = true;
            try
            {
                hkListViewAvailable.Items.Clear();
                foreach (HotkeyCandidate candidate in GetHotkeyCandidates())
                {
                    bool available = _globalHotkeyManager.IsAvailable(candidate.Modifiers, candidate.Key);
                    var item = new ListViewItem(FormatHotkey(candidate.Modifiers, candidate.Key));
                    item.SubItems.Add(available ? "Available" : "In use");
                    item.Tag = candidate;
                    hkListViewAvailable.Items.Add(item);
                }
            }
            finally
            {
                _isUpdatingHotkeyList = false;
            }
        }

        private IEnumerable<HotkeyCandidate> GetHotkeyCandidates()
        {
            Keys[] modifiers =
            [
                Keys.Control,
                Keys.Alt,
                Keys.Shift,
                Keys.Control | Keys.Alt,
                Keys.Control | Keys.Shift,
                Keys.Alt | Keys.Shift,
                Keys.Control | Keys.Alt | Keys.Shift
            ];

            Keys[] keys =
            [
                .. Enumerable.Range((int)Keys.F1, 12).Select(value => (Keys)value),
                Keys.PrintScreen,
                Keys.Pause,
                Keys.Insert,
                Keys.Home,
                Keys.End,
                Keys.PageUp,
                Keys.PageDown,
                Keys.Up,
                Keys.Down,
                Keys.Left,
                Keys.Right,
                .. Enumerable.Range((int)Keys.A, 26).Select(value => (Keys)value),
                .. Enumerable.Range((int)Keys.D0, 10).Select(value => (Keys)value)
            ];

            foreach (Keys modifier in modifiers)
            {
                foreach (Keys key in keys)
                {
                    yield return new HotkeyCandidate(modifier, key);
                }
            }

            HotkeyCandidate current = new(_hotkeySettings.Modifiers, _hotkeySettings.Key);
            if (!modifiers.Contains(current.Modifiers) || !keys.Contains(current.Key))
            {
                yield return current;
            }
        }

        private static string FormatHotkey(Keys modifiers, Keys key)
        {
            if (key == Keys.None)
            {
                return string.Empty;
            }

            return new KeysConverter().ConvertToString(modifiers | (key & Keys.KeyCode)) ?? string.Empty;
        }

        private sealed class HotkeyListViewItemComparer : System.Collections.IComparer
        {
            private readonly int _column;
            private readonly SortOrder _sortOrder;

            public HotkeyListViewItemComparer(int column, SortOrder sortOrder)
            {
                _column = column;
                _sortOrder = sortOrder;
            }

            public int Compare(object? x, object? y)
            {
                if (x is not ListViewItem left || y is not ListViewItem right)
                {
                    return 0;
                }

                string leftText = left.SubItems[_column].Text;
                string rightText = right.SubItems[_column].Text;
                int result = StringComparer.CurrentCultureIgnoreCase.Compare(leftText, rightText);
                return _sortOrder == SortOrder.Descending ? -result : result;
            }
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
            _downscaleSettings.ResizeMode = GetSelectedDownscaleMode();
            _downscaleSettings.ResizeTargetHeight = (int)dsNumericTargetHeight.Value;
            _downscaleSettings.ResizePercentage = (int)dsNumericPercentage.Value;
            _downscaleSettings.ResizeMaxWidth = (int)dsNumericMaxWidth.Value;
            _downscaleSettings.ResizeBoundingBoxWidth = (int)dsNumericBoundingWidth.Value;
            _downscaleSettings.ResizeBoundingBoxHeight = (int)dsNumericBoundingHeight.Value;
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
            _imageProcessingSettings.ColorTemperature = (ImageProcessColorTempMode)Math.Clamp(
                ipComboBoxColorTemperature.SelectedIndex,
                (int)ImageProcessColorTempMode.Neutral,
                (int)ImageProcessColorTempMode.Cool);
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
            //_retentionSettings.Action = (RetentionAction)Math.Clamp(
            //    rtComboBoxAction.SelectedIndex,
            //    (int)RetentionAction.MoveToRecycleBin,
            //    (int)RetentionAction.BackupThenDelete);
            _retentionSettings.Action = GetSelectedRetentionAction(); // RetentionActionHelper.ToModeCode(rtComboBoxAction.Text);
            _retentionSettings.BackupFolder = rtTextBoxBackupFolder.Text.Trim();
            _retentionSettings.PerSessionSubfolder = rtCheckBoxPerSessionSubfolder.Checked;
            _retentionSettings.DryRunMode = rtCheckBoxDryRun.Checked;
            _retentionSettings.IncludeSubfolders = rtCheckBoxIncludeSubfolders.Checked;
            return true;
        }

        private void SelectImageColorMode(ImageProcessColorMode mode)
        {
            ipRadioFullColor.Checked = mode == ImageProcessColorMode.FullColor;
            ipRadioGrayscale.Checked = mode == ImageProcessColorMode.Grayscale;
            ipRadioMonochrome1Bit.Checked = mode == ImageProcessColorMode.Monochrome1Bit;
            ipRadioColor16.Checked = mode == ImageProcessColorMode.Color16;
            ipRadioColor256.Checked = mode == ImageProcessColorMode.Color256;
            ipRadioAdaptivePalette.Checked = mode == ImageProcessColorMode.AdaptivePalette;
        }

        private ImageProcessColorMode GetSelectedImageColorMode()
        {
            if (ipRadioGrayscale.Checked)
            {
                return ImageProcessColorMode.Grayscale;
            }

            if (ipRadioMonochrome1Bit.Checked)
            {
                return ImageProcessColorMode.Monochrome1Bit;
            }

            if (ipRadioColor16.Checked)
            {
                return ImageProcessColorMode.Color16;
            }

            if (ipRadioColor256.Checked)
            {
                return ImageProcessColorMode.Color256;
            }

            if (ipRadioAdaptivePalette.Checked)
            {
                return ImageProcessColorMode.AdaptivePalette;
            }

            return ImageProcessColorMode.FullColor;
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

        private void SelectDownscaleMode(DownscaleResizeMode mode)
        {
            bool previousState = _isUpdatingDownscaleUi;
            _isUpdatingDownscaleUi = true;
            dsRadioTargetHeight.Checked = mode == DownscaleResizeMode.TargetHeight;
            dsRadioPercentage.Checked = mode == DownscaleResizeMode.Percentage;
            dsRadioMaxWidth.Checked = mode == DownscaleResizeMode.MaxWidth;
            dsRadioBoundingBox.Checked = mode == DownscaleResizeMode.BoundingBox;
            _isUpdatingDownscaleUi = previousState;
        }

        private DownscaleResizeMode GetSelectedDownscaleMode()
        {
            if (dsRadioPercentage.Checked)
            {
                return DownscaleResizeMode.Percentage;
            }

            if (dsRadioMaxWidth.Checked)
            {
                return DownscaleResizeMode.MaxWidth;
            }

            if (dsRadioBoundingBox.Checked)
            {
                return DownscaleResizeMode.BoundingBox;
            }

            return DownscaleResizeMode.TargetHeight;
        }

        private void UpdateDownscaleControlState()
        {
            bool enabled = dsToggleEnable.Checked;
            dsGroupBoxModes.Enabled = enabled;
            dsGroupBoxProcessing.Enabled = enabled;

            DownscaleResizeMode selectedMode = GetSelectedDownscaleMode();
            dsComboBoxTargetHeight.Enabled = enabled && selectedMode == DownscaleResizeMode.TargetHeight;
            dsNumericTargetHeight.Enabled = enabled && selectedMode == DownscaleResizeMode.TargetHeight;
            dsComboBoxPercentage.Enabled = enabled && selectedMode == DownscaleResizeMode.Percentage;
            dsNumericPercentage.Enabled = enabled && selectedMode == DownscaleResizeMode.Percentage;
            dsComboBoxMaxWidth.Enabled = enabled && selectedMode == DownscaleResizeMode.MaxWidth;
            dsNumericMaxWidth.Enabled = enabled && selectedMode == DownscaleResizeMode.MaxWidth;
            dsComboBoxBoundingBox.Enabled = enabled && selectedMode == DownscaleResizeMode.BoundingBox;
            dsNumericBoundingWidth.Enabled = enabled && selectedMode == DownscaleResizeMode.BoundingBox;
            dsNumericBoundingHeight.Enabled = enabled && selectedMode == DownscaleResizeMode.BoundingBox;
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
                DownscaleResizeMode.TargetHeight => $"Resize to {dsNumericTargetHeight.Value:N0}px height.",
                DownscaleResizeMode.Percentage => $"Resize to {dsNumericPercentage.Value}% of the captured size.",
                DownscaleResizeMode.MaxWidth => $"Reduce to {dsNumericMaxWidth.Value:N0}px width.",
                DownscaleResizeMode.BoundingBox => $"Fit within {dsNumericBoundingWidth.Value:N0} × {dsNumericBoundingHeight.Value:N0}.",
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
            bool controlsEnabled = !_isRetentionOperationRunning && rtToggleSwitchAutoCleanup.Checked;

            //rtToggleSwitchAutoCleanup.Enabled = controlsEnabled;
            rtButtonSelectionDefault.Enabled = controlsEnabled;
            rtCheckBoxMaxDays.Enabled = controlsEnabled;
            rtNumericMaxDays.Enabled = controlsEnabled && rtCheckBoxMaxDays.Checked;
            rtLabelMaxDaysSuffix.Enabled = controlsEnabled && rtCheckBoxMaxDays.Checked;
            rtCheckBoxMaxFiles.Enabled = controlsEnabled;
            rtNumericMaxFiles.Enabled = controlsEnabled && rtCheckBoxMaxFiles.Checked;
            rtLabelMaxFilesSuffix.Enabled = controlsEnabled && rtCheckBoxMaxFiles.Checked;
            rtCheckBoxIncludeSubfolders.Enabled = controlsEnabled;

            rtButtonActionDefault.Enabled = controlsEnabled;
            rtLabelAction.Enabled = controlsEnabled;
            rtComboBoxAction.Enabled = controlsEnabled;
            rtLabelBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtTextBoxBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtButtonBrowseBackupFolder.Enabled = controlsEnabled && backupSelected;
            rtCheckBoxPerSessionSubfolder.Enabled = controlsEnabled && backupSelected;
            rtCheckBoxDryRun.Enabled = controlsEnabled;
            rtButtonCleanupNow.Enabled = controlsEnabled && hasSaveFolder;

            if (!hasSaveFolder && !_isRetentionOperationRunning)
            {
                rtTextBoxCleanupResult.Text = "Select a folder to save screenshots on the main window before running cleanup.";
            }
        }

        private RetentionAction GetSelectedRetentionAction()
        {
            //return (RetentionAction)Math.Clamp(
            //    rtComboBoxAction.SelectedIndex,
            //    (int)RetentionAction.MoveToRecycleBin,
            //    (int)RetentionAction.BackupThenDelete);
            return RetentionActionHelper.ToModeCode(rtComboBoxAction.Text);
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
                //wmLabelFontDescription.Text = $"{_watermarkFont.Name}, {_watermarkFont.SizeInPoints:0.#} pt";
                //float originalSize = wmLabelFontDescription.Font.Size;
                //wmLabelFontDescription.Font = new Font(_watermarkFont.FontFamily, originalSize, _watermarkFont.Style);
                SetWatermarkTextDescription();
            }
        }

        private void SetWatermarkTextDescription()
        {
            wmLabelFontDescription.Text = $"{_watermarkFont.Name}, {_watermarkFont.SizeInPoints:0.#} pt";
            float originalSize = wmLabelFontDescription.Font.Size;
            wmLabelFontDescription.Font = new Font(_watermarkFont.FontFamily, originalSize, _watermarkFont.Style);
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

        private void WmButtonUseTextDefault_Click(object sender, EventArgs e)
        {
            InitializeTabWatermark(WatermarkResetDefaultScope.Text);
        }

        private void WmButtonUseImageDefault_Click(object sender, EventArgs e)
        {
            InitializeTabWatermark(WatermarkResetDefaultScope.Image);
        }

        private void WmButtonCommonDefault_Click(object sender, EventArgs e)
        {
            InitializeTabWatermark(WatermarkResetDefaultScope.Common);
        }

        private void AnButtonDefault_Click(object sender, EventArgs e)
        {
            InitializeTabAnnotation(resetToDefaults: true);
        }

        private void DsButtonModesDefault_Click(object sender, EventArgs e)
        {
            InitializeTabDownscale(DownscaleResetDefaultScope.Resize);
        }

        private void DsButtonProcessingDefault_Click(object sender, EventArgs e)
        {
            InitializeTabDownscale(DownscaleResetDefaultScope.Processing);
        }

        private void IpButtonColorModeDefault_Click(object sender, EventArgs e)
        {
            InitializeTabImageProcessing(ImageProcessingResetDefaultScope.ColorMode);
        }

        private void IpButtonAdvProcessingDefault_Click(object sender, EventArgs e)
        {
            InitializeTabImageProcessing(ImageProcessingResetDefaultScope.AdvProcessing);
        }

        private void DdButtonDefault_Click(object sender, EventArgs e)
        {
            InitializeTabDedupe(resetToDefaults: true);
        }

        private void RtButtonSelectionDefault_Click(object sender, EventArgs e)
        {
            InitializeTabRetention(resetDefaultScope: RetentionResetDefaultScope.Selection);
        }

        private void RtButtonActionDefault_Click(object sender, EventArgs e)
        {
            InitializeTabRetention(resetDefaultScope: RetentionResetDefaultScope.Action);
        }

        private void HkButtonDefault_Click(object sender, EventArgs e)
        {
            InitializeTabHotkeys(resetToDefaults: true);
        }

        private void AnToggleUseAnnotation_CheckedChanged(object sender, EventArgs e)
        {
            UpdateAnnotationControlState();
        }

        private void AnComboBoxAnnotationFormat_TextChanged(object sender, EventArgs e)
        {
            UpdateAnnotationControlState();
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

        private void AnComboBoxAnnotationFormat_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateAnnotationSelection();
        }

        private void AnComboBoxAnnotationFormat_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateAnnotationSelection();
        }

        private void AnComboBoxAnnotationFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateAnnotationSelection();
        }

        private void UpdateAnnotationSelection()
        {
            _annotationSelectionStart = Math.Max(0, anComboBoxAnnotationFormat.SelectionStart);
            _annotationSelectionLength = Math.Max(0, anComboBoxAnnotationFormat.SelectionLength);
        }

        private void AnButtonAnnotationFields_Click(object sender, EventArgs e)
        {
            var menu = new ContextMenuStrip(/*components*/);
            if (!_ownedMenus.Contains(menu))
            {
                _ownedMenus.Add(menu);
            }

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
            //anComboBoxAnnotationFormat.Focus();
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

            wmButtonUseTextDefault.Enabled = textEnabled;
            wmTextBoxWatermarkText.Enabled = textEnabled;
            wmButtonChooseFont.Enabled = textEnabled;
            wmLabelFontDescription.Enabled = textEnabled;

            wmButtonUseImageDefault.Enabled = imageEnabled;
            wmTextBoxWatermarkImagePath.Enabled = imageEnabled;
            wmButtonBrowseWatermarkImage.Enabled = imageEnabled;
            wmTrackBarWatermarkImageScale.Enabled = imageEnabled;
            wmLabelWatermarkImageScaleValue.Enabled = imageEnabled;

            wmButtonCommonDefault.Enabled = commonEnabled;
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
            anButtonDefault.Enabled = enabled;
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
                ResizeMode = settings.ResizeMode,
                ResizeTargetHeight = settings.ResizeTargetHeight,
                ResizePercentage = settings.ResizePercentage,
                ResizeMaxWidth = settings.ResizeMaxWidth,
                ResizeBoundingBoxWidth = settings.ResizeBoundingBoxWidth,
                ResizeBoundingBoxHeight = settings.ResizeBoundingBoxHeight,
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

        private static HotkeySettings Clone(HotkeySettings settings)
        {
            return new HotkeySettings
            {
                Enabled = settings.Enabled,
                Modifiers = settings.Modifiers,
                Key = settings.Key,
                HasConflict = settings.HasConflict
            };
        }

        private static DedupeSettings Clone(DedupeSettings settings)
        {
            return new DedupeSettings
            {
                Enabled = settings.Enabled,
                CompareMode = settings.CompareMode,
                Sensitivity = settings.Sensitivity,
                TimeoutMilliseconds = settings.TimeoutMilliseconds,
                DynamicRegionMode = settings.DynamicRegionMode,
                ExcludeRegions = [.. settings.ExcludeRegions],
                ComparePerActiveWindow = settings.ComparePerActiveWindow,
                UpdateReferenceOnlyOnSave = settings.UpdateReferenceOnlyOnSave,
                MinimumMinutesBetweenSaves = settings.MinimumMinutesBetweenSaves,
                DryRun = settings.DryRun,
                ShowSkippedNotifications = settings.ShowSkippedNotifications,
                EnableDiagnostics = settings.EnableDiagnostics
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
                && left.ResizeMode == right.ResizeMode
                && left.ResizeTargetHeight == right.ResizeTargetHeight
                && left.ResizePercentage == right.ResizePercentage
                && left.ResizeMaxWidth == right.ResizeMaxWidth
                && left.ResizeBoundingBoxWidth == right.ResizeBoundingBoxWidth
                && left.ResizeBoundingBoxHeight == right.ResizeBoundingBoxHeight
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

        private static bool AreEqual(HotkeySettings left, HotkeySettings right)
        {
            return left.Enabled == right.Enabled
                && left.Modifiers == right.Modifiers
                && left.Key == right.Key;
        }

        private static bool AreEqual(DedupeSettings left, DedupeSettings right)
        {
            return left.Enabled == right.Enabled
                && left.CompareMode == right.CompareMode
                && left.Sensitivity == right.Sensitivity
                && left.TimeoutMilliseconds == right.TimeoutMilliseconds
                && left.DynamicRegionMode == right.DynamicRegionMode
                && left.ComparePerActiveWindow == right.ComparePerActiveWindow
                && left.UpdateReferenceOnlyOnSave == right.UpdateReferenceOnlyOnSave
                && left.MinimumMinutesBetweenSaves == right.MinimumMinutesBetweenSaves
                && left.DryRun == right.DryRun
                && left.ShowSkippedNotifications == right.ShowSkippedNotifications
                && left.EnableDiagnostics == right.EnableDiagnostics
                && left.ExcludeRegions.SequenceEqual(right.ExcludeRegions);
        }

        private void hkLabelEnable_Click(object sender, EventArgs e)
        {

        }

        private void TabControlSettings_DrawItem(object? sender, DrawItemEventArgs e)
        {
            Color accent = Color.FromArgb(0, 120, 215);     // Windows accent blue
            Color hover = Color.FromArgb(230, 235, 255);    // Subtle hover
            Color normal = Color.FromArgb(245, 245, 245);   // Background
            Color text = Color.FromArgb(32, 32, 32);        // Fluent primary text
            Color muted = Color.FromArgb(96, 96, 96);       // Unselected text. Fluent Gray 80

            var tab = tabControlSettings.TabPages[e.Index];
            bool isSelected = (e.Index == tabControlSettings.SelectedIndex);

            Rectangle bounds = e.Bounds;
            bounds.Inflate(-2, -2);

            // Hover detection
            bool isHover = bounds.Contains(tabControlSettings.PointToClient(Cursor.Position));

            // Background
            Color backColor = isSelected ? normal : (isHover ? hover : normal);
            using (var brush = new SolidBrush(backColor))
                e.Graphics.FillRectangle(brush, bounds);

            // Text
            Font baseFont = e.Font ?? tabControlSettings.Font;
            Font font = isSelected ? new Font(baseFont, FontStyle.Bold) : baseFont;

            Color textColor = isSelected ? text : muted;

            TextRenderer.DrawText(
                e.Graphics,
                tab.Text,
                font,
                bounds,
                textColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            );

            // Fluent accent underline
            if (isSelected)
            {
                using var pen = new Pen(accent, 3);
                int y = bounds.Bottom - 2;
                e.Graphics.DrawLine(pen, bounds.Left + 12, y, bounds.Right - 12, y);
            }

        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

    }
}
