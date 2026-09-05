using System.Reflection;
using Microsoft.Win32;

namespace Capillume
{
    public partial class Form1 : Form
    {
        private ScreenshotService? _screenshotService;
        private AppSettings _settings;
        private bool WatermarkSettingsChanged = false;
        private bool AnnotationSettingsChanged = false;
        private bool DownscaleSettingsChanged = false;
        private Icon? _appIcon;
        private bool _isStartedWithWindows;
        private bool _isSessionLocked;
        private bool _isSuspended;
        private bool _isExiting;

        private bool _isUpdatingUi;

        public Form1(bool isStartedWithWindows = false)
        {
            InitializeComponent();

            _isStartedWithWindows = isStartedWithWindows;
            _settings = SettingsManager.LoadSettings();
            InitializeUI();
            InitializeScreenshotService();
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isExiting)
            {
                UnsubscribeSystemEvents();
            }

            if (!_isExiting && HasUnsavedChanges())
            {
                DialogResult result = MessageBox.Show(
                    "There are unsaved settings changes. Do you want to save them?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (!SaveSettings(showSuccessMessage: false))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (result == DialogResult.No)
                {
                    RestoreSettingsToUi();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!_isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon.ShowBalloonTip(2000, "Capillume",
                    "Application minimized to system tray. Right-click the icon to access options.",
                    ToolTipIcon.Info);
            }

            if (_isExiting)
            {
                _screenshotService?.Stop();
            }
        }

        private void UnsubscribeSystemEvents()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
        }

        private void SystemEvents_SessionSwitch(object? sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    _isSessionLocked = true;
                    UpdateLifecyclePauseState();
                    break;
                case SessionSwitchReason.SessionUnlock:
                    _isSessionLocked = false;
                    UpdateLifecyclePauseState();
                    break;
            }
        }

