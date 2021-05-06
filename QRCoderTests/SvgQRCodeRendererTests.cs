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

        private string GetAssemblyPath()
        {
            return
#if NET5_0
                System.Reflection.Assembly.GetExecutingAssembly().Location;
#else
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
#endif
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode()
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

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_without_quietzones()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White, false);

            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(svg));
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();

            result.ShouldBe("24392f47d4c1c2c5097bd6b3f8eefccc");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_with_png_logo()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var logoBitmap = (Bitmap)Bitmap.FromFile(GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png");
            var logoObj = new SvgQRCode.SvgLogo(logoBitmap, 15);

            var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(svg));
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();

            result.ShouldBe("4ff45872787f321524cc4d071239c25e");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_with_svg_logo()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909361
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\noun_Scientist_2909361.svg");
            var logoObj = new SvgQRCode.SvgLogo(logoSvg, 30);

            var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(svg));
            var result = BitConverter.ToString(hash).Replace("-", "").ToLower();

            result.ShouldBe("b4ded3964e2e640b6b6c74d1c89d71fa");
        }
#endif
    }
}



