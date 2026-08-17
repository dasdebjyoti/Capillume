namespace CapIilume
{
    partial class AboutForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pictureBoxLogo = new PictureBox();
            labelAppName = new Label();
            labelVersion = new Label();
            labelDescription = new Label();
            labelCopyright = new Label();
            linkLabelGitHub = new LinkLabel();
            buttonClose = new Button();
            panelHeader = new Panel();

            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).BeginInit();
            panelHeader.SuspendLayout();
            SuspendLayout();

            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(0, 120, 212);
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelAppName);
            panelHeader.Controls.Add(labelVersion);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(500, 120);
            panelHeader.TabIndex = 0;

            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.Location = new Point(30, 25);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(70, 70);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;

            // 
            // labelAppName
            // 
            labelAppName.AutoSize = true;
            labelAppName.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
            labelAppName.ForeColor = Color.White;
            labelAppName.Location = new Point(120, 35);
            labelAppName.Name = "labelAppName";
            labelAppName.Size = new Size(177, 45);
            labelAppName.TabIndex = 1;
            labelAppName.Text = "Capillume";

            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            labelVersion.ForeColor = Color.White;
            labelVersion.Location = new Point(125, 75);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(89, 20);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "Version 1.2.0";

            // 
            // labelDescription
            // 
            labelDescription.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            labelDescription.ForeColor = Color.FromArgb(64, 64, 64);
            labelDescription.Location = new Point(30, 140);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(440, 120);
            labelDescription.TabIndex = 1;
            labelDescription.Text = "Capillume is a lightweight screenshot automation tool designed to capture " +
                "your screen at regular intervals. Effortlessly preserve your workflow, monitor activities, " +
                "or document your progress with minimal resource usage.\n\n" +
                "Features:\n" +
                "• Automated screenshot capture at customizable intervals\n" +
                "• Full screen or active window capture modes\n" +
                "• Multiple image formats (PNG, JPG, BMP, WEBP)\n" +
                "• System tray integration for seamless background operation";

            // 
            // labelCopyright
            // 
            labelCopyright.AutoSize = true;
            labelCopyright.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            labelCopyright.ForeColor = Color.Gray;
            labelCopyright.Location = new Point(30, 280);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new Size(240, 15);
            labelCopyright.TabIndex = 2;
            labelCopyright.Text = "© 2024 Capillume. All rights reserved.";

            // 
            // linkLabelGitHub
            // 
            linkLabelGitHub.AutoSize = true;
            linkLabelGitHub.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            linkLabelGitHub.Location = new Point(30, 310);
            linkLabelGitHub.Name = "linkLabelGitHub";
            linkLabelGitHub.Size = new Size(270, 19);
            linkLabelGitHub.TabIndex = 3;
            linkLabelGitHub.TabStop = true;
            linkLabelGitHub.Text = "GitHub: github.com/dasdebjyoti/CapIilume";
            linkLabelGitHub.LinkClicked += LinkLabelGitHub_LinkClicked;

            // 
            // buttonClose
            // 
            buttonClose.BackColor = Color.FromArgb(0, 120, 212);
            buttonClose.FlatAppearance.BorderSize = 0;
            buttonClose.FlatStyle = FlatStyle.Flat;
            buttonClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            buttonClose.ForeColor = Color.White;
            buttonClose.Location = new Point(365, 350);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(105, 35);
            buttonClose.TabIndex = 4;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += ButtonClose_Click;

            // 
            // AboutForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(500, 400);
            Controls.Add(buttonClose);
            Controls.Add(linkLabelGitHub);
            Controls.Add(labelCopyright);
            Controls.Add(labelDescription);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "About Capillume";
            ((System.ComponentModel.ISupportInitialize)pictureBoxLogo).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelHeader;
        private PictureBox pictureBoxLogo;
        private Label labelAppName;
        private Label labelVersion;
        private Label labelDescription;
        private Label labelCopyright;
        private LinkLabel linkLabelGitHub;
        private Button buttonClose;
    }
}
