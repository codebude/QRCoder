namespace QRCoderTests;

public class Base64QRCodeRendererTests
{
    private readonly QRCodeData _data;

    public Base64QRCodeRendererTests()
    {
        var gen = new QRCodeGenerator();
        _data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
    }

    [Fact]
    public void can_render_base64_qrcode_blackwhite()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5);
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    public void can_render_base64_qrcode_noquietzones()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, false);
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Black, Color.White, false);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    public void can_render_base64_qrcode_color()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, new byte[] { 255, 0, 0 }, new byte[] { 0, 0, 255 });
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Red, Color.Blue);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    public void can_render_base64_qrcode_transparent()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, new byte[] { 0, 255, 0, 255 }, new byte[] { 255, 255, 255, 0 });
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Lime, Color.Transparent);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

#if SYSTEM_DRAWING
    [Fact]
    public void can_render_base64_qrcode_jpeg()
    {
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Black, Color.White, true, Base64QRCode.ImageType.Jpeg);
        var data = Convert.FromBase64String(base64QRCode);
        data.ShouldMatchApprovedImage(asMonochrome: true); // remove JPEG compression artifacts by converting to monochrome
    }
#endif
}
