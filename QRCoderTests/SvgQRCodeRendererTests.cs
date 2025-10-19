namespace QRCoderTests;

public class SvgQRCodeRendererTests
{
    [Fact]
    public void can_render_svg_qrcode_simple_unscaled()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var svg = new SvgQRCode(data).GetGraphic();
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_simple()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var svg = new SvgQRCode(data).GetGraphic(5);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_viewbox_mode()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = new SvgQRCode(data).GetGraphic(new Size(128, 128));
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_viewbox_mode_viewboxattr()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = new SvgQRCode(data).GetGraphic(new Size(128, 128), sizingMode: SvgQRCode.SizingMode.ViewBoxAttribute);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_without_quietzones()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = new SvgQRCode(data).GetGraphic(10, Color.Red, Color.White, false);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_without_quietzones_hex()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = new SvgQRCode(data).GetGraphic(10, "#000000", "#ffffff", false);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_with_png_logo_bytearray()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        var logoBitmap = HelperFunctions.GetIconBytes();
        var logoObj = new SvgQRCode.SvgLogo(iconRasterized: logoBitmap, 15);
        logoObj.GetMediaType().ShouldBe(SvgQRCode.SvgLogo.MediaType.PNG);

        var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_with_svg_logo_embedded()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909361
        var logoSvg = HelperFunctions.GetIconSvg();
        var logoObj = new SvgQRCode.SvgLogo(logoSvg, 20);
        logoObj.GetMediaType().ShouldBe(SvgQRCode.SvgLogo.MediaType.SVG);

        var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_render_svg_qrcode_with_svg_logo_image_tag()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);

        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909361
        var logoSvg = HelperFunctions.GetIconSvg();
        var logoObj = new SvgQRCode.SvgLogo(logoSvg, 20, iconEmbedded: false);

        var svg = new SvgQRCode(data).GetGraphic(10, Color.DarkGray, Color.White, logo: logoObj);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Fact]
    public void can_instantate_parameterless()
    {
        var svgCode = new SvgQRCode();
        svgCode.ShouldNotBeNull();
        svgCode.ShouldBeOfType<SvgQRCode>();
    }

    [Fact]
    public void can_render_svg_qrcode_from_helper()
    {
        //Create QR code                   
        var svg = SvgQRCodeHelper.GetQRCode("A", 2, "#000000", "#ffffff", QRCodeGenerator.ECCLevel.Q);
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void can_render_svg_qrcode_blue_light_with_half_red_dark(bool useColor)
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var svg = useColor
            ? new SvgQRCode(data).GetGraphic(10, Color.FromArgb(128, 255, 0, 0), Color.Blue)
            : new SvgQRCode(data).GetGraphic(10, "#FF000080", "#0000FF");
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void can_render_svg_qrcode_transparent_light_with_black_dark(bool useColor)
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var svg = useColor
            ? new SvgQRCode(data).GetGraphic(10, Color.Black, Color.Transparent)
            : new SvgQRCode(data).GetGraphic(10, "#000", "transparent");
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void can_render_svg_qrcode_transparent_light_with_half_red_dark(bool useColor)
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var svg = useColor
            ? new SvgQRCode(data).GetGraphic(10, Color.FromArgb(128, 255, 0, 0), Color.Transparent)
            : new SvgQRCode(data).GetGraphic(10, "#FF000080", "transparent");
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void can_render_svg_qrcode_transparent_dark_with_black_light(bool useColor)
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var svg = useColor
            ? new SvgQRCode(data).GetGraphic(10, Color.Transparent, Color.Black)
            : new SvgQRCode(data).GetGraphic(10, "transparent", "#000");
        svg.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("svg"));
    }
}
