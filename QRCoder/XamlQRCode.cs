using System;
using System.Windows;
using System.Windows.Media;

namespace QRCoder
{
    public class XamlQRCode : AbstractQRCode, IDisposable
    {
        /// <summary>
        /// Constructor without params to be used in COM Objects connections
        /// </summary>
        public XamlQRCode() { }

        public XamlQRCode(QRCodeData data) : base(data) { }

        public DrawingImage GetGraphic(int pixelsPerModule)
        {
            return this.GetGraphic(pixelsPerModule, true);
        }

        public DrawingImage GetGraphic(int pixelsPerModule, bool drawQuietZones)
        {
            var drawableModulesCount = GetDrawableModulesCount(drawQuietZones);
            var viewBox = new Size(pixelsPerModule * drawableModulesCount, pixelsPerModule * drawableModulesCount);
            return this.GetGraphic(viewBox, new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White), drawQuietZones);
        }

        public DrawingImage GetGraphic(Size viewBox, bool drawQuietZones = true)
        {
            return this.GetGraphic(viewBox, new SolidColorBrush(Colors.Black), new SolidColorBrush(Colors.White), drawQuietZones);
        }

        public DrawingImage GetGraphic(int pixelsPerModule, string darkColorHex, string lightColorHex, bool drawQuietZones = true)
        {
            var drawableModulesCount = GetDrawableModulesCount(drawQuietZones);
            var viewBox = new Size(pixelsPerModule * drawableModulesCount, pixelsPerModule * drawableModulesCount);
            return this.GetGraphic(viewBox, new SolidColorBrush((Color)ColorConverter.ConvertFromString(darkColorHex)), new SolidColorBrush((Color)ColorConverter.ConvertFromString(lightColorHex)), drawQuietZones);
        }

        public DrawingImage GetGraphic(Size viewBox, Brush darkBrush, Brush lightBrush, bool drawQuietZones = true)
        {
            var drawableModulesCount = GetDrawableModulesCount(drawQuietZones);
            var qrSize = Math.Min(viewBox.Width, viewBox.Height);
            var unitsPerModule = qrSize / drawableModulesCount;
            var offsetModules = drawQuietZones ? 0 : 4;

            DrawingGroup drawing = new DrawingGroup();
            drawing.Children.Add(new GeometryDrawing(lightBrush, null, new RectangleGeometry(new Rect(new Point(0, 0), new Size(qrSize, qrSize)))));

            var group = new GeometryGroup();
            double x = 0d, y = 0d;
            for (int xi = offsetModules; xi < drawableModulesCount + offsetModules; xi++)
            {
                y = 0d;
                for (int yi = offsetModules; yi < drawableModulesCount + offsetModules; yi++)
                {
                    if (this.QrCodeData.ModuleMatrix[yi][xi])
                    {
                        group.Children.Add(new RectangleGeometry(new Rect(x, y, unitsPerModule, unitsPerModule)));
                    }
                    y += unitsPerModule;
                }
                x += unitsPerModule;
            }
            drawing.Children.Add(new GeometryDrawing(darkBrush, null, group));

            return new DrawingImage(drawing);
        }

        private int GetDrawableModulesCount(bool drawQuietZones = true)
        {
            return this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8);
        }

        public double GetUnitsPerModule(Size viewBox, bool drawQuietZones = true)
        {
            var drawableModulesCount = GetDrawableModulesCount(drawQuietZones);
            var qrSize = Math.Min(viewBox.Width, viewBox.Height);
            return qrSize / drawableModulesCount;
        }
    }
}
