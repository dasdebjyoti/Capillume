namespace Capillume
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
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
            // pictureBoxLogo
            // 
            pictureBoxLogo.Image = Properties.Resources.icon;
            pictureBoxLogo.Location = new Point(44, 50);
            pictureBoxLogo.Margin = new Padding(6);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new Size(160, 160);
            pictureBoxLogo.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxLogo.TabIndex = 0;
            pictureBoxLogo.TabStop = false;
            // 
            // labelAppName
            // 
            labelAppName.AutoSize = true;
            labelAppName.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            labelAppName.ForeColor = Color.Black;
            labelAppName.Location = new Point(226, 70);
            labelAppName.Margin = new Padding(6, 0, 6, 0);
            labelAppName.Name = "labelAppName";
            labelAppName.Size = new Size(338, 86);
            labelAppName.TabIndex = 1;
            labelAppName.Text = "Capillume";
            // 
            // labelVersion
            // 
            labelVersion.AutoSize = true;
            labelVersion.Font = new Font("Segoe UI", 11F);
            labelVersion.ForeColor = Color.Black;
            labelVersion.Location = new Point(236, 150);
            labelVersion.Margin = new Padding(6, 0, 6, 0);
            labelVersion.Name = "labelVersion";
            labelVersion.Size = new Size(209, 41);
            labelVersion.TabIndex = 2;
            labelVersion.Text = "Version 1.1.0.0";
            // 
            // labelDescription
            // 
            labelDescription.Font = new Font("Segoe UI", 10F);
            labelDescription.ForeColor = Color.FromArgb(64, 64, 64);
            labelDescription.Location = new Point(60, 280);
            labelDescription.Margin = new Padding(6, 0, 6, 0);
            labelDescription.Name = "labelDescription";
            labelDescription.Size = new Size(880, 240);
            labelDescription.TabIndex = 1;
            labelDescription.Text = resources.GetString("labelDescription.Text");
            // 
            // labelCopyright
            // 
            labelCopyright.AutoSize = true;
            labelCopyright.Font = new Font("Segoe UI", 9F);
            labelCopyright.ForeColor = Color.Gray;
            labelCopyright.Location = new Point(60, 560);
            labelCopyright.Margin = new Padding(6, 0, 6, 0);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.Size = new Size(358, 32);
            labelCopyright.TabIndex = 2;
            labelCopyright.Text = "© 2026 Capillume - MIT License";
            // 
            // linkLabelGitHub
            // 
            linkLabelGitHub.AutoSize = true;
            linkLabelGitHub.Font = new Font("Segoe UI", 10F);
            linkLabelGitHub.Location = new Point(60, 620);
            linkLabelGitHub.Margin = new Padding(6, 0, 6, 0);
            linkLabelGitHub.Name = "linkLabelGitHub";
            linkLabelGitHub.Size = new Size(531, 37);
            linkLabelGitHub.TabIndex = 3;
            linkLabelGitHub.TabStop = true;
            linkLabelGitHub.Text = "GitHub: github.com/dasdebjyoti/Capillume";
            linkLabelGitHub.LinkClicked += LinkLabelGitHub_LinkClicked;
            // 
            // buttonClose
            // 
            buttonClose.BackColor = Color.FromArgb(0, 120, 212);
            buttonClose.FlatAppearance.BorderSize = 0;
            buttonClose.FlatStyle = FlatStyle.Flat;
            buttonClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonClose.ForeColor = Color.White;
            buttonClose.Location = new Point(730, 700);
            buttonClose.Margin = new Padding(6);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(210, 70);
            buttonClose.TabIndex = 4;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += ButtonClose_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.LightGray;
            panelHeader.Controls.Add(pictureBoxLogo);
            panelHeader.Controls.Add(labelAppName);
            panelHeader.Controls.Add(labelVersion);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(6);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1000, 240);
            panelHeader.TabIndex = 0;
            // 
            // AboutForm
            // 
            AcceptButton = buttonClose;
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = buttonClose;
            ClientSize = new Size(1000, 800);
            Controls.Add(buttonClose);
            Controls.Add(linkLabelGitHub);
            Controls.Add(labelCopyright);
            Controls.Add(labelDescription);
            Controls.Add(panelHeader);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(6);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            ShowInTaskbar = false;
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
