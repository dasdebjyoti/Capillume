namespace Capillume
{
    partial class FormSettings
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
            tabControlSettings = new TabControl();
            tabPageWatermark = new TabPage();
            wmGroupBoxCommon = new GroupBox();
            wmComboBoxWatermarkRotation = new ComboBox();
            wmLabelWatermarkRotation = new Label();
            wmComboBoxWatermarkPosition = new ComboBox();
            wmLabelWatermarkPosition = new Label();
            wmTrackBarOpacity = new TrackBar();
            wmLabelOpacity = new Label();
            wmLabelOpacityValue = new Label();
            wmGroupBoxUseImage = new GroupBox();
            wmPictureBoxWatermarkImage = new PictureBox();
            wmLabelWatermarkImageScaleValue = new Label();
            wmLabelWatermarkImageScale = new Label();
            wmTrackBarWatermarkImageScale = new TrackBar();
            wmButtonBrowseWatermarkImage = new Button();
            wmTextBoxWatermarkImagePath = new TextBox();
            wmLabelWatermarkImage = new Label();
            wmToggleUseImage = new ToggleSwitch();
            wmGroupBoxUseText = new GroupBox();
            wmToggleUseText = new ToggleSwitch();
            wmLabelWatermarkText = new Label();
            wmTextBoxWatermarkText = new TextBox();
            wmButtonChooseFont = new Button();
            wmLabelFontDescription = new Label();
            tabPageAnnotation = new TabPage();
            anGroupBoxAnnotation = new GroupBox();
            anTrackBarOpacity = new TrackBar();
            anLabelOpacity = new Label();
            anLabelOpacityValue = new Label();
            anLabelAnnotationSample = new Label();
            anButtonAnnotationBackgroundColor = new Button();
            anButtonAnnotationFont = new Button();
            anButtonAnnotationFields = new Button();
            anComboBoxAnnotationFormat = new ComboBox();
            anLabelAnnotationFormat = new Label();
            anToggleUseAnnotation = new ToggleSwitch();
            buttonCancel = new Button();
            buttonOk = new Button();
            tabControlSettings.SuspendLayout();
            tabPageWatermark.SuspendLayout();
            wmGroupBoxCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarOpacity).BeginInit();
            wmGroupBoxUseImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)wmPictureBoxWatermarkImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarWatermarkImageScale).BeginInit();
            wmGroupBoxUseText.SuspendLayout();
            tabPageAnnotation.SuspendLayout();
            anGroupBoxAnnotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)anTrackBarOpacity).BeginInit();
            SuspendLayout();
            // 
            // tabControlSettings
            // 
            tabControlSettings.Controls.Add(tabPageWatermark);
            tabControlSettings.Controls.Add(tabPageAnnotation);
            tabControlSettings.Font = new Font("Segoe UI", 10F);
            tabControlSettings.Location = new Point(24, 22);
            tabControlSettings.Name = "tabControlSettings";
            tabControlSettings.SelectedIndex = 0;
            tabControlSettings.Size = new Size(1456, 760);
            tabControlSettings.TabIndex = 0;
            // 
            // tabPageWatermark
            // 
            tabPageWatermark.BackColor = Color.White;
            tabPageWatermark.Controls.Add(wmGroupBoxCommon);
            tabPageWatermark.Controls.Add(wmGroupBoxUseImage);
            tabPageWatermark.Controls.Add(wmGroupBoxUseText);
            tabPageWatermark.Location = new Point(8, 51);
            tabPageWatermark.Name = "tabPageWatermark";
            tabPageWatermark.Padding = new Padding(3);
            tabPageWatermark.Size = new Size(1440, 701);
            tabPageWatermark.TabIndex = 1;
            tabPageWatermark.Text = "Watermark";
            // 
            // wmGroupBoxCommon
            // 
            wmGroupBoxCommon.Controls.Add(wmComboBoxWatermarkRotation);
            wmGroupBoxCommon.Controls.Add(wmLabelWatermarkRotation);
            wmGroupBoxCommon.Controls.Add(wmComboBoxWatermarkPosition);
            wmGroupBoxCommon.Controls.Add(wmLabelWatermarkPosition);
            wmGroupBoxCommon.Controls.Add(wmTrackBarOpacity);
            wmGroupBoxCommon.Controls.Add(wmLabelOpacity);
            wmGroupBoxCommon.Controls.Add(wmLabelOpacityValue);
            wmGroupBoxCommon.Font = new Font("Segoe UI", 10F);
            wmGroupBoxCommon.Location = new Point(16, 351);
            wmGroupBoxCommon.Name = "wmGroupBoxCommon";
            wmGroupBoxCommon.Size = new Size(1408, 142);
            wmGroupBoxCommon.TabIndex = 2;
            wmGroupBoxCommon.TabStop = false;
            wmGroupBoxCommon.Text = "Common Settings";
            // 
            // wmComboBoxWatermarkRotation
            // 
            wmComboBoxWatermarkRotation.DropDownStyle = ComboBoxStyle.DropDownList;
            wmComboBoxWatermarkRotation.Font = new Font("Segoe UI", 10F);
            wmComboBoxWatermarkRotation.FormattingEnabled = true;
            wmComboBoxWatermarkRotation.Items.AddRange(new object[] { "0°", "90°", "180°", "270°" });
            wmComboBoxWatermarkRotation.Location = new Point(1196, 52);
            wmComboBoxWatermarkRotation.Name = "wmComboBoxWatermarkRotation";
            wmComboBoxWatermarkRotation.Size = new Size(190, 45);
            wmComboBoxWatermarkRotation.TabIndex = 6;
            // 
            // wmLabelWatermarkRotation
            // 
            wmLabelWatermarkRotation.AutoSize = true;
            wmLabelWatermarkRotation.Font = new Font("Segoe UI", 10F);
            wmLabelWatermarkRotation.Location = new Point(1054, 56);
            wmLabelWatermarkRotation.Name = "wmLabelWatermarkRotation";
            wmLabelWatermarkRotation.Size = new Size(118, 37);
            wmLabelWatermarkRotation.TabIndex = 5;
            wmLabelWatermarkRotation.Text = "&Rotation";
            // 
            // wmComboBoxWatermarkPosition
            // 
            wmComboBoxWatermarkPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            wmComboBoxWatermarkPosition.Font = new Font("Segoe UI", 10F);
            wmComboBoxWatermarkPosition.FormattingEnabled = true;
            wmComboBoxWatermarkPosition.Items.AddRange(new object[] { "Top Left", "Top Center", "Top Right", "Center Left", "Center", "Center Right", "Bottom Left", "Bottom Center", "Bottom Right" });
            wmComboBoxWatermarkPosition.Location = new Point(747, 52);
            wmComboBoxWatermarkPosition.Name = "wmComboBoxWatermarkPosition";
            wmComboBoxWatermarkPosition.Size = new Size(242, 45);
            wmComboBoxWatermarkPosition.TabIndex = 4;
            // 
            // wmLabelWatermarkPosition
            // 
            wmLabelWatermarkPosition.AutoSize = true;
            wmLabelWatermarkPosition.Font = new Font("Segoe UI", 10F);
            wmLabelWatermarkPosition.Location = new Point(616, 56);
            wmLabelWatermarkPosition.Name = "wmLabelWatermarkPosition";
            wmLabelWatermarkPosition.Size = new Size(112, 37);
            wmLabelWatermarkPosition.TabIndex = 3;
            wmLabelWatermarkPosition.Text = "&Position";
            // 
            // wmTrackBarOpacity
            // 
            wmTrackBarOpacity.AutoSize = false;
            wmTrackBarOpacity.BackColor = Color.White;
            wmTrackBarOpacity.Location = new Point(149, 49);
            wmTrackBarOpacity.Margin = new Padding(6);
            wmTrackBarOpacity.Maximum = 100;
            wmTrackBarOpacity.Minimum = 1;
            wmTrackBarOpacity.Name = "wmTrackBarOpacity";
            wmTrackBarOpacity.Size = new Size(339, 50);
            wmTrackBarOpacity.TabIndex = 1;
            wmTrackBarOpacity.TickFrequency = 10;
            wmTrackBarOpacity.TickStyle = TickStyle.None;
            wmTrackBarOpacity.Value = 50;
            wmTrackBarOpacity.ValueChanged += WmTrackBarOpacity_ValueChanged;
            // 
            // wmLabelOpacity
            // 
            wmLabelOpacity.AutoSize = true;
            wmLabelOpacity.Font = new Font("Segoe UI", 10F);
            wmLabelOpacity.Location = new Point(36, 56);
            wmLabelOpacity.Margin = new Padding(6, 0, 6, 0);
            wmLabelOpacity.Name = "wmLabelOpacity";
            wmLabelOpacity.Size = new Size(108, 37);
            wmLabelOpacity.TabIndex = 0;
            wmLabelOpacity.Text = "&Opacity";
            // 
            // wmLabelOpacityValue
            // 
            wmLabelOpacityValue.AutoSize = true;
            wmLabelOpacityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            wmLabelOpacityValue.Location = new Point(500, 56);
            wmLabelOpacityValue.Margin = new Padding(6, 0, 6, 0);
            wmLabelOpacityValue.Name = "wmLabelOpacityValue";
            wmLabelOpacityValue.Size = new Size(72, 37);
            wmLabelOpacityValue.TabIndex = 2;
            wmLabelOpacityValue.Text = "50%";
            wmLabelOpacityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // wmGroupBoxUseImage
            // 
            wmGroupBoxUseImage.Controls.Add(wmPictureBoxWatermarkImage);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImageScaleValue);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImageScale);
            wmGroupBoxUseImage.Controls.Add(wmTrackBarWatermarkImageScale);
            wmGroupBoxUseImage.Controls.Add(wmButtonBrowseWatermarkImage);
            wmGroupBoxUseImage.Controls.Add(wmTextBoxWatermarkImagePath);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImage);
            wmGroupBoxUseImage.Controls.Add(wmToggleUseImage);
            wmGroupBoxUseImage.Font = new Font("Segoe UI", 10F);
            wmGroupBoxUseImage.Location = new Point(727, 24);
            wmGroupBoxUseImage.Name = "wmGroupBoxUseImage";
            wmGroupBoxUseImage.Size = new Size(697, 309);
            wmGroupBoxUseImage.TabIndex = 1;
            wmGroupBoxUseImage.TabStop = false;
            wmGroupBoxUseImage.Text = "             Use Image";
            // 
            // wmPictureBoxWatermarkImage
            // 
            wmPictureBoxWatermarkImage.BorderStyle = BorderStyle.FixedSingle;
            wmPictureBoxWatermarkImage.Location = new Point(432, 146);
            wmPictureBoxWatermarkImage.Name = "wmPictureBoxWatermarkImage";
            wmPictureBoxWatermarkImage.Size = new Size(246, 146);
            wmPictureBoxWatermarkImage.SizeMode = PictureBoxSizeMode.Zoom;
            wmPictureBoxWatermarkImage.TabIndex = 7;
            wmPictureBoxWatermarkImage.TabStop = false;
            // 
            // wmLabelWatermarkImageScaleValue
            // 
            wmLabelWatermarkImageScaleValue.AutoSize = true;
            wmLabelWatermarkImageScaleValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            wmLabelWatermarkImageScaleValue.Location = new Point(351, 156);
            wmLabelWatermarkImageScaleValue.Margin = new Padding(6, 0, 6, 0);
            wmLabelWatermarkImageScaleValue.Name = "wmLabelWatermarkImageScaleValue";
            wmLabelWatermarkImageScaleValue.Size = new Size(72, 37);
            wmLabelWatermarkImageScaleValue.TabIndex = 6;
            wmLabelWatermarkImageScaleValue.Text = "50%";
            wmLabelWatermarkImageScaleValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // wmLabelWatermarkImageScale
            // 
            wmLabelWatermarkImageScale.AutoSize = true;
            wmLabelWatermarkImageScale.Location = new Point(49, 156);
            wmLabelWatermarkImageScale.Name = "wmLabelWatermarkImageScale";
            wmLabelWatermarkImageScale.Size = new Size(78, 37);
            wmLabelWatermarkImageScale.TabIndex = 4;
            wmLabelWatermarkImageScale.Text = "S&cale";
            // 
            // wmTrackBarWatermarkImageScale
            // 
            wmTrackBarWatermarkImageScale.AutoSize = false;
            wmTrackBarWatermarkImageScale.BackColor = Color.White;
            wmTrackBarWatermarkImageScale.Location = new Point(149, 156);
            wmTrackBarWatermarkImageScale.Margin = new Padding(6);
            wmTrackBarWatermarkImageScale.Maximum = 100;
            wmTrackBarWatermarkImageScale.Minimum = 1;
            wmTrackBarWatermarkImageScale.Name = "wmTrackBarWatermarkImageScale";
            wmTrackBarWatermarkImageScale.Size = new Size(170, 50);
            wmTrackBarWatermarkImageScale.TabIndex = 5;
            wmTrackBarWatermarkImageScale.TickFrequency = 10;
            wmTrackBarWatermarkImageScale.TickStyle = TickStyle.None;
            wmTrackBarWatermarkImageScale.Value = 50;
            wmTrackBarWatermarkImageScale.ValueChanged += WmTrackBarWatermarkImageScale_ValueChanged;
            // 
            // wmButtonBrowseWatermarkImage
            // 
            wmButtonBrowseWatermarkImage.Location = new Point(527, 79);
            wmButtonBrowseWatermarkImage.Name = "wmButtonBrowseWatermarkImage";
            wmButtonBrowseWatermarkImage.Size = new Size(150, 46);
            wmButtonBrowseWatermarkImage.TabIndex = 3;
            wmButtonBrowseWatermarkImage.Text = "&Browse";
            wmButtonBrowseWatermarkImage.UseVisualStyleBackColor = true;
            wmButtonBrowseWatermarkImage.Click += WmButtonBrowseWatermarkImage_Click;
            // 
            // wmTextBoxWatermarkImagePath
            // 
            wmTextBoxWatermarkImagePath.Location = new Point(149, 81);
            wmTextBoxWatermarkImagePath.Name = "wmTextBoxWatermarkImagePath";
            wmTextBoxWatermarkImagePath.Size = new Size(361, 43);
            wmTextBoxWatermarkImagePath.TabIndex = 2;
            wmTextBoxWatermarkImagePath.TextChanged += WmTextBoxWatermarkImagePath_TextChanged;
            // 
            // wmLabelWatermarkImage
            // 
            wmLabelWatermarkImage.AutoSize = true;
            wmLabelWatermarkImage.Location = new Point(36, 84);
            wmLabelWatermarkImage.Name = "wmLabelWatermarkImage";
            wmLabelWatermarkImage.Size = new Size(91, 37);
            wmLabelWatermarkImage.TabIndex = 1;
            wmLabelWatermarkImage.Text = "&Image";
            // 
            // wmToggleUseImage
            // 
            wmToggleUseImage.Checked = false;
            wmToggleUseImage.Location = new Point(17, 5);
            wmToggleUseImage.Margin = new Padding(6);
            wmToggleUseImage.Name = "wmToggleUseImage";
            wmToggleUseImage.OffColor = Color.FromArgb(200, 200, 200);
            wmToggleUseImage.OnColor = Color.FromArgb(0, 120, 212);
            wmToggleUseImage.Size = new Size(75, 30);
            wmToggleUseImage.TabIndex = 0;
            wmToggleUseImage.ThumbColor = Color.White;
            wmToggleUseImage.CheckedChanged += WmToggleUseImage_CheckedChanged;
            // 
            // wmGroupBoxUseText
            // 
            wmGroupBoxUseText.Controls.Add(wmToggleUseText);
            wmGroupBoxUseText.Controls.Add(wmLabelWatermarkText);
            wmGroupBoxUseText.Controls.Add(wmTextBoxWatermarkText);
            wmGroupBoxUseText.Controls.Add(wmButtonChooseFont);
            wmGroupBoxUseText.Controls.Add(wmLabelFontDescription);
            wmGroupBoxUseText.Font = new Font("Segoe UI", 10F);
            wmGroupBoxUseText.Location = new Point(16, 24);
            wmGroupBoxUseText.Name = "wmGroupBoxUseText";
            wmGroupBoxUseText.Size = new Size(691, 309);
            wmGroupBoxUseText.TabIndex = 1;
            wmGroupBoxUseText.TabStop = false;
            wmGroupBoxUseText.Text = "             Use Text";
            // 
            // wmToggleUseText
            // 
            wmToggleUseText.Checked = false;
            wmToggleUseText.Location = new Point(17, 5);
            wmToggleUseText.Margin = new Padding(6);
            wmToggleUseText.Name = "wmToggleUseText";
            wmToggleUseText.OffColor = Color.FromArgb(200, 200, 200);
            wmToggleUseText.OnColor = Color.FromArgb(0, 120, 212);
            wmToggleUseText.Size = new Size(75, 30);
            wmToggleUseText.TabIndex = 0;
            wmToggleUseText.ThumbColor = Color.White;
            wmToggleUseText.CheckedChanged += WmToggleUseText_CheckedChanged;
            // 
            // wmLabelWatermarkText
            // 
            wmLabelWatermarkText.AutoSize = true;
            wmLabelWatermarkText.Location = new Point(36, 83);
            wmLabelWatermarkText.Name = "wmLabelWatermarkText";
            wmLabelWatermarkText.Size = new Size(63, 37);
            wmLabelWatermarkText.TabIndex = 0;
            wmLabelWatermarkText.Text = "Te&xt";
            // 
            // wmTextBoxWatermarkText
            // 
            wmTextBoxWatermarkText.Location = new Point(149, 83);
            wmTextBoxWatermarkText.Name = "wmTextBoxWatermarkText";
            wmTextBoxWatermarkText.Size = new Size(516, 43);
            wmTextBoxWatermarkText.TabIndex = 1;
            // 
            // wmButtonChooseFont
            // 
            wmButtonChooseFont.Location = new Point(36, 156);
            wmButtonChooseFont.Name = "wmButtonChooseFont";
            wmButtonChooseFont.Size = new Size(150, 46);
            wmButtonChooseFont.TabIndex = 2;
            wmButtonChooseFont.Text = "&Font";
            wmButtonChooseFont.UseVisualStyleBackColor = true;
            wmButtonChooseFont.Click += WmButtonChooseFont_Click;
            // 
            // wmLabelFontDescription
            // 
            wmLabelFontDescription.AutoSize = true;
            wmLabelFontDescription.Font = new Font("Segoe UI", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            wmLabelFontDescription.Location = new Point(204, 161);
            wmLabelFontDescription.Name = "wmLabelFontDescription";
            wmLabelFontDescription.Size = new Size(90, 37);
            wmLabelFontDescription.TabIndex = 3;
            wmLabelFontDescription.Text = "label1";
            // 
            // tabPageAnnotation
            // 
            tabPageAnnotation.BackColor = Color.White;
            tabPageAnnotation.Controls.Add(anGroupBoxAnnotation);
            tabPageAnnotation.Location = new Point(8, 51);
            tabPageAnnotation.Name = "tabPageAnnotation";
            tabPageAnnotation.Padding = new Padding(3);
            tabPageAnnotation.Size = new Size(1440, 701);
            tabPageAnnotation.TabIndex = 1;
            tabPageAnnotation.Text = "Annotation";
            // 
            // anGroupBoxAnnotation
            // 
            anGroupBoxAnnotation.Controls.Add(anTrackBarOpacity);
            anGroupBoxAnnotation.Controls.Add(anLabelOpacity);
            anGroupBoxAnnotation.Controls.Add(anLabelOpacityValue);
            anGroupBoxAnnotation.Controls.Add(anLabelAnnotationSample);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationBackgroundColor);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationFont);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationFields);
            anGroupBoxAnnotation.Controls.Add(anComboBoxAnnotationFormat);
            anGroupBoxAnnotation.Controls.Add(anLabelAnnotationFormat);
            anGroupBoxAnnotation.Controls.Add(anToggleUseAnnotation);
            anGroupBoxAnnotation.Font = new Font("Segoe UI", 10F);
            anGroupBoxAnnotation.Location = new Point(16, 24);
            anGroupBoxAnnotation.Name = "anGroupBoxAnnotation";
            anGroupBoxAnnotation.Size = new Size(1408, 417);
            anGroupBoxAnnotation.TabIndex = 0;
            anGroupBoxAnnotation.TabStop = false;
            anGroupBoxAnnotation.Text = "             Use Annotation";
            // 
            // anTrackBarOpacity
            // 
            anTrackBarOpacity.AutoSize = false;
            anTrackBarOpacity.Location = new Point(149, 311);
            anTrackBarOpacity.Margin = new Padding(6);
            anTrackBarOpacity.Maximum = 100;
            anTrackBarOpacity.Minimum = 1;
            anTrackBarOpacity.Name = "anTrackBarOpacity";
            anTrackBarOpacity.Size = new Size(1110, 46);
            anTrackBarOpacity.TabIndex = 8;
            anTrackBarOpacity.TickFrequency = 10;
            anTrackBarOpacity.Value = 50;
            anTrackBarOpacity.ValueChanged += AnTrackBarOpacity_ValueChanged;
            // 
            // anLabelOpacity
            // 
            anLabelOpacity.AutoSize = true;
            anLabelOpacity.Font = new Font("Segoe UI", 10F);
            anLabelOpacity.Location = new Point(39, 310);
            anLabelOpacity.Margin = new Padding(6, 0, 6, 0);
            anLabelOpacity.Name = "anLabelOpacity";
            anLabelOpacity.Size = new Size(108, 37);
            anLabelOpacity.TabIndex = 7;
            anLabelOpacity.Text = "&Opacity";
            // 
            // anLabelOpacityValue
            // 
            anLabelOpacityValue.AutoSize = true;
            anLabelOpacityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            anLabelOpacityValue.Location = new Point(1284, 310);
            anLabelOpacityValue.Margin = new Padding(6, 0, 6, 0);
            anLabelOpacityValue.Name = "anLabelOpacityValue";
            anLabelOpacityValue.Size = new Size(72, 37);
            anLabelOpacityValue.TabIndex = 9;
            anLabelOpacityValue.Text = "50%";
            anLabelOpacityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // anLabelAnnotationSample
            // 
            anLabelAnnotationSample.AutoSize = true;
            anLabelAnnotationSample.Font = new Font("Segoe UI", 12F);
            anLabelAnnotationSample.Location = new Point(36, 193);
            anLabelAnnotationSample.Name = "anLabelAnnotationSample";
            anLabelAnnotationSample.Size = new Size(105, 45);
            anLabelAnnotationSample.TabIndex = 6;
            anLabelAnnotationSample.Text = "label1";
            // 
            // anButtonAnnotationBackgroundColor
            // 
            anButtonAnnotationBackgroundColor.Location = new Point(1206, 80);
            anButtonAnnotationBackgroundColor.Name = "anButtonAnnotationBackgroundColor";
            anButtonAnnotationBackgroundColor.Size = new Size(150, 46);
            anButtonAnnotationBackgroundColor.TabIndex = 5;
            anButtonAnnotationBackgroundColor.Text = "&Highlight";
            anButtonAnnotationBackgroundColor.UseVisualStyleBackColor = true;
            anButtonAnnotationBackgroundColor.Click += AnButtonAnnotationBackgroundColor_Click;
            // 
            // anButtonAnnotationFont
            // 
            anButtonAnnotationFont.Font = new Font("Segoe UI", 10F);
            anButtonAnnotationFont.Location = new Point(1049, 80);
            anButtonAnnotationFont.Name = "anButtonAnnotationFont";
            anButtonAnnotationFont.Size = new Size(150, 46);
            anButtonAnnotationFont.TabIndex = 4;
            anButtonAnnotationFont.Text = "&Font";
            anButtonAnnotationFont.UseVisualStyleBackColor = true;
            anButtonAnnotationFont.Click += AnButtonAnnotationFont_Click;
            // 
            // anButtonAnnotationFields
            // 
            anButtonAnnotationFields.Font = new Font("Segoe UI", 10F);
            anButtonAnnotationFields.Location = new Point(892, 80);
            anButtonAnnotationFields.Name = "anButtonAnnotationFields";
            anButtonAnnotationFields.Size = new Size(150, 46);
            anButtonAnnotationFields.TabIndex = 3;
            anButtonAnnotationFields.Text = "Fiel&ds";
            anButtonAnnotationFields.UseVisualStyleBackColor = true;
            anButtonAnnotationFields.Click += AnButtonAnnotationFields_Click;
            // 
            // anComboBoxAnnotationFormat
            // 
            anComboBoxAnnotationFormat.Font = new Font("Segoe UI", 10F);
            anComboBoxAnnotationFormat.Location = new Point(149, 82);
            anComboBoxAnnotationFormat.Name = "anComboBoxAnnotationFormat";
            anComboBoxAnnotationFormat.Size = new Size(733, 45);
            anComboBoxAnnotationFormat.TabIndex = 2;
            anComboBoxAnnotationFormat.TextChanged += AnComboBoxAnnotationFormat_TextChanged;
            // 
            // anLabelAnnotationFormat
            // 
            anLabelAnnotationFormat.AutoSize = true;
            anLabelAnnotationFormat.Font = new Font("Segoe UI", 10F);
            anLabelAnnotationFormat.Location = new Point(36, 83);
            anLabelAnnotationFormat.Name = "anLabelAnnotationFormat";
            anLabelAnnotationFormat.Size = new Size(101, 37);
            anLabelAnnotationFormat.TabIndex = 1;
            anLabelAnnotationFormat.Text = "&Format";
            // 
            // anToggleUseAnnotation
            // 
            anToggleUseAnnotation.Checked = false;
            anToggleUseAnnotation.Location = new Point(17, 5);
            anToggleUseAnnotation.Margin = new Padding(6);
            anToggleUseAnnotation.Name = "anToggleUseAnnotation";
            anToggleUseAnnotation.OffColor = Color.FromArgb(200, 200, 200);
            anToggleUseAnnotation.OnColor = Color.FromArgb(0, 120, 212);
            anToggleUseAnnotation.Size = new Size(75, 30);
            anToggleUseAnnotation.TabIndex = 0;
            anToggleUseAnnotation.ThumbColor = Color.White;
            anToggleUseAnnotation.CheckedChanged += AnToggleUseAnnotation_CheckedChanged;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Segoe UI", 10F);
            buttonCancel.Location = new Point(1009, 798);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(230, 70);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // buttonOk
            // 
            buttonOk.BackColor = Color.FromArgb(0, 120, 212);
            buttonOk.FlatAppearance.BorderSize = 0;
            buttonOk.FlatStyle = FlatStyle.Flat;
            buttonOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            buttonOk.ForeColor = Color.White;
            buttonOk.Location = new Point(1250, 798);
            buttonOk.Margin = new Padding(6);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(230, 70);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = false;
            buttonOk.Click += ButtonOk_Click;
            // 
            // FormSettings
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = buttonCancel;
            ClientSize = new Size(1504, 890);
            Controls.Add(buttonOk);
            Controls.Add(buttonCancel);
            Controls.Add(tabControlSettings);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Advanced Settings";
            tabControlSettings.ResumeLayout(false);
            tabPageWatermark.ResumeLayout(false);
            wmGroupBoxCommon.ResumeLayout(false);
            wmGroupBoxCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarOpacity).EndInit();
            wmGroupBoxUseImage.ResumeLayout(false);
            wmGroupBoxUseImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)wmPictureBoxWatermarkImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarWatermarkImageScale).EndInit();
            wmGroupBoxUseText.ResumeLayout(false);
            wmGroupBoxUseText.PerformLayout();
            tabPageAnnotation.ResumeLayout(false);
            anGroupBoxAnnotation.ResumeLayout(false);
            anGroupBoxAnnotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)anTrackBarOpacity).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlSettings;
        private TabPage tabPageWatermark;
        private TabPage tabPageAnnotation;
        private Button buttonCancel;
        private Button buttonOk;
        private GroupBox wmGroupBoxUseText;
        private ToggleSwitch wmToggleUseText;
        private Label wmLabelWatermarkText;
        private TextBox wmTextBoxWatermarkText;
        private Button wmButtonChooseFont;
        private Label wmLabelFontDescription;
        private GroupBox wmGroupBoxUseImage;
        private ToggleSwitch wmToggleUseImage;
        private Label wmLabelWatermarkImage;
        private TextBox wmTextBoxWatermarkImagePath;
        private Button wmButtonBrowseWatermarkImage;
        private TrackBar wmTrackBarWatermarkImageScale;
        private Label wmLabelWatermarkImageScale;
        private Label wmLabelWatermarkImageScaleValue;
        private PictureBox wmPictureBoxWatermarkImage;
        private GroupBox wmGroupBoxCommon;
        private TrackBar wmTrackBarOpacity;
        private Label wmLabelOpacity;
        private Label wmLabelOpacityValue;
        private ComboBox wmComboBoxWatermarkPosition;
        private Label wmLabelWatermarkPosition;
        private ComboBox wmComboBoxWatermarkRotation;
        private Label wmLabelWatermarkRotation;
        private GroupBox anGroupBoxAnnotation;
        private ToggleSwitch anToggleUseAnnotation;
        private Label anLabelAnnotationFormat;
        private ComboBox anComboBoxAnnotationFormat;
        private Button anButtonAnnotationFields;
        private Button anButtonAnnotationFont;
        private Button anButtonAnnotationBackgroundColor;
        private Label anLabelAnnotationSample;
        private TrackBar anTrackBarOpacity;
        private Label anLabelOpacity;
        private Label anLabelOpacityValue;
    }
}
