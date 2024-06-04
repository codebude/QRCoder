#if !NETCOREAPP1_1
using QRCoder;
using QRCoderTests.Helpers;
using QRCoderTests.Helpers.XUnitExtenstions;
using Shouldly;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace QRCoderTests;

public class PostscriptQRCodeRendererTests
{
    [Fact]
    [Category("QRRenderer/PostscriptQRCode")]
    public void can_render_postscript_qrcode_simple()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5);

        var result = HelperFunctions.StringToHash(RemoveCreationDate(ps));
        result.ShouldBe("06b90d1e64bf022a248453e5f91101a0");
    }

    [Fact]
    [Category("QRRenderer/PostscriptQRCode")]
    public void can_render_postscript_qrcode_eps()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, true);

        var result = HelperFunctions.StringToHash(RemoveCreationDate(ps));
        result.ShouldBe("50f6152cdb0b685595d80e7888712d3b");
    }

    [Fact]
    [Category("QRRenderer/PostscriptQRCode")]
    public void can_render_postscript_qrcode_size()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(33, 33));

        var result = HelperFunctions.StringToHash(RemoveCreationDate(ps));
        result.ShouldBe("49c7faaafef312eb4b6ea1fec195e63d");
    }

    [Fact]
    [Category("QRRenderer/PostscriptQRCode")]
    public void can_render_postscript_qrcode_size_no_quiet_zones()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(new Size(50, 50), false);

        var result = HelperFunctions.StringToHash(RemoveCreationDate(ps));
        result.ShouldBe("9bfa0468e125d9815a39902133a10762");
    }

    [Fact]
    [Category("QRRenderer/PostscriptQRCode")]
    public void can_render_postscript_qrcode_colors()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var ps = new PostscriptQRCode(data).GetGraphic(5, Color.Red, Color.Blue);

        var result = HelperFunctions.StringToHash(RemoveCreationDate(ps));
        result.ShouldBe("2e001d7f67a446eb1b5df32ff5321808");
    }

    private static string RemoveCreationDate(string text)
    {
        // Regex pattern to match lines that start with %%CreationDate: followed by any characters until the end of the line
        string pattern = @"%%CreationDate:.*\r?\n?";

        // Use Regex.Replace to remove matching lines
        return Regex.Replace(text, pattern, string.Empty, RegexOptions.Multiline);
    }
}
#endif
