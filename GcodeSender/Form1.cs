using Druid.Viewer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GrblOutput
{
    public partial class Form1 : Form
    {
		string RxString;
		List<string> lines = new List<string>();
		int CurrentRow = 0;
		bool transfer = false;


        private ViewerDevice mViewDevice;
        //private Canvas m2dTargetControl;
        private Viewer3d m3dTargetControl;

        public Form1()
        {
			InitializeComponent();

			System.Windows.Forms.Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			AppDomain currentDomain = AppDomain.CurrentDomain;
			currentDomain.UnhandledException += new UnhandledExceptionEventHandler(Application_UnhandledException);

			disableControlsForPrinting();
			stopPrintBtn.Enabled = false;

            Setup3dViewer();
            
            // Try to reduce flicker
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;
        }

        private void Setup3dViewer()
        {
            m3dTargetControl = new Viewer3d();
            m3dTargetControl.Dock = DockStyle.Fill;
            m3dTargetControl.BackColor = RenderPanel.BackColor;
            this.RenderPanel.Controls.Add(m3dTargetControl);

            mViewDevice = new Viewer3dDevice(m3dTargetControl);
            mViewDevice.Initialize();
            mViewDevice.CodeChanged += new EventHandler(mDevice_CodeChanged);

            gcodeparser.DeviceFactory.RegisterDevice(mViewDevice);
        }

        private void mDevice_CodeChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
			Exception ex = e.Exception;
			MessageBox.Show(ex.Message, "Thread exception");
		}

		private static void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
			if (e.ExceptionObject != null) {
				Exception ex = (Exception)e.ExceptionObject;
				MessageBox.Show(ex.Message, "Application exception");
			}
		}

		private void BrowseBtn_Click(object sender, EventArgs e)
        {
			openFileDialog1.Filter = "G-code Files(*.CNC;*.NC;*.TAP;*.TXT)|*.CNC;*.NC;*.TAP;*.TXT|All files (*.*)|*.*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = openFileDialog1.FileName;

				if(File.Exists(textBox1.Text))
                {
                    mViewDevice.Clear();

                    using (StreamReader r = new StreamReader(textBox1.Text))
                    {
                        string line = String.Empty;
                        int rowCounter = 0;
                        while ((line = r.ReadLine()) != null)
                        {
                            if (line.Trim() != "")
                            {
                                gcodeparser.GCodeParser.ParseLine(line);
                                rowCounter++;
                            }
                        }

                        serialResponseList.Items.Clear();

                        mViewDevice.CurrentCodeLineIndex = 0;

                        SetupJob();

                        RenderPanel.Invalidate();

                        rowsInFileLbl.Text = "Rows: " + rowCounter.ToString();
                    }
				}

			}
		}

        private void SetupJob()
        {
            float distance = mViewDevice.GetTotalDistance();
            float mmPerMinute = ViewerDevice.MmPerMinute;

            //TotalDistanceTextbox.Text = string.Format("{0:0.0} mm", distance);
            //EstimatedTimeTextbox.Text = string.Format("{0:0.0} minutes", distance / mmPerMinute);
        }
        private void buttonStart_Click(object sender, EventArgs e) {
			serialPort1.PortName = comboBox1.SelectedItem.ToString();
			serialPort1.BaudRate = 115200;

			serialPort1.Open();
			if (serialPort1.IsOpen) {
				comboBox1.Enabled = false;
				ReloadBtn.Enabled = false;
				StartBtn.Enabled = false;
				StopBtn.Enabled = true;
				textBox3.ReadOnly = false;
				enableControlsForPrinting();
			}
		}

		private void buttonStop_Click(object sender, EventArgs e)
        {
			if (serialPort1.IsOpen) {
				serialPort1.Close();
				comboBox1.Enabled = true;
				ReloadBtn.Enabled = true;
				StartBtn.Enabled = true;
				StopBtn.Enabled = false;
				textBox3.ReadOnly = true;
				transfer = false;
				disableControlsForPrinting();
				BrowseBtn.Enabled = true;
				stopPrintBtn.Enabled = false;
			}

		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
			if (serialPort1.IsOpen) serialPort1.Close();
		}

		private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
			if (e.KeyChar == (char)13)
            {
				if (!serialPort1.IsOpen) return;
				textBox3.Text += "\r\n";
				char[] buff = new char[textBox3.Text.Length];
				buff = textBox3.Text.ToCharArray();
				serialPort1.Write(buff, 0, buff.Count());
				textBox3.Text = "";
			}
		}

		private void DisplayText(object sender, EventArgs e)
        {
			string[] arr = RxString.Replace("\n\r", "\r\n").Replace("\r\n", "\n").Trim('\n').Split('\n');
			foreach (string item in arr)
            {
				if (item != "ok")
                {
					serialResponseList.Items.Add(item);
					serialResponseList.TopIndex = serialResponseList.Items.Count - 1;
				}
			}
		}

		private void DisplayCurrentRow(object sender, EventArgs e)
        {
            serialResponseList.BeginUpdate();

            serialResponseList.Items.Add(lines[CurrentRow-1]);
			if(scrollOutputChkbox.Checked)
				serialResponseList.TopIndex = serialResponseList.Items.Count - 1;
			sentRowsLbl.Text = "Sent rows: " + CurrentRow.ToString();

            serialResponseList.EndUpdate();
        }

		private void PrintDone(object sender, EventArgs e)
        {
			MessageBox.Show("Yeay, all the G-code was sent to Grbl.", "Done...", MessageBoxButtons.OK, MessageBoxIcon.Information);
			transfer = false;
			enableControlsForPrinting();
		}

		private void serialPort1_DataReceived
		  (object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
			RxString += serialPort1.ReadExisting();
			if ((RxString.EndsWith("\r\n") && radioButton2.Checked) || (RxString.EndsWith("\n\r") && radioButton1.Checked)) {				// NORMAL: \n\r
				this.Invoke(new EventHandler(DisplayText));
				RxString = String.Empty;
				if (transfer)
					sendNextLine();
			}
		}

		private void sendNextLine()
        {
			try
            {
				if(serialPort1.IsOpen)
                {
					if (CurrentRow < lines.Count)
                    {
						// Check if we need to override and F command. This is a little rude, we just replaces first F in line...
						if (lines[CurrentRow].ToLower().IndexOf('f') > -1 && overrideSpeedChkbox.Checked) {
							// Yea, this can be done a lot better.
							int startPos = lines[CurrentRow].ToLower().IndexOf('f') + 1;
							string firstPart = lines[CurrentRow].Substring(0, startPos);
							string lastPart = lines[CurrentRow].Substring(startPos);
							lastPart = Regex.Replace(lastPart, "^\\d+(.\\d*)", speedOverrideNumber.Value.ToString());
							lines[CurrentRow] = firstPart + lastPart;
						}

                        serialPort1.WriteLine(lines[CurrentRow]);

                        mViewDevice.CurrentCodeLineIndex = CurrentRow;

						CurrentRow++;

                        this.Invoke(new EventHandler(DisplayCurrentRow));
					}
                    else
                    {
						transfer = false;
						this.Invoke(new EventHandler(PrintDone));
					}
				}
                else
                {
					MessageBox.Show("Port not open.");
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void PrintBtn_Click(object sender, EventArgs e)
        {
			if (File.Exists(textBox1.Text)) {
                using (StreamReader r = new StreamReader(textBox1.Text))
                {
                    disableControlsForPrinting();
                    lines = new List<string>();
                    CurrentRow = 0;
                    transfer = true;

                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        if (line.Trim() != "")
                            lines.Add(line.Trim());
                    }
                    rowsInFileLbl.Text = "Rows: " + lines.Count.ToString();
                    sendNextLine();
                }
			}
		}

		private void enableControlsForPrinting()
        {
			stopPrintBtn.Enabled = false;
            rewBtn.Enabled = true;
			PrintBtn.Enabled = true;
			BrowseBtn.Enabled = true;
			overrideSpeedChkbox.Enabled = true;
			speedOverrideNumber.Enabled = true;
			textBox1.Enabled = true;

            groupJog.Enabled = true;
        }

		private void disableControlsForPrinting()
        {
			PrintBtn.Enabled = false;
            rewBtn.Enabled = false;
            BrowseBtn.Enabled = false;
			overrideSpeedChkbox.Enabled = false;
			speedOverrideNumber.Enabled = false;
			stopPrintBtn.Enabled = true;
			textBox1.Enabled = false;

            groupJog.Enabled = false;

		}

		private void loadPortSelector()
        {
			List<String> tList = new List<String>();

			comboBox1.Items.Clear();

			foreach (string s in SerialPort.GetPortNames())
            {
				tList.Add(s);
			}

			if (tList.Count < 1)
            {
				MessageBox.Show("No serial ports found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
            else
            {
				tList.Sort();
				comboBox1.Items.AddRange(tList.ToArray());
				comboBox1.SelectedIndex = 0;
			}
		}

		private void Form1_Load(object sender, EventArgs e)
        {
			loadPortSelector();
		}

		private void ReloadBtn_Click(object sender, EventArgs e)
        {
			loadPortSelector();
		}

		private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
			try {
				if (serialPort1.IsOpen)
					serialPort1.Close();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void stopPrintBtn_Click(object sender, EventArgs e)
        {
			transfer = false;
			enableControlsForPrinting();
			serialPort1.WriteLine("M5");
			serialPort1.WriteLine("G0 X0 Y0");
		}

		private void serialResponseList_DrawItem(object sender, DrawItemEventArgs e)
        {
			if (e.Index > -1)
			{
				Color C = Color.White;
				if (serialResponseList.Items[e.Index].ToString().Contains("error:"))
					C = Color.LightPink;
				else if (serialResponseList.Items[e.Index].ToString().Contains("Grbl") || serialResponseList.Items[e.Index].ToString().Contains('$'))
					C = Color.LightSkyBlue;

				e.DrawBackground();
				Graphics g = e.Graphics;

				// draw the background color you want
				g.FillRectangle(new SolidBrush(C), e.Bounds);

				// draw the text of the list item, not doing this will only show the background color
				g.DrawString(serialResponseList.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));

				e.DrawFocusRectangle();
			}
		}

        private void rewBtn_Click(object sender, EventArgs e)
        {
            CurrentRow = 0;
            serialResponseList.Items.Clear();

            sentRowsLbl.Text = "Sent rows: 0";
            mViewDevice.CurrentCodeLineIndex = 0;
            SetupJob();
            RenderPanel.Invalidate();
        }

        private void btnProcessSVG_Click(object sender, EventArgs e)
        {
            SVGProcess processForm = new SVGProcess();
            processForm.ShowDialog();
        }

        private void jogUp_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("F1000");
            serialPort1.WriteLine("G91");
            serialPort1.WriteLine("G01Y" + nJogAmount.Value.ToString());
            serialPort1.WriteLine("G90");
        }

        private void jogDown_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("F1000");
            serialPort1.WriteLine("G91");
            serialPort1.WriteLine("G01Y-" + nJogAmount.Value.ToString());
            serialPort1.WriteLine("G90");
        }

        private void jogLeft_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("F1000");
            serialPort1.WriteLine("G91");
            serialPort1.WriteLine("G01X-" + nJogAmount.Value.ToString());
            serialPort1.WriteLine("G90");
        }

        private void jogRight_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("F1000");
            serialPort1.WriteLine("G91");
            serialPort1.WriteLine("G01X" + nJogAmount.Value.ToString());
            serialPort1.WriteLine("G90");
        }

        private void penUp_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("M03S0");
        }

        private void goHome_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("S0");
            serialPort1.WriteLine("M5");
            serialPort1.WriteLine("G0 X0 Y0");
        }

        private void penDown_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("M03S50");
        }

        private void setHome_Click(object sender, EventArgs e)
        {
            serialPort1.WriteLine("G92 X0 Y0 Z0");
        }
    }
}
