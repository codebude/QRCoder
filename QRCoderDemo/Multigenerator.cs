using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QRCoderDemo
{
    using QRCoder;

    public partial class Multigenerator : Form
    {
        public Multigenerator()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxECC.SelectedIndex = 0; //Pre-select ECC level "L"
            renderQRCode();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            renderQRCode();
        }

        private void renderQRCode()
        {
            string level = comboBoxECC.SelectedItem.ToString();
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator()){
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(textBoxQRCode.Text, eccLevel)){
            using (QRCode qrCode = new QRCode(qrCodeData)){
            var g=qrCode.GetGraphic(20, Color.Black, Color.White, getIconBitmap(), (int)iconSize.Value);
            this.imageList1.Images.Add(g);
            this.listView1.Refresh();
			}
			}
            }
        }
        
        private Bitmap getIconBitmap()
        {
            Bitmap img = null;
            if (iconPath.Text.Length > 0)
            {
                try
                {
                    img = new Bitmap(iconPath.Text);
                }
                catch (Exception) 
                { 
                }
            }
            return img;
        }

        private void selectIconBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Title = "Select icon";
            openFileDlg.Multiselect = false;
            openFileDlg.CheckFileExists = true;
            if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                iconPath.Text = openFileDlg.FileName;
                if (iconSize.Value == 0)
                {
                    iconSize.Value = 15;
                }
            }
            else
            {
                iconPath.Text = "";
            }
        }

        private void textBoxQRCode_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (this.textBoxQRCode.Text == "")
                this.textBoxQRCode.Text = 1.ToString();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox1_TextChanged(object sender, KeyPressEventArgs e)
        {
            if (this.textBox1.Text == "")
                this.textBox1.Text = 1.ToString();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
