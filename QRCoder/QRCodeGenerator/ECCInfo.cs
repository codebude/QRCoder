namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Represents the error correction coding (ECC) information for a specific version and error correction level of a QR code.
    /// </summary>
    private struct ECCInfo
    {
        /// <summary>
        /// Initializes a new instance of the ECCInfo struct with specified properties.
        /// </summary>
        /// <param name="version">The version number of the QR code.</param>
        /// <param name="errorCorrectionLevel">The error correction level used in the QR code.</param>
        /// <param name="totalDataCodewords">The total number of data codewords for this version and error correction level.</param>
        /// <param name="eccPerBlock">The number of error correction codewords per block.</param>
        /// <param name="blocksInGroup1">The number of blocks in group 1.</param>
        /// <param name="codewordsInGroup1">The number of codewords in each block of group 1.</param>
        /// <param name="blocksInGroup2">The number of blocks in group 2, if any.</param>
        /// <param name="codewordsInGroup2">The number of codewords in each block of group 2, if any.</param>
        public ECCInfo(int version, ECCLevel errorCorrectionLevel, int totalDataCodewords, int eccPerBlock, int blocksInGroup1,
            int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
        {
            Version = version;
            ErrorCorrectionLevel = errorCorrectionLevel;
            TotalDataCodewords = totalDataCodewords;
            ECCPerBlock = eccPerBlock;
            BlocksInGroup1 = blocksInGroup1;
            CodewordsInGroup1 = codewordsInGroup1;
            BlocksInGroup2 = blocksInGroup2;
            CodewordsInGroup2 = codewordsInGroup2;
        }

        /// <summary>
        /// Gets the version number of the QR code.
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Gets the error correction level of the QR code.
        /// </summary>
        public ECCLevel ErrorCorrectionLevel { get; }

        /// <summary>
        /// Gets the total number of data codewords for this version and error correction level.
        /// </summary>
        public int TotalDataCodewords { get; }

        /// <summary>
        /// Gets the number of error correction codewords per block.
        /// </summary>
        public int ECCPerBlock { get; }

        /// <summary>
        /// Gets the number of blocks in group 1.
        /// </summary>
        public int BlocksInGroup1 { get; }

        /// <summary>
        /// Gets the number of codewords in each block of group 1.
        /// </summary>
        public int CodewordsInGroup1 { get; }

        /// <summary>
        /// Gets the number of blocks in group 2, if any.
        /// </summary>
        public int BlocksInGroup2 { get; }

        /// <summary>
        /// Gets the number of codewords in each block of group 2, if any.
        /// </summary>
        public int CodewordsInGroup2 { get; }
    }
}
