using BenchmarkDotNet.Attributes;
using QRCoder;

namespace QRCoderBenchmarks;

[MemoryDiagnoser]
public class PngByteQRCodeBenchmark
{
    private readonly Dictionary<string, QRCodeData> _samples;

    public PngByteQRCodeBenchmark()
    {
        var eccLvl = QRCoder.QRCodeGenerator.ECCLevel.L;
        _samples = new Dictionary<string, QRCodeData>()
        {
            { "small", QRCoder.QRCodeGenerator.GenerateQrCode("ABCD", eccLvl) },
            { "medium", QRCoder.QRCodeGenerator.GenerateQrCode("https://github.com/codebude/QRCoder/blob/f89aa90081f369983a9ba114e49cc6ebf0b2a7b1/QRCoder/Framework4.0Methods/Stream4Methods.cs", eccLvl) },
            { "big", QRCoder.QRCodeGenerator.GenerateQrCode( new string('a', 2600), eccLvl) }
        };
    }


    [Benchmark]
    public void RenderPngByteQRCodeSmall()
    {
        var qrCode = new PngByteQRCode(_samples["small"]);
        _ = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderPngByteQRCodeMedium()
    {
        var qrCode = new PngByteQRCode(_samples["medium"]);
        _ = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderPngByteQRCodeBig()
    {
        var qrCode = new PngByteQRCode(_samples["big"]);
        _ = qrCode.GetGraphic(10);
    }

    [Benchmark]
    public void RenderPngByteQRCodeHuge()
    {
        var qrCode = new PngByteQRCode(_samples["big"]);
        _ = qrCode.GetGraphic(50);
    }
}
