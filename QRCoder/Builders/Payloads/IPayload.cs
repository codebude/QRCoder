namespace QRCoder.Builders.Payloads;

public interface IPayload
{
    public QRCodeData ToMatrix();
}
