using System.IO;
using QRCoder.Builders.Payloads;

namespace QRCoder.Builders.Renderers.Implementations
{
    public class PngRenderer : RendererBase, IConfigurablePixelsPerModule, IStreamRenderer
    {
        private int _pixelsPerModule = 10;
        private byte[] _darkColor;
        private byte[] _lightColor;

        int IConfigurablePixelsPerModule.PixelsPerModule { get => _pixelsPerModule; set => _pixelsPerModule = value; }

#if !NETSTANDARD1_3
        public PngRenderer WithColors(System.Drawing.Color darkColor, System.Drawing.Color lightColor)
        {
            _darkColor = new byte[] { darkColor.R, darkColor.G, darkColor.B, darkColor.A };
            _lightColor = new byte[] { lightColor.R, lightColor.G, lightColor.B, lightColor.A };
            return this;
        }
#endif

        public PngRenderer WithColors(byte[] darkColor, byte[] lightColor)
        {
            _darkColor = darkColor;
            _lightColor = lightColor;
            return this;
        }

        public byte[] ToArray()
        {
            if (_darkColor == null && _lightColor == null)
                return new PngByteQRCode(QrCodeData).GetGraphic(_pixelsPerModule, QuietZone);
            return new PngByteQRCode(QrCodeData).GetGraphic(_pixelsPerModule, _darkColor, _lightColor, QuietZone);
        }

        public MemoryStream ToStream()
        {
            var arr = ToArray();
            return new MemoryStream(arr, 0, arr.Length, false, true);
        }
    }
}
