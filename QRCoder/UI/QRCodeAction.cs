using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QRCoder.UI
{
    public class QRCodeAction : IQRCodeAction
    {

        public void Perform(IQRCodeView view)
        {
            _view = view;

            _view.QRCode = "1234567890"; // Default value
            _view.GenerateButtonText = "Generate";
            _view.SelectedEccLevel = "L";
            _view.EccLevelLabelText = "ECC-Level";
            _view.EccLevels = new System.Collections.Generic.List<string>() { "L", "M", "Q", "H" };
            _view.PrimaryColor = Color.Black;
            _view.PrimaryColorLabelText = "Primary color";
            _view.BackgroundColor = Color.White;
            _view.BackgroundColorLabelText = "Background color";
            _view.IconLabelText = "Icon";
            _view.IconSizeLabelText = "Icon size";

            _view.SaveButtonText = "Save QR Code";
            _view.SelectButtonText = "Select";


            // event Handlers
            if (_view.SaveClicked == null)
            {
                _view.SaveClicked = new SaveClick(OnSaveClicked);
                _view.GenerateClicked = new ButtonClick(OnGenerateClicked);
                _view.SelectClicked = new SelectClick(OnSelectClicked);
                _view.CodeChanged = new ValueChange(OnCodeChanged);
                _view.ECCLevelChanged = new ValueChange(OnEccChanged);
                _view.PrimaryColorChanged = new ColorChange(OnPCChanged);
                _view.BackgroundColorChanged = new ColorChange(OnBGChanged);
            }

            RenderQrCode();

        }

      




        #region fields
        private IQRCodeView _view;
        #endregion

        #region methods
        private void RenderQrCode()
        {
            string level = _view.SelectedEccLevel;
            QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(_view.QRCode, eccLevel))
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                _view.QRCodeImage = qrCode.GetGraphic(20, _view.PrimaryColor, _view.BackgroundColor,
                    _view.IconImage, _view.IconSize);

                _view.QRCodeImageSize = new System.Drawing.Size(462, 418);
                //Set the SizeMode to center the image.
                // this.pictureBoxQRCode.SizeMode = PictureBoxSizeMode.CenterImage;

                _view.QRCodeImageSizeMode = 1;//StretchImage;
            }
        }
        #endregion

        #region events

        private void OnEccChanged(string newValue)
        {
            RenderQrCode();
        }

        private void OnBGChanged(Color newColor)
        {
            RenderQrCode();
        }

        private void OnPCChanged(Color newColor)
        {
            RenderQrCode();
        }


        private void OnCodeChanged(string newCode)
        {
            RenderQrCode();
        }

        private void OnSaveClicked(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            // Saves the Image via a FileStream created by the OpenFile method.
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.

                ImageFormat imageFormat = null;
                string imageExt = Path.GetExtension(filename).ToLower();
                switch(imageExt)
                {
                   case ".bmp"  :
                        imageFormat = ImageFormat.Bmp;
                        break;
                   case ".png"  :
                        imageFormat = ImageFormat.Png;
                        break;
                    case ".jpeg"  :
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    default:
                        throw new NotSupportedException("File extension is not supported");
                };
                 
                _view.QRCodeImage.Save(fs, imageFormat);
            }
        }

        private void OnSelectClicked(string filename)
        {
            _view.IconPath = filename;
            if (_view.IconSize == 0)
            {
                _view.IconSize = 15;
            }
        }

        private void OnGenerateClicked()
        {
            RenderQrCode();
        }


        #endregion
    }
}
