namespace Capillume
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
            toolTipCaptureNow = new ToolTip(components);
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
            CaptureNow = new CaptureNowButton();
            labelEnableScreenshots = new Label();
            toggleSwitchEnabled = new ToggleSwitch();
            toggleSwitchNotify = new ToggleSwitch();
            toggleSwitchStartWithWindows = new ToggleSwitch();
            labelCaptureMode = new Label();
            comboBoxCaptureMode = new ComboBox();
            labelInterval = new Label();
            numericUpDownInterval = new NumericUpDown();
            labelFileFormat = new Label();
            comboBoxFormat = new ComboBox();
            labelQuality = new Label();
            trackBarQuality = new TrackBar();
            labelQualityValue = new Label();
            buttonSave = new Button();
            labelShowNotifications = new Label();
            labelStartWithWindows = new Label();
            labelStatus = new Label();
            checkBoxIncludeCapillume = new CheckBox();
            groupBox2 = new GroupBox();
            panel5 = new Panel();
            panel6 = new Panel();
            buttonOpenFolder = new Button();
            buttonBrowse = new Button();
            textBoxFolder = new TextBox();
            groupBox3 = new GroupBox();
            panel3 = new Panel();
            panel4 = new Panel();
            groupBox1 = new GroupBox();
            buttonSettings = new Button();
            contextMenuStrip.SuspendLayout();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownInterval).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarQuality).BeginInit();
            groupBox2.SuspendLayout();
            panel5.SuspendLayout();
            groupBox3.SuspendLayout();
            panel3.SuspendLayout();
            groupBox1.SuspendLayout();
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
            panelHeader.BackColor = Color.Gainsboro;
            panelHeader.Controls.Add(linkLabelAbout);
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Controls.Add(CaptureNow);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(6);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1480, 139);
            panelHeader.TabIndex = 1;
            // 
            // linkLabelAbout
            // 
            linkLabelAbout.AutoSize = true;
            linkLabelAbout.Font = new Font("Segoe UI", 10F);
            linkLabelAbout.Location = new Point(462, 75);
            linkLabelAbout.Margin = new Padding(6, 0, 6, 0);
            linkLabelAbout.Name = "linkLabelAbout";
            linkLabelAbout.Size = new Size(90, 37);
            linkLabelAbout.TabIndex = 1;
            linkLabelAbout.TabStop = true;
            linkLabelAbout.Text = "About";
            linkLabelAbout.LinkClicked += LinkLabelAbout_LinkClicked;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = Color.Transparent;
            pictureBoxLogo.Image = Properties.Resources.icon;
            pictureBoxLogo.Location = new Point(28, 30);
            pictureBoxLogo.Margin = new Padding(6);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(90, 90);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 2;
            pictureBoxLogo.TabStop = false;
            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 24F);
            labelTitle.ForeColor = Color.FromArgb(64, 64, 64);
            labelTitle.Location = new Point(130, 34);
            labelTitle.Margin = new Padding(6, 0, 6, 0);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(320, 86);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Capillume";
            // 
            // CaptureNow
            // 
            CaptureNow.AccessibleName = "Capture screenshot now";
            CaptureNow.AccessibleRole = AccessibleRole.PushButton;
            CaptureNow.ButtonBackColor = Color.FromArgb(0, 120, 212);
            CaptureNow.ButtonTextColor = Color.White;
            CaptureNow.CornerRadius = 10;
            CaptureNow.Font = new Font("Segoe UI Semibold", 10F);
            CaptureNow.HoverBackColor = Color.FromArgb(16, 110, 190);
            CaptureNow.Location = new Point(1155, 30);
            CaptureNow.Name = "CaptureNow";
            CaptureNow.PressedBackColor = Color.FromArgb(0, 92, 158);
            CaptureNow.Size = new Size(299, 86);
            CaptureNow.TabIndex = 2;
            CaptureNow.Text = "&Capture Now";
            CaptureNow.Click += CaptureNow_Click;
            // 
            // labelEnableScreenshots
            // 
            labelEnableScreenshots.AutoSize = true;
            labelEnableScreenshots.Font = new Font("Segoe UI", 10F);
            labelEnableScreenshots.Location = new Point(30, 62);
            labelEnableScreenshots.Margin = new Padding(6, 0, 6, 0);
            labelEnableScreenshots.Name = "labelEnableScreenshots";
            labelEnableScreenshots.Size = new Size(281, 37);
            labelEnableScreenshots.TabIndex = 0;
            labelEnableScreenshots.Text = "&Automate Screenshots";
            // 
            // toggleSwitchEnabled
            // 
            toggleSwitchEnabled.Checked = false;
            toggleSwitchEnabled.Location = new Point(574, 58);
            toggleSwitchEnabled.Margin = new Padding(6);
            toggleSwitchEnabled.Name = "toggleSwitchEnabled";
            toggleSwitchEnabled.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchEnabled.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchEnabled.Size = new Size(100, 45);
            toggleSwitchEnabled.TabIndex = 1;
            toggleSwitchEnabled.ThumbColor = Color.White;
            toggleSwitchEnabled.CheckedChanged += ToggleSwitchEnabled_CheckedChanged;
            // 
            // toggleSwitchNotify
            // 
            toggleSwitchNotify.Checked = false;
            toggleSwitchNotify.Location = new Point(574, 119);
            toggleSwitchNotify.Margin = new Padding(6);
            toggleSwitchNotify.Name = "toggleSwitchNotify";
            toggleSwitchNotify.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchNotify.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchNotify.Size = new Size(100, 45);
            toggleSwitchNotify.TabIndex = 3;
            toggleSwitchNotify.ThumbColor = Color.White;
            toggleSwitchNotify.CheckedChanged += ToggleSwitchNotify_CheckedChanged;
            // 
            // toggleSwitchStartWithWindows
            // 
            toggleSwitchStartWithWindows.Checked = false;
            toggleSwitchStartWithWindows.Location = new Point(574, 185);
            toggleSwitchStartWithWindows.Margin = new Padding(6);
            toggleSwitchStartWithWindows.Name = "toggleSwitchStartWithWindows";
            toggleSwitchStartWithWindows.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchStartWithWindows.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchStartWithWindows.Size = new Size(100, 45);
            toggleSwitchStartWithWindows.TabIndex = 5;
            toggleSwitchStartWithWindows.ThumbColor = Color.White;
            toggleSwitchStartWithWindows.CheckedChanged += ToggleSwitchStartWithWindows_CheckedChanged;
            // 
            // labelCaptureMode
            // 
            labelCaptureMode.AutoSize = true;
            labelCaptureMode.Font = new Font("Segoe UI", 10F);
            labelCaptureMode.Location = new Point(30, 253);
            labelCaptureMode.Margin = new Padding(6, 0, 6, 0);
            labelCaptureMode.Name = "labelCaptureMode";
            labelCaptureMode.Size = new Size(188, 37);
            labelCaptureMode.TabIndex = 6;
            labelCaptureMode.Text = "Capture &Mode";
            // 
            // comboBoxCaptureMode
            // 
            comboBoxCaptureMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCaptureMode.Font = new Font("Segoe UI", 10F);
            comboBoxCaptureMode.FormattingEnabled = true;
            comboBoxCaptureMode.Items.AddRange(new object[] { "Full Screen", "Active Window" });
            comboBoxCaptureMode.Location = new Point(30, 296);
            comboBoxCaptureMode.Margin = new Padding(6);
            comboBoxCaptureMode.Name = "comboBoxCaptureMode";
            comboBoxCaptureMode.Size = new Size(256, 45);
            comboBoxCaptureMode.TabIndex = 7;
            comboBoxCaptureMode.SelectedIndexChanged += ComboBoxCaptureMode_SelectedIndexChanged;
            // 
            // labelInterval
            // 
            labelInterval.AutoSize = true;
            labelInterval.Font = new Font("Segoe UI", 10F);
            labelInterval.Location = new Point(409, 253);
            labelInterval.Margin = new Padding(6, 0, 6, 0);
            labelInterval.Name = "labelInterval";
            labelInterval.Size = new Size(222, 37);
            labelInterval.TabIndex = 8;
            labelInterval.Text = "&Interval (minutes)";
            // 
            // numericUpDownInterval
            // 
            numericUpDownInterval.Font = new Font("Segoe UI", 10F);
            numericUpDownInterval.Location = new Point(418, 298);
            numericUpDownInterval.Margin = new Padding(6);
            numericUpDownInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numericUpDownInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownInterval.Name = "numericUpDownInterval";
            numericUpDownInterval.Size = new Size(256, 43);
            numericUpDownInterval.TabIndex = 9;
            numericUpDownInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDownInterval.ValueChanged += NumericUpDownInterval_ValueChanged;
            // 
            // labelFileFormat
            // 
            labelFileFormat.AutoSize = true;
            labelFileFormat.Font = new Font("Segoe UI", 10F);
            labelFileFormat.Location = new Point(43, 62);
            labelFileFormat.Margin = new Padding(6, 0, 6, 0);
            labelFileFormat.Name = "labelFileFormat";
            labelFileFormat.Size = new Size(149, 37);
            labelFileFormat.TabIndex = 0;
            labelFileFormat.Text = "&File Format";
            // 
            // comboBoxFormat
            // 
            comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFormat.Font = new Font("Segoe UI", 10F);
            comboBoxFormat.FormattingEnabled = true;
            comboBoxFormat.Items.AddRange(new object[] { "JPG", "PNG", "BMP", "WEBP" });
            comboBoxFormat.Location = new Point(43, 119);
            comboBoxFormat.Margin = new Padding(6);
            comboBoxFormat.Name = "comboBoxFormat";
            comboBoxFormat.Size = new Size(149, 45);
            comboBoxFormat.TabIndex = 1;
            comboBoxFormat.SelectedIndexChanged += ComboBoxFormat_SelectedIndexChanged;
            // 
            // labelQuality
            // 
            labelQuality.AutoSize = true;
            labelQuality.Font = new Font("Segoe UI", 10F);
            labelQuality.Location = new Point(229, 62);
            labelQuality.Margin = new Padding(6, 0, 6, 0);
            labelQuality.Name = "labelQuality";
            labelQuality.Size = new Size(183, 37);
            labelQuality.TabIndex = 3;
            labelQuality.Text = "Image &Quality";
            // 
            // trackBarQuality
            // 
            trackBarQuality.AutoSize = false;
            trackBarQuality.Location = new Point(229, 119);
            trackBarQuality.Margin = new Padding(6);
            trackBarQuality.Maximum = 100;
            trackBarQuality.Minimum = 1;
            trackBarQuality.Name = "trackBarQuality";
            trackBarQuality.Size = new Size(359, 45);
            trackBarQuality.TabIndex = 4;
            trackBarQuality.TickFrequency = 10;
            trackBarQuality.TickStyle = TickStyle.None;
            trackBarQuality.Value = 70;
            trackBarQuality.Scroll += TrackBarQuality_Scroll;
            trackBarQuality.ValueChanged += TrackBarQuality_ValueChanged;
            // 
            // labelQualityValue
            // 
            labelQualityValue.AutoSize = true;
            labelQualityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelQualityValue.Location = new Point(600, 119);
            labelQualityValue.Margin = new Padding(6, 0, 6, 0);
            labelQualityValue.Name = "labelQualityValue";
            labelQualityValue.Size = new Size(72, 37);
            labelQualityValue.TabIndex = 5;
            labelQualityValue.Text = "70%";
            labelQualityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = Color.FromArgb(0, 120, 212);
            buttonSave.FlatAppearance.BorderSize = 0;
            buttonSave.FlatStyle = FlatStyle.Flat;
            buttonSave.Font = new Font("Segoe UI", 10F);
            buttonSave.ForeColor = Color.White;
            buttonSave.Location = new Point(1224, 682);
            buttonSave.Margin = new Padding(6);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(230, 70);
            buttonSave.TabIndex = 0;
            buttonSave.Text = "&Save Settings";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += ButtonSave_Click;
            // 
            // labelShowNotifications
            // 
            labelShowNotifications.AutoSize = true;
            labelShowNotifications.Font = new Font("Segoe UI", 10F);
            labelShowNotifications.Location = new Point(30, 123);
            labelShowNotifications.Name = "labelShowNotifications";
            labelShowNotifications.Size = new Size(240, 37);
            labelShowNotifications.TabIndex = 2;
            labelShowNotifications.Text = "Show &Notifications";
            // 
            // labelStartWithWindows
            // 
            labelStartWithWindows.AutoSize = true;
            labelStartWithWindows.Font = new Font("Segoe UI", 10F);
            labelStartWithWindows.Location = new Point(30, 189);
            labelStartWithWindows.Name = "labelStartWithWindows";
            labelStartWithWindows.Size = new Size(251, 37);
            labelStartWithWindows.TabIndex = 4;
            labelStartWithWindows.Text = "Start With &Windows";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Font = new Font("Segoe UI", 10F);
            labelStatus.Location = new Point(754, 699);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(95, 37);
            labelStatus.TabIndex = 6;
            labelStatus.Text = "Ready.";
            // 
            // checkBoxIncludeCapillume
            // 
            checkBoxIncludeCapillume.AutoSize = true;
            checkBoxIncludeCapillume.Location = new Point(43, 189);
            checkBoxIncludeCapillume.Name = "checkBoxIncludeCapillume";
            checkBoxIncludeCapillume.Size = new Size(435, 41);
            checkBoxIncludeCapillume.TabIndex = 6;
            checkBoxIncludeCapillume.Text = "Include Capillume in screenshots";
            checkBoxIncludeCapillume.UseVisualStyleBackColor = true;
            checkBoxIncludeCapillume.CheckedChanged += CheckBoxIncludeCapillume_CheckedChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(panel5);
            groupBox2.Controls.Add(labelFileFormat);
            groupBox2.Controls.Add(comboBoxFormat);
            groupBox2.Controls.Add(labelQuality);
            groupBox2.Controls.Add(checkBoxIncludeCapillume);
            groupBox2.Controls.Add(trackBarQuality);
            groupBox2.Controls.Add(labelQualityValue);
            groupBox2.Font = new Font("Segoe UI", 10F);
            groupBox2.Location = new Point(754, 181);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(700, 290);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Output";
            // 
            // panel5
            // 
            panel5.BorderStyle = BorderStyle.FixedSingle;
            panel5.Controls.Add(panel6);
            panel5.Location = new Point(217, 64);
            panel5.Name = "panel5";
            panel5.Size = new Size(1, 100);
            panel5.TabIndex = 2;
            // 
            // panel6
            // 
            panel6.BorderStyle = BorderStyle.FixedSingle;
            panel6.Location = new Point(0, -1);
            panel6.Name = "panel6";
            panel6.Size = new Size(1, 50);
            panel6.TabIndex = 29;
            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.Font = new Font("Segoe UI", 10F);
            buttonOpenFolder.Location = new Point(444, 119);
            buttonOpenFolder.Margin = new Padding(6);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(230, 46);
            buttonOpenFolder.TabIndex = 2;
            buttonOpenFolder.Text = "&Open Folder";
            buttonOpenFolder.UseVisualStyleBackColor = true;
            buttonOpenFolder.Click += ButtonOpenFolder_Click;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Font = new Font("Segoe UI", 10F);
            buttonBrowse.Location = new Point(444, 61);
            buttonBrowse.Margin = new Padding(6);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(230, 46);
            buttonBrowse.TabIndex = 1;
            buttonBrowse.Text = "&Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // textBoxFolder
            // 
            textBoxFolder.BackColor = Color.White;
            textBoxFolder.Font = new Font("Segoe UI", 10F);
            textBoxFolder.Location = new Point(30, 63);
            textBoxFolder.Margin = new Padding(6);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(402, 43);
            textBoxFolder.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBoxFolder);
            groupBox3.Controls.Add(buttonBrowse);
            groupBox3.Controls.Add(buttonOpenFolder);
            groupBox3.Font = new Font("Segoe UI", 10F);
            groupBox3.Location = new Point(28, 569);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(700, 183);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Storage";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(panel4);
            panel3.Location = new Point(350, 292);
            panel3.Name = "panel3";
            panel3.Size = new Size(1, 50);
            panel3.TabIndex = 28;
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Location = new Point(0, -1);
            panel4.Name = "panel4";
            panel4.Size = new Size(1, 50);
            panel4.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(labelEnableScreenshots);
            groupBox1.Controls.Add(toggleSwitchEnabled);
            groupBox1.Controls.Add(labelShowNotifications);
            groupBox1.Controls.Add(panel3);
            groupBox1.Controls.Add(toggleSwitchNotify);
            groupBox1.Controls.Add(labelStartWithWindows);
            groupBox1.Controls.Add(toggleSwitchStartWithWindows);
            groupBox1.Controls.Add(labelCaptureMode);
            groupBox1.Controls.Add(comboBoxCaptureMode);
            groupBox1.Controls.Add(labelInterval);
            groupBox1.Controls.Add(numericUpDownInterval);
            groupBox1.Font = new Font("Segoe UI", 10F);
            groupBox1.Location = new Point(28, 181);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(700, 365);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Automation";
            // 
            // buttonSettings
            // 
            buttonSettings.Font = new Font("Segoe UI", 10F);
            buttonSettings.Location = new Point(754, 479);
            buttonSettings.Name = "buttonSettings";
            buttonSettings.Size = new Size(700, 67);
            buttonSettings.TabIndex = 5;
            buttonSettings.Text = "A&dvanced Settings";
            buttonSettings.UseVisualStyleBackColor = true;
            buttonSettings.Click += ButtonSettings_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(1480, 772);
            Controls.Add(buttonSettings);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(labelStatus);
            Controls.Add(buttonSave);
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
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            panel5.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            panel3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon;
        private ToolTip toolTipCaptureNow;
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
        private LinkLabel linkLabelAbout;

        private Label labelEnableScreenshots;
        private ToggleSwitch toggleSwitchEnabled;
        private CaptureNowButton CaptureNow;

        private Label labelShowNotifications;
        private ToggleSwitch toggleSwitchNotify;

        private ToggleSwitch toggleSwitchStartWithWindows;

        private Label labelCaptureMode;
        private ComboBox comboBoxCaptureMode;

        private Label labelInterval;
        private NumericUpDown numericUpDownInterval;

        private Label labelFileFormat;
        private ComboBox comboBoxFormat;

        private Label labelQuality;
        private TrackBar trackBarQuality;
        private Label labelQualityValue;
        private Button buttonSave;
        private Label labelStartWithWindows;
        private Label labelStatus;
        private CheckBox checkBoxIncludeCapillume;
        private GroupBox groupBox2;
        private Panel panel5;
        private Panel panel6;
        private Button buttonOpenFolder;
        private Button buttonBrowse;
        private TextBox textBoxFolder;
        private GroupBox groupBox3;
        private Panel panel3;
        private Panel panel4;
        private GroupBox groupBox1;
        private Button buttonSettings;
    }
}
