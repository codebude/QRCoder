using BenchmarkDotNet.Attributes;

namespace QRCoderBenchmarks;

[MemoryDiagnoser]
public class QRCodeGenerator
{
    [Benchmark]
    public void CreateQRCode()
    {
        var payload = new QRCoder.PayloadGenerator.Url("HTTP://WWW.GOOGLE.COM/");
        QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
        _ = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.L);
    }

    [Benchmark]
    public void CreateQRCodeLong()
    {
        var payload = new QRCoder.PayloadGenerator.Url("https://github.com/codebude/QRCoder/blob/f89aa90081f369983a9ba114e49cc6ebf0b2a7b1/QRCoder/Framework4.0Methods/Stream4Methods.cs");
        QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
        _ = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.H);
    }
}
