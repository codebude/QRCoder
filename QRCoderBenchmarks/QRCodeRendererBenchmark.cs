using BenchmarkDotNet.Attributes;
using QRCoder;
using System.Drawing;

namespace QRCoderBenchmarks;

[MemoryDiagnoser]
public class QRCodeRendererBenchmark
{
    private readonly Dictionary<string, QRCodeData> _samples;

    public QRCodeRendererBenchmark()
    {
        var eccLvl = QRCoder.QRCodeGenerator.ECCLevel.L;
        _samples = new Dictionary<string, QRCodeData>()
        {
            { "small", QRCoder.QRCodeGenerator.GenerateQrCode("ABCD", eccLvl) },
            { "medium", QRCoder.QRCodeGenerator.GenerateQrCode("https://github.com/Shane32/QRCoder/blob/f89aa90081f369983a9ba114e49cc6ebf0b2a7b1/QRCoder/Framework4.0Methods/Stream4Methods.cs", eccLvl) },
            { "big", QRCoder.QRCodeGenerator.GenerateQrCode( new string('a', 2600), eccLvl) }
        };
    }

    [Benchmark]
    public void RenderQRCodeSmall()
    {
        var qrCode = new QRCode(_samples["small"]);
        using var bmp = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderQRCodeMedium()
    {
        var qrCode = new QRCode(_samples["medium"]);
        using var bmp = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderQRCodeBig()
    {
        var qrCode = new QRCode(_samples["big"]);
        using var bmp = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderQRCodeHuge()
    {
        var qrCode = new QRCode(_samples["big"]);
        using var bmp = qrCode.GetGraphic(50);
    }
}
