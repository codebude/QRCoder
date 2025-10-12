namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents a data segment for QR code encoding, containing the encoding mode, character count, and encoded data.
    /// </summary>
    private readonly struct DataSegment
    {
        /// <summary>
        /// The encoding mode for this segment (Numeric, Alphanumeric, Byte, etc.)
        /// </summary>
        public readonly EncodingMode EncodingMode;

        /// <summary>
        /// The character count (or byte count for byte mode)
        /// </summary>
        public readonly int CharacterCount;

        /// <summary>
        /// The encoded data as a BitArray
        /// </summary>
        public readonly BitArray Data;

        /// <summary>
        /// Whether this segment includes an ECI mode indicator
        /// </summary>
        public bool HasEciMode => EciMode == EciMode.Default;

        /// <summary>
        /// The ECI mode value (only valid if HasEciMode is true)
        /// </summary>
        public readonly EciMode EciMode;

        public DataSegment(EncodingMode encodingMode, int characterCount, BitArray data, EciMode eciMode)
        {
            EncodingMode = encodingMode;
            CharacterCount = characterCount;
            Data = data;
            EciMode = eciMode;
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment including mode indicator, count indicator, and data</returns>
        public int GetBitLength(int version)
        {
            int modeIndicatorLength = HasEciMode ? 16 : 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode);
            return modeIndicatorLength + countIndicatorLength + Data.Length;
        }
    }
}
