using QRCoder.SkiaSharp;
using QRCoderTests.Helpers;
using SkiaSharp;

namespace QRCoder.SkiaSharpTests
{
    [TestClass]
    public class SkiaSharpTests
    {
        [TestMethod]
        public void TestStandardQrCodeGeneration()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var qrcode = new SkiaSharpQRCode(data);
            using var image = qrcode.GetGraphic();
            using var imageData = image.Encode(SKEncodedImageFormat.Png, 80);
            using var stream = File.OpenWrite("qrcode_skia.png");

            imageData.SaveTo(stream);

        }

        [TestMethod]
        public void TestLogoQrCodeGeneration()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var qrcode = new SkiaSharpQRCode(data);

            var logoImagePath = HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png";

            using var logoStream = File.OpenRead(logoImagePath);
            using var logoImage = SKImage.FromEncodedData(logoStream);

            using var image = qrcode.GetGraphic(60, SKColors.Black,SKColors.White, SKColors.Red, logoImage, Models.LogoLocation.Center, Models.LogoBackgroundShape.Circle);
            using var imageData = image.Encode(SKEncodedImageFormat.Png, 80);
            using var stream = File.OpenWrite("qrcode_skia_logo.png");

            imageData.SaveTo(stream);
        }
    }
}