namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents a block of codewords in a QR code. QR codes are divided into several blocks for error correction purposes.
    /// Each block contains a series of data codewords followed by error correction codewords.
    /// </summary>
    private struct CodewordBlock
    {
        /// <summary>
        /// Initializes a new instance of the CodewordBlock struct with specified arrays of code words and error correction (ECC) words.
        /// </summary>
        /// <param name="codeWordsOffset">The offset of the data codewords within the main BitArray. Data codewords carry the actual information.</param>
        /// <param name="codeWordsLength">The length in bits of the data codewords within the main BitArray.</param>
        /// <param name="eccWords">The array of error correction codewords for this block. These codewords help recover the data if the QR code is damaged.</param>
        public CodewordBlock(int codeWordsOffset, int codeWordsLength, byte[] eccWords)
        {
            CodeWordsOffset = codeWordsOffset;
            CodeWordsLength = codeWordsLength;
            ECCWords = eccWords;
        }

        /// <summary>
        /// Gets the offset of the data codewords in the BitArray.
        /// </summary>
        public int CodeWordsOffset { get; }

        /// <summary>
        /// Gets the length of the data codewords in the BitArray.
        /// </summary>
        public int CodeWordsLength { get; }

        /// <summary>
        /// Gets the error correction codewords associated with this block.
        /// </summary>
        public byte[] ECCWords { get; }
    }
}
