﻿#if !NETCOREAPP1_1 && !NET6_0

using Xunit;
using QRCoder;
using Shouldly;
using System.Drawing;
using QRCoderTests.Helpers.XUnitExtenstions;
using QRCoderTests.Helpers;

namespace QRCoderTests
{

    public class ArtQRCodeRendererTests
    {


        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic(10);
            bmp.Save("original.png");
            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("cb38c3156eaf13cdfba699bdafc3a84c");
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_with_custom_finder()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var finder = new Bitmap(70, 70);
            using (var g = Graphics.FromImage(finder))
            {
                g.FillRectangle(Brushes.Red, 0, 0, 70, 70);
            }
            var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, finderPatternImage: finder);
            bmp.Save("finder.png");
            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("5df3f2892eeb01e9c282ad10f642dec2");
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_without_quietzone()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, drawQuietZones: false);

            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("632315c8695416fc82fe06a202688433");
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_with_background()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic((Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
            bmp.Save("custom_background.png");
            var result = HelperFunctions.BitmapToHash(bmp);

            result.ShouldBe("bbea08507282773175cfe7b52f0ddae4");
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void should_throw_pixelfactor_oor_exception()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var aCode = new ArtQRCode(data);
            
            var exception = Record.Exception(() => aCode.GetGraphic(10, Color.Black, Color.White, Color.Transparent, pixelSizeFactor: 2));
            Assert.NotNull(exception);
            Assert.IsType<System.Exception>(exception);
            exception.Message.ShouldBe("The parameter pixelSize must be between 0 and 1. (0-100%)");
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_instantate_parameterless()
        {
            var artCode = new ArtQRCode();
            artCode.ShouldNotBeNull();
            artCode.ShouldBeOfType<ArtQRCode>();
        }

        [Fact]
        [Category("QRRenderer/ArtQRCode")]
        public void can_render_artqrcode_from_helper()
        {
            //Create QR code
            var bmp = ArtQRCodeHelper.GetQRCode("A", 10, Color.Black, Color.White, Color.Transparent, QRCodeGenerator.ECCLevel.L);
            var result = HelperFunctions.BitmapToHash(bmp);
            result.ShouldBe("57ecaa9bdeadcdcbeac8a19d734907ff");
        }
    }
}
#endif