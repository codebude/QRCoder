#if !NETCOREAPP1_1
using System;
using System.Drawing;
using System.IO;
using QRCoder;
using QRCoderTests.Helpers.XUnitExtenstions;
using Shouldly;
using Xunit;

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
    [Category("QRRenderer/Base64QRCode")]
    public void can_render_base64_qrcode_blackwhite()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5);
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    [Category("QRRenderer/Base64QRCode")]
    public void can_render_base64_qrcode_noquietzones()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, false);
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Black, Color.White, false);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    [Category("QRRenderer/Base64QRCode")]
    public void can_render_base64_qrcode_color()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, new byte[] { 255, 0, 0 }, new byte[] { 0, 0, 255 });
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Red, Color.Blue);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

    [Fact]
    [Category("QRRenderer/Base64QRCode")]
    public void can_render_base64_qrcode_transparent()
    {
        var pngCodeGfx = new PngByteQRCode(_data).GetGraphic(5, new byte[] { 0, 255, 0, 255 }, new byte[] { 255, 255, 255, 0 });
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Lime, Color.Transparent);
        base64QRCode.ShouldBe(Convert.ToBase64String(pngCodeGfx));
    }

#if SYSTEM_DRAWING
    [Fact]
    [Category("QRRenderer/Base64QRCode")]
    public void can_render_base64_qrcode_jpeg()
    {
        var ms = new MemoryStream();
        using (var bitmap = new QRCode(_data).GetGraphic(5, Color.Black, Color.White, true))
        {
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        ms.Position = 0;
        var jpgString = Convert.ToBase64String(ms.ToArray());
        var base64QRCode = new Base64QRCode(_data).GetGraphic(5, Color.Black, Color.White, true, Base64QRCode.ImageType.Jpeg);
        base64QRCode.ShouldBe(jpgString);
    }
#endif
}

#endif
