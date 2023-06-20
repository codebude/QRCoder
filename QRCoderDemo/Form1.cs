using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using QRCoder.UI;

namespace QRCoderDemo
{
    public partial class Form1 : Form, IQRCodeView
    {
        public Form1()
        {
            InitializeComponent();

        }

        #region     fields
        private readonly QRCodeAction action = new QRCodeAction();
        #endregion

        #region events
        public ButtonClick GenerateClicked { get; set; }
        public SelectClick SelectClicked { get; set; }
        public SaveClick SaveClicked { get; set; }
        public ValueChange CodeChanged { get; set; }
        public ValueChange ECCLevelChanged { get; set; }
        public ColorChange PrimaryColorChanged { get; set; }
        public ColorChange BackgroundColorChanged { get; set; }
        #endregion

        #region props
        public string QRCode { get => textBoxQRCode.Text; set => textBoxQRCode.Text = value; }
        public Image QRCodeImage { get => pictureBoxQRCode.BackgroundImage; set => pictureBoxQRCode.BackgroundImage = value; }
        public Size QRCodeImageSize { get => pictureBoxQRCode.Size; set => pictureBoxQRCode.Size = value; }
        public int QRCodeImageSizeMode { get => (int)pictureBoxQRCode.SizeMode; set => pictureBoxQRCode.SizeMode = (PictureBoxSizeMode)value; }
        public int IconSize { get => (int)iconSize.Value; set => iconSize.Value = value; }
        public string IconPath { get => iconPath.Text; set => iconPath.Text = value; }
        public Bitmap IconImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IconPath))
                    return null;
                return new Bitmap(IconPath);
            }
        }

        public string SelectedEccLevel { get => comboBoxECC.SelectedItem as string; set => comboBoxECC.SelectedText = value; }
        public List<string> EccLevels { get => (comboBoxECC.DataSource as List<string>); set => comboBoxECC.DataSource = value; }
        public Color PrimaryColor { get => panelPreviewPrimaryColor.BackColor; set => panelPreviewPrimaryColor.BackColor = value; }
        public Color BackgroundColor { get => panelPreviewBackgroundColor.BackColor; set => panelPreviewBackgroundColor.BackColor = value; }

        public bool IsPC => true;

        #region props -> Texts
        public string GenerateButtonText { set => buttonGenerate.Text = value; }
        public string SelectButtonText { set => selectIconBtn.Text = value; }
        public string SaveButtonText { set => buttonSave.Text = value; }
        public string EccLevelLabelText { set => labelECC.Text = value; }
        public string PrimaryColorLabelText { set => labelPreviewPrimaryColor.Text = value; }
        public string BackgroundColorLabelText { set => labelPreviewBackgroundColor.Text = value; }
        public string IconSizeLabelText { set => labelIconsize.Text = value; }
        public string IconLabelText { set => labelIcon.Text = value; }
        #endregion
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            action.Perform(this);
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            GenerateClicked?.Invoke();
        }


        private void selectIconBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Title = "Select icon";
            openFileDlg.Multiselect = false;
            openFileDlg.CheckFileExists = true;
            if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectClicked?.Invoke(openFileDlg.FileName);
            }
            else
            {
                SelectClicked?.Invoke(string.Empty);
            }
        }


        private void btn_save_Click(object sender, EventArgs e)
        {

            // Displays a SaveFileDialog so the user can save the Image
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Bitmap Image|*.bmp|PNG Image|*.png|JPeg Image|*.jpg|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                SaveClicked?.Invoke(saveFileDialog1.FileName);
            }
        }



        private void textBoxQRCode_TextChanged(object sender, EventArgs e)
        {
            CodeChanged?.Invoke(QRCode);
        }

        private void comboBoxECC_SelectedIndexChanged(object sender, EventArgs e)
        {
            ECCLevelChanged?.Invoke(SelectedEccLevel);
        }

        private void panelPreviewPrimaryColor_Click(object sender, EventArgs e)
        {
            if (colorDialogPrimaryColor.ShowDialog() == DialogResult.OK)
            {
                PrimaryColor = colorDialogPrimaryColor.Color;
                PrimaryColorChanged?.Invoke(PrimaryColor);
            }
        }

        private void panelPreviewBackgroundColor_Click(object sender, EventArgs e)
        {
            if (colorDialogBackgroundColor.ShowDialog() == DialogResult.OK)
            {
                BackgroundColor = colorDialogBackgroundColor.Color;
                BackgroundColorChanged?.Invoke(BackgroundColor);
            }
        }

    }
}
