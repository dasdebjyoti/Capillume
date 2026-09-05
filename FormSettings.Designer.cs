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
            wmToggleUseImage = new ToggleSwitch();
            wmLabelWatermarkImage = new Label();
            wmTextBoxWatermarkImagePath = new TextBox();
            wmButtonBrowseWatermarkImage = new Button();
            wmLabelWatermarkImageScale = new Label();
            wmTrackBarWatermarkImageScale = new TrackBar();
            wmLabelWatermarkImageScaleValue = new Label();
            wmPictureBoxWatermarkImage = new PictureBox();
            wmGroupBoxUseText = new GroupBox();
            wmToggleUseText = new ToggleSwitch();
            wmLabelWatermarkText = new Label();
            wmTextBoxWatermarkText = new TextBox();
            wmButtonChooseFont = new Button();
            wmLabelFontDescription = new Label();
            tabPageAnnotation = new TabPage();
            anGroupBoxAnnotation = new GroupBox();
            anToggleUseAnnotation = new ToggleSwitch();
            anLabelAnnotationFormat = new Label();
            anComboBoxAnnotationFormat = new ComboBox();
            anButtonAnnotationFields = new Button();
            anButtonAnnotationFont = new Button();
            anButtonAnnotationBackgroundColor = new Button();
            anLabelAnnotationSample = new Label();
            anLabelOpacity = new Label();
            anTrackBarOpacity = new TrackBar();
            anLabelOpacityValue = new Label();
            tabPageDownscale = new TabPage();
            dsLabelDefaultSize = new Label();
            dsLabelSummary1 = new Label();
            dsLabelEnable = new Label();
            dsToggleEnable1 = new ToggleSwitch();
            dsGroupBoxProcessing1 = new GroupBox();
            dsLabelQuality1 = new Label();
            dsCheckBoxLossyOnly1 = new CheckBox();
            dsCheckBoxFullScreenOnly1 = new CheckBox();
            dsCheckBoxSkipSmaller1 = new CheckBox();
            dsCheckBoxSharpen1 = new CheckBox();
            dsComboBoxQuality1 = new ComboBox();
            dsGroupBoxModes1 = new GroupBox();
            dsLabelBoundingWidth1 = new Label();
            dsLabelMaxWidth1 = new Label();
            dsLabelBoundingHeight1 = new Label();
            dsLabelPercentageValue1 = new Label();
            dsLabelTargetHeight = new Label();
            dsNumericBoundingHeight1 = new NumericUpDown();
            dsNumericBoundingWidth1 = new NumericUpDown();
            dsNumericMaxWidth1 = new NumericUpDown();
            dsNumericPercentage1 = new NumericUpDown();
            dsNumericTargetHeight1 = new NumericUpDown();
            dsComboBoxBoundingBox1 = new ComboBox();
            dsComboBoxMaxWidth1 = new ComboBox();
            dsComboBoxPercentage1 = new ComboBox();
            dsComboBoxTargetHeight1 = new ComboBox();
            dsRadioBoundingBox1 = new RadioButton();
            dsRadioMaxWidth1 = new RadioButton();
            dsRadioPercentage1 = new RadioButton();
            dsRadioTargetHeight1 = new RadioButton();
            buttonCancel = new Button();
            buttonOk = new Button();
            tabControlSettings.SuspendLayout();
            tabPageWatermark.SuspendLayout();
            wmGroupBoxCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarOpacity).BeginInit();
            wmGroupBoxUseImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarWatermarkImageScale).BeginInit();
            ((System.ComponentModel.ISupportInitialize)wmPictureBoxWatermarkImage).BeginInit();
            wmGroupBoxUseText.SuspendLayout();
            tabPageAnnotation.SuspendLayout();
            anGroupBoxAnnotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)anTrackBarOpacity).BeginInit();
            tabPageDownscale.SuspendLayout();
            dsGroupBoxProcessing1.SuspendLayout();
            dsGroupBoxModes1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dsNumericBoundingHeight1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericBoundingWidth1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericMaxWidth1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericPercentage1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericTargetHeight1).BeginInit();
            SuspendLayout();
            // 
            // tabControlSettings
            // 
            tabControlSettings.Controls.Add(tabPageWatermark);
            tabControlSettings.Controls.Add(tabPageAnnotation);
            tabControlSettings.Controls.Add(tabPageDownscale);
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
            wmGroupBoxUseImage.Controls.Add(wmToggleUseImage);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImage);
            wmGroupBoxUseImage.Controls.Add(wmTextBoxWatermarkImagePath);
            wmGroupBoxUseImage.Controls.Add(wmButtonBrowseWatermarkImage);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImageScale);
            wmGroupBoxUseImage.Controls.Add(wmTrackBarWatermarkImageScale);
            wmGroupBoxUseImage.Controls.Add(wmLabelWatermarkImageScaleValue);
            wmGroupBoxUseImage.Controls.Add(wmPictureBoxWatermarkImage);
            wmGroupBoxUseImage.Font = new Font("Segoe UI", 10F);
            wmGroupBoxUseImage.Location = new Point(727, 24);
            wmGroupBoxUseImage.Name = "wmGroupBoxUseImage";
            wmGroupBoxUseImage.Size = new Size(697, 309);
            wmGroupBoxUseImage.TabIndex = 1;
            wmGroupBoxUseImage.TabStop = false;
            wmGroupBoxUseImage.Text = "             Use Image";
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
            // wmLabelWatermarkImage
            // 
            wmLabelWatermarkImage.AutoSize = true;
            wmLabelWatermarkImage.Location = new Point(36, 84);
            wmLabelWatermarkImage.Name = "wmLabelWatermarkImage";
            wmLabelWatermarkImage.Size = new Size(91, 37);
            wmLabelWatermarkImage.TabIndex = 1;
            wmLabelWatermarkImage.Text = "&Image";
            // 
            // wmTextBoxWatermarkImagePath
            // 
            wmTextBoxWatermarkImagePath.Location = new Point(149, 81);
            wmTextBoxWatermarkImagePath.Name = "wmTextBoxWatermarkImagePath";
            wmTextBoxWatermarkImagePath.Size = new Size(361, 43);
            wmTextBoxWatermarkImagePath.TabIndex = 2;
            wmTextBoxWatermarkImagePath.TextChanged += WmTextBoxWatermarkImagePath_TextChanged;
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
            anGroupBoxAnnotation.Controls.Add(anToggleUseAnnotation);
            anGroupBoxAnnotation.Controls.Add(anLabelAnnotationFormat);
            anGroupBoxAnnotation.Controls.Add(anComboBoxAnnotationFormat);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationFields);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationFont);
            anGroupBoxAnnotation.Controls.Add(anButtonAnnotationBackgroundColor);
            anGroupBoxAnnotation.Controls.Add(anLabelAnnotationSample);
            anGroupBoxAnnotation.Controls.Add(anLabelOpacity);
            anGroupBoxAnnotation.Controls.Add(anTrackBarOpacity);
            anGroupBoxAnnotation.Controls.Add(anLabelOpacityValue);
            anGroupBoxAnnotation.Font = new Font("Segoe UI", 10F);
            anGroupBoxAnnotation.Location = new Point(16, 24);
            anGroupBoxAnnotation.Name = "anGroupBoxAnnotation";
            anGroupBoxAnnotation.Size = new Size(1408, 417);
            anGroupBoxAnnotation.TabIndex = 0;
            anGroupBoxAnnotation.TabStop = false;
            anGroupBoxAnnotation.Text = "             Use Annotation";
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
            // anComboBoxAnnotationFormat
            // 
            anComboBoxAnnotationFormat.Font = new Font("Segoe UI", 10F);
            anComboBoxAnnotationFormat.Location = new Point(149, 82);
            anComboBoxAnnotationFormat.Name = "anComboBoxAnnotationFormat";
            anComboBoxAnnotationFormat.Size = new Size(733, 45);
            anComboBoxAnnotationFormat.TabIndex = 2;
            anComboBoxAnnotationFormat.TextChanged += AnComboBoxAnnotationFormat_TextChanged;
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
            // tabPageDownscale
            // 
            tabPageDownscale.BackColor = Color.White;
            tabPageDownscale.Controls.Add(dsLabelDefaultSize);
            tabPageDownscale.Controls.Add(dsLabelSummary1);
            tabPageDownscale.Controls.Add(dsLabelEnable);
            tabPageDownscale.Controls.Add(dsToggleEnable1);
            tabPageDownscale.Controls.Add(dsGroupBoxProcessing1);
            tabPageDownscale.Controls.Add(dsGroupBoxModes1);
            tabPageDownscale.Location = new Point(8, 51);
            tabPageDownscale.Name = "tabPageDownscale";
            tabPageDownscale.Padding = new Padding(3);
            tabPageDownscale.Size = new Size(1440, 701);
            tabPageDownscale.TabIndex = 2;
            tabPageDownscale.Text = "Downscale";
            // 
            // dsLabelDefaultSize
            // 
            dsLabelDefaultSize.ForeColor = SystemColors.Highlight;
            dsLabelDefaultSize.Location = new Point(615, 11);
            dsLabelDefaultSize.Name = "dsLabelDefaultSize";
            dsLabelDefaultSize.Size = new Size(809, 80);
            dsLabelDefaultSize.TabIndex = 4;
            dsLabelDefaultSize.Text = "Detected default size: full-screen captures 3,840×2,160; active-window captures keep the original window size.";
            // 
            // dsLabelSummary1
            // 
            dsLabelSummary1.BorderStyle = BorderStyle.FixedSingle;
            dsLabelSummary1.Location = new Point(16, 591);
            dsLabelSummary1.Name = "dsLabelSummary1";
            dsLabelSummary1.Size = new Size(1408, 99);
            dsLabelSummary1.TabIndex = 3;
            dsLabelSummary1.Text = "Downscaling is currently off. Screenshots will be saved at their original size.";
            // 
            // dsLabelEnable
            // 
            dsLabelEnable.AutoSize = true;
            dsLabelEnable.Location = new Point(129, 27);
            dsLabelEnable.Name = "dsLabelEnable";
            dsLabelEnable.Size = new Size(256, 37);
            dsLabelEnable.TabIndex = 2;
            dsLabelEnable.Text = "Enable Downscaling";
            // 
            // dsToggleEnable1
            // 
            dsToggleEnable1.Checked = false;
            dsToggleEnable1.Location = new Point(33, 29);
            dsToggleEnable1.Margin = new Padding(6);
            dsToggleEnable1.Name = "dsToggleEnable1";
            dsToggleEnable1.OffColor = Color.FromArgb(200, 200, 200);
            dsToggleEnable1.OnColor = Color.FromArgb(0, 120, 212);
            dsToggleEnable1.Size = new Size(75, 30);
            dsToggleEnable1.TabIndex = 0;
            dsToggleEnable1.Text = "Enable screenshot downscale";
            dsToggleEnable1.ThumbColor = Color.White;
            dsToggleEnable1.CheckedChanged += DownscaleSettingChanged;
            // 
            // dsGroupBoxProcessing1
            // 
            dsGroupBoxProcessing1.Controls.Add(dsLabelQuality1);
            dsGroupBoxProcessing1.Controls.Add(dsCheckBoxLossyOnly1);
            dsGroupBoxProcessing1.Controls.Add(dsCheckBoxFullScreenOnly1);
            dsGroupBoxProcessing1.Controls.Add(dsCheckBoxSkipSmaller1);
            dsGroupBoxProcessing1.Controls.Add(dsCheckBoxSharpen1);
            dsGroupBoxProcessing1.Controls.Add(dsComboBoxQuality1);
            dsGroupBoxProcessing1.Font = new Font("Segoe UI", 10F);
            dsGroupBoxProcessing1.Location = new Point(16, 362);
            dsGroupBoxProcessing1.Name = "dsGroupBoxProcessing1";
            dsGroupBoxProcessing1.Size = new Size(1408, 220);
            dsGroupBoxProcessing1.TabIndex = 1;
            dsGroupBoxProcessing1.TabStop = false;
            dsGroupBoxProcessing1.Text = "Processing";
            // 
            // dsLabelQuality1
            // 
            dsLabelQuality1.AutoSize = true;
            dsLabelQuality1.Location = new Point(46, 62);
            dsLabelQuality1.Name = "dsLabelQuality1";
            dsLabelQuality1.Size = new Size(247, 37);
            dsLabelQuality1.TabIndex = 3;
            dsLabelQuality1.Text = "Resampling Quality";
            // 
            // dsCheckBoxLossyOnly1
            // 
            dsCheckBoxLossyOnly1.AutoSize = true;
            dsCheckBoxLossyOnly1.Location = new Point(706, 166);
            dsCheckBoxLossyOnly1.Name = "dsCheckBoxLossyOnly1";
            dsCheckBoxLossyOnly1.Size = new Size(544, 41);
            dsCheckBoxLossyOnly1.TabIndex = 2;
            dsCheckBoxLossyOnly1.Text = "Downscale lossy formats only (JPG/WEBP)";
            dsCheckBoxLossyOnly1.UseVisualStyleBackColor = true;
            dsCheckBoxLossyOnly1.CheckedChanged += DownscaleSettingChanged;
            // 
            // dsCheckBoxFullScreenOnly1
            // 
            dsCheckBoxFullScreenOnly1.AutoSize = true;
            dsCheckBoxFullScreenOnly1.Location = new Point(706, 119);
            dsCheckBoxFullScreenOnly1.Name = "dsCheckBoxFullScreenOnly1";
            dsCheckBoxFullScreenOnly1.Size = new Size(472, 41);
            dsCheckBoxFullScreenOnly1.TabIndex = 2;
            dsCheckBoxFullScreenOnly1.Text = "Downscale full-screen captures only";
            dsCheckBoxFullScreenOnly1.UseVisualStyleBackColor = true;
            dsCheckBoxFullScreenOnly1.CheckedChanged += DownscaleSettingChanged;
            // 
            // dsCheckBoxSkipSmaller1
            // 
            dsCheckBoxSkipSmaller1.AutoSize = true;
            dsCheckBoxSkipSmaller1.Location = new Point(46, 166);
            dsCheckBoxSkipSmaller1.Name = "dsCheckBoxSkipSmaller1";
            dsCheckBoxSkipSmaller1.Size = new Size(389, 41);
            dsCheckBoxSkipSmaller1.TabIndex = 2;
            dsCheckBoxSkipSmaller1.Text = "Do not resize smaller images";
            dsCheckBoxSkipSmaller1.UseVisualStyleBackColor = true;
            dsCheckBoxSkipSmaller1.CheckedChanged += DownscaleSettingChanged;
            // 
            // dsCheckBoxSharpen1
            // 
            dsCheckBoxSharpen1.AutoSize = true;
            dsCheckBoxSharpen1.Location = new Point(46, 119);
            dsCheckBoxSharpen1.Name = "dsCheckBoxSharpen1";
            dsCheckBoxSharpen1.Size = new Size(339, 41);
            dsCheckBoxSharpen1.TabIndex = 2;
            dsCheckBoxSharpen1.Text = "Sharpen after downscale";
            dsCheckBoxSharpen1.UseVisualStyleBackColor = true;
            dsCheckBoxSharpen1.CheckedChanged += DownscaleSettingChanged;
            // 
            // dsComboBoxQuality1
            // 
            dsComboBoxQuality1.DropDownStyle = ComboBoxStyle.DropDownList;
            dsComboBoxQuality1.FormattingEnabled = true;
            dsComboBoxQuality1.Location = new Point(336, 59);
            dsComboBoxQuality1.Name = "dsComboBoxQuality1";
            dsComboBoxQuality1.Size = new Size(345, 45);
            dsComboBoxQuality1.TabIndex = 1;
            dsComboBoxQuality1.SelectedIndexChanged += DownscaleSettingChanged;
            // 
            // dsGroupBoxModes1
            // 
            dsGroupBoxModes1.Controls.Add(dsLabelBoundingWidth1);
            dsGroupBoxModes1.Controls.Add(dsLabelMaxWidth1);
            dsGroupBoxModes1.Controls.Add(dsLabelBoundingHeight1);
            dsGroupBoxModes1.Controls.Add(dsLabelPercentageValue1);
            dsGroupBoxModes1.Controls.Add(dsLabelTargetHeight);
            dsGroupBoxModes1.Controls.Add(dsNumericBoundingHeight1);
            dsGroupBoxModes1.Controls.Add(dsNumericBoundingWidth1);
            dsGroupBoxModes1.Controls.Add(dsNumericMaxWidth1);
            dsGroupBoxModes1.Controls.Add(dsNumericPercentage1);
            dsGroupBoxModes1.Controls.Add(dsNumericTargetHeight1);
            dsGroupBoxModes1.Controls.Add(dsComboBoxBoundingBox1);
            dsGroupBoxModes1.Controls.Add(dsComboBoxMaxWidth1);
            dsGroupBoxModes1.Controls.Add(dsComboBoxPercentage1);
            dsGroupBoxModes1.Controls.Add(dsComboBoxTargetHeight1);
            dsGroupBoxModes1.Controls.Add(dsRadioBoundingBox1);
            dsGroupBoxModes1.Controls.Add(dsRadioMaxWidth1);
            dsGroupBoxModes1.Controls.Add(dsRadioPercentage1);
            dsGroupBoxModes1.Controls.Add(dsRadioTargetHeight1);
            dsGroupBoxModes1.Font = new Font("Segoe UI", 10F);
            dsGroupBoxModes1.Location = new Point(16, 81);
            dsGroupBoxModes1.Name = "dsGroupBoxModes1";
            dsGroupBoxModes1.Size = new Size(1408, 271);
            dsGroupBoxModes1.TabIndex = 0;
            dsGroupBoxModes1.TabStop = false;
            dsGroupBoxModes1.Text = "Resize Mode";
            // 
            // dsLabelBoundingWidth1
            // 
            dsLabelBoundingWidth1.AutoSize = true;
            dsLabelBoundingWidth1.Location = new Point(706, 215);
            dsLabelBoundingWidth1.Name = "dsLabelBoundingWidth1";
            dsLabelBoundingWidth1.Size = new Size(89, 37);
            dsLabelBoundingWidth1.TabIndex = 3;
            dsLabelBoundingWidth1.Text = "Width";
            // 
            // dsLabelMaxWidth1
            // 
            dsLabelMaxWidth1.AutoSize = true;
            dsLabelMaxWidth1.Location = new Point(706, 169);
            dsLabelMaxWidth1.Name = "dsLabelMaxWidth1";
            dsLabelMaxWidth1.Size = new Size(89, 37);
            dsLabelMaxWidth1.TabIndex = 3;
            dsLabelMaxWidth1.Text = "Width";
            // 
            // dsLabelBoundingHeight1
            // 
            dsLabelBoundingHeight1.AutoSize = true;
            dsLabelBoundingHeight1.Location = new Point(1001, 217);
            dsLabelBoundingHeight1.Name = "dsLabelBoundingHeight1";
            dsLabelBoundingHeight1.Size = new Size(97, 37);
            dsLabelBoundingHeight1.TabIndex = 3;
            dsLabelBoundingHeight1.Text = "Height";
            // 
            // dsLabelPercentageValue1
            // 
            dsLabelPercentageValue1.AutoSize = true;
            dsLabelPercentageValue1.Location = new Point(706, 121);
            dsLabelPercentageValue1.Name = "dsLabelPercentageValue1";
            dsLabelPercentageValue1.Size = new Size(104, 37);
            dsLabelPercentageValue1.TabIndex = 3;
            dsLabelPercentageValue1.Text = "Percent";
            // 
            // dsLabelTargetHeight
            // 
            dsLabelTargetHeight.AutoSize = true;
            dsLabelTargetHeight.Location = new Point(706, 75);
            dsLabelTargetHeight.Name = "dsLabelTargetHeight";
            dsLabelTargetHeight.Size = new Size(97, 37);
            dsLabelTargetHeight.TabIndex = 3;
            dsLabelTargetHeight.Text = "Height";
            // 
            // dsNumericBoundingHeight1
            // 
            dsNumericBoundingHeight1.Location = new Point(1122, 213);
            dsNumericBoundingHeight1.Name = "dsNumericBoundingHeight1";
            dsNumericBoundingHeight1.Size = new Size(148, 43);
            dsNumericBoundingHeight1.TabIndex = 2;
            dsNumericBoundingHeight1.ThousandsSeparator = true;
            dsNumericBoundingHeight1.ValueChanged += DsNumericBoundingBox_ValueChanged;
            // 
            // dsNumericBoundingWidth1
            // 
            dsNumericBoundingWidth1.Location = new Point(821, 215);
            dsNumericBoundingWidth1.Name = "dsNumericBoundingWidth1";
            dsNumericBoundingWidth1.Size = new Size(148, 43);
            dsNumericBoundingWidth1.TabIndex = 2;
            dsNumericBoundingWidth1.ThousandsSeparator = true;
            dsNumericBoundingWidth1.ValueChanged += DsNumericBoundingBox_ValueChanged;
            // 
            // dsNumericMaxWidth1
            // 
            dsNumericMaxWidth1.Location = new Point(821, 166);
            dsNumericMaxWidth1.Name = "dsNumericMaxWidth1";
            dsNumericMaxWidth1.Size = new Size(148, 43);
            dsNumericMaxWidth1.TabIndex = 2;
            dsNumericMaxWidth1.ThousandsSeparator = true;
            dsNumericMaxWidth1.ValueChanged += DsNumericMaxWidth_ValueChanged;
            // 
            // dsNumericPercentage1
            // 
            dsNumericPercentage1.Location = new Point(821, 119);
            dsNumericPercentage1.Name = "dsNumericPercentage1";
            dsNumericPercentage1.Size = new Size(148, 43);
            dsNumericPercentage1.TabIndex = 2;
            dsNumericPercentage1.ValueChanged += DsNumericPercentage_ValueChanged;
            // 
            // dsNumericTargetHeight1
            // 
            dsNumericTargetHeight1.Location = new Point(821, 72);
            dsNumericTargetHeight1.Name = "dsNumericTargetHeight1";
            dsNumericTargetHeight1.Size = new Size(148, 43);
            dsNumericTargetHeight1.TabIndex = 2;
            dsNumericTargetHeight1.ThousandsSeparator = true;
            dsNumericTargetHeight1.ValueChanged += DsNumericTargetHeight_ValueChanged;
            // 
            // dsComboBoxBoundingBox1
            // 
            dsComboBoxBoundingBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            dsComboBoxBoundingBox1.FormattingEnabled = true;
            dsComboBoxBoundingBox1.Location = new Point(471, 213);
            dsComboBoxBoundingBox1.Name = "dsComboBoxBoundingBox1";
            dsComboBoxBoundingBox1.Size = new Size(210, 45);
            dsComboBoxBoundingBox1.TabIndex = 1;
            dsComboBoxBoundingBox1.SelectedIndexChanged += DsComboBoxBoundingBox_SelectedIndexChanged;
            // 
            // dsComboBoxMaxWidth1
            // 
            dsComboBoxMaxWidth1.DropDownStyle = ComboBoxStyle.DropDownList;
            dsComboBoxMaxWidth1.FormattingEnabled = true;
            dsComboBoxMaxWidth1.Location = new Point(471, 166);
            dsComboBoxMaxWidth1.Name = "dsComboBoxMaxWidth1";
            dsComboBoxMaxWidth1.Size = new Size(210, 45);
            dsComboBoxMaxWidth1.TabIndex = 1;
            dsComboBoxMaxWidth1.SelectedIndexChanged += DsComboBoxMaxWidth_SelectedIndexChanged;
            // 
            // dsComboBoxPercentage1
            // 
            dsComboBoxPercentage1.DropDownStyle = ComboBoxStyle.DropDownList;
            dsComboBoxPercentage1.FormattingEnabled = true;
            dsComboBoxPercentage1.Location = new Point(471, 119);
            dsComboBoxPercentage1.Name = "dsComboBoxPercentage1";
            dsComboBoxPercentage1.Size = new Size(210, 45);
            dsComboBoxPercentage1.TabIndex = 1;
            dsComboBoxPercentage1.SelectedIndexChanged += DsComboBoxPercentage_SelectedIndexChanged;
            // 
            // dsComboBoxTargetHeight1
            // 
            dsComboBoxTargetHeight1.DropDownStyle = ComboBoxStyle.DropDownList;
            dsComboBoxTargetHeight1.FormattingEnabled = true;
            dsComboBoxTargetHeight1.Location = new Point(471, 72);
            dsComboBoxTargetHeight1.Name = "dsComboBoxTargetHeight1";
            dsComboBoxTargetHeight1.Size = new Size(210, 45);
            dsComboBoxTargetHeight1.TabIndex = 1;
            dsComboBoxTargetHeight1.SelectedIndexChanged += DsComboBoxTargetHeight_SelectedIndexChanged;
            // 
            // dsRadioBoundingBox1
            // 
            dsRadioBoundingBox1.AutoSize = true;
            dsRadioBoundingBox1.Location = new Point(46, 215);
            dsRadioBoundingBox1.Name = "dsRadioBoundingBox1";
            dsRadioBoundingBox1.Size = new Size(334, 41);
            dsRadioBoundingBox1.TabIndex = 0;
            dsRadioBoundingBox1.TabStop = true;
            dsRadioBoundingBox1.Text = "Fit Within Bounding Box";
            dsRadioBoundingBox1.UseVisualStyleBackColor = true;
            dsRadioBoundingBox1.CheckedChanged += DownscaleModeChanged;
            // 
            // dsRadioMaxWidth1
            // 
            dsRadioMaxWidth1.AutoSize = true;
            dsRadioMaxWidth1.Location = new Point(46, 168);
            dsRadioMaxWidth1.Name = "dsRadioMaxWidth1";
            dsRadioMaxWidth1.Size = new Size(357, 41);
            dsRadioMaxWidth1.TabIndex = 0;
            dsRadioMaxWidth1.TabStop = true;
            dsRadioMaxWidth1.Text = "Resize to Maximum Width";
            dsRadioMaxWidth1.UseVisualStyleBackColor = true;
            dsRadioMaxWidth1.CheckedChanged += DownscaleModeChanged;
            // 
            // dsRadioPercentage1
            // 
            dsRadioPercentage1.AutoSize = true;
            dsRadioPercentage1.Location = new Point(46, 121);
            dsRadioPercentage1.Name = "dsRadioPercentage1";
            dsRadioPercentage1.Size = new Size(295, 41);
            dsRadioPercentage1.TabIndex = 0;
            dsRadioPercentage1.TabStop = true;
            dsRadioPercentage1.Text = "Resize by Percentage";
            dsRadioPercentage1.UseVisualStyleBackColor = true;
            dsRadioPercentage1.CheckedChanged += DownscaleModeChanged;
            // 
            // dsRadioTargetHeight1
            // 
            dsRadioTargetHeight1.AutoSize = true;
            dsRadioTargetHeight1.Location = new Point(46, 74);
            dsRadioTargetHeight1.Name = "dsRadioTargetHeight1";
            dsRadioTargetHeight1.Size = new Size(365, 41);
            dsRadioTargetHeight1.TabIndex = 0;
            dsRadioTargetHeight1.TabStop = true;
            dsRadioTargetHeight1.Text = "Resize to Maximum Height";
            dsRadioTargetHeight1.UseVisualStyleBackColor = true;
            dsRadioTargetHeight1.CheckedChanged += DownscaleModeChanged;
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
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Advanced Settings";
            tabControlSettings.ResumeLayout(false);
            tabPageWatermark.ResumeLayout(false);
            wmGroupBoxCommon.ResumeLayout(false);
            wmGroupBoxCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarOpacity).EndInit();
            wmGroupBoxUseImage.ResumeLayout(false);
            wmGroupBoxUseImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)wmTrackBarWatermarkImageScale).EndInit();
            ((System.ComponentModel.ISupportInitialize)wmPictureBoxWatermarkImage).EndInit();
            wmGroupBoxUseText.ResumeLayout(false);
            wmGroupBoxUseText.PerformLayout();
            tabPageAnnotation.ResumeLayout(false);
            anGroupBoxAnnotation.ResumeLayout(false);
            anGroupBoxAnnotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)anTrackBarOpacity).EndInit();
            tabPageDownscale.ResumeLayout(false);
            tabPageDownscale.PerformLayout();
            dsGroupBoxProcessing1.ResumeLayout(false);
            dsGroupBoxProcessing1.PerformLayout();
            dsGroupBoxModes1.ResumeLayout(false);
            dsGroupBoxModes1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dsNumericBoundingHeight1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericBoundingWidth1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericMaxWidth1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericPercentage1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dsNumericTargetHeight1).EndInit();
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
        private TabPage tabPageDownscale;
        private ToggleSwitch dsToggleEnable1;
        private GroupBox dsGroupBoxModes1;
        private RadioButton dsRadioBoundingBox1;
        private RadioButton dsRadioMaxWidth1;
        private RadioButton dsRadioPercentage1;
        private RadioButton dsRadioTargetHeight1;
        private ComboBox dsComboBoxBoundingBox1;
        private ComboBox dsComboBoxMaxWidth1;
        private ComboBox dsComboBoxPercentage1;
        private ComboBox dsComboBoxTargetHeight1;
        private NumericUpDown dsNumericBoundingHeight1;
        private NumericUpDown dsNumericBoundingWidth1;
        private NumericUpDown dsNumericMaxWidth1;
        private NumericUpDown dsNumericPercentage1;
        private NumericUpDown dsNumericTargetHeight1;
        private Label dsLabelTargetHeight;
        private Label dsLabelMaxWidth1;
        private Label dsLabelBoundingWidth1;
        private Label dsLabelBoundingHeight1;
        private Label dsLabelPercentageValue1;
        private GroupBox dsGroupBoxProcessing1;
        private ComboBox dsComboBoxQuality1;
        private CheckBox dsCheckBoxSharpen1;
        private CheckBox dsCheckBoxSkipSmaller1;
        private CheckBox dsCheckBoxFullScreenOnly1;
        private CheckBox dsCheckBoxLossyOnly1;
        private Label dsLabelQuality1;
        private Label dsLabelEnable;
        private Label dsLabelSummary1;
        private Label dsLabelDefaultSize;
    }
}
