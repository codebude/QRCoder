namespace QRCoder.Builders.Payloads;

public interface IConfigurableEciMode
{
    public QRCodeGenerator.EciMode EciMode { get; set; }
}
