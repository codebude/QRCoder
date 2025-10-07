namespace QRCoder.Builders.Payloads;

public interface IPayload
{
    QRCodeData ToMatrix();
}
