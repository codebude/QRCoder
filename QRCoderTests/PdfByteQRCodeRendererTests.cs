using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests;

public class PdfByteQRCodeRendererTests
{

    [Fact]
    public void can_render_pdfbyte_qrcode_blackwhite()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5);
        pdfCodeGfx.ShouldMatchApproved("pdf");
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_color()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5, "#FF0000", "#0000FF");
        pdfCodeGfx.ShouldMatchApproved("pdf");
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_custom_dpi()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5, "#000000", "#FFFFFF", 300);
        pdfCodeGfx.ShouldMatchApproved("pdf");
    }

    [Fact]
    public void can_instantate_pdfbyte_qrcode_parameterless()
    {
        var pdfCode = new PdfByteQRCode();
        pdfCode.ShouldNotBeNull();
        pdfCode.ShouldBeOfType<PdfByteQRCode>();
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_from_helper()
    {
        //Create QR code
        var pdfCodeGfx = PdfByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L, 10);
        pdfCodeGfx.ShouldMatchApproved("pdf");
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_from_helper_2()
    {
        //Create QR code
        var pdfCodeGfx = PdfByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", 5, "#FF0000", "#0000FF", QRCodeGenerator.ECCLevel.L);
        pdfCodeGfx.ShouldMatchApproved("pdf");
    }
}
