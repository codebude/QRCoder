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
        public string Text { get; }

        /// <summary>
        /// Gets the encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)
        /// </summary>
        public abstract EncodingMode EncodingMode { get; }

        /// <summary>
        /// Initializes a new instance of the DataSegment class.
        /// </summary>
        /// <param name="text">The text to encode</param>
        protected DataSegment(string text)
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
        public NumericDataSegment(string numericText)
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
            var data = PlainTextToBinaryNumeric(Text);
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

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
        public AlphanumericDataSegment(string alphanumericText)
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
            var data = AlphanumericEncoder.GetBitArray(Text);
            int length = modeIndicatorLength + countIndicatorLength + data.Length;

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
            var data = AlphanumericEncoder.GetBitArray(Text);
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

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
            var data = PlainTextToBinaryByte(Text, EciMode, Utf8BOM, ForceUtf8);
            int length = modeIndicatorLength + countIndicatorLength + data.Length;

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
            var data = PlainTextToBinaryByte(Text, EciMode, Utf8BOM, ForceUtf8);
            int characterCount = GetDataLength(EncodingMode.Byte, Text, data, ForceUtf8);
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            index = DecToBin(characterCount, countIndicatorLength, bitArray, index);

            // write data
            data.CopyTo(bitArray, 0, index, data.Length);
            index += data.Length;

            return index;
        }
    }
}
