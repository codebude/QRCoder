namespace QRCoder.Builders.Payloads;

public interface IConfigurableEccLevel
{
    public QRCodeGenerator.ECCLevel EccLevel { get; set; }
}