        private void SystemEvents_PowerModeChanged(object? sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                _isSuspended = true;
                UpdateLifecyclePauseState();
            }
            else if (e.Mode == PowerModes.Resume)
            {
                _isSuspended = false;
                UpdateLifecyclePauseState();
            }
        }

        private void UpdateLifecyclePauseState()
        {
            if (_isSessionLocked || _isSuspended)
            {
                _screenshotService?.Pause();
            }
            else
            {
                _screenshotService?.Resume();
            }
        }

        private void SystemEvents_SessionEnding(object? sender, SessionEndingEventArgs e)
        {
            _isExiting = true;
            _screenshotService?.Stop();

            try
            {
                if (IsHandleCreated)
                {
                    BeginInvoke(Application.Exit);
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (InvalidOperationException)
            {
                Application.Exit();
            }
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
                        notifyIcon.Icon = _appIcon;
                        this.Icon = _appIcon;
                    }
                    else
                    {
                        // Fallback to generated icon
                        _appIcon = FallbackIcon.CreateAppIconAdvanced();
                        notifyIcon.Icon = _appIcon;
                        this.Icon = _appIcon;
                    }
                }
            }
            catch
            {
                // Fallback to generated icon
                _appIcon = FallbackIcon.CreateAppIconAdvanced();
                notifyIcon.Icon = _appIcon;
                this.Icon = _appIcon;
            }

            // Load logo image for header
            try
            {
                using (var stream = assembly.GetManifestResourceStream("Capillume.icon.png"))
                {
                    if (null != stream)
                    {
                        pictureBoxLogo.Image = Image.FromStream(stream);
                    }
                }
            }
            catch
            {
                // Logo is optional, continue without it
            }

            // Load settings into UI
            this.Text = $"{Application.ProductName}"; // v{assembly.GetName().Version}";
            this.labelTitle.Text = $"{Application.ProductName}";

            toolTipCaptureNow.InitialDelay = 5000;
            toolTipCaptureNow.ReshowDelay = 100;
            toolTipCaptureNow.AutoPopDelay = 5000;
            toolTipCaptureNow.ShowAlways = true;
            toolTipCaptureNow.SetToolTip(CaptureNow, "Capture screenshot now");

            toggleSwitchEnabled.Checked = _settings.IsScreenshotEnabled;
            toggleSwitchNotify.Checked = _settings.ShowNotifications;
            toggleSwitchStartWithWindows.Checked = _settings.StartWithWindows;
            numericUpDownInterval.Value = _settings.IntervalMinutes;
            comboBoxCaptureMode.SelectedIndex = _settings.CaptureFullScreen ? 0 : 1;
            checkBoxIncludeCapillume.Checked = _settings.IncludeCapillume;

            textBoxFolder.Text = _settings.SaveFolder;
            comboBoxFormat.SelectedItem = _settings.ImageFormat;
            trackBarQuality.Value = Math.Clamp(_settings.ImageQuality, Constants.ScreenshotImageQualityMin, Constants.ScreenshotImageQualityMax);
            labelQualityValue.Text = _settings.ImageQuality.ToString() + "%";

            // Update quality control visibility
            UpdateQualityControlsVisibility();
            UpdateEnabledStatus();
            UpdateSaveButtonState();
            labelStatus.Text = Constants.StatusReady;
        }

        private void InitializeScreenshotService()
        {
            _screenshotService = new ScreenshotService(_settings, Handle);
            _screenshotService.ScreenshotCaptured += OnScreenshotCaptured;
            _screenshotService.ErrorOccurred += OnErrorOccurred;

            if (_settings.IsScreenshotEnabled)
            {
                _screenshotService.Start();
            }
        }

        private void OnScreenshotCaptured(object? sender, ScreenshotCapturedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnScreenshotCaptured(sender, e));
                return;
            }

            string fileName = Path.GetFileName(e.FilePath);
            long ms = e.ElapsedMilliseconds;
            double seconds = ms / 1000.0;

            // Show notification
            if (_settings.ShowNotifications)
            {
                notifyIcon.ShowBalloonTip(2000, $"Screenshot saved ({seconds:F2} sec)",
                    $"{fileName}", ToolTipIcon.Info);
                // $"Saved to: {fileName}", ToolTipIcon.Info);
            }
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnErrorOccurred(sender, error));
                return;
            }

            notifyIcon.ShowBalloonTip(3000, "Error", error, ToolTipIcon.Error);
        }

        private void UpdateEnabledStatus()
        {
            labelShowNotifications.Enabled = toggleSwitchEnabled.Checked;
            toggleSwitchNotify.Enabled = toggleSwitchEnabled.Checked;
            labelStartWithWindows.Enabled = toggleSwitchEnabled.Checked;
            toggleSwitchStartWithWindows.Enabled = toggleSwitchEnabled.Checked;
            labelCaptureMode.Enabled = toggleSwitchEnabled.Checked;
            comboBoxCaptureMode.Enabled = toggleSwitchEnabled.Checked;
            labelInterval.Enabled = toggleSwitchEnabled.Checked;
            numericUpDownInterval.Enabled = toggleSwitchEnabled.Checked;
            checkBoxIncludeCapillume.Enabled = toggleSwitchEnabled.Checked;
            labelFileFormat.Enabled = toggleSwitchEnabled.Checked;
            comboBoxFormat.Enabled = toggleSwitchEnabled.Checked;
            labelQuality.Enabled = toggleSwitchEnabled.Checked;
            trackBarQuality.Enabled = toggleSwitchEnabled.Checked;
            textBoxFolder.Enabled = toggleSwitchEnabled.Checked;
            buttonBrowse.Enabled = toggleSwitchEnabled.Checked;
            buttonSettings.Enabled = toggleSwitchEnabled.Checked;
        }

        private void ToggleSwitchEnabled_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateEnabledStatus();
            UpdateSaveButtonState();
        }

        private void ToggleSwitchNotify_CheckedChanged(object? sender, EventArgs e)
        {
            // UpdateEnabledStatus();
            UpdateSaveButtonState();
        }

        private void ToggleSwitchStartWithWindows_CheckedChanged(object? sender, EventArgs e)
        {
            // UpdateEnabledStatus();
            UpdateSaveButtonState();
        }

        private void CheckBoxIncludeCapillume_CheckedChanged(object? sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void ComboBoxFormat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateQualityControlsVisibility();
            UpdateSaveButtonState();
        }

        private void ComboBoxCaptureMode_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void NumericUpDownInterval_ValueChanged(object? sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void TrackBarQuality_Scroll(object? sender, EventArgs e)
        {
            //labelQualityValue.Text = $"{trackBarQuality.Value}%";
        }

        private void TrackBarQuality_ValueChanged(object? sender, EventArgs e)
        {
            labelQualityValue.Text = $"{trackBarQuality.Value}%";
            UpdateSaveButtonState();
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            SaveSettings();
        }

        private void ButtonBrowse_Click(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog
            {
                Description = "Select folder to save screenshots",
                SelectedPath = textBoxFolder.Text,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFolder.Text = dialog.SelectedPath;
                UpdateSaveButtonState();
            }
        }

        private void ButtonOpenFolder_Click(object? sender, EventArgs e)
        {
            if (Directory.Exists(textBoxFolder.Text))
            {
                System.Diagnostics.Process.Start("explorer.exe", textBoxFolder.Text);
            }
            else
            {
                MessageBox.Show("The folder does not exist yet. It will be created when the first screenshot is taken.",
                    "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            using var settingsForm = new FormSettings(_settings.Watermark, _settings.Annotation, _settings.Downscale);
            if (settingsForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            _settings.Watermark = settingsForm.WatermarkSettings;
            _settings.Annotation = settingsForm.AnnotationSettings;
            _settings.Downscale = settingsForm.DownscaleSettings;
            WatermarkSettingsChanged = WatermarkSettingsChanged || settingsForm.WatermarkSettingsChanged;
            AnnotationSettingsChanged = AnnotationSettingsChanged || settingsForm.AnnotationSettingsChanged;
            DownscaleSettingsChanged = DownscaleSettingsChanged || settingsForm.DownscaleSettingsChanged;
            UpdateSaveButtonState();
        }

        private void UpdateQualityControlsVisibility()
        {
            string? format = comboBoxFormat.SelectedItem?.ToString();
            bool showQuality = format == "JPG" || format == "WEBP";

            labelQuality.Enabled = showQuality;
            trackBarQuality.Enabled = showQuality;
            labelQualityValue.Enabled = showQuality;
        }

        private void NotifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            ShowForm();
        }

        private void ShowToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            ShowForm();
        }

        private void CaptureNowToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            CaptureScreenshotNow();
        }

        private void CaptureNow_Click(object? sender, EventArgs e)
        {
            CaptureScreenshotNow();
        }

        private void CaptureScreenshotNow()
        {
            _screenshotService?.CaptureScreenshot(
                captureWindowBelowCapillume: !_settings.IncludeCapillume && !_settings.CaptureFullScreen);
        }

        private void ExitToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _isExiting = true;
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using var aboutForm = new FormAbout();
            aboutForm.ShowDialog(this);
        }

        private void LinkLabelAbout_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using var aboutForm = new FormAbout();
            aboutForm.ShowDialog(this);
        }

        // Show the form and bring it to the front
        private void ShowForm()
        {
            _isStartedWithWindows = false;

            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            BringToFront();
            Activate();
        }

        private bool HasUnsavedChanges()
        {
            return toggleSwitchEnabled.Checked != _settings.IsScreenshotEnabled
                || toggleSwitchNotify.Checked != _settings.ShowNotifications
                || toggleSwitchStartWithWindows.Checked != _settings.StartWithWindows
                || numericUpDownInterval.Value != _settings.IntervalMinutes
                || comboBoxCaptureMode.SelectedIndex != (_settings.CaptureFullScreen ? 0 : 1)
                || checkBoxIncludeCapillume.Checked != _settings.IncludeCapillume
                || !string.Equals(textBoxFolder.Text, _settings.SaveFolder, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(comboBoxFormat.SelectedItem?.ToString(), _settings.ImageFormat, StringComparison.OrdinalIgnoreCase)
                || trackBarQuality.Value != _settings.ImageQuality
                || WatermarkSettingsChanged == true
                || AnnotationSettingsChanged == true
                || DownscaleSettingsChanged == true;
        }

        /// <summary>
        /// Update the state of Save and Undo buttons so that they are enabled only when there are unsaved changes.
        /// </summary>
        private void UpdateSaveButtonState()
        {
            if (_isUpdatingUi)
            {
                return;
            }

            bool hasChanges = HasUnsavedChanges();

            buttonSave.Enabled = hasChanges;

            if (hasChanges)
            {
                labelStatus.Text = "There are unsaved settings.";
            }
            else
            {
                labelStatus.Text = Constants.StatusReady;
            }
        }

        private void UiSettingChanged(object? sender, EventArgs e)
        {
            UpdateSaveButtonState();
        }

        private void RestoreSettingsToUi()
        {
            _isUpdatingUi = true;

            try
            {
                toggleSwitchEnabled.Checked = _settings.IsScreenshotEnabled;
                toggleSwitchNotify.Checked = _settings.ShowNotifications;
                toggleSwitchStartWithWindows.Checked = _settings.StartWithWindows;
                numericUpDownInterval.Value = _settings.IntervalMinutes;
                comboBoxCaptureMode.SelectedIndex = _settings.CaptureFullScreen ? 0 : 1;
                checkBoxIncludeCapillume.Checked = _settings.IncludeCapillume;
                textBoxFolder.Text = _settings.SaveFolder;
                comboBoxFormat.SelectedItem = _settings.ImageFormat;
                trackBarQuality.Value = _settings.ImageQuality;

                labelQualityValue.Text = $"{_settings.ImageQuality}%";
                UpdateEnabledStatus();
                UpdateQualityControlsVisibility();
                WatermarkSettingsChanged = false;
                AnnotationSettingsChanged = false;
                DownscaleSettingsChanged = false;
            }
            finally
            {
                _isUpdatingUi = false;
            }

            UpdateSaveButtonState();
        }

        private bool SaveSettings(bool showSuccessMessage = true)
        {
            if (string.IsNullOrWhiteSpace(textBoxFolder.Text))
            {
                MessageBox.Show(
                    "Please select a save folder.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            _settings.IsScreenshotEnabled = toggleSwitchEnabled.Checked;
            _settings.ShowNotifications = toggleSwitchNotify.Checked;
            _settings.StartWithWindows = toggleSwitchStartWithWindows.Checked;
            _settings.IntervalMinutes = (int)numericUpDownInterval.Value;
            _settings.CaptureFullScreen = comboBoxCaptureMode.SelectedIndex == 0;
            _settings.IncludeCapillume = checkBoxIncludeCapillume.Checked;
            _settings.SaveFolder = textBoxFolder.Text;
            _settings.ImageFormat = comboBoxFormat.SelectedItem?.ToString() ?? "JPG";
            _settings.ImageQuality = trackBarQuality.Value;

            SettingsManager.SaveSettings(_settings);
            SettingsManager.SetAutoStart(_settings.StartWithWindows);
            WatermarkSettingsChanged = false;
            AnnotationSettingsChanged = false;
            DownscaleSettingsChanged = false;

            _screenshotService?.UpdateSettings(_settings);

            if (_settings.IsScreenshotEnabled)
            {
                _screenshotService?.Start();
            }
            else
            {
                _screenshotService?.Stop();
            }

            UpdateSaveButtonState();

            if (showSuccessMessage)
            {
                labelStatus.Text = "Settings saved successfully.";
            }

            return true;
        }

    }
}
