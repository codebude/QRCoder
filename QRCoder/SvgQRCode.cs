using System;
using System.Drawing;
using System.Text;

namespace QRCoder
{
    public class SvgQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public SvgQRCode() { }
        public SvgQRCode(QRCodeData data) : base(data) { }
        
        public string GetGraphic(int pixelsPerModule)
        {
            var viewBox = new Size(pixelsPerModule*this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, Color.Black, Color.White);
        }
        public string GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, darkColor, lightColor, drawQuietZones, sizingMode);
        }

        public string GetGraphic(int pixelsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, darkColorHex, lightColorHex, drawQuietZones, sizingMode);
        }

        public string GetGraphic(Size viewBox, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {
            return this.GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones, sizingMode);
        }

        public string GetGraphic(Size viewBox, Color darkColor, Color lightColor, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {
            return this.GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(darkColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(lightColor.ToArgb())), drawQuietZones, sizingMode);
        }

        public string GetGraphic(Size viewBox, string darkColorHex, string lightColorHex, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {            
            var offset = drawQuietZones ? 0 : 4;
            var drawableModulesCount = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : offset * 2);
            var pixelsPerModule = (double)Math.Min(viewBox.Width, viewBox.Height) / (double)drawableModulesCount;
            var qrSize = drawableModulesCount * pixelsPerModule;
            var svgSizeAttributes = sizingMode.Equals(SizingMode.WidthHeightAttribute) ? $@"width=""{viewBox.Width}"" height=""{viewBox.Height}""" : $@"viewBox=""0 0 {viewBox.Width} {viewBox.Height}""";
            var svgFile = new StringBuilder($@"<svg version=""1.1"" baseProfile=""full"" shape-rendering=""crispEdges"" {svgSizeAttributes} xmlns=""http://www.w3.org/2000/svg"">");                                  
            svgFile.AppendLine($@"<rect x=""0"" y=""0"" width=""{CleanSvgVal(qrSize)}"" height=""{CleanSvgVal(qrSize)}"" fill=""{lightColorHex}"" />");
            for (int xi = offset; xi < offset + drawableModulesCount; xi++)
            {
                for (int yi = offset; yi < offset + drawableModulesCount; yi++)
                {
                    if (this.QrCodeData.ModuleMatrix[xi][yi])
                    {
                        var x = (xi - offset) * pixelsPerModule;
                        var y = (yi - offset) * pixelsPerModule;
                        svgFile.AppendLine($@"<rect x=""{CleanSvgVal(x)}"" y=""{CleanSvgVal(y)}"" width=""{CleanSvgVal(pixelsPerModule)}"" height=""{CleanSvgVal(pixelsPerModule)}"" fill=""{darkColorHex}"" />");
                    }
                }
            }
            svgFile.Append(@"</svg>");
            return svgFile.ToString();
        }
        
        private string CleanSvgVal(double input)
        {
            //Clean double values for international use/formats
            return input.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        public enum SizingMode
        {
            WidthHeightAttribute,
            ViewBoxAttribute
        }
    }
}
