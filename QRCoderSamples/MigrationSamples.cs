using QRCoder;
using System.Drawing.Imaging;

namespace QRCoderSamples;

public static class MigrationSamples
{
    /// <summary>
    /// Example showing how to convert a QR code to base64 with a custom image format (e.g., JPEG).
    /// This is useful when migrating from Base64QRCode which previously supported multiple image formats.
    /// </summary>
    public static string ConvertQRCodeToBase64Jpeg()
    {
        // Generate QR code data
        using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);

        // Use QRCode renderer to get a bitmap
        using var qrCode = new QRCode(qrCodeData);
        using var bitmap = qrCode.GetGraphic(20);

        // Convert bitmap to JPEG byte array
        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Jpeg);
        byte[] jpegBytes = ms.ToArray();

        // Convert to base64
        string base64String = Convert.ToBase64String(jpegBytes);

        return base64String;
    }

    /// <summary>
    /// Example showing how to convert a Bitmap to PNG byte array for use with SvgQRCode logos.
    /// SvgQRCode logos now require PNG-encoded byte arrays or SVG strings instead of Bitmap instances.
    /// </summary>
    public static string ConvertBitmapToSvgQRCodeWithLogo()
    {
        // Convert a Bitmap to PNG byte array
        using var bitmap = new System.Drawing.Bitmap("logo.jpg");
        using var ms = new MemoryStream();
        bitmap.Save(ms, ImageFormat.Png);
        byte[] pngBytes = ms.ToArray();

        // Create SvgLogo with PNG byte array
        var logo = new SvgQRCode.SvgLogo(pngBytes, iconSizePercent: 15);

        // Generate QR code with logo
        using var qrCodeData = QRCodeGenerator.GenerateQrCode("Hello World", QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new SvgQRCode(qrCodeData);
        string svg = qrCode.GetGraphic(20, "#000000", "#ffffff", true, SvgQRCode.SizingMode.WidthHeightAttribute, logo);

        return svg;
    }
}
