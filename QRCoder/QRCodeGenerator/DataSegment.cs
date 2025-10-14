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
}
