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
        /// <param name="characterPerModule">number of characters per module.</param>
        /// <param name="darkColorString">string or character for use as dark color bits</param>
        /// <param name="whiteSpaceString">string or character for use as white space bits</param>
        /// <returns></returns>
        public string[] GetLineByLineGraphic(int characterPerModule, string darkColorString, string whiteSpaceString)
        {
            var qrCode = new List<string>();
            var sideLength = QrCodeData.ModuleMatrix.Count * characterPerModule;

            for (var y = 0; y < sideLength; y++)
            {
                bool emptyLine = true;
                var lineBuilder = new StringBuilder();

                for (var x = 0; x < QrCodeData.ModuleMatrix.Count; x++)
                {
                    var module = QrCodeData.ModuleMatrix[x][(y + characterPerModule) / characterPerModule - 1];

                    for (var i = 0; i < characterPerModule; i++)
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