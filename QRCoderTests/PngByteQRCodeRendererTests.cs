using System.Drawing;
using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests;

/****************************************************************************************************
 * Note: Test cases compare the outcome visually even if it's slower than a byte-wise compare.
 *       This is necessary, because the Deflate implementation differs on the different target
 *       platforms and thus the outcome, even if visually identical, differs. Thus only a visual
 *       test method makes sense. In addition bytewise differences shouldn't be important, if the
 *       visual outcome is identical and thus the qr code is identical/scannable.
 ****************************************************************************************************/
public class PngByteQRCodeRendererTests
{


    [Fact]
    public void can_render_pngbyte_qrcode_blackwhite()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5);
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_color()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 0, 0 }, new byte[] { 0, 0, 255 });
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_drawing_color()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, Color.Red, Color.Blue);
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_color_with_alpha()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 });
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_color_without_quietzones()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pngCodeGfx = new PngByteQRCode(data).GetGraphic(5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 }, false);
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_instantate_pngbyte_qrcode_parameterless()
    {
        var pngCode = new PngByteQRCode();
        pngCode.ShouldNotBeNull();
        pngCode.ShouldBeOfType<PngByteQRCode>();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_from_helper()
    {
        //Create QR code                   
        var pngCodeGfx = PngByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L, 10);
        pngCodeGfx.ShouldMatchApprovedImage();
    }

    [Fact]
    public void can_render_pngbyte_qrcode_from_helper_2()
    {
        //Create QR code                   
        var pngCodeGfx = PngByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", 5, new byte[] { 255, 255, 255, 127 }, new byte[] { 0, 0, 255 }, QRCodeGenerator.ECCLevel.L);
        pngCodeGfx.ShouldMatchApprovedImage();
    }
}
