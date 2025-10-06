namespace QRCoderTests;

public class PostscriptQRCodeRendererTests
{
    [Fact]
    public void can_render_postscript_qrcode_simple()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5);
        ps.ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_eps()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, true);
        ps.ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_size()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(33, 33));
        ps.ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_size_no_quiet_zones()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(50, 50), false);
        ps.ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_colors()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, Color.Red, Color.Blue);
        ps.ShouldMatchApproved(x => x.NoDiff());
    }
}
