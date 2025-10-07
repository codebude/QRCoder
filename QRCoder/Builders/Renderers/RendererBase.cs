using QRCoder.Builders.Payloads;

namespace QRCoder.Builders.Renderers;

public abstract class RendererBase : IRenderer, IConfigurableQuietZones
{
    protected QRCodeData QrCodeData { private set; get; }

    protected bool QuietZone { get; set; } = true;
    bool IConfigurableQuietZones.QuietZone { get => QuietZone; set => QuietZone = value; }
    IPayload IRenderer.Payload { set => QrCodeData = value.ToMatrix(); }
}
