namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Specifies the encoding modes for the characters in a QR code.
    /// </summary>
    internal enum EncodingMode
    {
        /// <summary>
        /// Numeric encoding mode, which is used to encode numeric data (digits 0-9).
        /// Three characters are encoded into 10 bits.
        /// </summary>
        Numeric = 1,

        /// <summary>
        /// Alphanumeric encoding mode, which is used to encode alphanumeric characters (0-9, A-Z, space, and some punctuation).
        /// Two characters are encoded into 11 bits.
        /// </summary>
        Alphanumeric = 2,

        /// <summary>
        /// Byte encoding mode, primarily using the ISO-8859-1 character set. Each character is encoded into 8 bits.
        /// When combined with ECI, it can be adapted to use other character sets.
        /// </summary>
        Byte = 4,

        /// <summary>
        /// Kanji encoding mode, which is used to encode characters from the Shift JIS character set, primarily for Japanese Kanji and Kana characters.
        /// One character is encoded into 13 bits. This mode is not currently supported by QRCoder.
        /// </summary>
        Kanji = 8,

        /// <summary>
        /// Extended Channel Interpretation (ECI) mode, which specifies a character set via an 8-bit number followed by one of the other encoding modes.
        /// This allows adapting the byte encoding to accommodate various global text encodings.
        /// </summary>
        ECI = 7
    }
}
