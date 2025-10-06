using System.IO;

namespace QRCoderTests;

/// <summary>
/// Tests to verify that QR codes are not transposed along the main diagonal.
/// All tests use the same QR code data with default renderer settings.
/// Human verification of approval files is required to ensure correct orientation.
/// </summary>
public class TransposeVerificationTests
{
    private readonly QRCodeData _sharedQrCodeData;

    public TransposeVerificationTests()
    {
        // Create a single QR code sample that will be used across all tests
        // Using a distinctive pattern that makes transpose errors obvious
        var gen = new QRCodeGenerator();
        _sharedQrCodeData = gen.CreateQrCode("ABCD", QRCodeGenerator.ECCLevel.L);
    }

#if SYSTEM_DRAWING
    [Theory]
    [InlineData("QRCode")]
    [InlineData("BitmapByteQRCode")]
    [InlineData("Base64QRCode")]
    [InlineData("PngByteQRCode")]
    public void image_renderers(string rendererType)
    {
        byte[] imageBytes = rendererType switch
        {
            "QRCode" => GetQRCodeBytes(),
            "BitmapByteQRCode" => GetBitmapByteQRCodeBytes(),
            "Base64QRCode" => GetBase64QRCodeBytes(),
            "PngByteQRCode" => GetPngByteQRCodeBytes(),
            "SvgQRCode" => GetSvgQRCodeBytes(),
            _ => throw new ArgumentException($"Unknown renderer type: {rendererType}")
        };

        imageBytes.ShouldMatchApprovedImage(asMonochrome: true);
    }

    [Fact]
    public void artqrcode_renderer()
    {
        var qrCode = new ArtQRCode(_sharedQrCodeData);
        var bitmap = qrCode.GetGraphic(10, Color.Black, Color.White, Color.White, null, 1, true, ArtQRCode.QuietZoneStyle.Flat, ArtQRCode.BackgroundImageStyle.Fill, null);
        bitmap.ShouldMatchApproved();
    }

    private byte[] GetQRCodeBytes()
    {
        var qrCode = new QRCode(_sharedQrCodeData);
        var bitmap = qrCode.GetGraphic(10);
        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }

    private byte[] GetBitmapByteQRCodeBytes()
    {
        var qrCode = new BitmapByteQRCode(_sharedQrCodeData);
        return qrCode.GetGraphic(10);
    }

    private byte[] GetBase64QRCodeBytes()
    {
        var qrCode = new Base64QRCode(_sharedQrCodeData);
        var base64String = qrCode.GetGraphic(10);
        return Convert.FromBase64String(base64String);
    }

    private byte[] GetPngByteQRCodeBytes()
    {
        var qrCode = new PngByteQRCode(_sharedQrCodeData);
        return qrCode.GetGraphic(10);
    }

    private byte[] GetSvgQRCodeBytes()
    {
        var qrCode = new SvgQRCode(_sharedQrCodeData);
        var svgString = qrCode.GetGraphic(10);
        var bitmapSize = _sharedQrCodeData.ModuleMatrix.Count * 10;
        // use Svg.Net to render SVG to bitmap for comparison
        var svgDoc = Svg.SvgDocument.FromSvg<Svg.SvgDocument>(svgString);
        var bitmap = svgDoc.Draw(bitmapSize, bitmapSize);
        using var ms = new MemoryStream();
        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }
#endif

    [Theory]
    [InlineData("FullSize")]
    [InlineData("Small")]
    public void ascii_renderer(string sizeType)
    {
        var qrCode = new AsciiQRCode(_sharedQrCodeData);
        var asciiArt = sizeType switch
        {
            "FullSize" => qrCode.GetGraphic(1),
            "Small" => qrCode.GetGraphicSmall(),
            _ => throw new ArgumentException($"Unknown size type: {sizeType}")
        };
        asciiArt.ShouldMatchApproved(x => x.NoDiff().WithDiscriminator(sizeType));
    }

    [Fact]
    public void pdf_renderer()
    {
        var qrCode = new PdfByteQRCode(_sharedQrCodeData);
        var pdfBytes = qrCode.GetGraphic(10);
        pdfBytes.ShouldMatchApproved("pdf");
    }

    [Fact]
    public void postscript_renderer()
    {
        var qrCode = new PostscriptQRCode(_sharedQrCodeData);
        var postscript = qrCode.GetGraphic(10);
        postscript.ShouldMatchApproved(x => x.NoDiff().WithFileExtension("ps"));
    }

    [Fact]
    public void black_module_position()
    {
        // The black module in a QR code is always at position (4*version + 9, 8) + (4,4) due to the quiet zone
        // For our test QR code (version 1), it should be at (13, 8)
        // If transposed, it would be at (8, 13)
        var version = _sharedQrCodeData.Version;
        var expectedRow = 4 * version + 9 + 4;
        var expectedCol = 8 + 4;
        var transposedRow = expectedCol;
        var transposedCol = expectedRow;

        // Verify the black module is at the expected position
        _sharedQrCodeData.ModuleMatrix[expectedRow][expectedCol].ShouldBeTrue(
            $"Black module should be at position ({expectedRow}, {expectedCol})");

        // Verify it's NOT at the transposed position (unless they happen to be the same)
        if (expectedRow != transposedCol || expectedCol != transposedRow)
        {
            _sharedQrCodeData.ModuleMatrix[transposedRow][transposedCol].ShouldBeFalse(
                $"Black module should NOT be at transposed position ({transposedRow}, {transposedCol})");
        }
    }

#if SYSTEM_DRAWING
    [Fact]
    public void black_module_reference()
    {
        // Create a QR code data with the same size as our test matrix
        // but with only a single black module at the expected black module position
        var version = _sharedQrCodeData.Version;
        var blackModuleRow = 4 * version + 9 + 4;
        var blackModuleCol = 8 + 4;

        // Create a minimal QRCodeData with just the black module
        var referenceData = new QRCodeData(version, addPadding: true);

        // Set only the black module position to true
        referenceData.ModuleMatrix[blackModuleRow][blackModuleCol] = true;

        // Render this reference image for human verification
        var qrCode = new QRCode(referenceData);
        var bitmap = qrCode.GetGraphic(10);
        bitmap.ShouldMatchApproved();
    }
#endif
}
