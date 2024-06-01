using System;
using System.IO;
using System.Text;
using QRCoder.Builders.Renderers;

namespace QRCoder
{
    public static class RendererExtensions
    {
        public static T WithQuietZone<T>(this T payload, bool value)
            where T : IConfigurableQuietZones
        {
            payload.QuietZone = value;
            return payload;
        }

        public static T WithPixelsPerModule<T>(this T payload, int value)
            where T : IConfigurablePixelsPerModule
        {
            payload.PixelsPerModule = value;
            return payload;
        }

        public static void ToStream(this IStreamRenderer streamRenderer, Stream stream)
        {
            var memoryStream = streamRenderer.ToStream();
            memoryStream.CopyTo(stream);
        }

        public static byte[] ToArray(this IStreamRenderer streamRenderer)
        {
            var memoryStream = streamRenderer.ToStream();
#if NETSTANDARD || NETCOREAPP // todo: target .NET Framework 4.6 or newer so this code path is supported
            // by using TryGetBuffer, there is extremely small consequence to wrapping a byte[] in a MemoryStream temporarily
            if (memoryStream.TryGetBuffer(out var buffer) && buffer.Count == buffer.Array.Length)
            {
                return buffer.Array;
            }
#endif
            return memoryStream.ToArray();
        }

        public static ArraySegment<byte> ToArraySegment(this IStreamRenderer streamRenderer)
        {
            var memoryStream = streamRenderer.ToStream();
#if NETSTANDARD || NETCOREAPP // todo: target .NET Framework 4.6 or newer so this code path is supported
            if (memoryStream.TryGetBuffer(out var buffer))
            {
                return buffer;
            }
#else
            try
            {
                var buffer = memoryStream.GetBuffer();
                return new ArraySegment<byte>(buffer, 0, (int)memoryStream.Length);
            }
            catch { }
#endif
            return new ArraySegment<byte>(memoryStream.ToArray());
        }

        public static string ToBase64String(this IStreamRenderer streamRenderer)
        {
            var data = ToArraySegment(streamRenderer);
            return Convert.ToBase64String(data.Array, data.Offset, data.Count);
        }

        public static void ToFile(this IStreamRenderer streamRenderer, string fileName)
        {
            var memoryStream = streamRenderer.ToStream();
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                memoryStream.CopyTo(file);
            }
        }

        public static void ToFile(this ITextRenderer textRenderer, string fileName, Encoding encoding = null)
        {
            File.WriteAllText(fileName, textRenderer.ToString(), encoding ?? Encoding.UTF8);
        }

        public static void ToStream(this ITextRenderer textRenderer, Stream stream, Encoding encoding = null)
        {
            using (var writer = new StreamWriter(stream, encoding))
            {
                writer.Write(textRenderer.ToString());
                writer.Flush();
            }
        }

        public static MemoryStream ToStream(this ITextRenderer textRenderer, Encoding encoding = null)
        {
            var str = textRenderer.ToString();
            var ms = new MemoryStream(str.Length);
            using (var writer = new StreamWriter(ms, encoding ?? Encoding.UTF8))
            {
                writer.Write(str);
                writer.Flush();
            }
            ms.Position = 0;
            return ms;
        }
    }
}
