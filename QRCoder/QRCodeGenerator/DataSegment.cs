namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents an abstract data segment for QR code encoding.
    /// </summary>
    private abstract class DataSegment
    {
        /// <summary>
        /// The text to encode
        /// </summary>
#if HAS_SPAN
        public ReadOnlyMemory<char> Text { get; }
#else
        public string Text { get; }
#endif

        /// <summary>
        /// The next data segment in the chain, or null if this is the last segment
        /// </summary>
        public DataSegment? Next { get; set; }

        /// <summary>
        /// Gets the encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)
        /// </summary>
        public abstract EncodingMode EncodingMode { get; }

        /// <summary>
        /// Initializes a new instance of the DataSegment class.
        /// </summary>
        /// <param name="text">The text to encode</param>
#if HAS_SPAN
        protected DataSegment(ReadOnlyMemory<char> text)
#else
        protected DataSegment(string text)
#endif
        {
            Text = text;
        }

        /// <summary>
        /// Writes this data segment to an existing BitArray at the specified index for a specific QR code version.
        /// Chains to the next segment if present.
        /// </summary>
        /// <param name="bitArray">The target BitArray to write to</param>
        /// <param name="startIndex">The starting index in the BitArray where writing should begin</param>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The next index in the BitArray after the last bit written</returns>
        public abstract int WriteTo(BitArray bitArray, int startIndex, int version);

        /// <summary>
        /// Builds a complete BitArray from this data segment for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>A BitArray containing the complete encoded segment</returns>
        public BitArray ToBitArray(int version)
        {
            var bitArray = new BitArray(GetBitLength(version));
            WriteTo(bitArray, 0, version);
            return bitArray;
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// Includes the length of all chained segments.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment including mode indicator, count indicator, and data</returns>
        public abstract int GetBitLength(int version);
    }

    /// <summary>
    /// Standard implementation of a data segment for QR code encoding, containing the encoding mode, character count, and encoded data.
    /// </summary>
    private class StandardDataSegment : DataSegment
    {
        /// <summary>
        /// The encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)
        /// </summary>
        public override EncodingMode EncodingMode { get; }

        /// <summary>
        /// The character count (or byte count for byte mode)
        /// </summary>
        public int CharacterCount { get; }

        /// <summary>
        /// The encoded data as a BitArray
        /// </summary>
        public BitArray Data { get; }

        /// <summary>
        /// Whether this segment includes an ECI mode indicator
        /// </summary>
        public bool HasEciMode => EciMode != EciMode.Default;

        /// <summary>
        /// The ECI mode value (only valid if HasEciMode is true)
        /// </summary>
        public EciMode EciMode { get; }

        /// <summary>
        /// Initializes a new instance of the StandardDataSegment class.
        /// </summary>
        /// <param name="encodingMode">The encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)</param>
        /// <param name="characterCount">The character count (or byte count for byte mode)</param>
        /// <param name="data">The encoded data as a BitArray</param>
        /// <param name="eciMode">The ECI mode value (use EciMode.Default if not using ECI)</param>
        public StandardDataSegment(EncodingMode encodingMode, int characterCount, BitArray data, EciMode eciMode)
            : base(
#if HAS_SPAN
                ReadOnlyMemory<char>.Empty
#else
                string.Empty
#endif
            )
        {
            EncodingMode = encodingMode;
            CharacterCount = characterCount;
            Data = data;
            EciMode = eciMode;
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// Includes the length of all chained segments.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment including mode indicator, count indicator, and data</returns>
        public override int GetBitLength(int version)
        {
            int modeIndicatorLength = HasEciMode ? 16 : 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode);
            int length = modeIndicatorLength + countIndicatorLength + Data.Length;

            // Add length of next segment if present
            if (Next != null)
                length += Next.GetBitLength(version);

            return length;
        }

        /// <summary>
        /// Writes this data segment to an existing BitArray at the specified index for a specific QR code version.
        /// Chains to the next segment if present.
        /// </summary>
        /// <param name="bitArray">The target BitArray to write to</param>
        /// <param name="startIndex">The starting index in the BitArray where writing should begin</param>
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
            index = DecToBin((int)EncodingMode, 4, bitArray, index);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode);
            index = DecToBin(CharacterCount, countIndicatorLength, bitArray, index);

            // write data
            Data.CopyTo(bitArray, 0, index, Data.Length);
            index += Data.Length;

            // write next segment if present
            if (Next != null)
                index = Next.WriteTo(bitArray, index, version);

            return index;
        }
    }

    /// <summary>
    /// Data segment optimized for numeric data encoding.
    /// </summary>
    private class NumericDataSegment : DataSegment
    {
        /// <summary>
        /// Gets the encoding mode (always Numeric)
        /// </summary>
        public override EncodingMode EncodingMode => EncodingMode.Numeric;

        /// <summary>
        /// Initializes a new instance of the NumericDataSegment class.
        /// </summary>
        /// <param name="numericText">The numeric text to encode (should only contain digits)</param>
#if HAS_SPAN
        public NumericDataSegment(ReadOnlyMemory<char> numericText)
#else
        public NumericDataSegment(string numericText)
#endif
            : base(numericText)
        {
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment</returns>
        public override int GetBitLength(int version)
        {
            int modeIndicatorLength = 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Numeric);
            int dataLength = Text.Length / 3 * 10 + (Text.Length % 3 == 1 ? 4 : Text.Length % 3 == 2 ? 7 : 0);
            int length = modeIndicatorLength + countIndicatorLength + dataLength;

            // Add length of next segment if present
            if (Next != null)
                length += Next.GetBitLength(version);

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

            // write mode indicator
            index = DecToBin((int)EncodingMode.Numeric, 4, bitArray, index);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Numeric);
            index = DecToBin(Text.Length, countIndicatorLength, bitArray, index);

            // write data - encode numeric text
#if HAS_SPAN
            var data = PlainTextToBinaryNumeric(Text.ToString());
#else
            var data = PlainTextToBinaryNumeric(Text);
#endif
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

            // write next segment if present
            if (Next != null)
                index = Next.WriteTo(bitArray, index, version);

            return index;
        }
    }

    /// <summary>
    /// Data segment optimized for alphanumeric data encoding.
    /// </summary>
    private class AlphanumericDataSegment : DataSegment
    {
        /// <summary>
        /// Gets the encoding mode (always Alphanumeric)
        /// </summary>
        public override EncodingMode EncodingMode => EncodingMode.Alphanumeric;

        /// <summary>
        /// Initializes a new instance of the AlphanumericDataSegment class.
        /// </summary>
        /// <param name="alphanumericText">The alphanumeric text to encode</param>
#if HAS_SPAN
        public AlphanumericDataSegment(ReadOnlyMemory<char> alphanumericText)
#else
        public AlphanumericDataSegment(string alphanumericText)
#endif
            : base(alphanumericText)
        {
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment</returns>
        public override int GetBitLength(int version)
        {
            int modeIndicatorLength = 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Alphanumeric);
#if HAS_SPAN
            var data = AlphanumericEncoder.GetBitArray(Text.ToString());
#else
            var data = AlphanumericEncoder.GetBitArray(Text);
#endif
            int length = modeIndicatorLength + countIndicatorLength + data.Length;

            // Add length of next segment if present
            if (Next != null)
                length += Next.GetBitLength(version);

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

            // write mode indicator
            index = DecToBin((int)EncodingMode.Alphanumeric, 4, bitArray, index);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Alphanumeric);
            index = DecToBin(Text.Length, countIndicatorLength, bitArray, index);

            // write data - encode alphanumeric text
#if HAS_SPAN
            var data = AlphanumericEncoder.GetBitArray(Text.ToString());
#else
            var data = AlphanumericEncoder.GetBitArray(Text);
#endif
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

            // write next segment if present
            if (Next != null)
                index = Next.WriteTo(bitArray, index, version);

            return index;
        }
    }

    /// <summary>
    /// Data segment for byte mode encoding (used for UTF-8 and other text encodings).
    /// </summary>
    private class ByteDataSegment : DataSegment
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
#if HAS_SPAN
        public ByteDataSegment(ReadOnlyMemory<char> text, bool forceUtf8, bool utf8BOM, EciMode eciMode)
