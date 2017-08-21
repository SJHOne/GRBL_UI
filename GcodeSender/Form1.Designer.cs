namespace GrblOutput {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.StartBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.StopBtn = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.rowsInFileLbl = new System.Windows.Forms.Label();
            this.sentRowsLbl = new System.Windows.Forms.Label();
            this.stopPrintBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ReloadBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rewBtn = new System.Windows.Forms.Button();
            this.speedOverrideNumber = new System.Windows.Forms.NumericUpDown();
            this.overrideSpeedChkbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.scrollOutputChkbox = new System.Windows.Forms.CheckBox();
            this.RenderPanel = new System.Windows.Forms.Panel();
            this.setHome = new System.Windows.Forms.Button();
            this.goHome = new System.Windows.Forms.Button();
            this.groupJog = new System.Windows.Forms.GroupBox();
            this.penDown = new System.Windows.Forms.Button();
            this.penUp = new System.Windows.Forms.Button();
            this.jogRight = new System.Windows.Forms.Button();
            this.jogLeft = new System.Windows.Forms.Button();
            this.jogDown = new System.Windows.Forms.Button();
            this.jogUp = new System.Windows.Forms.Button();
            this.btnProcessSVG = new System.Windows.Forms.Button();
            this.nJogAmount = new System.Windows.Forms.NumericUpDown();
            this.serialResponseList = new GrblOutput.FlickerFreeListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedOverrideNumber)).BeginInit();
            this.groupJog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nJogAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Location = new System.Drawing.Point(231, 32);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(75, 24);
            this.BrowseBtn.TabIndex = 0;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.Location = new System.Drawing.Point(102, 61);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(75, 24);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Connect";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 34);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(219, 20);
            this.textBox1.TabIndex = 2;
            // 
            // StopBtn
            // 
            this.StopBtn.Enabled = false;
            this.StopBtn.Location = new System.Drawing.Point(183, 61);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(75, 24);
            this.StopBtn.TabIndex = 5;
            this.StopBtn.Text = "Disconnect";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(6, 35);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(300, 20);
            this.textBox3.TabIndex = 7;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(6, 59);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(52, 24);
            this.PrintBtn.TabIndex = 8;
            this.PrintBtn.Text = "RUN";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(6, 62);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(62, 21);
            this.comboBox1.TabIndex = 10;
            // 
            // rowsInFileLbl
            // 
            this.rowsInFileLbl.AutoSize = true;
            this.rowsInFileLbl.Location = new System.Drawing.Point(112, 93);
            this.rowsInFileLbl.Name = "rowsInFileLbl";
            this.rowsInFileLbl.Size = new System.Drawing.Size(46, 13);
            this.rowsInFileLbl.TabIndex = 12;
            this.rowsInFileLbl.Text = "Rows: 0";
            // 
            // sentRowsLbl
            // 
            this.sentRowsLbl.AutoSize = true;
            this.sentRowsLbl.Location = new System.Drawing.Point(6, 93);
            this.sentRowsLbl.Name = "sentRowsLbl";
            this.sentRowsLbl.Size = new System.Drawing.Size(66, 13);
            this.sentRowsLbl.TabIndex = 13;
            this.sentRowsLbl.Text = "Sent rows: 0";
            // 
            // stopPrintBtn
            // 
            this.stopPrintBtn.Enabled = false;
            this.stopPrintBtn.Location = new System.Drawing.Point(64, 59);
            this.stopPrintBtn.Name = "stopPrintBtn";
            this.stopPrintBtn.Size = new System.Drawing.Size(52, 24);
            this.stopPrintBtn.TabIndex = 14;
            this.stopPrintBtn.Text = "STOP";
            this.stopPrintBtn.UseVisualStyleBackColor = true;
            this.stopPrintBtn.Click += new System.EventHandler(this.stopPrintBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.StartBtn);
            this.groupBox1.Controls.Add(this.StopBtn);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.ReloadBtn);
            this.groupBox1.Location = new System.Drawing.Point(529, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 91);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(262, 17);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(44, 17);
            this.radioButton2.TabIndex = 18;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "\\r\\n";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(212, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(44, 17);
            this.radioButton1.TabIndex = 17;
            this.radioButton1.Text = "\\n\\r";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Command";
            // 
            // ReloadBtn
            // 
            this.ReloadBtn.Image = global::GrblOutput.Properties.Resources.reload_icon1;
            this.ReloadBtn.Location = new System.Drawing.Point(73, 61);
            this.ReloadBtn.Name = "ReloadBtn";
            this.ReloadBtn.Size = new System.Drawing.Size(24, 24);
            this.ReloadBtn.TabIndex = 11;
            this.ReloadBtn.UseVisualStyleBackColor = true;
            this.ReloadBtn.Click += new System.EventHandler(this.ReloadBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rewBtn);
            this.groupBox2.Controls.Add(this.speedOverrideNumber);
            this.groupBox2.Controls.Add(this.overrideSpeedChkbox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.BrowseBtn);
            this.groupBox2.Controls.Add(this.PrintBtn);
            this.groupBox2.Controls.Add(this.stopPrintBtn);
            this.groupBox2.Controls.Add(this.rowsInFileLbl);
            this.groupBox2.Controls.Add(this.sentRowsLbl);
            this.groupBox2.Location = new System.Drawing.Point(529, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(313, 114);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File transfer";
            // 
            // rewBtn
            // 
            this.rewBtn.Enabled = false;
            this.rewBtn.Location = new System.Drawing.Point(122, 59);
            this.rewBtn.Name = "rewBtn";
            this.rewBtn.Size = new System.Drawing.Size(52, 24);
            this.rewBtn.TabIndex = 19;
            this.rewBtn.Text = "REW";
            this.rewBtn.UseVisualStyleBackColor = true;
            this.rewBtn.Click += new System.EventHandler(this.rewBtn_Click);
            // 
            // speedOverrideNumber
            // 
            this.speedOverrideNumber.Location = new System.Drawing.Point(208, 63);
            this.speedOverrideNumber.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.speedOverrideNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.speedOverrideNumber.Name = "speedOverrideNumber";
            this.speedOverrideNumber.Size = new System.Drawing.Size(98, 20);
            this.speedOverrideNumber.TabIndex = 18;
            this.speedOverrideNumber.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // overrideSpeedChkbox
            // 
            this.overrideSpeedChkbox.AutoSize = true;
            this.overrideSpeedChkbox.Location = new System.Drawing.Point(208, 89);
            this.overrideSpeedChkbox.Name = "overrideSpeedChkbox";
            this.overrideSpeedChkbox.Size = new System.Drawing.Size(98, 17);
            this.overrideSpeedChkbox.TabIndex = 17;
            this.overrideSpeedChkbox.Text = "Override speed";
            this.overrideSpeedChkbox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "GCode File";
            // 
            // scrollOutputChkbox
            // 
            this.scrollOutputChkbox.AutoSize = true;
            this.scrollOutputChkbox.Checked = true;
            this.scrollOutputChkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scrollOutputChkbox.Location = new System.Drawing.Point(717, 434);
            this.scrollOutputChkbox.Name = "scrollOutputChkbox";
            this.scrollOutputChkbox.Size = new System.Drawing.Size(124, 17);
            this.scrollOutputChkbox.TabIndex = 18;
            this.scrollOutputChkbox.Text = "Scroll output window";
            this.scrollOutputChkbox.UseVisualStyleBackColor = true;
            // 
            // RenderPanel
            // 
            this.RenderPanel.BackColor = System.Drawing.Color.White;
            this.RenderPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RenderPanel.Location = new System.Drawing.Point(12, 12);
            this.RenderPanel.Name = "RenderPanel";
            this.RenderPanel.Size = new System.Drawing.Size(511, 416);
            this.RenderPanel.TabIndex = 20;
            // 
            // setHome
            // 
            this.setHome.Location = new System.Drawing.Point(10, 135);
            this.setHome.Name = "setHome";
            this.setHome.Size = new System.Drawing.Size(132, 22);
            this.setHome.TabIndex = 21;
            this.setHome.Text = "SET HOME";
            this.setHome.UseVisualStyleBackColor = true;
            this.setHome.Click += new System.EventHandler(this.setHome_Click);
            // 
            // goHome
            // 
            this.goHome.Location = new System.Drawing.Point(10, 163);
            this.goHome.Name = "goHome";
            this.goHome.Size = new System.Drawing.Size(132, 22);
            this.goHome.TabIndex = 22;
            this.goHome.Text = "HOME";
            this.goHome.UseVisualStyleBackColor = true;
            this.goHome.Click += new System.EventHandler(this.goHome_Click);
            // 
            // groupJog
            // 
            this.groupJog.Controls.Add(this.nJogAmount);
            this.groupJog.Controls.Add(this.penDown);
            this.groupJog.Controls.Add(this.setHome);
            this.groupJog.Controls.Add(this.goHome);
            this.groupJog.Controls.Add(this.penUp);
            this.groupJog.Controls.Add(this.jogRight);
            this.groupJog.Controls.Add(this.jogLeft);
            this.groupJog.Controls.Add(this.jogDown);
            this.groupJog.Controls.Add(this.jogUp);
            this.groupJog.Location = new System.Drawing.Point(849, 12);
            this.groupJog.Name = "groupJog";
            this.groupJog.Size = new System.Drawing.Size(151, 198);
            this.groupJog.TabIndex = 23;
            this.groupJog.TabStop = false;
            this.groupJog.Text = "Jog Controls";
            // 
            // penDown
            // 
            this.penDown.Location = new System.Drawing.Point(74, 105);
            this.penDown.Name = "penDown";
            this.penDown.Size = new System.Drawing.Size(71, 24);
            this.penDown.TabIndex = 7;
            this.penDown.Text = "Pen Down";
            this.penDown.UseVisualStyleBackColor = true;
            this.penDown.Click += new System.EventHandler(this.penDown_Click);
            // 
            // penUp
            // 
            this.penUp.Location = new System.Drawing.Point(10, 104);
            this.penUp.Name = "penUp";
            this.penUp.Size = new System.Drawing.Size(55, 24);
            this.penUp.TabIndex = 6;
            this.penUp.Text = "Pen Up";
            this.penUp.UseVisualStyleBackColor = true;
            this.penUp.Click += new System.EventHandler(this.penUp_Click);
            // 
            // jogRight
            // 
            this.jogRight.Location = new System.Drawing.Point(102, 47);
            this.jogRight.Name = "jogRight";
            this.jogRight.Size = new System.Drawing.Size(40, 24);
            this.jogRight.TabIndex = 5;
            this.jogRight.Text = "X+";
            this.jogRight.UseVisualStyleBackColor = true;
            this.jogRight.Click += new System.EventHandler(this.jogRight_Click);
            // 
            // jogLeft
            // 
            this.jogLeft.Location = new System.Drawing.Point(10, 47);
            this.jogLeft.Name = "jogLeft";
            this.jogLeft.Size = new System.Drawing.Size(40, 24);
            this.jogLeft.TabIndex = 4;
            this.jogLeft.Text = "X-";
            this.jogLeft.UseVisualStyleBackColor = true;
            this.jogLeft.Click += new System.EventHandler(this.jogLeft_Click);
            // 
            // jogDown
            // 
            this.jogDown.Location = new System.Drawing.Point(56, 74);
            this.jogDown.Name = "jogDown";
            this.jogDown.Size = new System.Drawing.Size(40, 24);
            this.jogDown.TabIndex = 3;
            this.jogDown.Text = "Y-";
            this.jogDown.UseVisualStyleBackColor = true;
            this.jogDown.Click += new System.EventHandler(this.jogDown_Click);
            // 
            // jogUp
            // 
            this.jogUp.Location = new System.Drawing.Point(56, 18);
            this.jogUp.Name = "jogUp";
            this.jogUp.Size = new System.Drawing.Size(40, 24);
            this.jogUp.TabIndex = 2;
            this.jogUp.Text = "Y+";
            this.jogUp.UseVisualStyleBackColor = true;
            this.jogUp.Click += new System.EventHandler(this.jogUp_Click);
            // 
            // btnProcessSVG
            // 
            this.btnProcessSVG.Location = new System.Drawing.Point(855, 216);
            this.btnProcessSVG.Name = "btnProcessSVG";
            this.btnProcessSVG.Size = new System.Drawing.Size(145, 23);
            this.btnProcessSVG.TabIndex = 24;
            this.btnProcessSVG.Text = "Process SVG File...";
            this.btnProcessSVG.UseVisualStyleBackColor = true;
            this.btnProcessSVG.Click += new System.EventHandler(this.btnProcessSVG_Click);
            // 
            // nJogAmount
            // 
            this.nJogAmount.Location = new System.Drawing.Point(56, 48);
            this.nJogAmount.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nJogAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nJogAmount.Name = "nJogAmount";
            this.nJogAmount.Size = new System.Drawing.Size(40, 20);
            this.nJogAmount.TabIndex = 20;
            this.nJogAmount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // serialResponseList
            // 
            this.serialResponseList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.serialResponseList.FormattingEnabled = true;
            this.serialResponseList.Location = new System.Drawing.Point(529, 229);
            this.serialResponseList.Name = "serialResponseList";
            this.serialResponseList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.serialResponseList.Size = new System.Drawing.Size(313, 199);
            this.serialResponseList.TabIndex = 15;
            this.serialResponseList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.serialResponseList_DrawItem);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 458);
            this.Controls.Add(this.btnProcessSVG);
            this.Controls.Add(this.groupJog);
            this.Controls.Add(this.RenderPanel);
            this.Controls.Add(this.scrollOutputChkbox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.serialResponseList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "EleksDraw G-Code Sender";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speedOverrideNumber)).EndInit();
            this.groupJog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nJogAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.IO.Ports.SerialPort serialPort1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Button BrowseBtn;
		private System.Windows.Forms.Button StartBtn;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button StopBtn;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Button PrintBtn;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button ReloadBtn;
		private System.Windows.Forms.Label rowsInFileLbl;
		private System.Windows.Forms.Label sentRowsLbl;
		private System.Windows.Forms.Button stopPrintBtn;
		private FlickerFreeListBox serialResponseList;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.CheckBox scrollOutputChkbox;
		private System.Windows.Forms.CheckBox overrideSpeedChkbox;
		private System.Windows.Forms.NumericUpDown speedOverrideNumber;
        private System.Windows.Forms.Panel RenderPanel;
        private System.Windows.Forms.Button setHome;
        private System.Windows.Forms.Button goHome;
        private System.Windows.Forms.Button rewBtn;
        private System.Windows.Forms.GroupBox groupJog;
        private System.Windows.Forms.Button penDown;
        private System.Windows.Forms.Button penUp;
        private System.Windows.Forms.Button jogRight;
        private System.Windows.Forms.Button jogLeft;
        private System.Windows.Forms.Button jogDown;
        private System.Windows.Forms.Button jogUp;
        private System.Windows.Forms.Button btnProcessSVG;
        private System.Windows.Forms.NumericUpDown nJogAmount;
    }
}

