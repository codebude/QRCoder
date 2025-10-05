using BenchmarkDotNet.Attributes;

namespace QRCoderBenchmarks;

[MemoryDiagnoser]
public class QRCodeGenerator
{
    [Benchmark]
    public void CreateQRCode()
    {
        var payload = new QRCoder.PayloadGenerator.Url("HTTP://WWW.GOOGLE.COM/");
        var qrGenerator = new QRCoder.QRCodeGenerator();
        _ = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.L);
    }

    [Benchmark]
    public void CreateQRCodeLong()
    {
        var payload = new QRCoder.PayloadGenerator.Url("https://github.com/Shane32/QRCoder/blob/f89aa90081f369983a9ba114e49cc6ebf0b2a7b1/QRCoder/Framework4.0Methods/Stream4Methods.cs");
        var qrGenerator = new QRCoder.QRCodeGenerator();
        _ = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.H);
    }

    [Benchmark]
    public void CreateQRCodeLongest()
    {
        var str = new string('a', 2600);
        var qrGenerator = new QRCoder.QRCodeGenerator();
        _ = qrGenerator.CreateQrCode(str, QRCoder.QRCodeGenerator.ECCLevel.L);
    }
}
