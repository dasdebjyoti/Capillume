namespace Capillume
{
    partial class FormAnnotation
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
            groupBoxAnnotation = new GroupBox();
            trackBarOpacity = new TrackBar();
            labelOpacity = new Label();
            labelOpacityValue = new Label();
            labelAnnotationSample = new Label();
            buttonAnnotationBackgroundColor = new Button();
            buttonAnnotationFont = new Button();
            buttonAnnotationFields = new Button();
            labelAnnotationFormat = new Label();
            comboBoxAnnotationFormat = new ComboBox();
            toggleUseAnnotation = new ToggleSwitch();
            buttonCancel = new Button();
            buttonOk = new Button();
            groupBoxAnnotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarOpacity).BeginInit();
            SuspendLayout();
            // 
            // groupBoxAnnotation
            // 
            groupBoxAnnotation.Controls.Add(trackBarOpacity);
            groupBoxAnnotation.Controls.Add(labelOpacity);
            groupBoxAnnotation.Controls.Add(labelOpacityValue);
            groupBoxAnnotation.Controls.Add(labelAnnotationSample);
            groupBoxAnnotation.Controls.Add(buttonAnnotationBackgroundColor);
            groupBoxAnnotation.Controls.Add(buttonAnnotationFont);
            groupBoxAnnotation.Controls.Add(buttonAnnotationFields);
            groupBoxAnnotation.Controls.Add(labelAnnotationFormat);
            groupBoxAnnotation.Controls.Add(comboBoxAnnotationFormat);
            groupBoxAnnotation.Controls.Add(toggleUseAnnotation);
            groupBoxAnnotation.Font = new Font("Segoe UI", 10F);
            groupBoxAnnotation.Location = new Point(27, 31);
            groupBoxAnnotation.Name = "groupBoxAnnotation";
            groupBoxAnnotation.Size = new Size(1260, 400);
            groupBoxAnnotation.TabIndex = 0;
            groupBoxAnnotation.TabStop = false;
            groupBoxAnnotation.Text = "             Use Annotation";
            // 
            // trackBarOpacity
            // 
            trackBarOpacity.Location = new Point(149, 307);
            trackBarOpacity.Margin = new Padding(6);
            trackBarOpacity.Maximum = 100;
            trackBarOpacity.Minimum = 1;
            trackBarOpacity.Name = "trackBarOpacity";
            trackBarOpacity.Size = new Size(1012, 90);
            trackBarOpacity.TabIndex = 8;
            trackBarOpacity.TickFrequency = 10;
            trackBarOpacity.Value = 50;
            trackBarOpacity.ValueChanged += TrackBarOpacity_ValueChanged;
            // 
            // labelOpacity
            // 
            labelOpacity.AutoSize = true;
            labelOpacity.Font = new Font("Segoe UI", 10F);
            labelOpacity.Location = new Point(39, 305);
            labelOpacity.Margin = new Padding(6, 0, 6, 0);
            labelOpacity.Name = "labelOpacity";
            labelOpacity.Size = new Size(108, 37);
            labelOpacity.TabIndex = 7;
            labelOpacity.Text = "&Opacity";
            // 
            // labelOpacityValue
            // 
            labelOpacityValue.AutoSize = true;
            labelOpacityValue.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            labelOpacityValue.Location = new Point(1173, 305);
            labelOpacityValue.Margin = new Padding(6, 0, 6, 0);
            labelOpacityValue.Name = "labelOpacityValue";
            labelOpacityValue.Size = new Size(72, 37);
            labelOpacityValue.TabIndex = 9;
            labelOpacityValue.Text = "50%";
            labelOpacityValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelAnnotationSample
            // 
            labelAnnotationSample.AutoSize = true;
            labelAnnotationSample.Font = new Font("Segoe UI", 12F);
            labelAnnotationSample.Location = new Point(36, 193);
            labelAnnotationSample.Name = "labelAnnotationSample";
            labelAnnotationSample.Size = new Size(105, 45);
            labelAnnotationSample.TabIndex = 6;
            labelAnnotationSample.Text = "label1";
            // 
            // buttonAnnotationBackgroundColor
            // 
            buttonAnnotationBackgroundColor.Location = new Point(1095, 78);
            buttonAnnotationBackgroundColor.Name = "buttonAnnotationBackgroundColor";
            buttonAnnotationBackgroundColor.Size = new Size(150, 46);
            buttonAnnotationBackgroundColor.TabIndex = 5;
            buttonAnnotationBackgroundColor.Text = "&Highlight";
            buttonAnnotationBackgroundColor.UseVisualStyleBackColor = true;
            buttonAnnotationBackgroundColor.Click += ButtonAnnotationBackgroundColor_Click;
            // 
            // buttonAnnotationFont
            // 
            buttonAnnotationFont.Font = new Font("Segoe UI", 10F);
            buttonAnnotationFont.Location = new Point(939, 78);
            buttonAnnotationFont.Name = "buttonAnnotationFont";
            buttonAnnotationFont.Size = new Size(150, 46);
            buttonAnnotationFont.TabIndex = 4;
            buttonAnnotationFont.Text = "&Font";
            buttonAnnotationFont.UseVisualStyleBackColor = true;
            buttonAnnotationFont.Click += ButtonAnnotationFont_Click;
            // 
            // buttonAnnotationFields
            // 
            buttonAnnotationFields.Font = new Font("Segoe UI", 10F);
            buttonAnnotationFields.Location = new Point(783, 78);
            buttonAnnotationFields.Name = "buttonAnnotationFields";
            buttonAnnotationFields.Size = new Size(150, 46);
            buttonAnnotationFields.TabIndex = 3;
            buttonAnnotationFields.Text = "Fiel&ds";
            buttonAnnotationFields.UseVisualStyleBackColor = true;
            buttonAnnotationFields.Click += ButtonAnnotationFields_Click;
            // 
            // labelAnnotationFormat
            // 
            labelAnnotationFormat.AutoSize = true;
            labelAnnotationFormat.Font = new Font("Segoe UI", 10F);
            labelAnnotationFormat.Location = new Point(36, 83);
            labelAnnotationFormat.Name = "labelAnnotationFormat";
            labelAnnotationFormat.Size = new Size(101, 37);
            labelAnnotationFormat.TabIndex = 1;
            labelAnnotationFormat.Text = "F&ormat";
            // 
            // comboBoxAnnotationFormat
            // 
            comboBoxAnnotationFormat.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxAnnotationFormat.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBoxAnnotationFormat.Font = new Font("Segoe UI", 10F);
            comboBoxAnnotationFormat.FormattingEnabled = true;
            comboBoxAnnotationFormat.Location = new Point(149, 80);
            comboBoxAnnotationFormat.Name = "comboBoxAnnotationFormat";
            comboBoxAnnotationFormat.Size = new Size(628, 45);
            comboBoxAnnotationFormat.TabIndex = 2;
            comboBoxAnnotationFormat.TextChanged += ComboBoxAnnotationFormat_TextChanged;
            // 
            // toggleUseAnnotation
            // 
            toggleUseAnnotation.Checked = false;
            toggleUseAnnotation.Location = new Point(17, 5);
            toggleUseAnnotation.Margin = new Padding(6);
            toggleUseAnnotation.Name = "toggleUseAnnotation";
            toggleUseAnnotation.OffColor = Color.FromArgb(200, 200, 200);
            toggleUseAnnotation.OnColor = Color.FromArgb(0, 120, 212);
            toggleUseAnnotation.Size = new Size(75, 30);
            toggleUseAnnotation.TabIndex = 0;
            toggleUseAnnotation.ThumbColor = Color.White;
            toggleUseAnnotation.CheckedChanged += ToggleUseAnnotation_CheckedChanged;
            // 
            // buttonCancel
            // 
            buttonCancel.Font = new Font("Segoe UI", 10F);
            buttonCancel.Location = new Point(818, 454);
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
            buttonOk.Location = new Point(1057, 454);
            buttonOk.Margin = new Padding(6);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new Size(230, 70);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = false;
            buttonOk.Click += ButtonOk_Click;
            // 
            // FormAnnotation
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new SizeF(192F, 192F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = buttonCancel;
            ClientSize = new Size(1316, 547);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(groupBoxAnnotation);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormAnnotation";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Annotation";
            groupBoxAnnotation.ResumeLayout(false);
            groupBoxAnnotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)trackBarOpacity).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxAnnotation;
        private ToggleSwitch toggleUseAnnotation;
        private ComboBox comboBoxAnnotationFormat;
        private Label labelAnnotationFormat;
        private Button buttonAnnotationFields;
        private Button buttonAnnotationFont;
        private Button buttonAnnotationBackgroundColor;
        private Label labelAnnotationSample;
        private Button buttonCancel;
        private Button buttonOk;
        private TrackBar trackBarOpacity;
        private Label labelOpacity;
        private Label labelOpacityValue;
    }
}