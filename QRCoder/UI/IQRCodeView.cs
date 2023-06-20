using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace QRCoder.UI
{
    public delegate void ButtonClick();
    public delegate void SelectClick(string filename);
    public delegate void SaveClick(string filename);
    public delegate void ValueChange(string newValue);
    public delegate void ColorChange(Color newColor);
    public interface IQRCodeView
    {
        ButtonClick GenerateClicked { get; set; }
        SelectClick SelectClicked { get; set; }
        SaveClick SaveClicked { get; set; }
        ValueChange CodeChanged { get; set; }
        ValueChange ECCLevelChanged { get; set; }
        ColorChange PrimaryColorChanged { get; set; }
        ColorChange BackgroundColorChanged { get; set; }

        string QRCode { get; set; }
        Image QRCodeImage { get; set; }
        Size QRCodeImageSize { get; set; }
        int IconSize { get; set; }
        string IconPath { get; set; }
        Bitmap IconImage { get; }
        int QRCodeImageSizeMode { get; set; }

        string SelectedEccLevel { get; set; }
        List<string> EccLevels { get; set; }

        Color PrimaryColor { get; set; }

        Color BackgroundColor { get; set; }

        bool IsPC { get; }
        // Texts

        string GenerateButtonText { set; }
        string SelectButtonText { set; }
        string SaveButtonText { set; }
        string EccLevelLabelText { set; }
        string PrimaryColorLabelText { set; }
        string BackgroundColorLabelText { set; }
        string IconSizeLabelText { set; }
        string IconLabelText { set; }



    }
}
