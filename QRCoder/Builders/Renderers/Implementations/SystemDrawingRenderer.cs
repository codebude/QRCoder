#if SYSTEM_DRAWING
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QRCoder.Builders.Renderers.Implementations
{
    public class SystemDrawingRenderer : RendererBase, IConfigurablePixelsPerModule, IStreamRenderer
    {
        private Color _darkColor = Color.Black;
        private Color _lightColor = Color.White;
        private int _pixelsPerModule;
        private Bitmap _icon;
        private double _iconSizePercent = 15;
        private int _iconBorderWidth = 0;
        private Color? _iconBackgroundColor;
        private ImageFormat _imageFormat = ImageFormat.Png;

        int IConfigurablePixelsPerModule.PixelsPerModule { get => _pixelsPerModule; set => _pixelsPerModule = value; }

        public SystemDrawingRenderer WithColors(Color darkColor, Color lightColor)
        {
            _darkColor = darkColor;
            _lightColor = lightColor;
            return this;
        }

        public SystemDrawingRenderer WithIcon(Bitmap icon, double iconSizePercent = 15, int iconBorderWidth = 0, Color? iconBackgroundColor = null)
        {
            _icon = icon;
            _iconSizePercent = iconSizePercent;
            _iconBorderWidth = iconBorderWidth;
            _iconBackgroundColor = iconBackgroundColor;
            return this;
        }

        public SystemDrawingRenderer WithImageFormat(ImageFormat imageFormat)
        {
            _imageFormat = imageFormat;
            return this;
        }

        public Bitmap ToBitmap()
        {
            return new QRCode(QrCodeData).GetGraphic(
                _pixelsPerModule, _darkColor, _lightColor, 
                _icon, (int)Math.Round(_iconSizePercent), _iconBorderWidth, 
                QuietZone, _iconBackgroundColor);
        }

        public MemoryStream ToStream()
        {
            var ms = new MemoryStream();
            using (var bitmap = ToBitmap())
            {
                bitmap.Save(ms, _imageFormat);
            }
            ms.Position = 0;
            return ms;
        }
    }
}
#endif