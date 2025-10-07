using System.IO;

namespace QRCoder.Builders.Renderers;

public interface IStreamRenderer
{
    public MemoryStream ToStream();
}
