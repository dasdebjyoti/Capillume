namespace CapIilume
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing)
            {
                _screenshotService?.Dispose();
                _appIcon?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components);
            contextMenuStrip = new ContextMenuStrip(components);
            showToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            captureNowToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            panelHeader = new Panel();
            linkLabelAbout = new LinkLabel();
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            labelSubtitle = new Label();
            labelEnableScreenshots = new Label();
            toggleSwitchEnabled = new ToggleSwitch();
            labelEnabledStatus = new Label();
            labelCaptureMode = new Label();
            comboBoxCaptureMode = new ComboBox();
            labelSaveFolder = new Label();
            textBoxFolder = new TextBox();
            buttonBrowse = new Button();
            labelInterval = new Label();
            numericUpDownInterval = new NumericUpDown();
            labelFileFormat = new Label();
            comboBoxFormat = new ComboBox();
            labelQuality = new Label();
            trackBarQuality = new TrackBar();
            labelQualityValue = new Label();
            buttonOpenFolder = new Button();
            buttonSave = new Button();
            labelStatus = new Label();
            contextMenuStrip.SuspendLayout();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarQuality).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Text = "Capillume";
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            // 
            // contextMenuStrip
            // 
            contextMenuStrip.ImageScalingSize = new Size(32, 32);
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { showToolStripMenuItem, toolStripSeparator1, captureNowToolStripMenuItem, toolStripSeparator2, aboutToolStripMenuItem, toolStripSeparator3, exitToolStripMenuItem });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(229, 174);
            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new Size(228, 38);
            showToolStripMenuItem.Text = "Show";
            showToolStripMenuItem.Click += ShowToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(225, 6);
            // 
            // captureNowToolStripMenuItem
            // 
            captureNowToolStripMenuItem.Name = "captureNowToolStripMenuItem";
            captureNowToolStripMenuItem.Size = new Size(228, 38);
            captureNowToolStripMenuItem.Text = "Capture Now";
            captureNowToolStripMenuItem.Click += CaptureNowToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(225, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(228, 38);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(225, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(228, 38);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(linkLabelAbout);
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Controls.Add(labelSubtitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(6);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1480, 160);
            panelHeader.TabIndex = 0;
            // 
            // linkLabelAbout
            // 
            linkLabelAbout.AutoSize = true;
            linkLabelAbout.Font = new Font("Segoe UI", 9F);
            linkLabelAbout.Location = new Point(1300, 60);
            linkLabelAbout.Margin = new Padding(6, 0, 6, 0);
            linkLabelAbout.Name = "linkLabelAbout";
            linkLabelAbout.Size = new Size(79, 32);
            linkLabelAbout.TabIndex = 3;
            linkLabelAbout.TabStop = true;
            linkLabelAbout.Text = "About";
            linkLabelAbout.LinkClicked += LinkLabelAbout_LinkClicked;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = Color.Transparent;
            pictureBoxLogo.Location = new Point(50, 30);
            pictureBoxLogo.Margin = new Padding(6);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(100, 100);
            pictureBoxLogo.TabIndex = 2;
            pictureBoxLogo.TabStop = false;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 24F);
            labelTitle.ForeColor = Color.FromArgb(64, 64, 64);
            labelTitle.Location = new Point(170, 30);
            labelTitle.Margin = new Padding(6, 0, 6, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(320, 86);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Capillume";
            // 
            // labelSubtitle
            // 
            labelSubtitle.AutoSize = true;
            labelSubtitle.Font = new Font("Segoe UI", 9F);
            labelSubtitle.ForeColor = Color.Gray;
            labelSubtitle.Location = new Point(180, 112);
            labelSubtitle.Margin = new Padding(6, 0, 6, 0);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(185, 32);
            labelSubtitle.TabIndex = 1;
            labelSubtitle.Text = "Settings • v1.2.0";
            // 
            // labelEnableScreenshots
            // 
            labelEnableScreenshots.AutoSize = true;
            labelEnableScreenshots.Font = new Font("Segoe UI", 10F);
            labelEnableScreenshots.Location = new Point(60, 210);
            labelEnableScreenshots.Margin = new Padding(6, 0, 6, 0);
            labelEnableScreenshots.Name = "labelEnableScreenshots";
            labelEnableScreenshots.Size = new Size(244, 37);
            labelEnableScreenshots.TabIndex = 1;
            labelEnableScreenshots.Text = "Enable Screenshots";
            // 
            // toggleSwitchEnabled
            // 
            toggleSwitchEnabled.Checked = false;
            toggleSwitchEnabled.Location = new Point(1320, 206);
            toggleSwitchEnabled.Margin = new Padding(6);
            toggleSwitchEnabled.Name = "toggleSwitchEnabled";
            toggleSwitchEnabled.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchEnabled.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchEnabled.Size = new Size(100, 50);
            toggleSwitchEnabled.TabIndex = 2;
            toggleSwitchEnabled.ThumbColor = Color.White;
            toggleSwitchEnabled.CheckedChanged += ToggleSwitchEnabled_CheckedChanged;
            // 
            // labelEnabledStatus
            // 
            labelEnabledStatus.AutoSize = true;
            labelEnabledStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelEnabledStatus.ForeColor = Color.Gray;
            labelEnabledStatus.Location = new Point(1240, 216);
            labelEnabledStatus.Margin = new Padding(6, 0, 6, 0);
            labelEnabledStatus.Name = "labelEnabledStatus";
            labelEnabledStatus.Size = new Size(56, 32);
            labelEnabledStatus.TabIndex = 3;
            labelEnabledStatus.Text = "OFF";
            labelEnabledStatus.TextAlign = ContentAlignment.MiddleRight;
            // 
            // labelCaptureMode
            // 
            labelCaptureMode.AutoSize = true;
            labelCaptureMode.Font = new Font("Segoe UI", 10F);
            labelCaptureMode.Location = new Point(60, 320);
            labelCaptureMode.Margin = new Padding(6, 0, 6, 0);
            labelCaptureMode.Name = "labelCaptureMode";
            labelCaptureMode.Size = new Size(188, 37);
            labelCaptureMode.TabIndex = 4;
            labelCaptureMode.Text = "Capture Mode";
            // 
            // comboBoxCaptureMode
            // 
            comboBoxCaptureMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCaptureMode.Font = new Font("Segoe UI", 10F);
            comboBoxCaptureMode.FormattingEnabled = true;
            comboBoxCaptureMode.Items.AddRange(new object[] { "Full Screen", "Active Window" });
            comboBoxCaptureMode.Location = new Point(60, 370);
            comboBoxCaptureMode.Margin = new Padding(6);
            comboBoxCaptureMode.Name = "comboBoxCaptureMode";
            comboBoxCaptureMode.Size = new Size(1356, 45);
            comboBoxCaptureMode.TabIndex = 5;
            // 
            // labelSaveFolder
            // 
            labelSaveFolder.AutoSize = true;
            labelSaveFolder.Font = new Font("Segoe UI", 10F);
            labelSaveFolder.Location = new Point(60, 460);
            labelSaveFolder.Margin = new Padding(6, 0, 6, 0);
            labelSaveFolder.Name = "labelSaveFolder";
            labelSaveFolder.Size = new Size(154, 37);
            labelSaveFolder.TabIndex = 6;
            labelSaveFolder.Text = "Save Folder";
            // 
            // textBoxFolder
            // 
            textBoxFolder.BackColor = Color.White;
            textBoxFolder.Font = new Font("Segoe UI", 10F);
            textBoxFolder.Location = new Point(60, 510);
            textBoxFolder.Margin = new Padding(6);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(1106, 43);
            textBoxFolder.TabIndex = 7;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Font = new Font("Segoe UI", 10F);
            buttonBrowse.Location = new Point(1190, 508);
            buttonBrowse.Margin = new Padding(6);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(230, 56);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // labelInterval
            // 
            labelInterval.AutoSize = true;
            labelInterval.Font = new Font("Segoe UI", 10F);
            labelInterval.Location = new Point(60, 610);
            labelInterval.Margin = new Padding(6, 0, 6, 0);
            labelInterval.Name = "labelInterval";
            labelInterval.Size = new Size(222, 37);
            labelInterval.TabIndex = 9;
            labelInterval.Text = "Interval (minutes)";
            // 
            // numericUpDownInterval
            // 
            numericUpDownInterval.Font = new Font("Segoe UI", 10F);
            numericUpDownInterval.Location = new Point(60, 660);
            numericUpDownInterval.Margin = new Padding(6);
            numericUpDownInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numericUpDownInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownInterval.Name = "numericUpDownInterval";
            numericUpDownInterval.Size = new Size(1360, 43);
            numericUpDownInterval.TabIndex = 10;
            numericUpDownInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // labelFileFormat
            // 
            labelFileFormat.AutoSize = true;
            labelFileFormat.Font = new Font("Segoe UI", 10F);
            labelFileFormat.Location = new Point(60, 750);
            labelFileFormat.Margin = new Padding(6, 0, 6, 0);
            labelFileFormat.Name = "labelFileFormat";
            labelFileFormat.Size = new Size(149, 37);
            labelFileFormat.TabIndex = 11;
            labelFileFormat.Text = "File Format";
            // 
            // comboBoxFormat
            // 
            comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFormat.Font = new Font("Segoe UI", 10F);
            comboBoxFormat.FormattingEnabled = true;
            comboBoxFormat.Items.AddRange(new object[] { "PNG", "JPG", "BMP", "WEBP" });
            comboBoxFormat.Location = new Point(60, 800);
            comboBoxFormat.Margin = new Padding(6);
            comboBoxFormat.Name = "comboBoxFormat";
            comboBoxFormat.Size = new Size(1356, 45);
            comboBoxFormat.TabIndex = 12;
            comboBoxFormat.SelectedIndexChanged += ComboBoxFormat_SelectedIndexChanged;
            // 
            // labelQuality
            // 
            labelQuality.AutoSize = true;
            labelQuality.Font = new Font("Segoe UI", 10F);
            labelQuality.Location = new Point(60, 890);
            labelQuality.Margin = new Padding(6, 0, 6, 0);
            labelQuality.Name = "labelQuality";
            labelQuality.Size = new Size(102, 37);
            labelQuality.TabIndex = 13;
            labelQuality.Text = "Quality";
            // 
            // trackBarQuality
            // 
            trackBarQuality.Location = new Point(60, 940);
            trackBarQuality.Margin = new Padding(6);
            trackBarQuality.Maximum = 100;
            trackBarQuality.Minimum = 1;
            trackBarQuality.Name = "trackBarQuality";
            trackBarQuality.Size = new Size(1280, 90);
            trackBarQuality.TabIndex = 14;
            trackBarQuality.TickFrequency = 10;
            trackBarQuality.Value = 70;
            trackBarQuality.Scroll += TrackBarQuality_Scroll;
            // 
            // labelQualityValue
            // 
            labelQualityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelQualityValue.Location = new Point(1340, 954);
            labelQualityValue.Margin = new Padding(6, 0, 6, 0);
            labelQualityValue.Name = "labelQualityValue";
            labelQualityValue.Size = new Size(80, 38);
            labelQualityValue.TabIndex = 15;
            labelQualityValue.Text = "70%";
            labelQualityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.BackColor = Color.White;
            buttonOpenFolder.FlatStyle = FlatStyle.Flat;
            buttonOpenFolder.Font = new Font("Segoe UI", 10F);
            buttonOpenFolder.Location = new Point(934, 1080);
            buttonOpenFolder.Margin = new Padding(6);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(230, 70);
            buttonOpenFolder.TabIndex = 16;
            buttonOpenFolder.Text = "Open Folder";
            buttonOpenFolder.UseVisualStyleBackColor = true;
            buttonOpenFolder.Click += ButtonOpenFolder_Click;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = Color.FromArgb(0, 120, 212);
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.FlatStyle = FlatStyle.Flat;
            buttonSave.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonSave.ForeColor = Color.White;
            buttonSave.Location = new Point(1190, 1080);
            buttonSave.Margin = new Padding(6);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(230, 70);
            buttonSave.TabIndex = 17;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += ButtonSave_Click;
            // 
            // labelStatus
            // 
            labelStatus.BackColor = Color.FromArgb(240, 240, 240);
            labelStatus.Dock = DockStyle.Bottom;
            labelStatus.Font = new Font("Segoe UI", 8.25F);
            labelStatus.ForeColor = Color.Gray;
            labelStatus.Location = new Point(0, 1200);
            labelStatus.Margin = new Padding(6, 0, 6, 0);
            labelStatus.Name = "labelStatus";
            labelStatus.Padding = new Padding(20, 16, 20, 16);
            labelStatus.Size = new Size(1480, 80);
            labelStatus.TabIndex = 18;
            labelStatus.Text = "Capillume is running in the background. Right-click the tray icon to capture or quit.";
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(1480, 1280);
            Controls.Add(labelStatus);
            Controls.Add(buttonSave);
            Controls.Add(buttonOpenFolder);
            Controls.Add(labelQualityValue);
            Controls.Add(trackBarQuality);
            Controls.Add(labelQuality);
            Controls.Add(comboBoxFormat);
            Controls.Add(labelFileFormat);
            Controls.Add(numericUpDownInterval);
            Controls.Add(labelInterval);
            Controls.Add(buttonBrowse);
            Controls.Add(textBoxFolder);
            Controls.Add(labelSaveFolder);
            Controls.Add(comboBoxCaptureMode);
            Controls.Add(labelCaptureMode);
            Controls.Add(labelEnabledStatus);
            Controls.Add(toggleSwitchEnabled);
            Controls.Add(labelEnableScreenshots);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6);
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Capillume";
            FormClosing += Form1_FormClosing;
            contextMenuStrip.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInterval).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarQuality).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem captureNowToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem exitToolStripMenuItem;

        private Panel panelHeader;
        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelSubtitle;
        private LinkLabel linkLabelAbout;

        private Label labelEnableScreenshots;
        private ToggleSwitch toggleSwitchEnabled;
        private Label labelEnabledStatus;

        private Label labelCaptureMode;
        private ComboBox comboBoxCaptureMode;

        private Label labelSaveFolder;
        private TextBox textBoxFolder;
        private Button buttonBrowse;

        private Label labelInterval;
        private NumericUpDown numericUpDownInterval;

        private Label labelFileFormat;
        private ComboBox comboBoxFormat;

        private Label labelQuality;
        private TrackBar trackBarQuality;
        private Label labelQualityValue;

        private Button buttonOpenFolder;
        private Button buttonSave;
        private Label labelStatus;
    }
}
