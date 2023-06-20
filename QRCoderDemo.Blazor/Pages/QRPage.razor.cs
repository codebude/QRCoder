using Microsoft.AspNetCore.Components;
using QRCoder.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRCoderDemo.Blazor.Pages
{
    public partial class QRPage : ComponentBase, IQRCodeView
    {
        private QRCodeAction action = new QRCodeAction();
        public ButtonClick GenerateClicked { get; set; }
        public SelectClick SelectClicked { get; set; }
        public SaveClick SaveClicked { get; set; }
        public ValueChange CodeChanged { get; set; }
        public ValueChange ECCLevelChanged { get; set; }
        public ColorChange PrimaryColorChanged { get; set; }
        public ColorChange BackgroundColorChanged { get; set; }
        public string QRCode { get; set; }
        private Image _QRCodeImage;
        public Image QRCodeImage
        {
            get => _QRCodeImage;
            set
            {
                _QRCodeImage = value;
                GenerateCode();
            }
        }
        public string QRCodeImage64 { get; set; }
        public Size QRCodeImageSize { get; set; }
        public int IconSize { get; set; }
        public string IconPath { get; set; }

        public Bitmap IconImage { get; set; }

        public int QRCodeImageSizeMode { get; set; }
        public string SelectedEccLevel { get; set; }
        public List<string> EccLevels { get; set; }
        public Color PrimaryColor { get; set; }
        public string HexPrimaryColor
        {
            get => System.Drawing.ColorTranslator.ToHtml(PrimaryColor);
            set => PrimaryColor = System.Drawing.ColorTranslator.FromHtml(value);
        }
        public Color BackgroundColor { get; set; }
        public string HexBackgroundColor
        {
            get => System.Drawing.ColorTranslator.ToHtml(BackgroundColor);
            set => BackgroundColor = System.Drawing.ColorTranslator.FromHtml(value);
        }

        public bool IsPC => false;

        public string GenerateButtonText { get; set; }
        public string SelectButtonText { get; set; }
        public string SaveButtonText { get; set; }
        public string EccLevelLabelText { get; set; }
        public string PrimaryColorLabelText { get; set; }
        public string BackgroundColorLabelText { get; set; }
        public string IconSizeLabelText { get; set; }
        public string IconLabelText { get; set; }


        protected override void OnInitialized()
        {
            action.Perform(this);
        }

        void BgColorChanged(ChangeEventArgs ar)
        {
            HexBackgroundColor = ar.Value.ToString();
            BackgroundColorChanged?.Invoke(BackgroundColor);
        }

        void PColorChanged(ChangeEventArgs ar)
        {
            HexPrimaryColor= ar.Value.ToString();
            PrimaryColorChanged?.Invoke(PrimaryColor);
        }

        void LevelChanged(ChangeEventArgs ar)
        {
            SelectedEccLevel = ar.Value.ToString();
            ECCLevelChanged?.Invoke(SelectedEccLevel);
        }

        void QRCodeChanged(ChangeEventArgs ar)
        {
            QRCode = ar.Value.ToString();
            CodeChanged?.Invoke(QRCode);
        }

        void GenerateCode()
        {
            using (MemoryStream outStream = new MemoryStream())
            {

                QRCodeImage.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
                outStream.Position = 0;

                QRCodeImage64 = "data:image/png;base64, " + Convert.ToBase64String(outStream.ToArray());
            }
            StateHasChanged();
        }

    }
}
