namespace QRCoder.Builders.Renderers.Implementations;

public class AsciiRenderer : RendererBase, ITextRenderer
{
    private bool _small;
    private string _darkString = "██";
    private string _lightString = "  ";
    private int _repeatPerModule = 1;
    private string _endOfLine = System.Environment.NewLine;
    private bool _inverseDarkLight = false;

    public AsciiRenderer WithSmall()
    {
        _small = true;
        return this;
    }

    public AsciiRenderer WithText(string darkString, string lightString)
    {
        _darkString = darkString;
        _lightString = lightString;
        return this;
    }

    public AsciiRenderer WithRepeatPerModule(int repeatPerModule)
    {
        _repeatPerModule = repeatPerModule;
        return this;
    }

    public AsciiRenderer WithEndOfLine(string endOfLine)
    {
        _endOfLine = endOfLine;
        return this;
    }

    public AsciiRenderer WithInverseDarkLight()
    {
        _inverseDarkLight = true;
        return this;
    }

    public override string ToString()
    {
        return new AsciiQRCode(QrCodeData).GetGraphic(
            _repeatPerModule,
            _inverseDarkLight ? _lightString : _darkString,
            _inverseDarkLight ? _darkString : _lightString,
            QuietZone,
            _endOfLine);
    }
}
