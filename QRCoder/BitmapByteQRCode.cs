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
            List<byte> image = new List<byte>() { 0x42, 0x4d };

            var width = this.QrCodeData.ModuleMatrix.Count;
            var height = pixelsPerModule * this.QrCodeData.ModuleMatrix.Count;
           
             var unitsPerModule = (int)Math.Floor(Convert.ToDouble(Math.Min(width, height)) / this.QrCodeData.ModuleMatrix.Count);
             var size = this.QrCodeData.ModuleMatrix.Count * unitsPerModule;
           
             for (var x = 0; x < size; x = x + unitsPerModule)
             {
                 for (var y = 0; y < size; y = y + unitsPerModule)
                 {
                     var module = this.QrCodeData.ModuleMatrix[(y + unitsPerModule) / unitsPerModule - 1][(x + unitsPerModule) / unitsPerModule - 1];
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
