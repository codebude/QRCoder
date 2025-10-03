#if SYSTEM_DRAWING

namespace QRCoderTests;

public class ArtQRCodeRendererTests
{
    [Fact]
    public void can_create_standard_qrcode_graphic()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic(10);
        bmp.ShouldMatchApproved();
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
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_standard_qrcode_graphic_without_quietzone()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic(10, Color.Black, Color.White, Color.Transparent, drawQuietZones: false);
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_standard_qrcode_graphic_with_background()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new ArtQRCode(data).GetGraphic(HelperFunctions.GetIconBitmap());
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void should_throw_pixelfactor_oor_exception()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var aCode = new ArtQRCode(data);

        var exception = Should.Throw<System.Exception>(() => aCode.GetGraphic(10, Color.Black, Color.White, Color.Transparent, pixelSizeFactor: 2));
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
        bmp.ShouldMatchApproved();
    }
}
#endif
