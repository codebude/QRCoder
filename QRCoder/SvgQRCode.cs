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
        public string GetGraphic(int pixelsPerModule, Color foregroundColor, Color backgroundColor, bool drawQuietZones = true)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, foregroundColor, backgroundColor, drawQuietZones);
        }

        public string GetGraphic(int pixelsPerModule, string foregroundColorHex, string backgroundColorHex, bool drawQuietZones = true)
        {
            var viewBox = new Size(pixelsPerModule * this.QrCodeData.ModuleMatrix.Count, pixelsPerModule * this.QrCodeData.ModuleMatrix.Count);
            return this.GetGraphic(viewBox, foregroundColorHex, backgroundColorHex, drawQuietZones);
        }

        public string GetGraphic(Size viewBox, bool drawQuietZones = true)
        {
            return this.GetGraphic(viewBox, Color.Black, Color.White, drawQuietZones);
        }

        public string GetGraphic(Size viewBox, Color foregroundColor, Color backgroundColor, bool drawQuietZones = true)
        {
            return this.GetGraphic(viewBox, ColorTranslator.ToHtml(Color.FromArgb(foregroundColor.ToArgb())), ColorTranslator.ToHtml(Color.FromArgb(backgroundColor.ToArgb())), drawQuietZones);
        }

        public string GetGraphic(Size viewBox, string foregroundColorHex, string backgroundColorHex, bool drawQuietZones = true)
        {
            
			int matrixWidth = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
			decimal preciseUnitsPerModule = Convert.ToDecimal(Math.Min(viewBox.Width, viewBox.Height)) / (decimal)matrixWidth;
			decimal unitsPerModule = Decimal.Floor(preciseUnitsPerModule * 100m) / 100m; //takes first 2 decimal places (rounds down - cuts off remaining decimal places). This makes sure, that resulting SVG doesn't overflow requested size.
            var matrixOffset = drawQuietZones ? 0 : 4;

			var svgFile = new StringBuilder($"<svg version=\"1.1\" baseProfile=\"full\" width=\"{viewBox.Width}\" height=\"{viewBox.Height}\" xmlns=\"http://www.w3.org/2000/svg\">");
			svgFile.AppendLine($"<rect x=\"0\" y=\"0\" width=\"{matrixWidth * unitsPerModule}\" height=\"{matrixWidth * unitsPerModule}\" fill=\"{backgroundColorHex}\" />");

            for (var x = matrixOffset; x < this.QrCodeData.ModuleMatrix.Count - matrixOffset; x++)
            {
                for (var y = matrixOffset; y < this.QrCodeData.ModuleMatrix.Count - matrixOffset; y++)
                {
                    var module = this.QrCodeData.ModuleMatrix[y][x];
                    if (module)
                    {
                        svgFile.AppendLine($"<rect x=\"{(x - matrixOffset) * unitsPerModule}\" y=\"{(y - matrixOffset) * unitsPerModule}\" width=\"{unitsPerModule}\" height=\"{unitsPerModule}\" fill=\"{foregroundColorHex}\" />");
                    }
                }
            }
            svgFile.Append("</svg>");
            return svgFile.ToString();
        }

        public void Dispose()
        {
            this.QrCodeData = null;
        }
    }
}
