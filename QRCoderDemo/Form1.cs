using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using QRCoder;

namespace QRCoderDemo;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        comboBoxECC.SelectedIndex = 0; //Pre-select ECC level "L"
        RenderQrCode();
    }

    private void buttonGenerate_Click(object sender, EventArgs e)
        => RenderQrCode();

    private void RenderQrCode()
    {
        string level = comboBoxECC.SelectedItem?.ToString() ?? "L";
        var eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
        using var qrGenerator = new QRCodeGenerator();
        using var qrCodeData = qrGenerator.CreateQrCode(textBoxQRCode.Text, eccLevel);
        using var qrCode = new QRCode(qrCodeData);
        pictureBoxQRCode.BackgroundImage = qrCode.GetGraphic(20, GetPrimaryColor(), GetBackgroundColor(),
            GetIconBitmap(), (int)iconSize.Value);

        pictureBoxQRCode.Size = new System.Drawing.Size(pictureBoxQRCode.Width, pictureBoxQRCode.Height);
        //Set the SizeMode to center the image.
        pictureBoxQRCode.SizeMode = PictureBoxSizeMode.CenterImage;

        pictureBoxQRCode.SizeMode = PictureBoxSizeMode.StretchImage;
    }

    private Bitmap? GetIconBitmap()
    {
        if (iconPath.Text.Length == 0)
        {
            return null;
        }
        try
        {
            return new Bitmap(iconPath.Text);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void selectIconBtn_Click(object sender, EventArgs e)
    {
        var openFileDlg = new OpenFileDialog
        {
            Title = "Select icon",
            Multiselect = false,
            CheckFileExists = true
        };
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


    private void btn_save_Click(object sender, EventArgs e)
    {

        // Displays a SaveFileDialog so the user can save the Image
        var saveFileDialog1 = new SaveFileDialog
        {
            Filter = "Bitmap Image|*.bmp|PNG Image|*.png|JPeg Image|*.jpg|Gif Image|*.gif",
            Title = "Save an Image File"
        };
        saveFileDialog1.ShowDialog();

        // If the file name is not an empty string open it for saving.
        if (saveFileDialog1.FileName != "")
        {
            // Saves the Image via a FileStream created by the OpenFile method.
            using var fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
            // Saves the Image in the appropriate ImageFormat based upon the
            // File type selected in the dialog box.
            // NOTE that the FilterIndex property is one-based.

            ImageFormat? imageFormat = saveFileDialog1.FilterIndex switch
            {
                1 => ImageFormat.Bmp,
                2 => ImageFormat.Png,
                3 => ImageFormat.Jpeg,
                4 => ImageFormat.Gif,
                _ => throw new NotSupportedException("File extension is not supported"),
            };
            pictureBoxQRCode.BackgroundImage.Save(fs, imageFormat);
        }
    }

    private void textBoxQRCode_TextChanged(object sender, EventArgs e)
        => RenderQrCode();

    private void comboBoxECC_SelectedIndexChanged(object sender, EventArgs e)
        => RenderQrCode();

    private void panelPreviewPrimaryColor_Click(object sender, EventArgs e)
    {
        if (colorDialogPrimaryColor.ShowDialog() == DialogResult.OK)
        {
            panelPreviewPrimaryColor.BackColor = colorDialogPrimaryColor.Color;
            RenderQrCode();
        }
    }

    private void panelPreviewBackgroundColor_Click(object sender, EventArgs e)
    {
        if (colorDialogBackgroundColor.ShowDialog() == DialogResult.OK)
        {
            panelPreviewBackgroundColor.BackColor = colorDialogBackgroundColor.Color;
            RenderQrCode();
        }
    }

    private Color GetPrimaryColor() => panelPreviewPrimaryColor.BackColor;

    private Color GetBackgroundColor() => panelPreviewBackgroundColor.BackColor;
}
