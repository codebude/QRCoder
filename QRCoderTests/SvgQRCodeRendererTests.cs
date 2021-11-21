using System;
using Xunit;
using QRCoder;
using Shouldly;
using QRCoderTests.Helpers.XUnitExtenstions;
using System.IO;
using System.Security.Cryptography;
using QRCoderTests.Helpers;

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
                AppDomain.CurrentDomain.BaseDirectory;
#else
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\", "");
#endif
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_simple()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
            var svg = new SvgQRCode(data).GetGraphic(5);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("5c251275a435a9aed7e591eb9c2e9949");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode()
        {        
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("1baa8c6ac3bd8c1eabcd2c5422dd9f78");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_viewbox_mode()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(new Size(128,128));

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("56719c7db39937c74377855a5dc4af0a");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_viewbox_mode_viewboxattr()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(new Size(128, 128), sizingMode: SvgQRCode.SizingMode.ViewBoxAttribute);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("788afdb693b0b71eed344e495c180b60");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_without_quietzones()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White, false);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("2a582427d86b51504c08ebcbcf0472bd");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_without_quietzones_hex()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var svg = new SvgQRCode(data).GetGraphic(10, "#000000", "#ffffff", false);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("4ab0417cc6127e347ca1b2322c49ed7d");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_with_png_logo()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var logoBitmap = (Bitmap)Image.FromFile(GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png");
            var logoObj = new SvgQRCode.SvgLogo(iconRasterized: logoBitmap, 15);
            logoObj.GetMediaType().ShouldBe<SvgQRCode.SvgLogo.MediaType>(SvgQRCode.SvgLogo.MediaType.PNG);

            var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("78e02e8ba415f15817d5ed88c4afca31");            
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_with_svg_logo_embedded()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909361
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\noun_Scientist_2909361.svg");            
            var logoObj = new SvgQRCode.SvgLogo(logoSvg, 20);
            logoObj.GetMediaType().ShouldBe<SvgQRCode.SvgLogo.MediaType>(SvgQRCode.SvgLogo.MediaType.SVG);

            var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("855eb988d3af035abd273ed1629aa952");            
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_with_svg_logo_image_tag()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909361
            var logoSvg = File.ReadAllText(GetAssemblyPath() + "\\assets\\noun_Scientist_2909361.svg");
            var logoObj = new SvgQRCode.SvgLogo(logoSvg, 20, iconEmbedded: false);

            var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("bd442ea77d45a41a4f490b8d41591e04");
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_instantate_parameterless()
        {
            var svgCode = new SvgQRCode();
            svgCode.ShouldNotBeNull();
            svgCode.ShouldBeOfType<SvgQRCode>();
        }

        [Fact]
        [Category("QRRenderer/SvgQRCode")]
        public void can_render_svg_qrcode_from_helper()
        {
            //Create QR code                   
            var svg = SvgQRCodeHelper.GetQRCode("A", 2, "#000000", "#ffffff", QRCodeGenerator.ECCLevel.Q);

            var result = HelperFunctions.StringToHash(svg);
            result.ShouldBe("f5ec37aa9fb207e3701cc0d86c4a357d");
        }
#endif
    }
}



