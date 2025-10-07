namespace QRCoder.Builders.Payloads;

public abstract class PayloadBase : IPayload, IConfigurableEccLevel, IConfigurableEciMode, IConfigurableVersion
{
    protected virtual QRCodeGenerator.ECCLevel EccLevel { get; set; } = QRCodeGenerator.ECCLevel.Default;
    QRCodeGenerator.ECCLevel IConfigurableEccLevel.EccLevel { get => EccLevel; set => EccLevel = value; }

    protected virtual QRCodeGenerator.EciMode EciMode { get; set; } = QRCodeGenerator.EciMode.Default;
    QRCodeGenerator.EciMode IConfigurableEciMode.EciMode { get => EciMode; set => EciMode = value; }

    protected virtual int Version { get; set; } = -1;
    int IConfigurableVersion.Version { get => Version; set => Version = value; }

    protected abstract string Value { get; }

    public virtual QRCodeData ToMatrix()
    {
        return QRCodeGenerator.GenerateQrCode(Value, EccLevel, false, false, EciMode, Version);
    }
}
