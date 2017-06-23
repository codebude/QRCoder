using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace QRCoder
{
    public class ASCIIQRCode : AbstractQRCode<String>, IDisposable
    {
        public ASCIIQRCode(QRCodeData data) : base(data) { }

        public override string GetGraphic(int characterPerModule)
        {

            return String.Join("\n", GetLineByLineGraphic(characterPerModule));
        }

        public string[] GetLineByLineGraphic(int characterPerModule)
        {
            return GetLineByLineGraphic(characterPerModule, "██", "  ");
        }

        public string GetGraphic(int characterPerModule, string darkColorString, string whiteSpaceString, string endOfLine = "\n")
        {
            return String.Join(endOfLine, GetLineByLineGraphic(characterPerModule, darkColorString, whiteSpaceString));
        }

        /// <summary>
        /// Returns an array of strings that contain each line of the resulting QR code
        /// </summary>
        /// <param name="repeatPerModule">number of repeated string or character per module.</param>
        /// <param name="darkColorString">string or character for use as dark color bits. In case of string make sure whiteSpaceString has the same length</param>
        /// <param name="whiteSpaceString">string or character for use as white space bits. In case of string make sure darkColorString has the same length</param>
        /// <returns></returns>
        public string[] GetLineByLineGraphic(int repeatPerModule, string darkColorString, string whiteSpaceString)
        {
            var qrCode = new List<string>();
            //We need to adjust the repeatPerModule based on number of characters in darkColorString 
            //(we assume whiteSpaceString has the same number of characters) 
            //to keep the QR code as square as possible.
            var adjustmentValueForNumberOfCharacters = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
            var verticalNumberOfRepeats = repeatPerModule + adjustmentValueForNumberOfCharacters;
            var sideLength = QrCodeData.ModuleMatrix.Count * verticalNumberOfRepeats;
            for (var y = 0; y < sideLength; y++)
            {
                bool emptyLine = true;
                var lineBuilder = new StringBuilder();

                for (var x = 0; x < QrCodeData.ModuleMatrix.Count; x++)
                {
                    var module = QrCodeData.ModuleMatrix[x][(y + verticalNumberOfRepeats) / verticalNumberOfRepeats - 1];

                    for (var i = 0; i < repeatPerModule; i++)
                    {
                        lineBuilder.Append(module ? darkColorString : whiteSpaceString);
                    }
                    if (module)
                    {
                        emptyLine = false;
                    }
                }
                if (!emptyLine)
                {
                    qrCode.Add(lineBuilder.ToString());
                }

            }
            return qrCode.ToArray();
        }
        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}