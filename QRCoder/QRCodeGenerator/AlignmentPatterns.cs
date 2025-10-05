namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// This class contains the alignment patterns used in QR codes.
    /// </summary>
    private static class AlignmentPatterns
    {
        /// <summary>
        /// A lookup table mapping QR code versions to their corresponding alignment patterns.
        /// </summary>
        private static readonly Dictionary<int, AlignmentPattern> _alignmentPatternTable = CreateAlignmentPatternTable();

        /// <summary>
        /// Retrieves the alignment pattern for a specific QR code version.
        /// </summary>
        public static AlignmentPattern FromVersion(int version) => _alignmentPatternTable[version];

        /// <summary>
        /// Creates a lookup table mapping QR code versions to their corresponding alignment patterns.
        /// Alignment patterns are used in QR codes to help scanners accurately read the code at high speeds and when partially obscured.
        /// This table provides the necessary patterns based on the QR code version which dictates the size and complexity of the QR code.
        /// </summary>
        /// <returns>A dictionary where keys are QR code version numbers and values are AlignmentPattern structures detailing the positions of alignment patterns for each version.</returns>
        private static Dictionary<int, AlignmentPattern> CreateAlignmentPatternTable()
        {
            var alignmentPatternBaseValues = new int[] { 0, 0, 0, 0, 0, 0, 0, 6, 18, 0, 0, 0, 0, 0, 6, 22, 0, 0, 0, 0, 0, 6, 26, 0, 0, 0, 0, 0, 6, 30, 0, 0, 0, 0, 0, 6, 34, 0, 0, 0, 0, 0, 6, 22, 38, 0, 0, 0, 0, 6, 24, 42, 0, 0, 0, 0, 6, 26, 46, 0, 0, 0, 0, 6, 28, 50, 0, 0, 0, 0, 6, 30, 54, 0, 0, 0, 0, 6, 32, 58, 0, 0, 0, 0, 6, 34, 62, 0, 0, 0, 0, 6, 26, 46, 66, 0, 0, 0, 6, 26, 48, 70, 0, 0, 0, 6, 26, 50, 74, 0, 0, 0, 6, 30, 54, 78, 0, 0, 0, 6, 30, 56, 82, 0, 0, 0, 6, 30, 58, 86, 0, 0, 0, 6, 34, 62, 90, 0, 0, 0, 6, 28, 50, 72, 94, 0, 0, 6, 26, 50, 74, 98, 0, 0, 6, 30, 54, 78, 102, 0, 0, 6, 28, 54, 80, 106, 0, 0, 6, 32, 58, 84, 110, 0, 0, 6, 30, 58, 86, 114, 0, 0, 6, 34, 62, 90, 118, 0, 0, 6, 26, 50, 74, 98, 122, 0, 6, 30, 54, 78, 102, 126, 0, 6, 26, 52, 78, 104, 130, 0, 6, 30, 56, 82, 108, 134, 0, 6, 34, 60, 86, 112, 138, 0, 6, 30, 58, 86, 114, 142, 0, 6, 34, 62, 90, 118, 146, 0, 6, 30, 54, 78, 102, 126, 150, 6, 24, 50, 76, 102, 128, 154, 6, 28, 54, 80, 106, 132, 158, 6, 32, 58, 84, 110, 136, 162, 6, 26, 54, 82, 110, 138, 166, 6, 30, 58, 86, 114, 142, 170 };
            var localAlignmentPatternTable = new Dictionary<int, AlignmentPattern>(40 + 4);

            for (var i = 0; i < (7 * 40); i += 7)
            {
                var points = new List<Point>(50);
                for (var x = 0; x < 7; x++)
                {
                    if (alignmentPatternBaseValues[i + x] != 0)
                    {
                        for (var y = 0; y < 7; y++)
                        {
                            if (alignmentPatternBaseValues[i + y] != 0)
                            {
                                var p = new Point(alignmentPatternBaseValues[i + x] - 2, alignmentPatternBaseValues[i + y] - 2);
                                if (!points.Contains(p))
                                    points.Add(p);
                            }
                        }
                    }
                }

                var version = (i + 7) / 7;
                localAlignmentPatternTable.Add(version, new AlignmentPattern()
                {
                    Version = version,
                    PatternPositions = points
                });
            }

            // Micro QR codes do not have alignment patterns.
            var emptyPointList = new List<Point>();
            localAlignmentPatternTable.Add(-1, new AlignmentPattern { Version = -1, PatternPositions = emptyPointList });
            localAlignmentPatternTable.Add(-2, new AlignmentPattern { Version = -2, PatternPositions = emptyPointList });
            localAlignmentPatternTable.Add(-3, new AlignmentPattern { Version = -3, PatternPositions = emptyPointList });
            localAlignmentPatternTable.Add(-4, new AlignmentPattern { Version = -4, PatternPositions = emptyPointList });

            return localAlignmentPatternTable;
        }
    }
}
