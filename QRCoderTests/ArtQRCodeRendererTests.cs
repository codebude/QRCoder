#if SYSTEM_DRAWING

using System.Drawing;
using QRCoder;
using QRCoderTests.Helpers;
using Shouldly;
using Xunit;

namespace QRCoderTests;

public class ArtQRCodeRendererTests
{
    [Fact]
    public void can_create_standard_qrcode_graphic()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic(10);

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("df510ce9feddc0dd8c23c54e700abbf0");
    }

    [Fact]
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

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("e28a3779b9b975b85984e36f596c9a35");
    }

    [Fact]
    public void can_create_standard_qrcode_graphic_without_quietzone()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, drawQuietZones: false);

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("54408da26852d6c67ab7cad2656da7fa");
    }

    [Fact]
    public void can_create_standard_qrcode_graphic_with_background()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic((Bitmap)Image.FromFile(HelperFunctions.GetAssemblyPath() + "\\assets\\noun_software engineer_2909346.png"));
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346

        var result = HelperFunctions.BitmapToHash(bmp);

        result.ShouldBe("7f039ccde219ae78e4f768466376a17f");
    }

    [Fact]
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
    public void can_instantate_parameterless()
    {
        var artCode = new ArtQRCode();
        artCode.ShouldNotBeNull();
        artCode.ShouldBeOfType<ArtQRCode>();
    }

    [Fact]
    public void can_render_artqrcode_from_helper()
    {
        //Create QR code
        var bmp = ArtQRCodeHelper.GetQRCode("A", 10, Color.Black, Color.White, Color.Transparent, QRCodeGenerator.ECCLevel.L);
        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("a1975852df9b537344468bd44d54abe0");
    }
}
#endif
