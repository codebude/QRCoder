
#if !NET35 && NET6_0
using QRCoder;
using QRCoderTests.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using FluentAssertions;
#if NET6_0
using QRCoder.ImageSharp;
#endif

namespace QRCoderTests
{
    [TestClass]
    public class ImageSharpArtQRCodeRendererTests
    {


        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic(10);
            var result = HelperFunctions.ImageToHash(bmp);
            //bmp.SaveAsPng("qrcode_imagesharp.png");
            result.Should().Be("e8de533db63b5784de075c4e4cc3e0c9"); //different hash than the System.Drawing example since the algorithm is slighty different -> (anti-aliasing).
        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_with_custom_finder()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var finder = new Image<Rgba32>(15, 15);
            var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, finderPatternImage: finder);
            //bmp.SaveAsPng("finder_custom.png");
            var result = HelperFunctions.ImageToHash(bmp);

            result.Should().Be("63dcb31fd8910a10aba57929b6327790");

        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_without_quietzone()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, drawQuietZones: false);

            var result = HelperFunctions.ImageToHash(bmp);

            //bmp.SaveAsPng("without_quietzone.png");

            result.Should().Be("d96f9c8c64cdb5c651dd59bab6b564d7");

        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_create_standard_qrcode_graphic_with_background()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var bmp = new ArtQRCode(data).GetGraphic(Image.Load(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
            //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

            var result = HelperFunctions.ImageToHash(bmp);

            bmp.SaveAsPng("custom_background.png");

            result.Should().Be("aea31c69506b0d933fd49205e7b37f33");

        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void should_throw_pixelfactor_oor_exception()
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
            var aCode = new ArtQRCode(data);
            
            var exception = Assert.ThrowsException<ArgumentException>(() => aCode.GetGraphic(10, Color.Black, Color.White, Color.Transparent, pixelSizeFactor: 2));
           
            exception.Message.Should().Be("The parameter pixelSize must be between 0 and 1. (0-100%)");
        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_instantate_parameterless()
        {
            var asciiCode = new ArtQRCode();
            asciiCode.Should().NotBeNull();
            asciiCode.Should().BeOfType<ArtQRCode>();
        }

        [TestMethod]
        [TestCategory("QRRenderer/ArtQRCode")]
        public void can_render_artqrcode_from_helper()
        {
            //Create QR code
            var bmp = ArtQRCodeHelper.GetQRCode("A", 10, Color.Black, Color.White, Color.Transparent, QRCodeGenerator.ECCLevel.L);

            var result = HelperFunctions.ImageToHash(bmp);
            //bmp.SaveAsPng("Helper.png");
            result.Should().Be("1dbda9e61f832a7ebdb5d97f8c6e8fb6");

        }
    }
}
#endif