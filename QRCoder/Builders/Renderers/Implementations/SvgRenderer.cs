#if !NETSTANDARD1_3
using System.Drawing;

namespace QRCoder.Builders.Renderers.Implementations;

public class SvgRenderer : RendererBase, IConfigurablePixelsPerModule, ITextRenderer
{
    private int _pixelsPerModule = 10;
    private Color _darkColor;
    private Color _lightColor;
    private SvgQRCode.SvgLogo _logo;
    private SvgQRCode.SizingMode _sizingMode = SvgQRCode.SizingMode.WidthHeightAttribute;

    int IConfigurablePixelsPerModule.PixelsPerModule { get => _pixelsPerModule; set => _pixelsPerModule = value; }

    public SvgRenderer WithColors(Color darkColor, Color lightColor)
    {
        _darkColor = darkColor;
        _lightColor = lightColor;
        return this;
    }

    public SvgRenderer WithLogo(SvgQRCode.SvgLogo logo)
    {
        _logo = logo;
        return this;
    }

    public SvgRenderer WithSizingMode(SvgQRCode.SizingMode sizingMode)
    {
        _sizingMode = sizingMode;
        return this;
    }

    public override string ToString()
    {
        return new SvgQRCode(QrCodeData).GetGraphic(
            _pixelsPerModule, _darkColor, _lightColor, QuietZone, _sizingMode, _logo);
    }
}
#endif
