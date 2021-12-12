#if NETFRAMEWORK || NETSTANDARD2_0 || NET5_0 || NET6_0_WINDOWS
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static QRCoder.Base64QRCode;
using static QRCoder.QRCodeGenerator;

namespace QRCoder
{
#if NET6_0_WINDOWS
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public class Base64QRCode : AbstractQRCode, IDisposable
    {
        private QRCode qr;

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public Base64QRCode() {
            qr = new QRCode();
        }

        public Base64QRCode(QRCodeData data) : base(data) {
            qr = new QRCode(data);
        }

        public override void SetQRCodeData(QRCodeData data) {
            this.qr.SetQRCodeData(data);
        }

        public string GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }


        public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            return this.GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones, imgType);
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Bitmap bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }
            return base64;
        }


        private string BitmapToBase64(Bitmap bmp, ImageType imgType)
        {
            var base64 = string.Empty;
            ImageFormat iFormat;
            switch (imgType) {
                case ImageType.Png:
                    iFormat = ImageFormat.Png;
                    break;
                case ImageType.Jpeg:
                    iFormat = ImageFormat.Jpeg;
                    break;
                case ImageType.Gif:
                    iFormat = ImageFormat.Gif;
                    break;
                default:
                    iFormat = ImageFormat.Png;
                    break;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bmp.Save(memoryStream, iFormat);
                base64 = Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
            }
            return base64;
        }

        public enum ImageType
        {
            Gif,
            Jpeg,
            Png
        }

    }

#if NET6_0_WINDOWS
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public static class Base64QRCodeHelper
    {
        public static string GetQRCode(string plainText, int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new Base64QRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColorHtmlHex, lightColorHtmlHex, drawQuietZones, imgType);
        }
    }
}

#endif