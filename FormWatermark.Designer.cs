namespace Capillume
{
    partial class FormWatermark
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBoxUseText = new GroupBox();
            toggleUseText = new ToggleSwitch();
            labelWatermarkText = new Label();
            labelFontDescription = new Label();
            buttonChooseFont = new Button();
            textBoxWatermarkText = new TextBox();
            groupBoxUseImage = new GroupBox();
            pictureBoxWatermarkImage = new PictureBox();
            labelWatermarkImageScaleValue = new Label();
            labelWatermarkImageScale = new Label();
            trackBarWatermarkImageScale = new TrackBar();
            labelWatermarkImage = new Label();
            buttonBrowseWatermarkImage = new Button();
            textBoxWatermarkImagePath = new TextBox();
            toggleUseImage = new ToggleSwitch();
            labelOpacityValue = new Label();
            labelOpacity = new Label();
            trackBarOpacity = new TrackBar();
            labelWatermarkPosition = new Label();
            comboBoxWatermarkPosition = new ComboBox();
            labelWatermarkRotation = new Label();
            comboBoxWatermarkRotation = new ComboBox();
            buttonOk = new Button();
            buttonCancel = new Button();
            groupBoxCommon = new GroupBox();
            groupBoxUseText.SuspendLayout();
            groupBoxUseImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWatermarkImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarWatermarkImageScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBarOpacity).BeginInit();
            groupBoxCommon.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxUseText
            // 
            groupBoxUseText.Controls.Add(toggleUseText);
            groupBoxUseText.Controls.Add(labelWatermarkText);
            groupBoxUseText.Controls.Add(labelFontDescription);
            groupBoxUseText.Controls.Add(buttonChooseFont);
            groupBoxUseText.Controls.Add(textBoxWatermarkText);
            groupBoxUseText.Font = new Font("Segoe UI", 10F);
            groupBoxUseText.Location = new Point(27, 31);
            groupBoxUseText.Name = "groupBoxUseText";
            groupBoxUseText.Size = new Size(680, 309);
            groupBoxUseText.TabIndex = 0;
            groupBoxUseText.TabStop = false;
            groupBoxUseText.Text = "             Use Text";
            // 
            // toggleUseText
            // 
            toggleUseText.Checked = false;
            toggleUseText.Location = new Point(17, 5);
            toggleUseText.Margin = new Padding(6);
            toggleUseText.Name = "toggleUseText";
            toggleUseText.OffColor = Color.FromArgb(200, 200, 200);
            toggleUseText.OnColor = Color.FromArgb(0, 120, 212);
            toggleUseText.Size = new Size(75, 30);
            toggleUseText.TabIndex = 0;
            toggleUseText.ThumbColor = Color.White;
            toggleUseText.CheckedChanged += ToggleUseText_CheckedChanged;
            // 
            // labelWatermarkText
            // 
            labelWatermarkText.AutoSize = true;
            labelWatermarkText.Location = new Point(36, 83);
            labelWatermarkText.Name = "labelWatermarkText";
            labelWatermarkText.Size = new Size(63, 37);
            labelWatermarkText.TabIndex = 1;
            labelWatermarkText.Text = "Te&xt";
            // 
            // labelFontDescription
            // 
            labelFontDescription.AutoSize = true;
            labelFontDescription.Font = new Font("Segoe UI", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelFontDescription.Location = new Point(204, 161);
            labelFontDescription.Name = "labelFontDescription";
            labelFontDescription.Size = new Size(90, 37);
            labelFontDescription.TabIndex = 4;
            labelFontDescription.Text = "label1";
            // 
            // buttonChooseFont
            // 
            buttonChooseFont.Location = new Point(36, 156);
            buttonChooseFont.Name = "buttonChooseFont";
            buttonChooseFont.Size = new Size(150, 46);
            buttonChooseFont.TabIndex = 3;
            buttonChooseFont.Text = "&Font";
            buttonChooseFont.UseVisualStyleBackColor = true;
            buttonChooseFont.Click += ButtonChooseFont_Click;
            // 
            // textBoxWatermarkText
            // 
            textBoxWatermarkText.Location = new Point(149, 83);
            textBoxWatermarkText.Name = "textBoxWatermarkText";
            textBoxWatermarkText.Size = new Size(516, 43);
            textBoxWatermarkText.TabIndex = 2;
            // 
            // groupBoxUseImage
            // 
            groupBoxUseImage.Controls.Add(pictureBoxWatermarkImage);
            groupBoxUseImage.Controls.Add(labelWatermarkImageScaleValue);
            groupBoxUseImage.Controls.Add(labelWatermarkImageScale);
            groupBoxUseImage.Controls.Add(trackBarWatermarkImageScale);
            groupBoxUseImage.Controls.Add(labelWatermarkImage);
            groupBoxUseImage.Controls.Add(buttonBrowseWatermarkImage);
            groupBoxUseImage.Controls.Add(textBoxWatermarkImagePath);
            groupBoxUseImage.Controls.Add(toggleUseImage);
            groupBoxUseImage.Font = new Font("Segoe UI", 10F);
            groupBoxUseImage.Location = new Point(727, 31);
            groupBoxUseImage.Name = "groupBoxUseImage";
            groupBoxUseImage.Size = new Size(680, 309);
            groupBoxUseImage.TabIndex = 1;
            groupBoxUseImage.TabStop = false;
            groupBoxUseImage.Text = "             Use Image";
            // 
            // pictureBoxWatermarkImage
            // 
            pictureBoxWatermarkImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxWatermarkImage.Location = new Point(417, 146);
            pictureBoxWatermarkImage.Name = "pictureBoxWatermarkImage";
            pictureBoxWatermarkImage.Size = new Size(246, 146);
            pictureBoxWatermarkImage.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxWatermarkImage.TabIndex = 38;
            pictureBoxWatermarkImage.TabStop = false;
            // 
            // labelWatermarkImageScaleValue
            // 
            labelWatermarkImageScaleValue.AutoSize = true;
            labelWatermarkImageScaleValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelWatermarkImageScaleValue.Location = new Point(339, 156);
            labelWatermarkImageScaleValue.Name = "labelWatermarkImageScaleValue";
            labelWatermarkImageScaleValue.Size = new Size(72, 37);
            labelWatermarkImageScaleValue.TabIndex = 6;
            labelWatermarkImageScaleValue.Text = "50%";
            // 
            // labelWatermarkImageScale
            // 
            labelWatermarkImageScale.AutoSize = true;
            labelWatermarkImageScale.Location = new Point(37, 156);
            labelWatermarkImageScale.Name = "labelWatermarkImageScale";
            labelWatermarkImageScale.Size = new Size(78, 37);
            labelWatermarkImageScale.TabIndex = 4;
            labelWatermarkImageScale.Text = "S&cale";
            // 
            // trackBarWatermarkImageScale
            // 
            trackBarWatermarkImageScale.Location = new Point(152, 156);
            trackBarWatermarkImageScale.Maximum = 100;
            trackBarWatermarkImageScale.Minimum = 1;
            trackBarWatermarkImageScale.Name = "trackBarWatermarkImageScale";
            trackBarWatermarkImageScale.Size = new Size(180, 90);
            trackBarWatermarkImageScale.TabIndex = 5;
            trackBarWatermarkImageScale.TickFrequency = 10;
            trackBarWatermarkImageScale.Value = 50;
            trackBarWatermarkImageScale.Scroll += TrackBarWatermarkImageScale_Scroll;
            trackBarWatermarkImageScale.ValueChanged += TrackBarWatermarkImageScale_ValueChanged;
            // 
            // labelWatermarkImage
            // 
            labelWatermarkImage.AutoSize = true;
            labelWatermarkImage.Location = new Point(37, 83);
            labelWatermarkImage.Name = "labelWatermarkImage";
            labelWatermarkImage.Size = new Size(91, 37);
            labelWatermarkImage.TabIndex = 1;
            labelWatermarkImage.Text = "Image";
            // 
            // buttonBrowseWatermarkImage
            // 
            buttonBrowseWatermarkImage.Location = new Point(513, 81);
            buttonBrowseWatermarkImage.Name = "buttonBrowseWatermarkImage";
            buttonBrowseWatermarkImage.Size = new Size(150, 46);
            buttonBrowseWatermarkImage.TabIndex = 3;
            buttonBrowseWatermarkImage.Text = "&Browse";
            buttonBrowseWatermarkImage.UseVisualStyleBackColor = true;
            buttonBrowseWatermarkImage.Click += ButtonBrowseWatermarkImage_Click;
            // 
            // textBoxWatermarkImagePath
            // 
            textBoxWatermarkImagePath.Location = new Point(152, 83);
            textBoxWatermarkImagePath.Name = "textBoxWatermarkImagePath";
            textBoxWatermarkImagePath.Size = new Size(355, 43);
            textBoxWatermarkImagePath.TabIndex = 2;
            textBoxWatermarkImagePath.TextChanged += TextBoxWatermarkImagePath_TextChanged;
            // 
            // toggleUseImage
            // 
            toggleUseImage.Checked = false;
            toggleUseImage.Location = new Point(17, 5);
            toggleUseImage.Margin = new Padding(6);
            toggleUseImage.Name = "toggleUseImage";
            toggleUseImage.OffColor = Color.FromArgb(200, 200, 200);
            toggleUseImage.OnColor = Color.FromArgb(0, 120, 212);
            toggleUseImage.Size = new Size(75, 30);
            toggleUseImage.TabIndex = 0;
            toggleUseImage.ThumbColor = Color.White;
            toggleUseImage.CheckedChanged += ToggleUseImage_CheckedChanged;
            // 
            // labelOpacityValue
            // 
            labelOpacityValue.AutoSize = true;
            labelOpacityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelOpacityValue.Location = new Point(363, 61);
            labelOpacityValue.Margin = new Padding(6, 0, 6, 0);
            labelOpacityValue.Name = "labelOpacityValue";
            labelOpacityValue.Size = new Size(72, 37);
            labelOpacityValue.TabIndex = 2;
            labelOpacityValue.Text = "50%";
            labelOpacityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelOpacity
            // 
            labelOpacity.AutoSize = true;
            labelOpacity.Font = new Font("Segoe UI", 10F);
            labelOpacity.Location = new Point(17, 61);
            labelOpacity.Margin = new Padding(6, 0, 6, 0);
            labelOpacity.Name = "labelOpacity";
            labelOpacity.Size = new Size(108, 37);
            labelOpacity.TabIndex = 0;
            labelOpacity.Text = "Opacity";
            // 
            // trackBarOpacity
            // 
            trackBarOpacity.Location = new Point(127, 63);
            trackBarOpacity.Margin = new Padding(6);
            trackBarOpacity.Maximum = 100;
            trackBarOpacity.Minimum = 1;
            trackBarOpacity.Name = "trackBarOpacity";
            trackBarOpacity.Size = new Size(229, 90);
            trackBarOpacity.TabIndex = 1;
            trackBarOpacity.TickFrequency = 10;
            trackBarOpacity.Value = 50;
            trackBarOpacity.Scroll += TrackBarOpacity_Scroll;
            trackBarOpacity.ValueChanged += TrackBarOpacity_ValueChanged;
            // 
            // labelWatermarkPosition
            // 
            labelWatermarkPosition.AutoSize = true;
            labelWatermarkPosition.Font = new Font("Segoe UI", 10F);
            labelWatermarkPosition.Location = new Point(539, 61);
            labelWatermarkPosition.Name = "labelWatermarkPosition";
            labelWatermarkPosition.Size = new Size(112, 37);
            labelWatermarkPosition.TabIndex = 3;
            labelWatermarkPosition.Text = "Position";
            // 
            // comboBoxWatermarkPosition
            // 
            comboBoxWatermarkPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxWatermarkPosition.Font = new Font("Segoe UI", 10F);
            comboBoxWatermarkPosition.FormattingEnabled = true;
            comboBoxWatermarkPosition.Items.AddRange(new object[] { "Top Left", "Top Center", "Top Right", "Center Left", "Center", "Center Right", "Bottom Left", "Bottom Center", "Bottom Right" });
            comboBoxWatermarkPosition.Location = new Point(672, 57);
            comboBoxWatermarkPosition.Name = "comboBoxWatermarkPosition";
            comboBoxWatermarkPosition.Size = new Size(242, 45);
            comboBoxWatermarkPosition.TabIndex = 4;
            comboBoxWatermarkPosition.SelectedIndexChanged += ComboBoxWatermarkPosition_SelectedIndexChanged;
            // 
            // labelWatermarkRotation
            // 
            labelWatermarkRotation.AutoSize = true;
            labelWatermarkRotation.Font = new Font("Segoe UI", 10F);
            labelWatermarkRotation.Location = new Point(991, 61);
            labelWatermarkRotation.Name = "labelWatermarkRotation";
            labelWatermarkRotation.Size = new Size(118, 37);
            labelWatermarkRotation.TabIndex = 5;
            labelWatermarkRotation.Text = "Rotation";
            // 
            // comboBoxWatermarkRotation
            // 
            comboBoxWatermarkRotation.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxWatermarkRotation.Font = new Font("Segoe UI", 10F);
            comboBoxWatermarkRotation.FormattingEnabled = true;
            comboBoxWatermarkRotation.Items.AddRange(new object[] { "0°", "90°", "180°", "270°" });
            comboBoxWatermarkRotation.Location = new Point(1121, 57);
            comboBoxWatermarkRotation.Name = "comboBoxWatermarkRotation";
            comboBoxWatermarkRotation.Size = new Size(242, 45);
            comboBoxWatermarkRotation.TabIndex = 6;
            comboBoxWatermarkRotation.SelectedIndexChanged += ComboBoxWatermarkRotation_SelectedIndexChanged;
            // 
            // buttonOk
            // 
            buttonOk.BackColor = Color.FromArgb(0, 120, 212);
            buttonOk.FlatAppearance.BorderSize = 0;
            buttonOk.FlatStyle = FlatStyle.Flat;
            buttonOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonOk.ForeColor = Color.White;
            buttonOk.Location = new Point(1177, 535);
            buttonOk.Margin = new Padding(6);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(230, 70);
            buttonOk.TabIndex = 4;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = false;
            buttonOk.Click += ButtonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Segoe UI", 10F);
            buttonCancel.Location = new Point(938, 535);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(230, 70);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // groupBoxCommon
            // 
            groupBoxCommon.Controls.Add(trackBarOpacity);
            groupBoxCommon.Controls.Add(labelOpacity);
            groupBoxCommon.Controls.Add(labelOpacityValue);
            groupBoxCommon.Controls.Add(comboBoxWatermarkPosition);
            groupBoxCommon.Controls.Add(labelWatermarkPosition);
            groupBoxCommon.Controls.Add(labelWatermarkRotation);
            groupBoxCommon.Controls.Add(comboBoxWatermarkRotation);
            groupBoxCommon.Location = new Point(27, 358);
            groupBoxCommon.Name = "groupBoxCommon";
            groupBoxCommon.Size = new Size(1380, 159);
            groupBoxCommon.TabIndex = 2;
            groupBoxCommon.TabStop = false;
            groupBoxCommon.Text = "Common Settings";
            // 
            // FormWatermark
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = buttonCancel;
            ClientSize = new Size(1430, 625);
            Controls.Add(groupBoxCommon);
            Controls.Add(groupBoxUseImage);
            Controls.Add(groupBoxUseText);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormWatermark";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Watermark";
            FormClosing += FormWatermark_FormClosing;
            groupBoxUseText.ResumeLayout(false);
            groupBoxUseText.PerformLayout();
            groupBoxUseImage.ResumeLayout(false);
            groupBoxUseImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxWatermarkImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarWatermarkImageScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBarOpacity).EndInit();
            groupBoxCommon.ResumeLayout(false);
            groupBoxCommon.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Label labelOpacityValue;
        private TrackBar trackBarOpacity;
        private Label labelOpacity;
        private ComboBox comboBoxWatermarkPosition;
        private Label labelWatermarkPosition;
        private Label labelWatermarkRotation;
        private ComboBox comboBoxWatermarkRotation;
        private Button buttonOk;
        private Button buttonCancel;
        private GroupBox groupBoxUseText;
        private Label labelWatermarkText;
        private Label labelFontDescription;
        private Button buttonChooseFont;
        private TextBox textBoxWatermarkText;
        private ToggleSwitch toggleUseText;
        private GroupBox groupBoxUseImage;
        private Label labelWatermarkImage;
        private Button buttonBrowseWatermarkImage;
        private TextBox textBoxWatermarkImagePath;
        private ToggleSwitch toggleUseImage;
        private Label labelWatermarkImageScaleValue;
        private Label labelWatermarkImageScale;
        private TrackBar trackBarWatermarkImageScale;
        private PictureBox pictureBoxWatermarkImage;
        private GroupBox groupBoxCommon;
    }
}