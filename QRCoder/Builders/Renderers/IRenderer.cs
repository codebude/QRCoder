using QRCoder.Builders.Payloads;

namespace QRCoder.Builders.Renderers
{
    public interface IRenderer
    {
        IPayload Payload { set; }
    }
}
