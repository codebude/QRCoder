namespace QRCoder.Builders.Payloads;

public interface IConfigurableEccLevel
{
    QRCodeGenerator.ECCLevel EccLevel { get; set; }
}
