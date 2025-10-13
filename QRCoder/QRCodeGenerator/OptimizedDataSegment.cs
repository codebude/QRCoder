#pragma warning disable IDE0018 // Inline variable declaration -- false positive

namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Creates an optimized chain of data segments from plain text by analyzing character patterns
    /// and switching between encoding modes to minimize the total bit length.
    /// This implements the QR Code optimization algorithm from ISO/IEC 18004:2015 Annex J.2.
    /// It does not support Kanji mode.
    /// </summary>
    private static DataSegment CreateOptimizedLatin1DataSegment(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return new DataSegment(EncodingMode.Byte, 0, _emptyBitArray, EciMode.Default);

        // Start recursive segment creation from position 0 with initial mode
        var initialMode = SelectInitialMode(plainText, 0);
        return CreateSegment(plainText, 0, initialMode);

        // Recursively creates a data segment starting at the given position with the specified initial mode.
        // Returns the first segment in a chain, with subsequent segments linked via the Next property.
        DataSegment CreateSegment(string text, int startPos, EncodingMode mode)
        {
            // Find the extent of the current mode
            EncodingMode nextMode;
            var segmentEnd = mode switch
            {
                EncodingMode.Byte => ProcessByteMode(text, startPos, out nextMode),
                EncodingMode.Alphanumeric => ProcessAlphanumericMode(text, startPos, out nextMode),
                EncodingMode.Numeric => ProcessNumericMode(text, startPos, out nextMode),
                _ => throw new InvalidOperationException("Unsupported encoding mode")
            };

            var segmentLength = segmentEnd - startPos;
            var segmentData = mode switch
            {
                EncodingMode.Byte => PlainTextToBinaryByte(text, startPos, segmentLength, EciMode.Iso8859_1, false, false),
                EncodingMode.Alphanumeric => AlphanumericEncoder.GetBitArray(text, startPos, segmentLength),
                EncodingMode.Numeric => PlainTextToBinaryNumeric(text, startPos, segmentLength),
                _ => throw new InvalidOperationException("Unsupported encoding mode")
            };
            var segment = new DataSegment(mode, segmentLength, segmentData, EciMode.Default);

            // Recursively create the next segment if there's more text
            if (segmentEnd < text.Length)
                segment.Next = CreateSegment(text, segmentEnd, nextMode);

            return segment;
        }

        // Local functions

        // Selects the initial encoding mode based on the first character(s) of the input.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section a.
        static EncodingMode SelectInitialMode(string text, int startPos)
        {
            var c = text[startPos];

            // Rule a.1: If initial input data is in the exclusive subset of the Byte character set, select Byte mode
            if (!AlphanumericEncoder.CanEncode(c))
                return EncodingMode.Byte;

            // Rule a.4: If initial data is numeric, AND if there are less than [4,4,5] characters followed by data from the
            // exclusive subset of the Byte character set, THEN select Byte mode
            if (IsNumeric(c))
            {
                var numericCount = CountConsecutive(text, startPos, IsNumeric);
                if (numericCount < 4)
                {
                    var nextPos = startPos + numericCount;
                    if (nextPos < text.Length && !AlphanumericEncoder.CanEncodeNonDigit(text[nextPos]))
                        return EncodingMode.Byte;
                }
                // ELSE IF there are less than [7-9] characters followed by data from the exclusive subset of the Alphanumeric character set
                // THEN select Alphanumeric mode ELSE select Numeric mode
                if (numericCount < 7)
                {
                    var nextPos = startPos + numericCount;
                    if (nextPos < text.Length && AlphanumericEncoder.CanEncodeNonDigit(text[nextPos]))
                        return EncodingMode.Alphanumeric;
                }
                return EncodingMode.Numeric;
            }

            // Rule a.3: If initial input data is in the exclusive subset of the Alphanumeric character set AND if there are
            // less than [6-8] characters followed by data from the remainder of the Byte character set, THEN select Byte mode
            var alphanumericCount = CountConsecutive(text, startPos, IsAlphanumeric);
            if (alphanumericCount < 6)
            {
                var nextPos = startPos + alphanumericCount;
                if (nextPos < text.Length && !AlphanumericEncoder.CanEncode(text[nextPos]))
                    return EncodingMode.Byte;
            }
            return EncodingMode.Alphanumeric;
        }

        // Processes text in Byte mode and determines when to switch to another mode.
        // Implements rules from ISO/IEC 18004:2015 Annex J.2 section b.
        static int ProcessByteMode(string text, int startPos, out EncodingMode nextMode)
        {
            var pos = startPos;

            while (pos < text.Length)
            {
                var c = text[pos];

                // Rule b.3: If a sequence of at least [6,8,9] Numeric characters occurs before more data from the exclusive subset of the Byte character set, switch to Numeric mode
                var numericCount = CountConsecutive(text, pos, IsNumeric);
                if (numericCount >= 6)
                {
                    nextMode = EncodingMode.Numeric;
                    return pos;
                }

                // Rule b.2: If a sequence of at least [11,15,16] character from the exclusive subset of the Alphanumeric character set occurs before more data from the exclusive subset of the Byte character set, switch to Alphanumeric mode
                var alphanumericCount = CountConsecutive(text, pos, IsAlphanumeric);
                if (alphanumericCount >= 11)
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
        static int ProcessAlphanumericMode(string text, int startPos, out EncodingMode nextMode)
        {
            var pos = startPos;

            while (pos < text.Length)
            {
                var c = text[pos];

                // Rule c.2: If one or more characters from the exclusive subset of the Byte character set occurs, switch to Byte mode
                if (!AlphanumericEncoder.CanEncode(c))
                {
                    nextMode = EncodingMode.Byte;
                    return pos;
                }

                // Rule c.3: If a sequence of at least [13,15,17] Numeric characters occurs before more data from the exclusive subset of the Alphanumeric character set, switch to Numeric mode
                var numericCount = CountConsecutive(text, pos, IsNumeric);
                if (numericCount >= 13)
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
        static int ProcessNumericMode(string text, int startPos, out EncodingMode nextMode)
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
                    nextMode = SelectInitialMode(text, pos);
                    return pos;
                }

                // Continue in Numeric mode
                pos++;
            }

            nextMode = EncodingMode.Numeric;
            return pos;
        }

        // Counts consecutive characters matching a predicate starting from a position.
        static int CountConsecutive(string text, int startPos, Func<char, bool> predicate)
        {
            var count = 0;
            for (var i = startPos; i < text.Length && predicate(text[i]); i++)
                count++;
            return count;
        }

        // Checks if a character is numeric (0-9).
        static bool IsNumeric(char c) => IsInRange(c, '0', '9');

        // Checks if a character is alphanumeric (can be encoded in alphanumeric mode).
        static bool IsAlphanumeric(char c) => AlphanumericEncoder.CanEncode(c);
    }
}
