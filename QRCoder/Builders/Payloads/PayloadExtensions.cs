using QRCoder.Builders.Payloads;
using QRCoder.Builders.Renderers;
using QRCoder.Builders.Renderers.Implementations;

namespace QRCoder
{
    public static class PayloadExtensions
    {
        public static T WithErrorCorrection<T>(this T payload, QRCodeGenerator.ECCLevel eccLevel)
            where T : IConfigurableEccLevel
        {
            payload.EccLevel = eccLevel;
            return payload;
        }

        public static T WithEciMode<T>(this T payload, QRCodeGenerator.EciMode eciMode)
            where T : IConfigurableEciMode
        {
            payload.EciMode = eciMode;
            return payload;
        }

        public static T WithVersion<T>(this T payload, int version)
            where T : IConfigurableVersion
        {
            payload.Version = version;
            return payload;
        }

        public static T RenderWith<T>(this IPayload payload)
            where T : IRenderer, new()
        {
            var renderer = new T();
            renderer.Payload = payload;
            return renderer;
        }

        public static T RenderWith<T>(this IPayload payload, int pixelsPerModule)
            where T : IRenderer, IConfigurablePixelsPerModule, new()
        {
            var renderer = new T();
            renderer.Payload = payload;
            renderer.PixelsPerModule = pixelsPerModule;
            return renderer;
        }
    }
}
