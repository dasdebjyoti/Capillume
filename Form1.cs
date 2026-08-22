using System.Reflection;

namespace Capillume
{
    public partial class Form1 : Form
    {
        private ScreenshotService? _screenshotService;
        private AppSettings _settings;
        private Icon? _appIcon;
        private bool _isStartedWithWindows;

        private bool _isUpdatingUi;

        public Form1(bool isStartedWithWindows = false)
        {
            InitializeComponent();
            _isStartedWithWindows = isStartedWithWindows;
            _settings = SettingsManager.LoadSettings();
            InitializeUI();
            InitializeScreenshotService();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (HasUnsavedChanges())
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
            
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon.ShowBalloonTip(2000, "Capillume",
                    "Application minimized to system tray. Right-click the icon to access options.",
                    ToolTipIcon.Info);
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
            this.labelSubtitle.Text = $"Settings • v{assembly.GetName().Version}";
            this.labelTitle.Text = $"{Application.ProductName}";

            toggleSwitchEnabled.Checked = _settings.IsEnabled;
            UpdateEnabledStatus();

            toggleSwitchNotify.Checked = _settings.ShowNotifications;
            toggleSwitchStartWithWindows.Checked = _settings.StartWithWindows;
            numericUpDownInterval.Value = _settings.IntervalMinutes;

            comboBoxCaptureMode.SelectedIndex = _settings.CaptureFullScreen ? 0 : 1;

            textBoxFolder.Text = _settings.SaveFolder;
            comboBoxFormat.SelectedItem = _settings.ImageFormat;
            trackBarQuality.Value = _settings.ImageQuality;
            labelQualityValue.Text = _settings.ImageQuality.ToString() + "%";

            // Update quality control visibility
            UpdateQualityControlsVisibility();

            UpdateSaveButtonState();
            labelStatus.Text = "Ready";
        }

        private void InitializeScreenshotService()
        {
            _screenshotService = new ScreenshotService(_settings);
            _screenshotService.ScreenshotCaptured += OnScreenshotCaptured;
            _screenshotService.ErrorOccurred += OnErrorOccurred;

            if (_settings.IsEnabled)
            {
                _screenshotService.Start();
            }
        }

        private void OnScreenshotCaptured(object? sender, string filePath)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnScreenshotCaptured(sender, filePath));
                return;
            }

            string fileName = Path.GetFileName(filePath);

            // Show notification
            if (_settings.ShowNotifications)
            {
                notifyIcon.ShowBalloonTip(2000, "Screenshot Captured",
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
            if (toggleSwitchEnabled.Checked)
            {
                labelEnabledStatus.Text = "ON";
                labelEnabledStatus.ForeColor = Color.FromArgb(0, 120, 212);
            }
            else
            {
                labelEnabledStatus.Text = "OFF";
                labelEnabledStatus.ForeColor = Color.Gray;
            }
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
            labelQualityValue.Text = $"{trackBarQuality.Value}%";
            // UpdateSaveButtonState(); // Do this inside TrackBarQuality_ValueChanged
        }

        private void TrackBarQuality_ValueChanged(object? sender, EventArgs e)
        {
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
            _screenshotService?.CaptureScreenshot();
        }

        private void ExitToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }

        private void LinkLabelAbout_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            using var aboutForm = new AboutForm();
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
            return toggleSwitchEnabled.Checked != _settings.IsEnabled
                || toggleSwitchNotify.Checked != _settings.ShowNotifications
                || toggleSwitchStartWithWindows.Checked != _settings.StartWithWindows
                || numericUpDownInterval.Value != _settings.IntervalMinutes
                || comboBoxCaptureMode.SelectedIndex != (_settings.CaptureFullScreen ? 0 : 1)
                || !string.Equals(textBoxFolder.Text, _settings.SaveFolder, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(comboBoxFormat.SelectedItem?.ToString(), _settings.ImageFormat, StringComparison.OrdinalIgnoreCase)
                || trackBarQuality.Value != _settings.ImageQuality;
        }

        private void UpdateSaveButtonState()
        {
            if (_isUpdatingUi)
            {
                return;
            }

            bool hasChanges = HasUnsavedChanges();

            buttonSave.Enabled = hasChanges;
            buttonUndo.Enabled = hasChanges;

            if (hasChanges)
            {
                labelStatus.Text = "Unsaved changes";
            }
            else
            {
                labelStatus.Text = "Ready";
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
                toggleSwitchEnabled.Checked = _settings.IsEnabled;
                toggleSwitchNotify.Checked = _settings.ShowNotifications;
                toggleSwitchStartWithWindows.Checked = _settings.StartWithWindows;
                numericUpDownInterval.Value = _settings.IntervalMinutes;
                comboBoxCaptureMode.SelectedIndex = _settings.CaptureFullScreen ? 0 : 1;
                textBoxFolder.Text = _settings.SaveFolder;
                comboBoxFormat.SelectedItem = _settings.ImageFormat;
                trackBarQuality.Value = _settings.ImageQuality;

                labelQualityValue.Text = $"{_settings.ImageQuality}%";
                UpdateEnabledStatus();
                UpdateQualityControlsVisibility();
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

            _settings.IsEnabled = toggleSwitchEnabled.Checked;
            _settings.ShowNotifications = toggleSwitchNotify.Checked;
            _settings.StartWithWindows = toggleSwitchStartWithWindows.Checked;
            _settings.IntervalMinutes = (int)numericUpDownInterval.Value;
            _settings.CaptureFullScreen = comboBoxCaptureMode.SelectedIndex == 0;
            _settings.SaveFolder = textBoxFolder.Text;
            _settings.ImageFormat = comboBoxFormat.SelectedItem?.ToString() ?? "JPG";
            _settings.ImageQuality = trackBarQuality.Value;

            SettingsManager.SaveSettings(_settings);
            SettingsManager.SetAutoStart(_settings.StartWithWindows);

            _screenshotService?.UpdateSettings(_settings);

            if (_settings.IsEnabled)
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
                labelStatus.Text = "Settings saved successfully";
                /*MessageBox.Show(
                    "Settings saved successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);*/
            }

            return true;
        }

        private void ButtonUndo_Click(object sender, EventArgs e)
        {
            RestoreSettingsToUi();
        }
    }
}
