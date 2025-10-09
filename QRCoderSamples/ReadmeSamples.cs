using QRCoder;

namespace QRCoderSamples;

/// <summary>
/// Sample code from the README.md file
/// </summary>
public static class ReadmeSamples
{
    /// <summary>
    /// Quick Start - Generate a simple black and white PNG QR code
    /// </summary>
    public static void QuickStart_SimplePng()
    {
        // Generate a simple black and white PNG QR code
        byte[] qrCodeImage = PngByteQRCodeHelper.GetQRCode("Hello World", QRCodeGenerator.ECCLevel.Q, 20);
    }

    /// <summary>
    /// Quick Start - Generate a scalable black and white SVG QR code
    /// </summary>
    public static void QuickStart_SimpleSvg()
    {
        // Generate a scalable black and white SVG QR code
        using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);
        using var svgRenderer = new SvgQRCode(qrCodeData);
        string svg = svgRenderer.GetGraphic();
    }

    /// <summary>
    /// Payload Generator - Create a bookmark payload
    /// </summary>
    public static void PayloadGenerator_Bookmark()
    {
        // Create a bookmark payload
        var bookmarkPayload = new PayloadGenerator.Bookmark("https://github.com/Shane32/QRCoder", "QRCoder Repository");

        // Generate the QR code data from the payload
        using var qrCodeData = QRCodeGenerator.GenerateQrCode(bookmarkPayload);

        // Or override the ECC level
        using var qrCodeData2 = QRCodeGenerator.GenerateQrCode(bookmarkPayload, QRCodeGenerator.ECCLevel.H);

        // Render the QR code
        using var pngRenderer = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = pngRenderer.GetGraphic(20);
    }

    /// <summary>
    /// Micro QR Code - Generate a Micro QR code
    /// </summary>
    public static void MicroQRCode_Simple()
    {
        // Generate a Micro QR code (versions M1-M4, represented as -1 to -4)
        using var qrCodeData = QRCodeGenerator.GenerateMicroQrCode("Hello", QRCodeGenerator.ECCLevel.L, requestedVersion: -2);
        using var qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);
    }

    /// <summary>
    /// Working with QRCodeData - Access the module matrix directly
    /// </summary>
    public static void QRCodeData_ModuleMatrix()
    {
        // Generate QR code data
        using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);

        // Access the module matrix
        var moduleMatrix = qrCodeData.ModuleMatrix;
        int size = moduleMatrix.Count; // Size of the QR code (includes quiet zone)

        // Manually render as ASCII (versus the included ASCII renderer)
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                // Check if module is dark (true) or light (false)
                bool isDark = moduleMatrix[y][x];
                Console.Write(isDark ? "██" : "  ");
            }
            Console.WriteLine();
        }
    }
}