#else
        public ByteDataSegment(string text, bool forceUtf8, bool utf8BOM, EciMode eciMode)
#endif
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
#if HAS_SPAN
            var textStr = Text.ToString();
            var data = PlainTextToBinaryByte(textStr, EciMode, Utf8BOM, ForceUtf8);
            int dataLength = GetDataLength(EncodingMode.Byte, textStr, data, ForceUtf8);
#else
            var data = PlainTextToBinaryByte(Text, EciMode, Utf8BOM, ForceUtf8);
            int dataLength = GetDataLength(EncodingMode.Byte, Text, data, ForceUtf8);
#endif
            int length = modeIndicatorLength + countIndicatorLength + data.Length;

            // Add length of next segment if present
            if (Next != null)
                length += Next.GetBitLength(version);

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
#if HAS_SPAN
            var textStr = Text.ToString();
            var data = PlainTextToBinaryByte(textStr, EciMode, Utf8BOM, ForceUtf8);
            int characterCount = GetDataLength(EncodingMode.Byte, textStr, data, ForceUtf8);
#else
            var data = PlainTextToBinaryByte(Text, EciMode, Utf8BOM, ForceUtf8);
            int characterCount = GetDataLength(EncodingMode.Byte, Text, data, ForceUtf8);
#endif
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            index = DecToBin(characterCount, countIndicatorLength, bitArray, index);

            // write data
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

            // write next segment if present
            if (Next != null)
                index = Next.WriteTo(bitArray, index, version);

            return index;
        }
    }
}
