using QRCoder.Builders.Payloads;

namespace QRCoder.Builders.Renderers;

public interface IRenderer
{
    public IPayload Payload { set; }
}
