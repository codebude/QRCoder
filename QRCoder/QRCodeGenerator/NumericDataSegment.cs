namespace QRCoder;

public partial class QRCodeGenerator
{
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
        var index = 0;

        // Process each group of three digits.
        for (int i = 0; i < plainText.Length - 2; i += 3)
        {
            // Parse the next three characters as a decimal integer.
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            // Convert the decimal to binary and store it in the BitArray.
            index = DecToBin(dec, 10, bitArray, index);
        }

        // Handle any remaining digits if the total number is not a multiple of three.
        if (plainText.Length % 3 == 2)  // Two remaining digits are encoded in 7 bits.
        {
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            index = DecToBin(dec, 7, bitArray, index);
        }
        else if (plainText.Length % 3 == 1)  // One remaining digit is encoded in 4 bits.
        {
#if HAS_SPAN
            var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#else
            var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
            index = DecToBin(dec, 4, bitArray, index);
        }

        return bitArray;
    }
}
