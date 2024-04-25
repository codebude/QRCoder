using QRCoder.Models;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCoder.SkiaSharp.Extensions
{
    public static class SaveExtensions
    {
        /// <summary>
        /// Saves a png of the qr code
        /// </summary>
        /// <param name="skiaSharpQRCode"></param>
        /// <param name="quality"></param>
        public static void SaveToPng(this SkiaSharpQRCode skiaSharpQRCode, string path, int quality = 80)
        {
            using var image = skiaSharpQRCode.GetGraphic();
            using var imageData = image.Encode(SKEncodedImageFormat.Png, quality);
            using var stream = File.OpenWrite(path);
            imageData.SaveTo(stream);
        }

        public static void SaveToPng(this SkiaSharpQRCode skiaSharpQRCode, Stream stream, int quality, int pixelsPerModule, SKImage? logoImage = null, LogoLocation logoLocation = LogoLocation.BottomRight, LogoBackgroundShape logoBackgroundShape = LogoBackgroundShape.Circle) => SaveToPng(skiaSharpQRCode, stream, quality, pixelsPerModule, SKColors.Black, SKColors.White, SKColors.White, logoImage, logoLocation, logoBackgroundShape);

        public static void SaveToPng(this SkiaSharpQRCode skiaSharpQRCode, Stream stream, int quality, int pixelsPerModule, SKColor darkColor, SKColor lightColor, SKColor backgroundColor, SKImage? logoImage = null, LogoLocation logoLocation = LogoLocation.BottomRight, LogoBackgroundShape logoBackgroundShape = LogoBackgroundShape.Circle, bool drawQuietZones = true)
        {
            using var image = skiaSharpQRCode.GetGraphic(pixelsPerModule, darkColor, lightColor, backgroundColor, logoImage, logoLocation, logoBackgroundShape, drawQuietZones: drawQuietZones);
            using var imageData = image.Encode(SKEncodedImageFormat.Png, quality);
            imageData.SaveTo(stream);
            stream.Position = 0;
        }
    }
}
