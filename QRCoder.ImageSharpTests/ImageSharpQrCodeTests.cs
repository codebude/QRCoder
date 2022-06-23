using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRCoder.ImageSharp;
using QRCoder.Models;
using QRCoderTests.Helpers;
using SixLabors.ImageSharp;

namespace QRCoder.ImageSharpTests
{
    [TestClass]
    public class ImageSharpQrCodeTests
    {
        [TestMethod]
        public void TestRegularQrCode()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var qrcode = new ImageSharpQRCode(data);
            var image = qrcode.GetGraphic();
            image.SaveAsPng("imageSharpQr.png");
        }

        [TestMethod]
        public void TestLogoQrCode()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var qrcode = new ImageSharpQRCode(data);
            var image = qrcode.GetGraphic(60, Image.Load(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"), LogoLocation.Center, LogoBackgroundShape.Circle);
            image.SaveAsPng("imageSharpQr_logo.png");
        }
    }
}
