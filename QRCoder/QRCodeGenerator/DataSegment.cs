namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents a data segment for QR code encoding, containing the encoding mode, character count, and encoded data.
    /// </summary>
    private class DataSegment
    {
        /// <summary>
        /// The encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)
        /// </summary>
        public EncodingMode EncodingMode { get; }

        /// <summary>
        /// The character count (or byte count for byte mode)
        /// </summary>
        public int CharacterCount { get; }

        /// <summary>
        /// The encoded data as a BitArray
        /// </summary>
        public BitArray Data { get; }

        /// <summary>
        /// The next data segment in the chain, or null if this is the last segment
        /// </summary>
        public DataSegment? Next { get; set; }

        /// <summary>
        /// Whether this segment includes an ECI mode indicator
        /// </summary>
        public bool HasEciMode => EciMode != EciMode.Default;

        /// <summary>
        /// The ECI mode value (only valid if HasEciMode is true)
        /// </summary>
        public EciMode EciMode { get; }

        /// <summary>
        /// Initializes a new instance of the DataSegment struct.
        /// </summary>
        /// <param name="encodingMode">The encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)</param>
        /// <param name="characterCount">The character count (or byte count for byte mode)</param>
        /// <param name="data">The encoded data as a BitArray</param>
        /// <param name="eciMode">The ECI mode value (use EciMode.Default if not using ECI)</param>
        public DataSegment(EncodingMode encodingMode, int characterCount, BitArray data, EciMode eciMode)
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
        public int GetBitLength(int version)
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
        /// Builds a complete BitArray from this data segment for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>A BitArray containing the complete encoded segment including mode indicator, count indicator, and data</returns>
        public BitArray ToBitArray(int version)
        {
            var bitArray = new BitArray(GetBitLength(version));
            WriteTo(bitArray, 0, version);
            return bitArray;
        }

        /// <summary>
        /// Writes this data segment to an existing BitArray at the specified index for a specific QR code version.
        /// Chains to the next segment if present.
        /// </summary>
        /// <param name="bitArray">The target BitArray to write to</param>
        /// <param name="startIndex">The starting index in the BitArray where writing should begin</param>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The next index in the BitArray after the last bit written</returns>
        public int WriteTo(BitArray bitArray, int startIndex, int version)
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
}
