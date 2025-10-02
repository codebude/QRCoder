using QRCoder;
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
        bmp.ShouldMatchApprovedImage();
    }


    [Fact]
    public void can_render_bitmapbyte_qrcode_color_bytearray()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new BitmapByteQRCode(data).GetGraphic(10, new byte[] { 30, 30, 30 }, new byte[] { 255, 0, 0 });
        bmp.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_bitmapbyte_qrcode_drawing_color()
    {
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.H);
        var bmp = new BitmapByteQRCode(data).GetGraphic(10, "#e3e3e3", "#ffffff");
        bmp.ShouldMatchApprovedImage();
    }
}
