namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Data segment optimized for alphanumeric data encoding.
    /// </summary>
    private sealed class AlphanumericDataSegment : DataSegment
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
            int dataLength = AlphanumericEncoder.GetBitLength(Text);
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
            index = DecToBin((int)EncodingMode.Alphanumeric, 4, bitArray, index);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Alphanumeric);
            index = DecToBin(Text.Length, countIndicatorLength, bitArray, index);

            // write data - encode alphanumeric text
            index = AlphanumericEncoder.WriteToBitArray(Text, bitArray, index);

            return index;
        }
    }
}
