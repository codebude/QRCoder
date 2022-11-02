using System;
using System.Collections.Generic;
using System.Text;
using static QRCoder.QRCodeGenerator;

namespace QRCoder
{
    public class Utf8QRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public Utf8QRCode() { }

        public Utf8QRCode(QRCodeData data) : base(data) { }


        /// <summary>
        /// Returns a string that contains the resulting QR code, represented by the UTF8 chars SPACE, FULL BLOCK, UPPER HALF BLOCK and LOWER HALF BLOCK.
        /// </summary>
        /// <param name="drawQuietZones">If true, the mandatory space around the QR code is included</param>
        /// <param name="endOfLine">End of line separator. (Default: \n)</param>
        /// <param name="invert">If true, the returned QR code will be drawn inverted</param>
        public string GetGraphic(bool drawQuietZones = true, string endOfLine = "\n", bool invert = false)
        {
            return string.Join(endOfLine, GetLineByLineGraphic(drawQuietZones, invert));
        }

        private IEnumerable<string> GetLineByLineGraphic(bool drawQuietZones = true, bool invert = false)
        {
            var quietZonesModifier = (drawQuietZones ? 0 : 8);
            var quietZonesOffset = (int)(quietZonesModifier * 0.5);
            var sideLength = QrCodeData.ModuleMatrix.Count - quietZonesModifier;

            var lineBuilder = new StringBuilder(sideLength);

            for (var y = 0; y < sideLength; y += 2)
            {
                for (var x = 0; x < sideLength; x++)
                {
                    var module1 = QrCodeData.ModuleMatrix[x + quietZonesOffset][y + quietZonesOffset] ^ invert;
                    var module2 = (y + quietZonesOffset + 1 < QrCodeData.ModuleMatrix.Count && QrCodeData.ModuleMatrix[x + quietZonesOffset][y + quietZonesOffset + 1]) ^ invert;

                    if (module1 && module2)
                    {
                        lineBuilder.Append('\u2588');
                    }
                    else if (module1)
                    {
                        lineBuilder.Append('\u2580');
                    }
                    else if (module2)
                    {
                        lineBuilder.Append('\u2584');
                    }
                    else
                    {
                        lineBuilder.Append(' ');
                    }
                }
                yield return lineBuilder.ToString();
                lineBuilder.Clear();
            }
        }
    }

    public static class Utf8QRCodeHelper
    {
        public static string GetQRCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, string endOfLine = "\n", bool drawQuietZones = true, bool invert = false)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new Utf8QRCode(qrCodeData))
                return qrCode.GetGraphic(drawQuietZones, endOfLine, invert);
        }
    }
}