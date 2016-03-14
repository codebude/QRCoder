using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace QRCoder
{
    public class SvgQRCode : AbstractQRCode<string>, IDisposable
    {
        public SvgQRCode(QRCodeData data) : base(data) { }


        public override string GetGraphic(int pixelsPerModule)
        {
            Size viewBox = new Size(pixelsPerModule*qrCodeData.ModuleMatrix.Count, pixelsPerModule * qrCodeData.ModuleMatrix.Count);
            return GetGraphic(viewBox, Color.Black, Color.White);
        }

        public string GetGraphic(Size viewBox)
        {
            return GetGraphic(viewBox, Color.Black, Color.White);
        }

        public string GetGraphic(Size viewBox, Color darkColor, Color lightColor)
        {
            return GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(darkColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(lightColor.ToArgb())));
        }

        public string GetGraphic(Size viewBox, string darkColorHex, string lightColorHex)
        {
            string svgFile = @"<svg version=""1.1""
    baseProfile = ""full""
    width=""" + viewBox.Width + @""" height=""" + viewBox.Height + @"""
    xmlns=""http://www.w3.org/2000/svg"" >
    ";
          
            int unitsPerModule = (int)Math.Floor(Convert.ToDouble(Math.Min(viewBox.Width, viewBox.Height))/ qrCodeData.ModuleMatrix.Count);
            int drawableSize = unitsPerModule*qrCodeData.ModuleMatrix.Count;
            for (int x = 0; x < drawableSize; x = x + unitsPerModule)
            {
                for (int y = 0; y < drawableSize; y = y + unitsPerModule)
                {
                    var module = qrCodeData.ModuleMatrix[(y + unitsPerModule) / unitsPerModule - 1][(x + unitsPerModule) / unitsPerModule - 1];
                    svgFile += "\t<rect x=\"" + x + "\" y=\"" + y + "\" width=\"" + unitsPerModule  + "\" height=\"" + unitsPerModule  + "\" fill=\"" + (module ? darkColorHex : lightColorHex) + "\" />\n";
                }
            }
            svgFile += "\t</svg>";
            return svgFile;
        }
        
        public void Dispose()
        {
            this.qrCodeData = null;
        }
    }
}
