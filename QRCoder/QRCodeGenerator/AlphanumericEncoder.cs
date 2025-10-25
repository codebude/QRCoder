namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Encodes alphanumeric characters (<c>0–9</c>, <c>A–Z</c> (uppercase), space, <c>$</c>, <c>%</c>, <c>*</c>, <c>+</c>, <c>-</c>, period, <c>/</c>, colon) into a binary format suitable for QR codes.
    /// </summary>
    internal static class AlphanumericEncoder
    {
        // With C# 7.3 and later, this byte array is inlined into the assembly's read-only data section, improving performance and reducing memory usage.
        // See: https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-core-3-0/
        internal static ReadOnlySpan<byte> _map =>
        [
            // 0..31
            255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255,
            // 32..47  (space, ! " # $ % & ' ( ) * + , - . /)
            36, 255, 255, 255, 37, 38, 255, 255, 255, 255, 39, 40, 255, 41, 42, 43,
            // 48..57  (0..9)
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            // 58..64  (: ; < = > ? @)
            44, 255, 255, 255, 255, 255, 255,
            // 65..90  (A..Z)
            10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35
            // (we don't index > 90)
        ];

        /// <summary>
        /// Checks if a character is present in the alphanumeric encoding table.
        /// </summary>
        public static bool CanEncode(char c) => c <= 90 && _map[c] != 255;

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
        /// <param name="index">The starting index in the text to encode from.</param>
        /// <param name="count">The number of characters to encode.</param>
        /// <param name="codeText">The target BitArray to write to.</param>
        /// <param name="codeIndex">The starting index in the BitArray where writing should begin.</param>
        /// <returns>The next index in the BitArray after the last bit written.</returns>
        public static int WriteToBitArray(string plainText, int index, int count, BitArray codeText, int codeIndex)
        {
            // Process each pair of characters.
            while (count >= 2)
            {
                // Convert each pair of characters to a number by looking them up in the alphanumeric dictionary and calculating.
                var dec = _map[plainText[index++]] * 45 + _map[plainText[index++]];
                // Convert the number to binary and store it in the BitArray.
                codeIndex = DecToBin(dec, 11, codeText, codeIndex);
                count -= 2;
            }

            // Handle the last character if the length is odd.
            if (count > 0)
            {
                codeIndex = DecToBin(_map[plainText[index]], 6, codeText, codeIndex);
            }

            return codeIndex;
        }
    }
}
