using System;
using System.Collections.Generic;
using System.Text;
using static QRCoder.QRCodeGenerator;

namespace QRCoder
{
    public class AsciiQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public AsciiQRCode() { }

        public AsciiQRCode(QRCodeData data) : base(data) { }

       
        /// <summary>
        /// Returns a strings that contains the resulting QR code as textual representation.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
        /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
        /// <param name="drawQuietZones">Bool that defines if quiet zones around the QR code shall be drawn</param>
        /// <param name="endOfLine">End of line separator. (Default: \n)</param>
        /// <returns></returns>
        public string GetGraphic(int repeatPerModule, string darkColorString = "██", string whiteSpaceString = "  ", bool drawQuietZones = true, string endOfLine = "\n")
        {
            if (repeatPerModule < 1)
                throw new Exception("The repeatPerModule-parameter must be 1 or greater.");
            return string.Join(endOfLine, GetLineByLineGraphic(repeatPerModule, darkColorString, whiteSpaceString, drawQuietZones));
        }


        /// <summary>
        /// Returns an array of strings that contains each line of the resulting QR code as ASCII chars.
        /// </summary>
        /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
        /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
        /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
        /// <param name="drawQuietZones">Bool that defines if quiet zones around the QR code shall be drawn</param>
        /// <returns></returns>
        public string[] GetLineByLineGraphic(int repeatPerModule, string darkColorString = "██", string whiteSpaceString = "  ", bool drawQuietZones = true)
        {
            var qrCode = new List<string>();
            //We need to adjust the repeatPerModule based on number of characters in darkColorString
            //(we assume whiteSpaceString has the same number of characters)
            //to keep the QR code as square as possible.
            var quietZonesModifier = (drawQuietZones ? 0 : 8);
            var quietZonesOffset = (int)(quietZonesModifier * 0.5);
            var adjustmentValueForNumberOfCharacters = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
            var verticalNumberOfRepeats = repeatPerModule + adjustmentValueForNumberOfCharacters;
            var sideLength = (QrCodeData.ModuleMatrix.Count - quietZonesModifier) * verticalNumberOfRepeats;
            for (var y = 0; y < sideLength; y++)
            {
                var lineBuilder = new StringBuilder();
                for (var x = 0; x < QrCodeData.ModuleMatrix.Count - quietZonesModifier; x++)
                {
                    var module = QrCodeData.ModuleMatrix[((y + verticalNumberOfRepeats) / verticalNumberOfRepeats - 1)+quietZonesOffset][x + quietZonesOffset];
                    for (var i = 0; i < repeatPerModule; i++)
                    {
                        lineBuilder.Append(module ? darkColorString : whiteSpaceString);
                    }
                }
                qrCode.Add(lineBuilder.ToString());
            }
            return qrCode.ToArray();
        }

        /// <summary>
        /// Returns a strings that contains the resulting QR code as minified textual representation.
        /// </summary>
        /// <param name="drawQuietZones">Bool that defines if quiet zones around the QR code shall be drawn</param>
        /// <param name="invert">If set to true, dark and light colors will be inverted</param>
        /// <param name="endOfLine">End of line separator. (Default: \n)</param>
        /// <returns></returns>
        public string GetGraphicSmall(bool drawQuietZones = true, bool invert = false, string endOfLine = "\n")
        {
            bool BLACK = true, WHITE = false;

            var palette = new
            {
                WHITE_ALL = "\u2588",
                WHITE_BLACK = "\u2580",
                BLACK_WHITE = "\u2584",
                BLACK_ALL = " ",
            };

            var moduleData = QrCodeData.ModuleMatrix;
            var sbSize = (moduleData.Count + endOfLine.Length) * (int)Math.Ceiling(moduleData.Count / 2.0) - 1;
            var lineBuilder = new StringBuilder(sbSize);

            var quietZonesModifier = (drawQuietZones ? 0 : 8);
            var quietZonesOffset = (int)(quietZonesModifier * 0.5);
            var sideLength = (moduleData.Count - quietZonesModifier);

            for (var row = 0; row < sideLength; row += 2)
            {
                for (var col = 0; col < sideLength; col++)
                {
                    var current = moduleData[col + quietZonesOffset][row + quietZonesOffset] ^ invert;
                    var nextRowId = row + quietZonesOffset + 1;

                    // Set next to whitespace "color"
                    var next = BLACK;
                    // Fill next with value, if in data range
                    if (nextRowId < QrCodeData.ModuleMatrix.Count)
                        next = moduleData[col + quietZonesOffset][nextRowId] ^ invert;
                        
                    if (current == WHITE && next == WHITE)
                        lineBuilder.Append(palette.WHITE_ALL);
                    else if (current == WHITE && next == BLACK)
                        lineBuilder.Append(palette.WHITE_BLACK);
                    else if (current == BLACK && next == WHITE)
                        lineBuilder.Append(palette.BLACK_WHITE);
                    else
                        lineBuilder.Append(palette.BLACK_ALL);
                }
                if (row + 2 < sideLength)
                    lineBuilder.Append(endOfLine);
            }
            return lineBuilder.ToString();
        }
    }


    public static class AsciiQRCodeHelper
    {
        public static string GetQRCode(string plainText, int pixelsPerModule, string darkColorString, string whiteSpaceString, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, string endOfLine = "\n", bool drawQuietZones = true)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new AsciiQRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColorString, whiteSpaceString, drawQuietZones, endOfLine);
        }

        public static string GetQRCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, string endOfLine = "\n", bool drawQuietZones = true, bool invert = true)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new AsciiQRCode(qrCodeData))
                return qrCode.GetGraphicSmall(drawQuietZones, invert, endOfLine);
        }
    }
}
