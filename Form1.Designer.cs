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
            exitToolStripMenuItem = new ToolStripMenuItem();

            panelHeader = new Panel();
            pictureBoxLogo = new PictureBox();
            labelTitle = new Label();
            labelSubtitle = new Label();

            labelEnableScreenshots = new Label();
            checkBoxEnabled = new CheckBox();
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
            contextMenuStrip.Items.AddRange(new ToolStripItem[] {
                showToolStripMenuItem,
                toolStripSeparator1,
                captureNowToolStripMenuItem,
                toolStripSeparator2,
                exitToolStripMenuItem
            });
            contextMenuStrip.Name = "contextMenuStrip";
            contextMenuStrip.Size = new Size(150, 82);

            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new Size(149, 22);
            showToolStripMenuItem.Text = "Show";
            showToolStripMenuItem.Click += ShowToolStripMenuItem_Click;

            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(146, 6);

            // 
            // captureNowToolStripMenuItem
            // 
            captureNowToolStripMenuItem.Name = "captureNowToolStripMenuItem";
            captureNowToolStripMenuItem.Size = new Size(149, 22);
            captureNowToolStripMenuItem.Text = "Capture Now";
            captureNowToolStripMenuItem.Click += CaptureNowToolStripMenuItem_Click;

            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(146, 6);

            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(149, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;

            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelTitle);
            panelHeader.Controls.Add(labelSubtitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(740, 80);
            panelHeader.TabIndex = 0;

            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new Point(25, 15);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(50, 50);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 2;
            pictureBoxLogo.TabStop = false;

            // 
            // labelTitle
            // 
            labelTitle.AutoSize = true;
            labelTitle.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            labelTitle.ForeColor = Color.FromArgb(64, 64, 64);
            labelTitle.Location = new Point(85, 15);
            labelTitle.Name = "labelTitle";
            labelTitle.Size = new Size(168, 45);
            labelTitle.TabIndex = 0;
            labelTitle.Text = "Capillume";

            // 
            // labelSubtitle
            // 
            labelSubtitle.AutoSize = true;
            labelSubtitle.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelSubtitle.ForeColor = Color.Gray;
            labelSubtitle.Location = new Point(90, 56);
            labelSubtitle.Name = "labelSubtitle";
            labelSubtitle.Size = new Size(97, 15);
            labelSubtitle.TabIndex = 1;
            labelSubtitle.Text = "Settings • v1.2.0";

            // 
            // labelEnableScreenshots
            // 
            labelEnableScreenshots.AutoSize = true;
            labelEnableScreenshots.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelEnableScreenshots.Location = new Point(30, 105);
            labelEnableScreenshots.Name = "labelEnableScreenshots";
            labelEnableScreenshots.Size = new Size(137, 19);
            labelEnableScreenshots.TabIndex = 1;
            labelEnableScreenshots.Text = "Enable Screenshots";

            // 
            // checkBoxEnabled
            // 
            checkBoxEnabled.Appearance = Appearance.Button;
            checkBoxEnabled.BackColor = Color.FromArgb(0, 120, 212);
            checkBoxEnabled.FlatAppearance.BorderSize = 0;
            checkBoxEnabled.FlatAppearance.CheckedBackColor = Color.FromArgb(0, 120, 212);
            checkBoxEnabled.FlatStyle = FlatStyle.Flat;
            checkBoxEnabled.ForeColor = Color.White;
            checkBoxEnabled.Location = new Point(650, 100);
            checkBoxEnabled.Name = "checkBoxEnabled";
            checkBoxEnabled.Size = new Size(60, 28);
            checkBoxEnabled.TabIndex = 2;
            checkBoxEnabled.TextAlign = ContentAlignment.MiddleCenter;
            checkBoxEnabled.UseVisualStyleBackColor = false;
            checkBoxEnabled.CheckedChanged += CheckBoxEnabled_CheckedChanged;

            // 
            // labelEnabledStatus
            // 
            labelEnabledStatus.AutoSize = true;
            labelEnabledStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelEnabledStatus.ForeColor = Color.FromArgb(0, 120, 212);
            labelEnabledStatus.Location = new Point(610, 107);
            labelEnabledStatus.Name = "labelEnabledStatus";
            labelEnabledStatus.Size = new Size(28, 15);
            labelEnabledStatus.TabIndex = 3;
            labelEnabledStatus.Text = "OFF";
            labelEnabledStatus.TextAlign = ContentAlignment.MiddleRight;

            // 
            // labelCaptureMode
            // 
            labelCaptureMode.AutoSize = true;
            labelCaptureMode.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelCaptureMode.Location = new Point(30, 160);
            labelCaptureMode.Name = "labelCaptureMode";
            labelCaptureMode.Size = new Size(98, 19);
            labelCaptureMode.TabIndex = 4;
            labelCaptureMode.Text = "Capture Mode";

            // 
            // comboBoxCaptureMode
            // 
            comboBoxCaptureMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxCaptureMode.Font = new Font("Segoe UI", 10F);
            comboBoxCaptureMode.FormattingEnabled = true;
            comboBoxCaptureMode.Items.AddRange(new object[] { "Full Screen", "Active Window" });
            comboBoxCaptureMode.Location = new Point(30, 185);
            comboBoxCaptureMode.Name = "comboBoxCaptureMode";
            comboBoxCaptureMode.Size = new Size(680, 25);
            comboBoxCaptureMode.TabIndex = 5;

            // 
            // labelSaveFolder
            // 
            labelSaveFolder.AutoSize = true;
            labelSaveFolder.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelSaveFolder.Location = new Point(30, 230);
            labelSaveFolder.Name = "labelSaveFolder";
            labelSaveFolder.Size = new Size(80, 19);
            labelSaveFolder.TabIndex = 6;
            labelSaveFolder.Text = "Save Folder";

            // 
            // textBoxFolder
            // 
            textBoxFolder.BackColor = Color.White;
            textBoxFolder.Font = new Font("Segoe UI", 10F);
            textBoxFolder.Location = new Point(30, 255);
            textBoxFolder.Name = "textBoxFolder";
            textBoxFolder.ReadOnly = true;
            textBoxFolder.Size = new Size(555, 25);
            textBoxFolder.TabIndex = 7;

            // 
            // buttonBrowse
            // 
            buttonBrowse.Font = new Font("Segoe UI", 10F);
            buttonBrowse.Location = new Point(595, 254);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(115, 28);
            buttonBrowse.TabIndex = 8;
            buttonBrowse.Text = "Browse...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;

            // 
            // labelInterval
            // 
            labelInterval.AutoSize = true;
            labelInterval.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelInterval.Location = new Point(30, 305);
            labelInterval.Name = "labelInterval";
            labelInterval.Size = new Size(119, 19);
            labelInterval.TabIndex = 9;
            labelInterval.Text = "Interval (minutes)";

            // 
            // numericUpDownInterval
            // 
            numericUpDownInterval.Font = new Font("Segoe UI", 10F);
            numericUpDownInterval.Location = new Point(30, 330);
            numericUpDownInterval.Maximum = new decimal(new int[] { 1440, 0, 0, 0 });
            numericUpDownInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numericUpDownInterval.Name = "numericUpDownInterval";
            numericUpDownInterval.Size = new Size(680, 25);
            numericUpDownInterval.TabIndex = 10;
            numericUpDownInterval.Value = new decimal(new int[] { 5, 0, 0, 0 });

            // 
            // labelFileFormat
            // 
            labelFileFormat.AutoSize = true;
            labelFileFormat.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelFileFormat.Location = new Point(30, 375);
            labelFileFormat.Name = "labelFileFormat";
            labelFileFormat.Size = new Size(79, 19);
            labelFileFormat.TabIndex = 11;
            labelFileFormat.Text = "File Format";

            // 
            // comboBoxFormat
            // 
            comboBoxFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxFormat.Font = new Font("Segoe UI", 10F);
            comboBoxFormat.FormattingEnabled = true;
            comboBoxFormat.Items.AddRange(new object[] { "PNG", "JPG", "BMP", "WEBP" });
            comboBoxFormat.Location = new Point(30, 400);
            comboBoxFormat.Name = "comboBoxFormat";
            comboBoxFormat.Size = new Size(680, 25);
            comboBoxFormat.TabIndex = 12;
            comboBoxFormat.SelectedIndexChanged += ComboBoxFormat_SelectedIndexChanged;

            // 
            // labelQuality
            // 
            labelQuality.AutoSize = true;
            labelQuality.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelQuality.Location = new Point(30, 445);
            labelQuality.Name = "labelQuality";
            labelQuality.Size = new Size(56, 19);
            labelQuality.TabIndex = 13;
            labelQuality.Text = "Quality";

            // 
            // trackBarQuality
            // 
            trackBarQuality.Location = new Point(30, 470);
            trackBarQuality.Maximum = 100;
            trackBarQuality.Minimum = 1;
            trackBarQuality.Name = "trackBarQuality";
            trackBarQuality.Size = new Size(640, 45);
            trackBarQuality.TabIndex = 14;
            trackBarQuality.TickFrequency = 10;
            trackBarQuality.Value = 70;
            trackBarQuality.Scroll += TrackBarQuality_Scroll;

            // 
            // labelQualityValue
            // 
            labelQualityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelQualityValue.Location = new Point(670, 477);
            labelQualityValue.Name = "labelQualityValue";
            labelQualityValue.Size = new Size(40, 19);
            labelQualityValue.TabIndex = 15;
            labelQualityValue.Text = "70%";
            labelQualityValue.TextAlign = ContentAlignment.MiddleLeft;

            // 
            // buttonOpenFolder
            // 
            buttonOpenFolder.BackColor = Color.White;
            buttonOpenFolder.FlatStyle = FlatStyle.Flat;
            buttonOpenFolder.Font = new Font("Segoe UI", 10F);
            buttonOpenFolder.Location = new Point(467, 540);
            buttonOpenFolder.Name = "buttonOpenFolder";
            buttonOpenFolder.Size = new Size(115, 35);
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
            buttonSave.Location = new Point(595, 540);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(115, 35);
            buttonSave.TabIndex = 17;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += ButtonSave_Click;

            // 
            // labelStatus
            // 
            labelStatus.BackColor = Color.FromArgb(240, 240, 240);
            labelStatus.Dock = DockStyle.Bottom;
            labelStatus.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            labelStatus.ForeColor = Color.Gray;
            labelStatus.Location = new Point(0, 600);
            labelStatus.Name = "labelStatus";
            labelStatus.Padding = new Padding(10, 8, 10, 8);
            labelStatus.Size = new Size(740, 40);
            labelStatus.TabIndex = 18;
            labelStatus.Text = "Capillume is running in the background. Right-click the tray icon to capture or quit.";
            labelStatus.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(740, 640);
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
            Controls.Add(checkBoxEnabled);
            Controls.Add(labelEnableScreenshots);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Capillume";
            FormClosing += Form1_FormClosing;
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
        private ToolStripMenuItem exitToolStripMenuItem;

        private Panel panelHeader;
        private PictureBox pictureBoxLogo;
        private Label labelTitle;
        private Label labelSubtitle;

        private Label labelEnableScreenshots;
        private CheckBox checkBoxEnabled;
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
