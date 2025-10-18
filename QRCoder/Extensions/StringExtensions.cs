using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace QRCoder;

internal static class StringExtensions
{
    /// <summary>
    /// Converts a hex color string to a byte array.
    /// </summary>
    /// <param name="colorString">Color in HEX format like #ffffff.</param>
    /// <returns>Returns the color as a byte array.</returns>
    internal static byte[] HexColorToByteArray(this string colorString)
    {
        var offset = 0;
        if (colorString.StartsWith('#'))
            offset = 1;
        byte[] byteColor = new byte[(colorString.Length - offset) / 2];
        for (int i = 0; i < byteColor.Length; i++)
#if !NETSTANDARD2_0
            byteColor[i] = byte.Parse(colorString.AsSpan(i * 2 + offset, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
#else
            byteColor[i] = byte.Parse(colorString.Substring(i * 2 + offset, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
#endif
        return byteColor;
    }

    /// <summary>
    /// Appends an integer value to the StringBuilder using invariant culture formatting.
    /// </summary>
    /// <param name="sb">The StringBuilder to append to.</param>
    /// <param name="num">The integer value to append.</param>
    internal static void AppendInvariant(this StringBuilder sb, int num)
    {
#if NET6_0_OR_GREATER
        sb.Append(CultureInfo.InvariantCulture, $"{num}");
#else
#if !NETSTANDARD2_0
        Span<char> buffer = stackalloc char[16];
        if (num.TryFormat(buffer, out int charsWritten, default, CultureInfo.InvariantCulture))
        {
            sb.Append(buffer.Slice(0, charsWritten));
            return;
        }
#endif
        sb.Append(num.ToString(CultureInfo.InvariantCulture));
#endif
    }

    /// <summary>
    /// Appends a float value to the StringBuilder using invariant culture formatting with G7 precision.
    /// </summary>
    /// <param name="sb">The StringBuilder to append to.</param>
    /// <param name="num">The float value to append.</param>
    internal static void AppendInvariant(this StringBuilder sb, float num)
    {
#if NET6_0_OR_GREATER
        sb.Append(CultureInfo.InvariantCulture, $"{num:G7}");
#else
#if !NETSTANDARD2_0
        Span<char> buffer = stackalloc char[16];
        if (num.TryFormat(buffer, out int charsWritten, "G7", CultureInfo.InvariantCulture))
        {
            sb.Append(buffer.Slice(0, charsWritten));
            return;
        }
#endif
        sb.Append(num.ToString("G7", CultureInfo.InvariantCulture));
#endif
    }

#if NETSTANDARD2_0
    public static bool StartsWith(this string target, char value)
    {
        return target.Length > 0 && target[0] == value;
    }
#endif
}
