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
            labelSubtitle = new Label();
            CaptureNow = new CaptureNowButton();
            labelEnableScreenshots = new Label();
            toggleSwitchEnabled = new ToggleSwitch();
            toggleSwitchNotify = new ToggleSwitch();
            toggleSwitchStartWithWindows = new ToggleSwitch();
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
            panel1 = new Panel();
            panel2 = new Panel();
            labelShowNotifications = new Label();
            labelStartWithWindows = new Label();
            labelStatus = new Label();
            buttonWatermark = new Button();
            buttonAnnotation = new Button();
            panel3 = new Panel();
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
            panelHeader.Controls.Add(CaptureNow);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(6);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1480, 160);
            panelHeader.TabIndex = 1;
            // 
            // linkLabelAbout
            // 
            linkLabelAbout.AutoSize = true;
            linkLabelAbout.Font = new Font("Segoe UI", 9F);
            linkLabelAbout.Location = new Point(398, 112);
            linkLabelAbout.Margin = new Padding(6, 0, 6, 0);
            linkLabelAbout.Name = "linkLabelAbout";
            linkLabelAbout.Size = new Size(79, 32);
            linkLabelAbout.TabIndex = 2;
            linkLabelAbout.TabStop = true;
            linkLabelAbout.Text = "About";
            linkLabelAbout.LinkClicked += LinkLabelAbout_LinkClicked;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = Color.Transparent;
            pictureBoxLogo.Image = Properties.Resources.icon;
            pictureBoxLogo.Location = new Point(45, 30);
            pictureBoxLogo.Margin = new Padding(6);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(114, 114);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
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
            labelSubtitle.Size = new Size(206, 32);
            labelSubtitle.TabIndex = 1;
            labelSubtitle.Text = "Settings • v1.X.X.X";
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
            CaptureNow.Location = new Point(1123, 30);
            CaptureNow.Name = "CaptureNow";
            CaptureNow.PressedBackColor = Color.FromArgb(0, 92, 158);
            CaptureNow.Size = new Size(299, 114);
            CaptureNow.TabIndex = 3;
            CaptureNow.Text = "&Capture Now";
            CaptureNow.Click += CaptureNow_Click;
            // 
            // labelEnableScreenshots
            // 
            labelEnableScreenshots.AutoSize = true;
            labelEnableScreenshots.Font = new Font("Segoe UI", 10F);
            labelEnableScreenshots.Location = new Point(60, 215);
            labelEnableScreenshots.Margin = new Padding(6, 0, 6, 0);
            labelEnableScreenshots.Name = "labelEnableScreenshots";
            labelEnableScreenshots.Size = new Size(281, 37);
            labelEnableScreenshots.TabIndex = 2;
            labelEnableScreenshots.Text = "A&utomate Screenshots";
            // 
            // toggleSwitchEnabled
            // 
            toggleSwitchEnabled.Checked = false;
            toggleSwitchEnabled.Location = new Point(400, 215);
            toggleSwitchEnabled.Margin = new Padding(6);
            toggleSwitchEnabled.Name = "toggleSwitchEnabled";
            toggleSwitchEnabled.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchEnabled.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchEnabled.Size = new Size(100, 45);
            toggleSwitchEnabled.TabIndex = 4;
            toggleSwitchEnabled.ThumbColor = Color.White;
            toggleSwitchEnabled.CheckedChanged += ToggleSwitchEnabled_CheckedChanged;
            // 
            // toggleSwitchNotify
            // 
            toggleSwitchNotify.Checked = false;
            toggleSwitchNotify.Location = new Point(873, 211);
            toggleSwitchNotify.Margin = new Padding(6);
            toggleSwitchNotify.Name = "toggleSwitchNotify";
            toggleSwitchNotify.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchNotify.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchNotify.Size = new Size(100, 45);
            toggleSwitchNotify.TabIndex = 6;
            toggleSwitchNotify.ThumbColor = Color.White;
            toggleSwitchNotify.CheckedChanged += ToggleSwitchNotify_CheckedChanged;
            // 
            // toggleSwitchStartWithWindows
            // 
            toggleSwitchStartWithWindows.Checked = false;
            toggleSwitchStartWithWindows.Location = new Point(1322, 211);
            toggleSwitchStartWithWindows.Margin = new Padding(6);
            toggleSwitchStartWithWindows.Name = "toggleSwitchStartWithWindows";
            toggleSwitchStartWithWindows.OffColor = Color.FromArgb(200, 200, 200);
            toggleSwitchStartWithWindows.OnColor = Color.FromArgb(0, 120, 212);
            toggleSwitchStartWithWindows.Size = new Size(100, 45);
            toggleSwitchStartWithWindows.TabIndex = 8;
            toggleSwitchStartWithWindows.ThumbColor = Color.White;
            toggleSwitchStartWithWindows.CheckedChanged += ToggleSwitchStartWithWindows_CheckedChanged;
            // 
            // labelCaptureMode
            // 
            labelCaptureMode.AutoSize = true;
            labelCaptureMode.Font = new Font("Segoe UI", 10F);
            labelCaptureMode.Location = new Point(60, 320);
            labelCaptureMode.Margin = new Padding(6, 0, 6, 0);
            labelCaptureMode.Name = "labelCaptureMode";
            labelCaptureMode.Size = new Size(188, 37);
            labelCaptureMode.TabIndex = 10;
            labelCaptureMode.Text = "Capture &Mode";
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
            comboBoxCaptureMode.Size = new Size(626, 45);
            comboBoxCaptureMode.TabIndex = 11;
            comboBoxCaptureMode.SelectedIndexChanged += ComboBoxCaptureMode_SelectedIndexChanged;
            // 
            // labelSaveFolder
            // 
            labelSaveFolder.AutoSize = true;
            labelSaveFolder.Font = new Font("Segoe UI", 10F);
            labelSaveFolder.Location = new Point(60, 568);
            labelSaveFolder.Margin = new Padding(6, 0, 6, 0);
            labelSaveFolder.Name = "labelSaveFolder";
            labelSaveFolder.Size = new Size(154, 37);
            labelSaveFolder.TabIndex = 19;
            labelSaveFolder.Text = "S&ave Folder";
            // 
            // textBoxFolder
            // 
            textBoxFolder.BackColor = Color.White;
            textBoxFolder.Font = new Font("Segoe UI", 10F);
            textBoxFolder.Location = new Point(60, 618);
            textBoxFolder.Margin = new Padding(6);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(347, 43);
            textBoxFolder.TabIndex = 20;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Font = new Font("Segoe UI", 10F);
            buttonBrowse.Location = new Point(419, 615);
            buttonBrowse.Margin = new Padding(6);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(230, 46);
            buttonBrowse.TabIndex = 21;
            buttonBrowse.Text = "&Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // labelInterval
            // 
            labelInterval.AutoSize = true;
            labelInterval.Font = new Font("Segoe UI", 10F);
            labelInterval.Location = new Point(790, 320);
            labelInterval.Margin = new Padding(6, 0, 6, 0);
            labelInterval.Name = "labelInterval";
            labelInterval.Size = new Size(222, 37);
            labelInterval.TabIndex = 12;
            labelInterval.Text = "&Interval (minutes)";
            // 
            // numericUpDownInterval
            // 
            numericUpDownInterval.Font = new Font("Segoe UI", 10F);
            numericUpDownInterval.Location = new Point(790, 370);
            numericUpDownInterval.Margin = new Padding(6);
            numericUpDownInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numericUpDownInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownInterval.Name = "numericUpDownInterval";
            numericUpDownInterval.Size = new Size(626, 43);
            numericUpDownInterval.TabIndex = 13;
            numericUpDownInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });
            numericUpDownInterval.ValueChanged += NumericUpDownInterval_ValueChanged;
            // 
            // labelFileFormat
            // 
            labelFileFormat.AutoSize = true;
            labelFileFormat.Font = new Font("Segoe UI", 10F);
            labelFileFormat.Location = new Point(60, 444);
            labelFileFormat.Margin = new Padding(6, 0, 6, 0);
            labelFileFormat.Name = "labelFileFormat";
            labelFileFormat.Size = new Size(230, 37);
            labelFileFormat.TabIndex = 14;
            labelFileFormat.Text = "Image &File Format";
            // 
            // comboBoxFormat
            // 
            comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFormat.Font = new Font("Segoe UI", 10F);
            comboBoxFormat.FormattingEnabled = true;
            comboBoxFormat.Items.AddRange(new object[] { "JPG", "PNG", "BMP", "WEBP" });
            comboBoxFormat.Location = new Point(60, 494);
            comboBoxFormat.Margin = new Padding(6);
            comboBoxFormat.Name = "comboBoxFormat";
            comboBoxFormat.Size = new Size(626, 45);
            comboBoxFormat.TabIndex = 15;
            comboBoxFormat.SelectedIndexChanged += ComboBoxFormat_SelectedIndexChanged;
            // 
            // labelQuality
            // 
            labelQuality.AutoSize = true;
            labelQuality.Font = new Font("Segoe UI", 10F);
            labelQuality.Location = new Point(790, 444);
            labelQuality.Margin = new Padding(6, 0, 6, 0);
            labelQuality.Name = "labelQuality";
            labelQuality.Size = new Size(183, 37);
            labelQuality.TabIndex = 16;
            labelQuality.Text = "Image &Quality";
            // 
            // trackBarQuality
            // 
            trackBarQuality.Location = new Point(790, 494);
            trackBarQuality.Margin = new Padding(6);
            trackBarQuality.Maximum = 100;
            trackBarQuality.Minimum = 1;
            trackBarQuality.Name = "trackBarQuality";
            trackBarQuality.Size = new Size(538, 90);
            trackBarQuality.TabIndex = 17;
            trackBarQuality.TickFrequency = 10;
            trackBarQuality.Value = 70;
            trackBarQuality.Scroll += TrackBarQuality_Scroll;
            trackBarQuality.ValueChanged += TrackBarQuality_ValueChanged;
            // 
            // labelQualityValue
            // 
            labelQualityValue.AutoSize = true;
            labelQualityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelQualityValue.Location = new Point(1344, 494);
            labelQualityValue.Margin = new Padding(6, 0, 6, 0);
            labelQualityValue.Name = "labelQualityValue";
            labelQualityValue.Size = new Size(72, 37);
            labelQualityValue.TabIndex = 18;
            labelQualityValue.Text = "70%";
            labelQualityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.Font = new Font("Segoe UI", 10F);
            buttonOpenFolder.Location = new Point(661, 615);
            buttonOpenFolder.Margin = new Padding(6);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(230, 46);
            buttonOpenFolder.TabIndex = 22;
            buttonOpenFolder.Text = "&Open Folder";
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
            buttonSave.Location = new Point(1192, 730);
            buttonSave.Margin = new Padding(6);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(230, 70);
            buttonSave.TabIndex = 0;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += ButtonSave_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Location = new Point(60, 283);
            panel1.Name = "panel1";
            panel1.Size = new Size(1356, 1);
            panel1.TabIndex = 9;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Location = new Point(62, 703);
            panel2.Name = "panel2";
            panel2.Size = new Size(1356, 1);
            panel2.TabIndex = 23;
            // 
            // labelShowNotifications
            // 
            labelShowNotifications.AutoSize = true;
            labelShowNotifications.Font = new Font("Segoe UI", 10F);
            labelShowNotifications.Location = new Point(613, 215);
            labelShowNotifications.Name = "labelShowNotifications";
            labelShowNotifications.Size = new Size(240, 37);
            labelShowNotifications.TabIndex = 5;
            labelShowNotifications.Text = "Show &Notifications";
            // 
            // labelStartWithWindows
            // 
            labelStartWithWindows.AutoSize = true;
            labelStartWithWindows.Location = new Point(1080, 217);
            labelStartWithWindows.Name = "labelStartWithWindows";
            labelStartWithWindows.Size = new Size(223, 32);
            labelStartWithWindows.TabIndex = 7;
            labelStartWithWindows.Text = "Start With &Windows";
            // 
            // labelStatus
            // 
            labelStatus.AutoSize = true;
            labelStatus.Font = new Font("Segoe UI", 10F);
            labelStatus.Location = new Point(45, 747);
            labelStatus.Name = "labelStatus";
            labelStatus.Size = new Size(95, 37);
            labelStatus.TabIndex = 24;
            labelStatus.Text = "Ready.";
            // 
            // buttonWatermark
            // 
            buttonWatermark.Font = new Font("Segoe UI", 10F);
            buttonWatermark.Location = new Point(1192, 615);
            buttonWatermark.Name = "buttonWatermark";
            buttonWatermark.Size = new Size(230, 46);
            buttonWatermark.TabIndex = 26;
            buttonWatermark.Text = "&Watermark";
            buttonWatermark.UseVisualStyleBackColor = true;
            buttonWatermark.Click += ButtonWatermark_Click;
            // 
            // buttonAnnotation
            // 
            buttonAnnotation.Font = new Font("Segoe UI", 10F);
            buttonAnnotation.Location = new Point(936, 615);
            buttonAnnotation.Name = "buttonAnnotation";
            buttonAnnotation.Size = new Size(230, 46);
            buttonAnnotation.TabIndex = 27;
            buttonAnnotation.Text = "Anno&tation";
            buttonAnnotation.UseVisualStyleBackColor = true;
            buttonAnnotation.Click += ButtonAnnotation_Click;
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Location = new Point(911, 615);
            panel3.Name = "panel3";
            panel3.Size = new Size(1, 50);
            panel3.TabIndex = 28;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(1480, 828);
            Controls.Add(panel3);
            Controls.Add(buttonAnnotation);
            Controls.Add(buttonWatermark);
            Controls.Add(labelStatus);
            Controls.Add(labelStartWithWindows);
            Controls.Add(panel2);
            Controls.Add(panel1);
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
            Controls.Add(toggleSwitchEnabled);
            Controls.Add(labelEnableScreenshots);
            Controls.Add(toggleSwitchNotify);
            Controls.Add(labelShowNotifications);
            Controls.Add(toggleSwitchStartWithWindows);
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
        private Label labelSubtitle;
        private LinkLabel linkLabelAbout;

        private Label labelEnableScreenshots;
        private ToggleSwitch toggleSwitchEnabled;
        private CaptureNowButton CaptureNow;

        private Label labelShowNotifications;
        private ToggleSwitch toggleSwitchNotify;

        private ToggleSwitch toggleSwitchStartWithWindows;

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
        private Panel panel1;
        private Panel panel2;
        private Label labelStartWithWindows;
        private Label labelStatus;
        private Button buttonWatermark;
        private Button buttonAnnotation;
        private Panel panel3;
    }
}
