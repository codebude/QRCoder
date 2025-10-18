#if NETSTANDARD2_0

using System;
using System.Collections.Generic;
using System.Text;

namespace QRCoder;

internal static class StreamExtensions
{
    public static void Write(this Stream stream, ReadOnlySpan<byte> bytes)
    {
        stream.Write(bytes.ToArray(), 0, bytes.Length);
    }
}

#endif
