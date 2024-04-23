using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using static QRCoder.QRCodeGenerator;

namespace QRCoder.ImageSharp
{
    public class Base64QRCode : AbstractQRCode, IDisposable
    {
        private QRCode qr;

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public Base64QRCode()
        {
            qr = new QRCode();
        }

        public Base64QRCode(QRCodeData data)
            : base(data)
        {
            qr = new QRCode(data);
        }

        public override void SetQRCodeData(QRCodeData data)
        {
            qr.SetQRCodeData(data);
        }

        public string GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.Black, Color.White, true);
        }

        public string GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            return GetGraphic(pixelsPerModule, Color.Parse(darkColorHtmlHex), Color.Parse(lightColorHtmlHex), drawQuietZones, imgType);
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Image img = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, drawQuietZones))
            {
                base64 = BitmapToBase64(img, imgType);
            }

            return base64;
        }

        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Image icon, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            var base64 = string.Empty;
            using (Image bmp = qr.GetGraphic(pixelsPerModule, darkColor, lightColor, icon, iconSizePercent, iconBorderWidth, drawQuietZones))
            {
                base64 = BitmapToBase64(bmp, imgType);
            }

            return base64;
        }

        private string BitmapToBase64(Image img, ImageType imgType)
        {
            var base64 = string.Empty;
            IImageEncoder iFormat;
            switch (imgType)
            {
                default:
                case ImageType.Png:
                    iFormat = new SixLabors.ImageSharp.Formats.Png.PngEncoder();
                    break;
                case ImageType.Jpeg:
                    iFormat = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder();
                    break;
                case ImageType.Gif:
                    iFormat = new SixLabors.ImageSharp.Formats.Gif.GifEncoder();
                    break;
            }

            using (var memoryStream = new MemoryStream())
            {
                img.Save(memoryStream, iFormat);
                base64 = Convert.ToBase64String(memoryStream.ToArray(), Base64FormattingOptions.None);
            }

            return base64;
        }
    }

    public static class ImageSharpBase64QRCodeHelper
    {
        public static string GetQRCode(string plainText, int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, bool drawQuietZones = true, ImageType imgType = ImageType.Png)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new Base64QRCode(qrCodeData))
            {
                return qrCode.GetGraphic(pixelsPerModule, darkColorHtmlHex, lightColorHtmlHex, drawQuietZones, imgType);
            }
        }
    }
}
