#if SYSTEM_DRAWING

namespace QRCoderTests;

public class QRCodeRendererTests
{
    [Fact]
    public void can_create_qrcode_standard_graphic()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(10);
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_qrcode_standard_graphic_hex()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(10, "#000000", "#ffffff");
        bmp.ShouldMatchApproved();
    }


    [Fact]
    public void can_create_qrcode_standard_graphic_without_quietzones()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(5, Color.Black, Color.White, false);
        bmp.ShouldMatchApproved();
    }


    [Fact]
    public void can_create_qrcode_with_transparent_logo_graphic()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: HelperFunctions.GetIconBitmap());
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_qrcode_with_non_transparent_logo_graphic()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: HelperFunctions.GetIconBitmap());
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_transparent_border()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        var logo = HelperFunctions.GetIconBitmap();
        var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: logo, iconBorderWidth: 6);
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_standard_border()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        var logo = HelperFunctions.GetIconBitmap();
        var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.White, icon: logo, iconBorderWidth: 6);
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_custom_border()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        var logo = HelperFunctions.GetIconBitmap();
        var bmp = new QRCode(data).GetGraphic(10, Color.Black, Color.Transparent, icon: logo, iconBorderWidth: 6, iconBackgroundColor: Color.DarkGreen);
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        bmp.ShouldMatchApproved();
    }

    [Fact]
    public void can_instantate_qrcode_parameterless()
    {
        var svgCode = new QRCode();
        svgCode.ShouldNotBeNull();
        svgCode.ShouldBeOfType<QRCode>();
    }

    [Fact]
    public void can_render_qrcode_from_helper()
    {
        //Create QR code                   
        var bmp = QRCodeHelper.GetQRCode("This is a quick test! 123#?", 10, Color.Black, Color.White, QRCodeGenerator.ECCLevel.H);
        bmp.ShouldMatchApproved();
    }
}
#endif
