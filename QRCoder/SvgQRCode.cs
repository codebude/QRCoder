#if NETFRAMEWORK || NETSTANDARD2_0
using System;
using System.Collections;
using System.Drawing;
using System.Text;
using static QRCoder.QRCodeGenerator;
using static QRCoder.SvgQRCode;

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
            int offset = drawQuietZones ? 0 : 4;
            int drawableModulesCount = this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : offset * 2);
            double pixelsPerModule = Math.Min(viewBox.Width, viewBox.Height) / (double)drawableModulesCount;
            double qrSize = drawableModulesCount * pixelsPerModule;
            string svgSizeAttributes = (sizingMode == SizingMode.WidthHeightAttribute) ? $@"width=""{viewBox.Width}"" height=""{viewBox.Height}""" : $@"viewBox=""0 0 {viewBox.Width} {viewBox.Height}""";

            // Merge horizontal rectangles
            int[,] matrix = new int[drawableModulesCount, drawableModulesCount];
            for (int yi = 0; yi < drawableModulesCount; yi += 1)
            {
                BitArray bitArray = this.QrCodeData.ModuleMatrix[yi+offset];

                int x0 = -1;
                int xL = 0;
                for (int xi = 0; xi < drawableModulesCount; xi += 1)
                {
                    matrix[yi, xi] = 0;
                    if (bitArray[xi+offset])
                    {
                        if(x0 == -1)
                        {
                            x0 = xi;
                        }
                        xL += 1;
                    }
                    else
                    {
                        if(xL > 0)
                        {
                            matrix[yi, x0] = xL;
                            x0 = -1;
                            xL = 0;
                        }
                    }
                }

                if (xL > 0)
                {
                    matrix[yi, x0] = xL;
                }
            }

            StringBuilder svgFile = new StringBuilder($@"<svg version=""1.1"" baseProfile=""full"" shape-rendering=""crispEdges"" {svgSizeAttributes} xmlns=""http://www.w3.org/2000/svg"">");
            svgFile.AppendLine($@"<rect x=""0"" y=""0"" width=""{CleanSvgVal(qrSize)}"" height=""{CleanSvgVal(qrSize)}"" fill=""{lightColorHex}"" />");
            for (int yi = 0; yi < drawableModulesCount; yi += 1)
            {
                double y = yi * pixelsPerModule;
                for (int xi = 0; xi < drawableModulesCount; xi += 1)
                {
                    int xL = matrix[yi, xi];
                    if(xL > 0)
                    {
                        // Merge vertical rectangles
                        int yL = 1;
                        for (int y2 = yi + 1; y2 < drawableModulesCount; y2 += 1)
                        {
                            if(matrix[y2, xi] == xL)
                            {
                                matrix[y2, xi] = 0;
                                yL += 1;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Output SVG rectangles
                        double x = xi * pixelsPerModule;
                        svgFile.AppendLine($@"<rect x=""{CleanSvgVal(x)}"" y=""{CleanSvgVal(y)}"" width=""{CleanSvgVal(xL * pixelsPerModule)}"" height=""{CleanSvgVal(yL * pixelsPerModule)}"" fill=""{darkColorHex}"" />");
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

    public static class SvgQRCodeHelper
    {
        public static string GetQRCode(string plainText, int pixelsPerModule, string darkColorHex, string lightColorHex, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1, bool drawQuietZones = true, SizingMode sizingMode = SizingMode.WidthHeightAttribute)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion))
            using (var qrCode = new SvgQRCode(qrCodeData))
                return qrCode.GetGraphic(pixelsPerModule, darkColorHex, lightColorHex, drawQuietZones, sizingMode);
        }
    }
}

#endif
