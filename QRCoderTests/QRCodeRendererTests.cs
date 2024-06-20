#if SYSTEM_DRAWING
using System.Drawing;
using QRCoder;
using QRCoderTests.Helpers;
using QRCoderTests.Helpers.XUnitExtenstions;
using Shouldly;
using Xunit;


namespace QRCoderTests;


public class QRCodeRendererTests
{
    [Fact]
    [Category("QRRenderer/QRCode")]
    public void can_create_qrcode_standard_graphic()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(10);

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("f2ed5073bd42dc012e442c0f750e9dae");
    }

    [Fact]
    [Category("QRRenderer/QRCode")]
    public void can_create_qrcode_standard_graphic_hex()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(10, "#000000", "#ffffff");

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("f2ed5073bd42dc012e442c0f750e9dae");
    }


    [Fact]
    [Category("QRRenderer/QRCode")]
    public void can_create_qrcode_standard_graphic_without_quietzones()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new QRCode(data).GetGraphic(5, Color.Black, Color.White, false);

        var result = HelperFunctions.BitmapToHash(bmp);
        result.ShouldBe("c401d45c01e636af3eb4b8ca6cd17d14");
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
        result.ShouldBe("c99a82b43ce48ddae18a75862c476a9e");
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
        result.ShouldBe("74808e52270bba92e7b821dbd067dfd2");
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
        result.ShouldBe("c99a82b43ce48ddae18a75862c476a9e");
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
        result.ShouldBe("943ecd2a847a4d9509ca0266dbbadd7b");
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
        result.ShouldBe("e60bdaafe807889ca322d47146fe8300");
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
        result.ShouldBe("f2ed5073bd42dc012e442c0f750e9dae");
    }

}
#endif
