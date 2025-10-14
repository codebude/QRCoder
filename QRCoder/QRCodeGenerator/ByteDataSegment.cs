#if HAS_SPAN
using System.Buffers;
#endif

namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Data segment for byte mode encoding (used for UTF-8 and other text encodings).
    /// </summary>
    private sealed class ByteDataSegment : DataSegment
    {
        /// <summary>
        /// Whether to force UTF-8 encoding
        /// </summary>
        public bool ForceUtf8 { get; }

        /// <summary>
        /// Whether to include UTF-8 BOM
        /// </summary>
        public bool Utf8BOM { get; }

        /// <summary>
        /// The ECI mode to use
        /// </summary>
        public EciMode EciMode { get; }

        /// <summary>
        /// Whether this segment includes an ECI mode indicator
        /// </summary>
        public bool HasEciMode => EciMode != EciMode.Default;

        /// <summary>
        /// Gets the encoding mode (always Byte)
        /// </summary>
        public override EncodingMode EncodingMode => EncodingMode.Byte;

        /// <summary>
        /// Initializes a new instance of the ByteDataSegment class.
        /// </summary>
        /// <param name="text">The text to encode</param>
        /// <param name="forceUtf8">Whether to force UTF-8 encoding</param>
        /// <param name="utf8BOM">Whether to include UTF-8 BOM</param>
        /// <param name="eciMode">The ECI mode to use</param>
        public ByteDataSegment(string text, bool forceUtf8, bool utf8BOM, EciMode eciMode)
            : base(text)
        {
            ForceUtf8 = forceUtf8;
            Utf8BOM = utf8BOM;
            EciMode = eciMode;
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment</returns>
        public override int GetBitLength(int version)
        {
            int modeIndicatorLength = HasEciMode ? 16 : 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            int dataBitLength = GetPlainTextToBinaryByteBitLength(Text, EciMode, Utf8BOM, ForceUtf8);
            int length = modeIndicatorLength + countIndicatorLength + dataBitLength;

            return length;
        }

        /// <summary>
        /// Writes this data segment to an existing BitArray at the specified index.
        /// </summary>
        /// <param name="bitArray">The target BitArray to write to</param>
        /// <param name="startIndex">The starting index in the BitArray</param>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The next index in the BitArray after the last bit written</returns>
        public override int WriteTo(BitArray bitArray, int startIndex, int version)
        {
            var index = startIndex;

            // write eci mode if present
            if (HasEciMode)
            {
                index = DecToBin((int)EncodingMode.ECI, 4, bitArray, index);
                index = DecToBin((int)EciMode, 8, bitArray, index);
            }

            // write mode indicator
            index = DecToBin((int)EncodingMode.Byte, 4, bitArray, index);

            // write count indicator
            int dataBitLength = GetPlainTextToBinaryByteBitLength(Text, EciMode, Utf8BOM, ForceUtf8);
            int characterCount = dataBitLength / 8;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            index = DecToBin(characterCount, countIndicatorLength, bitArray, index);

            // write data directly to the bit array
            index = PlainTextToBinaryByte(Text, EciMode, Utf8BOM, ForceUtf8, bitArray, index);

            return index;
        }
    }

    private static readonly Encoding _iso8859_1 =
#if NET5_0_OR_GREATER
        Encoding.Latin1;
#else
        Encoding.GetEncoding(28591); // ISO-8859-1
#endif
    private static Encoding? _iso8859_2;

    /// <summary>
    /// Determines the target encoding for the given text and encoding parameters.
    /// </summary>
    /// <param name="plainText">The text to be encoded.</param>
    /// <param name="eciMode">The ECI mode that specifies the character encoding to use.</param>
    /// <param name="utf8BOM">Specifies whether to include a Byte Order Mark (BOM) for UTF-8 encoding.</param>
    /// <param name="forceUtf8">Forces UTF-8 encoding regardless of the text content's compatibility with ISO-8859-1.</param>
    /// <param name="includeUtf8BOM">Output parameter indicating whether the UTF-8 BOM should be included.</param>
    /// <returns>The encoding to use for the text.</returns>
    private static Encoding GetTargetEncoding(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8, out bool includeUtf8BOM)
    {
        Encoding targetEncoding;

        // Check if the text is valid ISO-8859-1 and UTF-8 is not forced, then encode using ISO-8859-1.
        if (IsValidISO(plainText) && !forceUtf8)
        {
            targetEncoding = _iso8859_1;
            includeUtf8BOM = false;
        }
        else
        {
            // Determine the encoding based on the specified ECI mode.
            switch (eciMode)
            {
                case EciMode.Iso8859_1:
                    // Convert text to ISO-8859-1 and encode.
                    targetEncoding = _iso8859_1;
                    includeUtf8BOM = false;
                    break;
                case EciMode.Iso8859_2:
                    // Note: ISO-8859-2 is not natively supported on .NET Core
                    //
                    // Users must install the System.Text.Encoding.CodePages package and call Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
                    // before using this encoding mode.
                    _iso8859_2 ??= Encoding.GetEncoding(28592); // ISO-8859-2
                    // Convert text to ISO-8859-2 and encode.
                    targetEncoding = _iso8859_2;
                    includeUtf8BOM = false;
                    break;
                case EciMode.Default:
                case EciMode.Utf8:
                default:
                    // Handle UTF-8 encoding, optionally adding a BOM if specified.
                    targetEncoding = Encoding.UTF8;
                    includeUtf8BOM = utf8BOM;
                    break;
            }
        }

        return targetEncoding;
    }

    /// <summary>
    /// Calculates the bit length required to encode plain text using byte mode encoding.
    /// </summary>
    /// <param name="plainText">The text to be encoded.</param>
    /// <param name="eciMode">The ECI mode that specifies the character encoding to use.</param>
    /// <param name="utf8BOM">Specifies whether to include a Byte Order Mark (BOM) for UTF-8 encoding.</param>
    /// <param name="forceUtf8">Forces UTF-8 encoding regardless of the text content's compatibility with ISO-8859-1.</param>
    /// <returns>The number of bits required to encode the text.</returns>
    private static int GetPlainTextToBinaryByteBitLength(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
    {
        var targetEncoding = GetTargetEncoding(plainText, eciMode, utf8BOM, forceUtf8, out var includeUtf8BOM);
        int byteCount = targetEncoding.GetByteCount(plainText);
        return (byteCount * 8) + (includeUtf8BOM ? 24 : 0);
    }

    /// <summary>
    /// Converts plain text into a binary format using byte mode encoding, which supports various character encodings through ECI (Extended Channel Interpretations).
    /// </summary>
    /// <param name="plainText">The text to be encoded.</param>
    /// <param name="eciMode">The ECI mode that specifies the character encoding to use.</param>
    /// <param name="utf8BOM">Specifies whether to include a Byte Order Mark (BOM) for UTF-8 encoding.</param>
    /// <param name="forceUtf8">Forces UTF-8 encoding regardless of the text content's compatibility with ISO-8859-1.</param>
    /// <returns>A BitArray representing the binary data of the encoded text.</returns>
    /// <remarks>
    /// The returned text is always encoded as ISO-8859-1 unless either the text contains a non-ISO-8859-1 character or
    /// UTF-8 encoding is forced. This does not meet the QR Code standard, which requires the use of ECI to specify the encoding
    /// when not ISO-8859-1.
    /// </remarks>
    private static BitArray PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
    {
        int bitLength = GetPlainTextToBinaryByteBitLength(plainText, eciMode, utf8BOM, forceUtf8);
        var bitArray = new BitArray(bitLength);
        PlainTextToBinaryByte(plainText, eciMode, utf8BOM, forceUtf8, bitArray, 0);
        return bitArray;
    }

    /// <summary>
    /// Converts plain text into a binary format using byte mode encoding, writing directly to an existing BitArray at the specified offset.
    /// </summary>
    /// <param name="plainText">The text to be encoded.</param>
    /// <param name="eciMode">The ECI mode that specifies the character encoding to use.</param>
    /// <param name="utf8BOM">Specifies whether to include a Byte Order Mark (BOM) for UTF-8 encoding.</param>
    /// <param name="forceUtf8">Forces UTF-8 encoding regardless of the text content's compatibility with ISO-8859-1.</param>
    /// <param name="bitArray">The target BitArray to write to. Must be large enough to hold the encoded data.</param>
    /// <param name="offset">The starting offset in the BitArray where bits will be written.</param>
    /// <returns>The next offset in the BitArray after the last bit written.</returns>
    /// <remarks>
    /// The returned text is always encoded as ISO-8859-1 unless either the text contains a non-ISO-8859-1 character or
    /// UTF-8 encoding is forced. This does not meet the QR Code standard, which requires the use of ECI to specify the encoding
    /// when not ISO-8859-1.
    /// </remarks>
    private static int PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8, BitArray bitArray, int offset)
    {
        var targetEncoding = GetTargetEncoding(plainText, eciMode, utf8BOM, forceUtf8, out var includeUtf8BOM);

#if HAS_SPAN
        // We can use stackalloc for small arrays to prevent heap allocations
        const int MAX_STACK_SIZE_IN_BYTES = 512;

        int count = targetEncoding.GetByteCount(plainText);
        byte[]? bufferFromPool = null;
        Span<byte> codeBytes = (count <= MAX_STACK_SIZE_IN_BYTES)
            ? (stackalloc byte[MAX_STACK_SIZE_IN_BYTES])
            : (bufferFromPool = ArrayPool<byte>.Shared.Rent(count));
        codeBytes = codeBytes.Slice(0, count);
        targetEncoding.GetBytes(plainText, codeBytes);
#else
        byte[] codeBytes = targetEncoding.GetBytes(plainText);
#endif

        // Write the data to the BitArray
        if (includeUtf8BOM)
        {
            // write UTF8 preamble (EF BB BF) to the BitArray
            DecToBin(0xEF, 8, bitArray, offset);
            DecToBin(0xBB, 8, bitArray, offset + 8);
            DecToBin(0xBF, 8, bitArray, offset + 16);
            offset += 24;
        }
        CopyToBitArray(codeBytes, bitArray, offset);
        offset += (int)((uint)codeBytes.Length * 8);

#if HAS_SPAN
        if (bufferFromPool != null)
            ArrayPool<byte>.Shared.Return(bufferFromPool);
#endif

        return offset;
    }
}
