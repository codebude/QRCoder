namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Encodes alphanumeric characters (<c>0–9</c>, <c>A–Z</c> (uppercase), space, <c>$</c>, <c>%</c>, <c>*</c>, <c>+</c>, <c>-</c>, period, <c>/</c>, colon) into a binary format suitable for QR codes.
    /// </summary>
    private static class AlphanumericEncoder
    {
        private static readonly char[] _alphanumEncTable = { ' ', '$', '%', '*', '+', '-', '.', '/', ':' };

        /// <summary>
        /// A dictionary mapping alphanumeric characters to their respective positions used in QR code encoding.
        /// This includes digits 0-9, uppercase letters A-Z, and some special characters.
        /// </summary>
        private static readonly Dictionary<char, int> _alphanumEncDict = CreateAlphanumEncDict(_alphanumEncTable);

        /// <summary>
        /// Creates a dictionary mapping alphanumeric characters to their respective positions used in QR code encoding.
        /// This includes digits 0-9, uppercase letters A-Z, and some special characters.
        /// </summary>
        /// <returns>A dictionary mapping each supported alphanumeric character to its corresponding value.</returns>
        private static Dictionary<char, int> CreateAlphanumEncDict(char[] alphanumEncTable)
        {
            var localAlphanumEncDict = new Dictionary<char, int>(45);
            // Add 0-9
            for (char c = '0'; c <= '9'; c++)
                localAlphanumEncDict.Add(c, c - '0');
            // Add uppercase alphabetic characters.
            for (char c = 'A'; c <= 'Z'; c++)
                localAlphanumEncDict.Add(c, localAlphanumEncDict.Count);
            // Add special characters from a predefined table.
            for (int i = 0; i < _alphanumEncTable.Length; i++)
                localAlphanumEncDict.Add(alphanumEncTable[i], localAlphanumEncDict.Count);
            return localAlphanumEncDict;
        }

        /// <summary>
        /// Checks if a non-digit character is present in the alphanumeric encoding table.
        /// </summary>
        public static bool CanEncodeNonDigit(char c) => IsInRange(c, 'A', 'Z') || Array.IndexOf(_alphanumEncTable, c) >= 0;

        /// <summary>
        /// Calculates the bit length required to encode alphanumeric text of a given length.
        /// </summary>
        /// <param name="textLength">The length of the alphanumeric text to be encoded.</param>
        /// <returns>The number of bits required to encode the text.</returns>
        public static int GetBitLength(int textLength)
        {
            return (textLength / 2) * 11 + (textLength & 1) * 6;
        }

        /// <summary>
        /// Converts alphanumeric plain text into a binary format optimized for QR codes.
        /// Alphanumeric encoding packs characters into 11-bit groups for each pair of characters,
        /// and 6 bits for a single remaining character if the total count is odd.
        /// </summary>
        /// <param name="plainText">The alphanumeric text to be encoded, which should only contain characters valid in QR alphanumeric mode.</param>
        /// <returns>A BitArray representing the binary data of the encoded alphanumeric text.</returns>
        public static BitArray GetBitArray(string plainText)
        {
            var codeText = new BitArray(GetBitLength(plainText.Length));
            WriteToBitArray(plainText, 0, plainText.Length, codeText, 0);
            return codeText;
        }

        /// <summary>
        /// Writes a portion of alphanumeric plain text directly into an existing BitArray at the specified index.
        /// Alphanumeric encoding packs characters into 11-bit groups for each pair of characters,
        /// and 6 bits for a single remaining character if the total count is odd.
        /// </summary>
        /// <param name="plainText">The alphanumeric text to be encoded, which should only contain characters valid in QR alphanumeric mode.</param>
        /// <param name="startIndex">The starting index in the text to encode from.</param>
        /// <param name="length">The number of characters to encode.</param>
        /// <param name="codeText">The target BitArray to write to.</param>
        /// <param name="codeIndex">The starting index in the BitArray where writing should begin.</param>
        /// <returns>The next index in the BitArray after the last bit written.</returns>
        public static int WriteToBitArray(string plainText, int startIndex, int length, BitArray codeText, int codeIndex)
        {
            var index = startIndex;
            var count = length;
            var endIndex = startIndex + length;

            // Process each pair of characters.
            while (count >= 2)
            {
                // Convert each pair of characters to a number by looking them up in the alphanumeric dictionary and calculating.
                var dec = _alphanumEncDict[plainText[index++]] * 45 + _alphanumEncDict[plainText[index++]];
                // Convert the number to binary and store it in the BitArray.
                codeIndex = DecToBin(dec, 11, codeText, codeIndex);
                count -= 2;
            }

            // Handle the last character if the length is odd.
            if (count > 0)
            {
                codeIndex = DecToBin(_alphanumEncDict[plainText[index]], 6, codeText, codeIndex);
            }

            return codeIndex;
        }
    }
}
