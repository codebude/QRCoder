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
        RemoveCreationDate(ps).ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_eps()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, true);
        RemoveCreationDate(ps).ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_size()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(33, 33));
        RemoveCreationDate(ps).ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_size_no_quiet_zones()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(50, 50), false);
        RemoveCreationDate(ps).ShouldMatchApproved(x => x.NoDiff());
    }

    [Fact]
    public void can_render_postscript_qrcode_colors()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, Color.Red, Color.Blue);
        RemoveCreationDate(ps).ShouldMatchApproved(x => x.NoDiff());
    }

    private static string RemoveCreationDate(string text)
    {
        // Regex pattern to match lines that start with %%CreationDate: followed by any characters until the end of the line
        string pattern = @"%%CreationDate:.*\r?\n?";

        // Use Regex.Replace to remove matching lines
        return Regex.Replace(text, pattern, string.Empty, RegexOptions.Multiline);
    }
}
