using System.Reflection;

using System.Reflection;

namespace CapIilume
{
    public partial class Form1 : Form
    {
        private ScreenshotService? _screenshotService;
        private AppSettings _settings;
        private Icon? _appIcon;

        public Form1()
        {
            InitializeComponent();
            _settings = SettingsManager.LoadSettings();
            InitializeUI();
            InitializeScreenshotService();
        }

        private void InitializeUI()
        {
            // Load icon for the system tray and form
            try
            {
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico");
                if (File.Exists(iconPath))
                {
                    _appIcon = new Icon(iconPath);
                    notifyIcon.Icon = _appIcon;
                    this.Icon = _appIcon;
                }
                else
                {
                    // Fallback to generated icon
                    _appIcon = CreateAppIcon();
                    notifyIcon.Icon = _appIcon;
                    this.Icon = _appIcon;
                }
            }
            catch
            {
                // Fallback to generated icon
                _appIcon = CreateAppIcon();
                notifyIcon.Icon = _appIcon;
                this.Icon = _appIcon;
            }

            // Load logo image for header
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (var stream = assembly.GetManifestResourceStream("CapIilume.icon.png"))
                {
                    if (stream != null)
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
            toggleSwitchEnabled.Checked = _settings.IsEnabled;
            UpdateEnabledStatus();

            numericUpDownInterval.Value = _settings.IntervalMinutes;

            comboBoxCaptureMode.SelectedIndex = _settings.CaptureFullScreen ? 0 : 1;

            textBoxFolder.Text = _settings.SaveFolder;
            comboBoxFormat.SelectedItem = _settings.ImageFormat;
            trackBarQuality.Value = _settings.ImageQuality;
            labelQualityValue.Text = _settings.ImageQuality.ToString() + "%";

            // Update quality control visibility
            UpdateQualityControlsVisibility();

            // Update status
            UpdateStatus();
        }

        private Icon CreateAppIcon()
        {
            // Create a simple icon (camera-like representation)
            var bitmap = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw a simple camera shape
                using (var brush = new SolidBrush(Color.FromArgb(0, 120, 215)))
                {
                    g.FillRectangle(brush, 6, 10, 20, 14);
                    g.FillEllipse(brush, 12, 12, 8, 8);
                }

                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(pen, 13, 13, 6, 6);
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            return Icon.FromHandle(hIcon);
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
            UpdateStatus($"Captured: {fileName}");

            // Show notification
            notifyIcon.ShowBalloonTip(2000, "Screenshot Captured", 
                $"Saved to: {fileName}", ToolTipIcon.Info);
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnErrorOccurred(sender, error));
                return;
            }

            UpdateStatus($"Error: {error}");
            notifyIcon.ShowBalloonTip(3000, "Error", error, ToolTipIcon.Error);
        }

        private void UpdateStatus(string? message = null)
        {
            if (message != null)
            {
                labelStatus.Text = message;
            }
            else
            {
                string status = _settings.IsEnabled ? "Capillume is running in the background." : "Capillume is idle.";
                labelStatus.Text = status + " Right-click the tray icon to capture or quit.";
            }
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
        }

        private void ButtonSave_Click(object? sender, EventArgs e)
        {
            // Validate folder
            if (string.IsNullOrWhiteSpace(textBoxFolder.Text))
            {
                MessageBox.Show("Please select a save folder.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update settings
            _settings.IsEnabled = toggleSwitchEnabled.Checked;
            _settings.IntervalMinutes = (int)numericUpDownInterval.Value;
            _settings.CaptureFullScreen = comboBoxCaptureMode.SelectedIndex == 0;
            _settings.SaveFolder = textBoxFolder.Text;
            _settings.ImageFormat = comboBoxFormat.SelectedItem?.ToString() ?? "PNG";
            _settings.ImageQuality = trackBarQuality.Value;

            // Save settings
            SettingsManager.SaveSettings(_settings);

            // Update service
            _screenshotService?.UpdateSettings(_settings);

            if (_settings.IsEnabled)
            {
                _screenshotService?.Start();
            }
            else
            {
                _screenshotService?.Stop();
            }

            UpdateStatus();

            MessageBox.Show("Settings saved successfully!", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ComboBoxFormat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            UpdateQualityControlsVisibility();
        }

        private void UpdateQualityControlsVisibility()
        {
            string? format = comboBoxFormat.SelectedItem?.ToString();
            bool showQuality = format == "JPG" || format == "WEBP";

            labelQuality.Enabled = showQuality;
            trackBarQuality.Enabled = showQuality;
            labelQualityValue.Enabled = showQuality;
        }

        private void TrackBarQuality_Scroll(object? sender, EventArgs e)
        {
            labelQualityValue.Text = trackBarQuality.Value.ToString() + "%";
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

        private void ShowForm()
        {
            Show();
            WindowState = FormWindowState.Normal;
            BringToFront();
            Activate();
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                notifyIcon.ShowBalloonTip(2000, "Capillume", 
                    "Application minimized to system tray. Right-click the icon to access options.", 
                    ToolTipIcon.Info);
            }
        }
    }
}
