namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Data segment optimized for numeric data encoding.
    /// </summary>
    private sealed class NumericDataSegment : DataSegment
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
            return GetBitLength(Text.Length, version);
        }

        /// <summary>
        /// Calculates the total bit length for encoding numeric text of a given length for a specific QR code version.
        /// Includes mode indicator, count indicator, and data bits.
        /// </summary>
        /// <param name="textLength">The length of the numeric text</param>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required</returns>
        public static int GetBitLength(int textLength, int version)
        {
            int modeIndicatorLength = 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Numeric);
            int dataLength = textLength / 3 * 10 + (textLength % 3 == 1 ? 4 : textLength % 3 == 2 ? 7 : 0);
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
            return WriteTo(Text, 0, Text.Length, bitArray, startIndex, version);
        }

        /// <summary>
        /// Writes a portion of numeric text to a BitArray at the specified index.
        /// Includes mode indicator, count indicator, and data bits.
        /// </summary>
        /// <param name="text">The full numeric text</param>
        /// <param name="startIndex">The starting index in the text to encode from</param>
        /// <param name="length">The number of characters to encode</param>
        /// <param name="bitArray">The target BitArray to write to</param>
        /// <param name="bitIndex">The starting index in the BitArray</param>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The next index in the BitArray after the last bit written</returns>
        public static int WriteTo(string text, int startIndex, int length, BitArray bitArray, int bitIndex, int version)
        {
            var index = bitIndex;

            // write mode indicator
            index = DecToBin((int)EncodingMode.Numeric, 4, bitArray, index);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Numeric);
            index = DecToBin(length, countIndicatorLength, bitArray, index);

            // write data - encode numeric text
            index = PlainTextToBinaryNumeric(text, startIndex, length, bitArray, index);

            return index;
        }
    }

    /// <summary>
    /// Converts numeric plain text into a binary format specifically optimized for QR codes.
    /// Numeric compression groups up to 3 digits into 10 bits, less for remaining digits if they do not complete a group of three.
    /// </summary>
    /// <param name="plainText">The numeric text to be encoded, which should only contain digit characters.</param>
    /// <returns>A BitArray representing the binary data of the encoded numeric text.</returns>
    private static BitArray PlainTextToBinaryNumeric(string plainText)
    {
        // Calculate the length of the BitArray needed to encode the text.
        // Groups of three digits are encoded in 10 bits, remaining groups of two or one digits take 7 or 4 bits respectively.
        var bitArray = new BitArray(plainText.Length / 3 * 10 + (plainText.Length % 3 == 1 ? 4 : plainText.Length % 3 == 2 ? 7 : 0));
        PlainTextToBinaryNumeric(plainText, 0, plainText.Length, bitArray, 0);
        return bitArray;
    }

    /// <summary>
    /// Converts a portion of numeric plain text into a binary format specifically optimized for QR codes, writing directly to an existing BitArray.
    /// Numeric compression groups up to 3 digits into 10 bits, less for remaining digits if they do not complete a group of three.
    /// </summary>
    /// <param name="plainText">The numeric text to be encoded, which should only contain digit characters.</param>
    /// <param name="offset">The starting index in the text to encode from.</param>
    /// <param name="length">The number of characters to encode.</param>
    /// <param name="bitArray">The target BitArray to write to.</param>
    /// <param name="bitIndex">The starting index in the BitArray where bits will be written.</param>
    /// <returns>The next index in the BitArray after the last bit written.</returns>
    private static int PlainTextToBinaryNumeric(string plainText, int offset, int length, BitArray bitArray, int bitIndex)
    {
        var endIndex = offset + length;

        // Process each group of three digits.
        for (int i = offset; i < endIndex - 2; i += 3)
        {
            // Parse the next three characters as a decimal integer.
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            // Convert the decimal to binary and store it in the BitArray.
            bitIndex = DecToBin(dec, 10, bitArray, bitIndex);
            offset += 3;
            length -= 3;
        }

        // Handle any remaining digits if the total number is not a multiple of three.
        if (length == 2)  // Two remaining digits are encoded in 7 bits.
        {
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(offset, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(offset, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            bitIndex = DecToBin(dec, 7, bitArray, bitIndex);
        }
        else if (length == 1)  // One remaining digit is encoded in 4 bits.
        {
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(offset, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(offset, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            bitIndex = DecToBin(dec, 4, bitArray, bitIndex);
        }

        return bitIndex;
    }
}
