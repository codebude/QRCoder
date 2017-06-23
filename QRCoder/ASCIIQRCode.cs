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
            var qrCode = new List<string>();
            var sideLength = QrCodeData.ModuleMatrix.Count * characterPerModule;

            for (var y = 0; y < sideLength; y = y + characterPerModule)
            {
                bool emptyLine = true;
                var lineBuilder = new StringBuilder();
                for (var cm = 0; cm < characterPerModule; cm++)
                {
                    for (var x = sideLength - 1; x >= 0; x = x - characterPerModule)
                    {
                        var module =
                            QrCodeData.ModuleMatrix[(x + characterPerModule) / characterPerModule - 1][
                                (y + characterPerModule) / characterPerModule - 1];
                        for (var i = 0; i < characterPerModule; i++)
                        {
                            lineBuilder.Append(module ? "██" : "  ");
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
            }
            return qrCode.ToArray();
        }
        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}