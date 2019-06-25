using System;
using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.XUnitExtenstions;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace QRCoderTests
{
   
    public class QRCodeRendererTests
    {      
      
        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_standard_qrcode_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10);

            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Bmp);
            var imgBytes = ms.ToArray();
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(imgBytes);
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();
            ms.Dispose();

            result.ShouldBe("41d3313c10d84034d67d476eec04163f");
        }
    }
}



