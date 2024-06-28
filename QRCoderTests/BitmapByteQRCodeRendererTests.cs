using QRCoder;
using QRCoderTests.Helpers;
using Shouldly;
using Xunit;

namespace QRCoderTests;

public class BitmapByteQRCodeRendererTests
{
    [Fact]
    public void can_render_bitmapbyte_qrcode()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new BitmapByteQRCode(data).GetGraphic(10);

        var result = HelperFunctions.ByteArrayToHash(bmp);
        result.ShouldBe("2d262d074f5c436ad93025150392dd38");
    }


    [Fact]
    public void can_render_bitmapbyte_qrcode_color_bytearray()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new BitmapByteQRCode(data).GetGraphic(10, new byte[] { 30, 30, 30 }, new byte[] { 255, 0, 0 });

        var result = HelperFunctions.ByteArrayToHash(bmp);
        result.ShouldBe("1184507c7eb98f9ca76afd04313c41cb");
    }

    [Fact]
    public void can_render_bitmapbyte_qrcode_drawing_color()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new BitmapByteQRCode(data).GetGraphic(10, "#e3e3e3", "#ffffff");

        var result = HelperFunctions.ByteArrayToHash(bmp);
        result.ShouldBe("40cd208fc46aa726d6e98a2028ffd2b7");
    }
}
