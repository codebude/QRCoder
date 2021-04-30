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

    public class SvgQRCodeRendererTests
    {


#if !NETCOREAPP1_1 && !NETCOREAPP2_0

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_transparent_logo_graphic()
        {        
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White);

            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(svg));
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();

            result.ShouldBe("0ad8bc75675d04ba0caff51c7a89992c");
        }
#endif
    }
}



