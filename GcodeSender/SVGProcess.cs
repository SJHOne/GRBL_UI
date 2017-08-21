// Copyright (c) 2010 Chris Yerga
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace GrblOutput
{
    public partial class SVGProcess : Form
    {
        string filePath = null;
        SVGDocument document = null;

        public SVGProcess()
        {
            InitializeComponent();

            contourLabel.Text = "";
            pageStatsLabel.Text = "";

            // Default to CV mode always
            cvModeCheckbox.Checked = true;

            // Dropdown for material selection. Edit feeds/speeds
            // in materialComboBox_SelectedIndexChanged()
            materialComboBox.Items.Add("Acrylic (1.5mm)");
            materialComboBox.Items.Add("Acrylic (3mm)");
            materialComboBox.Items.Add("Acrylic (6mm)");
            materialComboBox.Items.Add("Paper (100#)");
            materialComboBox.Items.Add("Wood (1/8\" Birch)");
            materialComboBox.Items.Add("Wood (1/4\" Birch)");
            materialComboBox.Items.Add("Aluminum (Laptop, iPad, etc.)");
            materialComboBox.SelectedIndex = 1;

            progressBar.Visible = false;


            button1.Enabled = false;
        }

        private string GetRegValue(string key, string defaultValue)
        {
            RegistryKey rootKey = Registry.CurrentUser.CreateSubKey(@"Software\atomsandelectrons\LaserCAM");
            return (string)rootKey.GetValue(key, defaultValue);
        }

        private void SetRegValue(string key, string value)
        {
            RegistryKey rootKey = Registry.CurrentUser.CreateSubKey(@"Software\atomsandelectrons\LaserCAM");
            rootKey.SetValue(key, value);
        }


        private void OpenButton_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog dialog = new OpenFileDialog();

                dialog.Filter = "SVG Drawing|*.svg";
                dialog.InitialDirectory = GetRegValue("LastDirectory", Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SetRegValue("LastDirectory", System.IO.Path.GetDirectoryName(dialog.FileName));
                    filePath = dialog.FileName;
                    LoadSVG(dialog.FileName);

                    UpdateStats();

                    button1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void UpdateStats()
        {
            int contourCount = 0;
            int pointCount = 0;
            float xmin = 99999.0f;
            float xmax = -99999.0f;
            float ymin = 99999.0f;
            float ymax = -99999.0f;

            var contours = document.GetVectorContours((double)dpiControl.Value);
            foreach (var contour in contours)
            {
                ++contourCount;
                foreach (var point in contour.Points)
                {
                    ++pointCount;
                }
            }

            foreach (var point in document.GetPoints())
            {
                if (point.X < xmin)
                {
                    xmin = point.X;
                }
                if (point.X > xmax)
                {
                    xmax = point.X;
                }
                if (point.Y < ymin)
                {
                    ymin = point.Y;
                }
                if (point.Y > ymax)
                {
                    ymax = point.Y;
                }
            }

            contourLabel.Text = string.Format("{0} contours and {1} points", contourCount, pointCount);
            pageStatsLabel.Text = string.Format("{0:0.00}in x {1:0.00}in", xmax - xmin, ymax - ymin);
        }

        void UpdatePreview()
        {
            Bitmap preview = new Bitmap(filePreview.Width, filePreview.Height);
            Graphics gc = Graphics.FromImage(preview);

            gc.ScaleTransform(50, 50);
            document.Render(gc, false);
            filePreview.Image = preview;

            filePreview.Invalidate();
        }

        private void LoadSVG(string path)
        {
            document = SVGDocument.LoadFromFile(path);
            UpdatePreview();
        }

        private void Progress(double progress)
        {
            System.Console.WriteLine("Progress: {0}", (int)(100.0 * progress));
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = (int)(100.0 * progress);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            string gcodePath = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "-gcode.txt");
            document.EmitGCode(gcodePath,
                true, (int)dpiControl.Value, (int)rasterFeedControl.Value,
                true, (int)dpiControl.Value, (int)vectorFeedControl.Value, cvModeCheckbox.Checked,
                Progress);
            progressBar.Visible = false;
        }

        private void dpiControl_ValueChanged(object sender, EventArgs e)
        {
            UpdateStats();
        }

        private void materialComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            rasterCheckbox.Checked = true;
            vectorCheckbox.Checked = true;

            switch (materialComboBox.SelectedIndex)
            {
                case 0: // 1.5mm acrylic
                    rasterFeedControl.Value = 300;
                    vectorFeedControl.Value = 30;
                    break;

                case 1: // 3mm acrylic
                    rasterFeedControl.Value = 300;
                    vectorFeedControl.Value = 15;
                    break;

                case 2: // 6mm acrylic
                    rasterFeedControl.Value = 300;
                    vectorFeedControl.Value = 6;
                    break;

                case 3: // 100# paper
                    rasterFeedControl.Value = 1000;
                    vectorFeedControl.Value = 150;
                    break;

                case 4: // 0.125" wood
                    rasterFeedControl.Value = 200;
                    vectorFeedControl.Value = 20;
                    break;

                case 5: // 0.25" wood
                    rasterFeedControl.Value = 1000;
                    vectorFeedControl.Value = 10;
                    break;

                case 6: // Anodized aluminum
                    rasterFeedControl.Value = 1000;
                    vectorFeedControl.Value = 10;
                    break;

            }

        }
    }
}

