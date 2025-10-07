namespace QRCoder.Builders.Payloads;

public interface IConfigurableEciMode
{
    QRCodeGenerator.EciMode EciMode { get; set; }
}
