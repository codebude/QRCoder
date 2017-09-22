using System;
#if NETSTANDARD2_0
using System.DrawingCore;
#else
using System.Drawing;
#endif
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
            var svgFile = new StringBuilder(@"<svg version=""1.1"" baseProfile=""full"" shape-rendering=""crispEdges"" width=""" +viewBox.Width+ @""" height="""+viewBox.Height+ @""" xmlns=""http://www.w3.org/2000/svg"">");
            var drawableModulesCount = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
            var unitsPerModule = Math.Round(Convert.ToDouble(Math.Min(viewBox.Width, viewBox.Height)) / drawableModulesCount,4);
            if (unitsPerModule * drawableModulesCount > viewBox.Width)
                unitsPerModule -= 0.0001;            
            var offset = drawQuietZones ? 4 * unitsPerModule : 0;
            var offsetModules = drawQuietZones ? 0 : 4;
            var qrSize = unitsPerModule * drawableModulesCount;

            svgFile.AppendLine($@"<rect x=""0"" y=""0"" width=""{CleanSvgVal(qrSize)}"" height=""{CleanSvgVal(qrSize)}"" fill=""" + lightColorHex + @""" />");
            int xi = 0, yi = 0;
            for (var x = 0d; x < qrSize; x = x + unitsPerModule)
            {
                yi = 0;
                for (var y = 0d; y < qrSize; y = y + unitsPerModule)
                {
                    if (this.QrCodeData.ModuleMatrix[yi + offsetModules][xi + offsetModules])
                    {
                        svgFile.AppendLine($@"<rect x=""{CleanSvgVal(x)}"" y=""{CleanSvgVal(y)}"" width=""{CleanSvgVal(unitsPerModule)}"" height=""{CleanSvgVal(unitsPerModule)}"" fill=""{darkColorHex}"" />");
                    }
                    yi++;
                }
                xi++;
            }
            svgFile.Append(@"</svg>");
            return svgFile.ToString();
        }

        private string CleanSvgVal(double input)
        {
            //Clean double values for international use/formats
            return input.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}
