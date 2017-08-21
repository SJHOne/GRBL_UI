namespace GrblOutput
{
    partial class SVGProcess
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
            this.OpenButton = new System.Windows.Forms.Button();
            this.dpiControl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pageStatsLabel = new System.Windows.Forms.Label();
            this.contourLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cvModeCheckbox = new System.Windows.Forms.CheckBox();
            this.vectorFeedControl = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.vectorCheckbox = new System.Windows.Forms.CheckBox();
            this.rasterFeedControl = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.materialComboBox = new System.Windows.Forms.ComboBox();
            this.rasterCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.filePreview = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dpiControl)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vectorFeedControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rasterFeedControl)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.filePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(6, 19);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(75, 23);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Open...";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // dpiControl
            // 
            this.dpiControl.Location = new System.Drawing.Point(35, 47);
            this.dpiControl.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.dpiControl.Name = "dpiControl";
            this.dpiControl.Size = new System.Drawing.Size(51, 20);
            this.dpiControl.TabIndex = 2;
            this.dpiControl.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.dpiControl.ValueChanged += new System.EventHandler(this.dpiControl_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "DPI:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pageStatsLabel);
            this.groupBox1.Controls.Add(this.contourLabel);
            this.groupBox1.Controls.Add(this.OpenButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 100);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source SVG File";
            // 
            // pageStatsLabel
            // 
            this.pageStatsLabel.AutoSize = true;
            this.pageStatsLabel.Location = new System.Drawing.Point(7, 72);
            this.pageStatsLabel.Name = "pageStatsLabel";
            this.pageStatsLabel.Size = new System.Drawing.Size(52, 13);
            this.pageStatsLabel.TabIndex = 2;
            this.pageStatsLabel.Text = "Contours:";
            // 
            // contourLabel
            // 
            this.contourLabel.AutoSize = true;
            this.contourLabel.Location = new System.Drawing.Point(7, 49);
            this.contourLabel.Name = "contourLabel";
            this.contourLabel.Size = new System.Drawing.Size(52, 13);
            this.contourLabel.TabIndex = 1;
            this.contourLabel.Text = "Contours:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cvModeCheckbox);
            this.groupBox2.Controls.Add(this.vectorFeedControl);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.vectorCheckbox);
            this.groupBox2.Controls.Add(this.rasterFeedControl);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.materialComboBox);
            this.groupBox2.Controls.Add(this.rasterCheckbox);
            this.groupBox2.Controls.Add(this.dpiControl);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 236);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CAM Settings";
            // 
            // cvModeCheckbox
            // 
            this.cvModeCheckbox.AutoSize = true;
            this.cvModeCheckbox.Location = new System.Drawing.Point(10, 194);
            this.cvModeCheckbox.Name = "cvModeCheckbox";
            this.cvModeCheckbox.Size = new System.Drawing.Size(137, 17);
            this.cvModeCheckbox.TabIndex = 9;
            this.cvModeCheckbox.Text = "Constant Velocity (G64)";
            this.cvModeCheckbox.UseVisualStyleBackColor = true;
            // 
            // vectorFeedControl
            // 
            this.vectorFeedControl.Location = new System.Drawing.Point(77, 168);
            this.vectorFeedControl.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.vectorFeedControl.Name = "vectorFeedControl";
            this.vectorFeedControl.Size = new System.Drawing.Size(54, 20);
            this.vectorFeedControl.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Vector Feed:";
            // 
            // vectorCheckbox
            // 
            this.vectorCheckbox.AutoSize = true;
            this.vectorCheckbox.Location = new System.Drawing.Point(6, 148);
            this.vectorCheckbox.Name = "vectorCheckbox";
            this.vectorCheckbox.Size = new System.Drawing.Size(57, 17);
            this.vectorCheckbox.TabIndex = 6;
            this.vectorCheckbox.Text = "Vector";
            this.vectorCheckbox.UseVisualStyleBackColor = true;
            // 
            // rasterFeedControl
            // 
            this.rasterFeedControl.Location = new System.Drawing.Point(77, 109);
            this.rasterFeedControl.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.rasterFeedControl.Name = "rasterFeedControl";
            this.rasterFeedControl.Size = new System.Drawing.Size(54, 20);
            this.rasterFeedControl.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Raster Feed:";
            // 
            // materialComboBox
            // 
            this.materialComboBox.FormattingEnabled = true;
            this.materialComboBox.Location = new System.Drawing.Point(7, 20);
            this.materialComboBox.Name = "materialComboBox";
            this.materialComboBox.Size = new System.Drawing.Size(121, 21);
            this.materialComboBox.TabIndex = 1;
            this.materialComboBox.SelectedIndexChanged += new System.EventHandler(this.materialComboBox_SelectedIndexChanged);
            // 
            // rasterCheckbox
            // 
            this.rasterCheckbox.AutoSize = true;
            this.rasterCheckbox.Location = new System.Drawing.Point(6, 89);
            this.rasterCheckbox.Name = "rasterCheckbox";
            this.rasterCheckbox.Size = new System.Drawing.Size(57, 17);
            this.rasterCheckbox.TabIndex = 0;
            this.rasterCheckbox.Text = "Raster";
            this.rasterCheckbox.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.progressBar);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Location = new System.Drawing.Point(12, 361);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(152, 83);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GCode Output";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(7, 49);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(139, 23);
            this.progressBar.TabIndex = 5;
            // 
            // filePreview
            // 
            this.filePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePreview.BackColor = System.Drawing.Color.White;
            this.filePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.filePreview.Location = new System.Drawing.Point(170, 12);
            this.filePreview.Name = "filePreview";
            this.filePreview.Size = new System.Drawing.Size(547, 479);
            this.filePreview.TabIndex = 1;
            this.filePreview.TabStop = false;
            // 
            // SVGProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 503);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.filePreview);
            this.Name = "SVGProcess";
            this.Text = "SVG Process";
            ((System.ComponentModel.ISupportInitialize)(this.dpiControl)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vectorFeedControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rasterFeedControl)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.filePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.PictureBox filePreview;
        private System.Windows.Forms.NumericUpDown dpiControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label pageStatsLabel;
        private System.Windows.Forms.Label contourLabel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox materialComboBox;
        private System.Windows.Forms.CheckBox rasterCheckbox;
        private System.Windows.Forms.NumericUpDown vectorFeedControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox vectorCheckbox;
        private System.Windows.Forms.NumericUpDown rasterFeedControl;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox cvModeCheckbox;
    }
}

