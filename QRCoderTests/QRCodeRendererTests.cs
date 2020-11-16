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
#endif


#if !NETCOREAPP1_1 && !NETCOREAPP2_0
        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_transparent_logo_graphic()
        {        
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: Resources.noun_software_engineer_2909346);
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("ee65d96c3013f6032b561cc768251eef");
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_non_transparent_logo_graphic()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: Resources.noun_software_engineer_2909346);
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("1d718f06f904af4a46748f02af2d4eec");
        }
#endif
    }
}



