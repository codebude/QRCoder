#if SYSTEM_DRAWING
using System;
using System.Linq;
using System.Text;
using QRCoder;
using Shouldly;
using Xunit;

namespace QRCoderTests;

/****************************************************************************************************
 * Note: PDF tests replace JPEG data with placeholder text to avoid platform-specific compression
 *       differences in JPEG encoding. This allows us to verify the PDF structure while ignoring
 *       the binary image data that may differ across platforms.
 ****************************************************************************************************/
public class PdfByteQRCodeRendererTests
{
    /// <summary>
    /// Finds the JPEG data in a PDF byte array and replaces it with "JPEGDATAHERE" placeholder text.
    /// This allows testing PDF structure without being affected by platform-specific JPEG compression.
    /// </summary>
    /// <param name="pdfBytes">The PDF byte array</param>
    /// <param name="jpegData">Output parameter containing the extracted JPEG data</param>
    /// <returns>The PDF with JPEG data replaced by placeholder text</returns>
    private byte[] ReplaceJpegDataWithPlaceholder(byte[] pdfBytes, out byte[] jpegData)
    {
        // JPEG data in PDF starts after "/BitsPerComponent 8\r\n>>\r\nstream\r\n" and ends before "\r\nendstream"
        var imageStreamMarker = Encoding.ASCII.GetBytes("/BitsPerComponent 8\r\n>>\r\nstream\r\n");
        var endstreamMarker = Encoding.ASCII.GetBytes("\r\nendstream");
        var placeholder = Encoding.ASCII.GetBytes("JPEGDATAHERE");

        // Find the image stream (object 4 contains the JPEG data)
        int streamStart = -1;
        int streamEnd = -1;

        // Search for the specific image stream marker
        for (int i = 0; i < pdfBytes.Length - imageStreamMarker.Length; i++)
        {
            bool found = true;
            for (int j = 0; j < imageStreamMarker.Length; j++)
            {
                if (pdfBytes[i + j] != imageStreamMarker[j])
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                streamStart = i + imageStreamMarker.Length;
                break;
            }
        }

        // Search for endstream marker after the stream start
        if (streamStart != -1)
        {
            for (int i = streamStart; i < pdfBytes.Length - endstreamMarker.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < endstreamMarker.Length; j++)
                {
                    if (pdfBytes[i + j] != endstreamMarker[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    streamEnd = i;
                    break;
                }
            }
        }

        if (streamStart == -1 || streamEnd == -1)
        {
            throw new Exception("Could not find JPEG stream markers in PDF");
        }

        // Extract the JPEG data
        jpegData = new byte[streamEnd - streamStart];
        Array.Copy(pdfBytes, streamStart, jpegData, 0, jpegData.Length);

        // Create new byte array with JPEG data replaced
        var result = new byte[streamStart + placeholder.Length + (pdfBytes.Length - streamEnd)];
        Array.Copy(pdfBytes, 0, result, 0, streamStart);
        Array.Copy(placeholder, 0, result, streamStart, placeholder.Length);
        Array.Copy(pdfBytes, streamEnd, result, streamStart + placeholder.Length, pdfBytes.Length - streamEnd);

        return result;
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_blackwhite()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5);
        pdfCodeGfx.ShouldMatchApproved("pdf");
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_color()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5, "#222222", "#CCCCCC");
        pdfCodeGfx.ShouldMatchApproved("pdf");
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_custom_dpi()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5, "#000000", "#FFFFFF", 300);
        pdfCodeGfx.ShouldMatchApproved("pdf");
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_custom_quality()
    {
        //Create QR code
        var gen = new QRCodeGenerator();
        var data = gen.CreateQrCode("This is a quick test! 123#?", QRCodeGenerator.ECCLevel.L);
        var pdfCodeGfx = new PdfByteQRCode(data).GetGraphic(5, "#000000", "#FFFFFF", 150, 95);
        pdfCodeGfx.ShouldMatchApproved("pdf");
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
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
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
    }

    [Fact]
    public void can_render_pdfbyte_qrcode_from_helper_2()
    {
        //Create QR code
        var pdfCodeGfx = PdfByteQRCodeHelper.GetQRCode("This is a quick test! 123#?", 5, "#222222", "#CCCCCC", QRCodeGenerator.ECCLevel.L);
        pdfCodeGfx.ShouldMatchApproved("pdf");
        var pdfWithPlaceholder = ReplaceJpegDataWithPlaceholder(pdfCodeGfx, out var jpegData);
        pdfWithPlaceholder.ShouldMatchApproved("txt");
        jpegData.ShouldMatchApprovedImage(asMonochrome: true);
    }
}
#endif