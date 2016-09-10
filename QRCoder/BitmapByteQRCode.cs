using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRCoder
{
  
    public class BitmapByteQRCode : AbstractQRCode<byte[]>, IDisposable
    {
        public BitmapByteQRCode(QRCodeData data) : base(data) { }


        public override byte[] GetGraphic(int pixelsPerModule)
        {
            var sideLength = this.QrCodeData.ModuleMatrix.Count * pixelsPerModule;
          
             for (var x = 0; x < sideLength; x = x + pixelsPerModule)
             {
                 for (var y = 0; y < sideLength; y = y + pixelsPerModule)
                 {
                    var module = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];
                     
                    //svgFile.AppendLine(@"<rect x=""" + x + @""" y=""" + y + @""" width=""" + unitsPerModule + @""" height=""" + unitsPerModule + @""" fill=""" + (module ? darkColorHex : lightColorHex) + @""" />");
                 }
             }

            return new byte[0];
        }

        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}
