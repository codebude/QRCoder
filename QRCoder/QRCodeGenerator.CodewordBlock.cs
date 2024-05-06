using System.Collections;

namespace QRCoder
{
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
            /// <param name="codeWords">The array of data codewords for this block. Data codewords carry the actual information.</param>
            /// <param name="eccWords">The array of error correction codewords for this block. These codewords help recover the data if the QR code is damaged.</param>
            public CodewordBlock(/* byte[] codeWordsArray, */ BitArray codeWords, int codeWordsOffset, int codeWordsLength, byte[] eccWords)
            {
                //this.CodeWordsArray = codeWordsArray;
                this.CodeWords = codeWords;
                this.CodeWordsOffset = codeWordsOffset;
                this.CodeWordsLength = codeWordsLength;
                this.ECCWords = eccWords;
            }

            //public byte[] CodeWordsArray { get; }

            /// <summary>
            /// Gets the data codewords associated with this block. 
            /// </summary>
            public BitArray CodeWords { get; }

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
}
