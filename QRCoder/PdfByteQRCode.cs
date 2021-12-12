#if NETFRAMEWORK || NETSTANDARD2_0 || NET5_0 || NET6_0_WINDOWS
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using static QRCoder.QRCodeGenerator;

/* This renderer is inspired by RemusVasii: https://github.com/codebude/QRCoder/issues/223 */
namespace QRCoder
{

#if NET6_0_WINDOWS
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    // ReSharper disable once InconsistentNaming
    public class PdfByteQRCode : AbstractQRCode, IDisposable
    {
        private readonly byte[] pdfBinaryComment = new byte[] { 0x25, 0xe2, 0xe3, 0xcf, 0xd3 };

        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public PdfByteQRCode() { }

        public PdfByteQRCode(QRCodeData data) : base(data) { }

        /// <summary>
        /// Creates a PDF document with a black & white QR code
        /// </summary>
        /// <param name="pixelsPerModule"></param>
        /// <returns></returns>
        public byte[] GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, "#000000", "#ffffff");
        }

        /// <summary>
        /// Takes hexadecimal color string #000000 and returns byte[]{ 0, 0, 0 }
        /// </summary>
        /// <param name="colorString">Color in HEX format like #ffffff</param>
        /// <returns></returns>
        private byte[] HexColorToByteArray(string colorString)
        {
            if (colorString.StartsWith("#"))
                colorString = colorString.Substring(1);
            byte[] byteColor = new byte[colorString.Length / 2];
            for (int i = 0; i < byteColor.Length; i++)
                byteColor[i] = byte.Parse(colorString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return byteColor;
        }

        /// <summary>
        /// Creates a PDF document with given colors DPI and quality
        /// </summary>
        /// <param name="pixelsPerModule"></param>
        /// <param name="darkColorHtmlHex"></param>
        /// <param name="lightColorHtmlHex"></param>
        /// <param name="dpi"></param>
        /// <param name="jpgQuality"></param>
        /// <returns></returns>
        public byte[] GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, int dpi = 150, long jpgQuality = 85)
        {
            byte[] jpgArray = null, pngArray = null;
            var imgSize = QrCodeData.ModuleMatrix.Count * pixelsPerModule;
            var pdfMediaSize = (imgSize * 72 / dpi).ToString(CultureInfo.InvariantCulture);

            //Get QR code image
            using (var qrCode = new PngByteQRCode(QrCodeData))
            {
                pngArray = qrCode.GetGraphic(pixelsPerModule, HexColorToByteArray(darkColorHtmlHex), HexColorToByteArray(lightColorHtmlHex));
            }            

            //Create image and transofrm to JPG
            using (var msPng = new MemoryStream())
            {
                msPng.Write(pngArray, 0, pngArray.Length);
                var img = System.Drawing.Image.FromStream(msPng);
                using (var msJpeg = new MemoryStream())
                {
                    // Create JPEG with specified quality
                    var jpgImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(x => x.MimeType == "image/jpeg");
                    var jpgEncoderParameters = new EncoderParameters(1) { 
                        Param = new EncoderParameter[]{ new EncoderParameter(Encoder.Quality, jpgQuality) }
                    };
                    img.Save(msJpeg, jpgImageCodecInfo, jpgEncoderParameters);
                    jpgArray = msJpeg.ToArray();
                }
            }
            
            //Create PDF document
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream, System.Text.Encoding.GetEncoding("ASCII"));

                var xrefs = new List<long>();

                writer.Write("%PDF-1.5\r\n");
                writer.Flush();

                stream.Write(pdfBinaryComment, 0, pdfBinaryComment.Length);
                writer.WriteLine();

                writer.Flush();
                xrefs.Add(stream.Position);

                writer.Write(
                    xrefs.Count.ToString() + " 0 obj\r\n" +
                    "<<\r\n" +
                    "/Type /Catalog\r\n" +
                    "/Pages 2 0 R\r\n" +
                    ">>\r\n" +
                    "endobj\r\n"
                );

                writer.Flush();
                xrefs.Add(stream.Position);

                writer.Write(
                    xrefs.Count.ToString() + " 0 obj\r\n" +
                    "<<\r\n" +
                    "/Count 1\r\n" +
                    "/Kids [ <<\r\n" +
                    "/Type /Page\r\n" +
                    "/Parent 2 0 R\r\n" +
                    "/MediaBox [0 0 " + pdfMediaSize + " " + pdfMediaSize + "]\r\n" +
                    "/Resources << /ProcSet [ /PDF /ImageC ]\r\n" +
                    "/XObject << /Im1 4 0 R >> >>\r\n" +
                    "/Contents 3 0 R\r\n" +
                    ">> ]\r\n" +
                    ">>\r\n" +
                    "endobj\r\n"
                );

                var X = "q\r\n" +
                    pdfMediaSize + " 0 0 " + pdfMediaSize + " 0 0 cm\r\n" +
                    "/Im1 Do\r\n" +
                    "Q";

                writer.Flush();
                xrefs.Add(stream.Position);

                writer.Write(
                    xrefs.Count.ToString() + " 0 obj\r\n" +
                    "<< /Length " + X.Length.ToString() + " >>\r\n" +
                    "stream\r\n" +
                    X + "endstream\r\n" +
                    "endobj\r\n"
                );

                writer.Flush();
                xrefs.Add(stream.Position);

                writer.Write(
                    xrefs.Count.ToString() + " 0 obj\r\n" +
                    "<<\r\n" +
                    "/Name /Im1\r\n" +
                    "/Type /XObject\r\n" +
                    "/Subtype /Image\r\n" +
                    "/Width " + imgSize.ToString() + "/Height " + imgSize.ToString() + "/Length 5 0 R\r\n" +
                    "/Filter /DCTDecode\r\n" +
                    "/ColorSpace /DeviceRGB\r\n" +
                    "/BitsPerComponent 8\r\n" +
                    ">>\r\n" +
                    "stream\r\n"
                );
                writer.Flush();
                stream.Write(jpgArray, 0, jpgArray.Length);
                writer.Write(
                    "\r\n" +
                    "endstream\r\n" +
                    "endobj\r\n"
                );

                writer.Flush();
                xrefs.Add(stream.Position);

                writer.Write(
                    xrefs.Count.ToString() + " 0 obj\r\n" +
                    jpgArray.Length.ToString() + " endobj\r\n"
                );

                writer.Flush();
                var startxref = stream.Position;

                writer.Write(
                    "xref\r\n" +
                    "0 " + (xrefs.Count + 1).ToString() + "\r\n" +
                    "0000000000 65535 f\r\n"
                );

                foreach (var refValue in xrefs)
                    writer.Write(refValue.ToString("0000000000") + " 00000 n\r\n");

                writer.Write(
                    "trailer\r\n" +
                    "<<\r\n" +
                    "/Size " + (xrefs.Count + 1).ToString() + "\r\n" +
                    "/Root 1 0 R\r\n" +
                    ">>\r\n" +
                    "startxref\r\n" +
                    startxref.ToString() + "\r\n" +
                    "%%EOF"
                );

                writer.Flush();

                stream.Position = 0;

                return stream.ToArray();
            }
        }
    }

#if NET6_0_WINDOWS
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
#endif
    public static class PdfByteQRCodeHelper
    {
        public static byte[] GetQRCode(string plainText, int pixelsPerModule, string darkColorHtmlHex,
            string lightColorHtmlHex, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false,
            EciMode eciMode = EciMode.Default, int requestedVersion = -1)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (
                var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode,
                    requestedVersion))
            using (var qrCode = new PdfByteQRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColorHtmlHex, lightColorHtmlHex);
        }

        public static byte[] GetQRCode(string txt, ECCLevel eccLevel, int size)
        {
            using (var qrGen = new QRCodeGenerator())
            using (var qrCode = qrGen.CreateQrCode(txt, eccLevel))
            using (var qrBmp = new PdfByteQRCode(qrCode))
                return qrBmp.GetGraphic(size);

        }
    }
}
#endif