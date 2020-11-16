using System;
using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.XUnitExtenstions;
using System.IO;
using System.Security.Cryptography;
#if !NETCOREAPP1_1
using System.Drawing;
#endif

namespace QRCoderTests
{

    public class QRCodeRendererTests
    {
#if !NETCOREAPP1_1
        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_standard_qrcode_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10);

            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("41d3313c10d84034d67d476eec04163f");
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_transparent_logo_graphic()
        {
            //Create dummy logo
            var logo = new Bitmap(32, 32);
            var gfx = Graphics.FromImage(logo);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, 32, 32));
            gfx.FillEllipse(Brushes.Red, new Rectangle(0, 0, 32, 32));
            gfx.Save();

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: logo);

            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("8f03d2c3fc5998cebb46658c3e6293b1");
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_non_transparent_logo_graphic()
        {
            //Create dummy logo
            var logo = new Bitmap(32, 32);
            var gfx = Graphics.FromImage(logo);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, 32, 32));
            gfx.FillEllipse(Brushes.Red, new Rectangle(0, 0, 32, 32));
            gfx.Save();

            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: logo);

            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("c2aae3658c7fa20cb5d22948d859c614");
        }
#endif
    }
}



