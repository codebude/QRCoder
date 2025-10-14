#pragma warning disable IDE0018 // Inline variable declaration -- false positive

namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Data segment that optimizes encoding by analyzing character patterns and switching between
    /// encoding modes (Numeric, Alphanumeric, Byte) to minimize the total bit length.
    /// This implements the QR Code optimization algorithm from ISO/IEC 18004:2015 Annex J.2.
    /// It does not support Kanji mode.
    /// </summary>
    private sealed class OptimizedLatin1DataSegment : DataSegment
    {
        /// <summary>
        /// Checks if the given string can be encoded using optimized Latin-1 encoding.
        /// Returns true if all characters are within the ISO-8859-1 range (0x00-0xFF).
        /// </summary>
        /// <param name="plainText">The text to check</param>
        /// <returns>True if the text can be encoded as ISO-8859-1, false otherwise</returns>
        public static bool CanEncode(string plainText) => IsValidISO(plainText);

        /// <summary>
        /// Gets the encoding mode (not applicable for optimized segments as they use multiple modes)
        /// </summary>
        public override EncodingMode EncodingMode => EncodingMode.Byte;

        /// <summary>
        /// Initializes a new instance of the OptimizedDataSegment class.
        /// </summary>
        /// <param name="plainText">The text to encode with optimized mode switching</param>
        public OptimizedLatin1DataSegment(string plainText)
            : base(plainText)
        {
        }

        /// <summary>
        /// Calculates the total bit length for this segment when encoded for a specific QR code version.
        /// </summary>
        /// <param name="version">The QR code version (1-40, or -1 to -4 for Micro QR)</param>
        /// <returns>The total number of bits required for this segment</returns>
        public override int GetBitLength(int version)
        {
            if (string.IsNullOrEmpty(Text))
                return 0;

            var totalBits = 0;
            var mode = SelectInitialMode(Text, 0, version);
            var startPos = 0;

            do
            {
                // Find the extent of the current mode
                EncodingMode nextMode;
                var segmentEnd = mode switch
                {
                    EncodingMode.Byte => ProcessByteMode(Text, startPos, version, out nextMode),
                    EncodingMode.Alphanumeric => ProcessAlphanumericMode(Text, startPos, version, out nextMode),
                    EncodingMode.Numeric => ProcessNumericMode(Text, startPos, version, out nextMode),
                    _ => throw new InvalidOperationException("Unsupported encoding mode")
                };

                var segmentLength = segmentEnd - startPos;
                totalBits += mode switch
                {
                    EncodingMode.Numeric => NumericDataSegment.GetBitLength(segmentLength, version),
                    EncodingMode.Alphanumeric => AlphanumericDataSegment.GetBitLength(segmentLength, version),
                    EncodingMode.Byte => GetByteBitLength(segmentLength, version),
                    _ => throw new InvalidOperationException("Unsupported encoding mode")
                };

                // Move to the next segment
                startPos = segmentEnd;
                mode = nextMode;
            }
            while (startPos < Text.Length);

            return totalBits;
        }

        /// <summary>
        /// Calculates the bit length for a byte mode segment.
        /// </summary>
        private static int GetByteBitLength(int textLength, int version)
        {
            int modeIndicatorLength = 4;
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            int dataLength = textLength * 8; // ISO-8859-1 encoding
            return modeIndicatorLength + countIndicatorLength + dataLength;
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
            if (string.IsNullOrEmpty(Text))
                return startIndex;

            var bitIndex = startIndex;
            var mode = SelectInitialMode(Text, 0, version);
            var startPos = 0;

            do
            {
                // Find the extent of the current mode
                EncodingMode nextMode;
                var segmentEnd = mode switch
                {
                    EncodingMode.Byte => ProcessByteMode(Text, startPos, version, out nextMode),
                    EncodingMode.Alphanumeric => ProcessAlphanumericMode(Text, startPos, version, out nextMode),
                    EncodingMode.Numeric => ProcessNumericMode(Text, startPos, version, out nextMode),
                    _ => throw new InvalidOperationException("Unsupported encoding mode")
                };

                var segmentLength = segmentEnd - startPos;
                bitIndex = mode switch
                {
                    EncodingMode.Numeric => NumericDataSegment.WriteTo(Text, startPos, segmentLength, bitArray, bitIndex, version),
                    EncodingMode.Alphanumeric => AlphanumericDataSegment.WriteTo(Text, startPos, segmentLength, bitArray, bitIndex, version),
                    EncodingMode.Byte => WriteByteSegment(Text, startPos, segmentLength, bitArray, bitIndex, version),
                    _ => throw new InvalidOperationException("Unsupported encoding mode")
                };

                // Move to the next segment
                startPos = segmentEnd;
                mode = nextMode;
            }
            while (startPos < Text.Length);

            return bitIndex;
        }

        /// <summary>
        /// Writes a byte mode segment to the BitArray.
        /// </summary>
        private static int WriteByteSegment(string text, int offset, int length, BitArray bitArray, int bitIndex, int version)
        {
            // write mode indicator
            bitIndex = DecToBin((int)EncodingMode.Byte, 4, bitArray, bitIndex);

            // write count indicator
            int countIndicatorLength = GetCountIndicatorLength(version, EncodingMode.Byte);
            bitIndex = DecToBin(length, countIndicatorLength, bitArray, bitIndex);

            // write data - encode as ISO-8859-1
            for (int i = 0; i < length; i++)
            {
                bitIndex = DecToBin(text[offset + i], 8, bitArray, bitIndex);
            }

            return bitIndex;
        }

        // Selects the initial encoding mode based on the first character(s) of the input.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section a.
        private static EncodingMode SelectInitialMode(string text, int startPos, int version)
        {
            var c = text[startPos];

            // Rule a.1: If initial input data is in the exclusive subset of the Byte character set, select Byte mode
            if (!IsAlphanumeric(c))
                return EncodingMode.Byte;

            // Rule a.4: If initial data is numeric, AND if there are less than [4,4,5] characters followed by data from the
            // exclusive subset of the Byte character set, THEN select Byte mode
            if (IsNumeric(c))
            {
                var numericCount = CountConsecutive(text, startPos, IsNumeric);
                var threshold = GetBreakpoint(version, 4, 4, 5);
                if (numericCount < threshold)
                {
                    var nextPos = startPos + numericCount;
                    if (nextPos < text.Length && !IsAlphanumericNonDigit(text[nextPos]))
                        return EncodingMode.Byte;
                }
                // ELSE IF there are less than [7-9] characters followed by data from the exclusive subset of the Alphanumeric character set
                // THEN select Alphanumeric mode ELSE select Numeric mode
                threshold = GetBreakpoint(version, 7, 8, 9);
                if (numericCount < threshold)
                {
                    var nextPos = startPos + numericCount;
                    if (nextPos < text.Length && IsAlphanumericNonDigit(text[nextPos]))
                        return EncodingMode.Alphanumeric;
                }
                return EncodingMode.Numeric;
            }

            // Rule a.3: If initial input data is in the exclusive subset of the Alphanumeric character set AND if there are
            // less than [6-8] characters followed by data from the remainder of the Byte character set, THEN select Byte mode
            var alphanumericCount = CountConsecutive(text, startPos, IsAlphanumeric);
            var alphaThreshold = GetBreakpoint(version, 6, 7, 8);
            if (alphanumericCount < alphaThreshold)
            {
                var nextPos = startPos + alphanumericCount;
                if (nextPos < text.Length && !IsAlphanumeric(text[nextPos]))
                    return EncodingMode.Byte;
            }
            return EncodingMode.Alphanumeric;
        }

        // Processes text in Byte mode and determines when to switch to another mode.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section b.
        private static int ProcessByteMode(string text, int startPos, int version, out EncodingMode nextMode)
        {
            var pos = startPos;

            var numericThreshold = GetBreakpoint(version, 6, 8, 9);
            var alphaThreshold = GetBreakpoint(version, 11, 15, 16);
            while (pos < text.Length)
            {
                var c = text[pos];

                // Rule b.3: If a sequence of at least [6,8,9] Numeric characters occurs before more data from the exclusive subset of the Byte character set, switch to Numeric mode
                var numericCount = CountConsecutive(text, pos, IsNumeric);
                if (numericCount >= numericThreshold)
                {
                    nextMode = EncodingMode.Numeric;
                    return pos;
                }

                // Rule b.2: If a sequence of at least [11,15,16] character from the exclusive subset of the Alphanumeric character set occurs before more data from the exclusive subset of the Byte character set, switch to Alphanumeric mode
                var alphanumericCount = CountConsecutive(text, pos, IsAlphanumeric);
                if (alphanumericCount >= alphaThreshold)
                {
                    nextMode = EncodingMode.Alphanumeric;
                    return pos;
                }

                // Continue in Byte mode
                pos++;
            }

            nextMode = EncodingMode.Byte;
            return pos;
        }

        // Processes text in Alphanumeric mode and determines when to switch to another mode.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section c.
        private static int ProcessAlphanumericMode(string text, int startPos, int version, out EncodingMode nextMode)
        {
            var pos = startPos;

            var threshold = GetBreakpoint(version, 13, 15, 17);
            while (pos < text.Length)
            {
                var c = text[pos];

                // Rule c.2: If one or more characters from the exclusive subset of the Byte character set occurs, switch to Byte mode
                if (!IsAlphanumeric(c))
                {
                    nextMode = EncodingMode.Byte;
                    return pos;
                }

                // Rule c.3: If a sequence of at least [13,15,17] Numeric characters occurs before more data from the exclusive subset of the Alphanumeric character set, switch to Numeric mode
                var numericCount = CountConsecutive(text, pos, IsNumeric);
                if (numericCount >= threshold)
                {
                    nextMode = EncodingMode.Numeric;
                    return pos;
                }

                // Continue in Alphanumeric mode
                pos++;
            }

            nextMode = EncodingMode.Alphanumeric;
            return pos;
        }

        // Processes text in Numeric mode and determines when to switch to another mode.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section d.
        private static int ProcessNumericMode(string text, int startPos, int version, out EncodingMode nextMode)
        {
            var pos = startPos;

            while (pos < text.Length)
            {
                var c = text[pos];

                // Rule d.2: If one or more characters from the exclusive subset of the Byte character set occurs, switch to Byte mode
                // Rule d.3: If one or more characters from the exclusive subset of the Alphanumeric character set occurs, switch to Alphanumeric mode

                // Replaced by using the more intelligent intial mode logic:
                if (!IsNumeric(c))
                {
                    nextMode = SelectInitialMode(text, pos, version);
                    return pos;
                }

                // Continue in Numeric mode
                pos++;
            }

            nextMode = EncodingMode.Numeric;
            return pos;
        }

        // Gets the appropriate breakpoint value based on QR code version.
        // ISO/IEC 18004:2015 Annex J.2 specifies different thresholds for different version ranges:
        // - Versions 1-9: Use v1_9 value
        // - Versions 10-26: Use v10_26 value
        // - Versions 27-40: Use v27_40 value
        private static int GetBreakpoint(int version, int v1_9, int v10_26, int v27_40)
        {
            if (version < 10)
                return v1_9;
            else if (version < 27)
                return v10_26;
            else
                return v27_40;
        }

        // Counts consecutive characters matching a predicate starting from a position.
        private static int CountConsecutive(string text, int startPos, Func<char, bool> predicate)
        {
            var count = 0;
            for (var i = startPos; i < text.Length && predicate(text[i]); i++)
                count++;
            return count;
        }

        // Checks if a character is numeric (0-9).
        private static bool IsNumeric(char c) => IsInRange(c, '0', '9');

        // Checks if a character is alphanumeric (can be encoded in alphanumeric mode).
        private static bool IsAlphanumeric(char c) => IsNumeric(c) || IsAlphanumericNonDigit(c);

        // Checks if a non-digit character can be encoded in alphanumeric mode.
        private static bool IsAlphanumericNonDigit(char c) => AlphanumericEncoder.CanEncodeNonDigit(c);
    }
}
