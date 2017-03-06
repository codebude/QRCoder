using System;
using System.Drawing;
using System.Text;

namespace QRCoder
{
    public class SvgQRCode : AbstractQRCode<string>, IDisposable
    {
        public SvgQRCode(QRCodeData data) : base(data) { }


        public override string GetGraphic(int pixelsPerModule)
        {
            var viewBox = new Size(pixelsPerModule*this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, Color.Black, Color.White);
        }
        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, darkColor, lightColor, drawQuietZones);
        }

        public string GetGraphic(int pixelsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, darkColorHex, lightColorHex, drawQuietZones);
        }

        public string GetGraphic(Size viewBox, bool drawQuietZones = true)
        {
            return this.GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones);
        }

        public string GetGraphic(Size viewBox, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            return this.GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(darkColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(lightColor.ToArgb())), drawQuietZones);
        }

        public string GetGraphic(Size viewBox, string darkColorHex, string lightColorHex, bool drawQuietZones = true)
        {
            var svgFile = new StringBuilder(@"<svg version=""1.1"" baseProfile=""full"" width="""+viewBox.Width+ @""" height="""+viewBox.Height+ @""" xmlns=""http://www.w3.org/2000/svg"">");
            var unitsPerModule = (int)Math.Floor(Convert.ToDouble(Math.Min(viewBox.Width, viewBox.Height)) / this.QrCodeData.ModuleMatrix.Count);
            var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * unitsPerModule;
            var offset = drawQuietZones ? 0 : 4 * unitsPerModule;
            var drawableSize = size + offset;

            svgFile.AppendLine(@"<rect x=""" + (0 - offset) + @""" y=""" + (0 - offset) + @""" width=""" + (viewBox.Width - offset) + @""" height=""" + (viewBox.Height - offset) + @""" fill=""" + lightColorHex + @""" />");

            for (var x = 0; x < drawableSize; x = x + unitsPerModule)
            {
                for (var y = 0; y < drawableSize; y = y + unitsPerModule)
                {
                    var module = this.QrCodeData.ModuleMatrix[(y + unitsPerModule) / unitsPerModule - 1][(x + unitsPerModule) / unitsPerModule - 1];
                    if (module)
                    {
                        svgFile.AppendLine(@"<rect x=""" + (x - offset) + @""" y=""" + (y - offset) + @""" width=""" + unitsPerModule + @""" height=""" + unitsPerModule + @""" fill=""" + (module ? darkColorHex : lightColorHex) + @""" />");
                    }
                }
            }
            svgFile.Append(@"</svg>");
            return svgFile.ToString();
        }

        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}
