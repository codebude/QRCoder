using System;
using Xunit;
using QRCoder;
using Shouldly;
using System.IO;
using System.Security.Cryptography;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;
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
        public void can_create_qrcode_standard_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10);

            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("e8c61b8f0455924fe08ba68686d0d296");
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_standard_graphic_hex()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, "#000000", "#ffffff");

            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("e8c61b8f0455924fe08ba68686d0d296");
        }


        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_standard_graphic_without_quietzones()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(5, Color.Black, Color.White, false);

            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("329e1664f57cbe7332d8d4db04c1d480");
#else
            result.ShouldBe("d703e54a0ba541c6ea69e3d316e394e7");
#endif
        }


        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_transparent_logo_graphic()
        {        
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: (Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("ee65d96c3013f6032b561cc768251eef");
#else
            result.ShouldBe("150f8fc7dae4487ba2887d2b2bea1c25");
#endif
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_non_transparent_logo_graphic()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: (Bitmap)Bitmap.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("1d718f06f904af4a46748f02af2d4eec");
#else
            result.ShouldBe("c46a7ec51bf978d7a882059c322ca69d");
#endif
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_logo_and_with_transparent_border()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            var logo = (Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png");
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: logo, iconBorderWidth: 6);
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("ee65d96c3013f6032b561cc768251eef");
#else
            result.ShouldBe("150f8fc7dae4487ba2887d2b2bea1c25");
#endif
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_logo_and_with_standard_border()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            var logo = (Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png");
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: logo, iconBorderWidth: 6);
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("52207bd86ca5a532fb2095dbaa0ae04c");
#else
            result.ShouldBe("1c926ea1d48f42fdf8e6f1438b774cdd");
#endif
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_create_qrcode_with_logo_and_with_custom_border()
        {
            //Create QR code
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

            var logo = (Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png");
            var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: logo, iconBorderWidth: 6, iconBackgroundColor: Color.DarkGreen);
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            var result = HelperFunctions.BitmapToHash(bmp);
#if NET35_OR_GREATER || NET40_OR_GREATER
            result.ShouldBe("d2f20d34a973d92b9c3e05db1393b331");
#else
            result.ShouldBe("9a06bfbb72df999b6290b5af5c4037cb");
#endif
        }


        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_instantate_qrcode_parameterless()
        {
            var svgCode = new QRCode();
            svgCode.ShouldNotBeNull();
            svgCode.ShouldBeOfType<QRCode>();
        }

        [Fact]
        [Category("QRRenderer/QRCode")]
        public void can_render_qrcode_from_helper()
        {
            //Create QR code                   
            var bmp = QRCodeHelper.GetQRCode("This is a quick test! 123#?", 10, Color.Black, Color.White, QRCodeGenerator.ECCLevel.H);

            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("e8c61b8f0455924fe08ba68686d0d296");
        }
#endif
    }
}