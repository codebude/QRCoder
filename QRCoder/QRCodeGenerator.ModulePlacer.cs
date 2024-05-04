using System;
using System.Collections.Generic;
using System.Collections;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static partial class ModulePlacer
        {
            /// <summary>
            /// Adds a quiet zone around the QR code. A quiet zone is a blank margin used to separate the QR code
            /// from other visual elements, improving scanner readability. This zone consists of white modules
            /// extending 4 modules wide around the existing QR code pattern.
            /// </summary>
            /// <param name="qrCode">The QR code data structure to modify.</param>
            public static void AddQuietZone(QRCodeData qrCode)
            {
                // Calculate the required length for a new quiet line, including existing modules plus 8 additional for the quiet zone.
                var quietLine = new bool[qrCode.ModuleMatrix.Count + 8];
                // Initialize the quiet line with false values to represent white modules.
                for (var i = 0; i < quietLine.Length; i++)
                    quietLine[i] = false;
                // Add four new lines at the top of the QR code matrix to create the upper part of the quiet zone.
                for (var i = 0; i < 4; i++)
                    qrCode.ModuleMatrix.Insert(0, new BitArray(quietLine));
                // Add four new lines at the bottom of the QR code matrix to create the lower part of the quiet zone.
                for (var i = 0; i < 4; i++)
                    qrCode.ModuleMatrix.Add(new BitArray(quietLine));
                // Expand each line of the QR code matrix sideways by 4 modules on each side to complete the quiet zone.
                for (var i = 4; i < qrCode.ModuleMatrix.Count - 4; i++)
                {
                    qrCode.ModuleMatrix[i].Length += 8;
                    ShiftAwayFromBit0(qrCode.ModuleMatrix[i], 4);
                }
            }

            /// <summary>
            /// Places the version information on the QR code matrix for versions 7 and higher. Version information
            /// is encoded into two small rectangular areas near the bottom left and top right corners outside the timing patterns.
            /// </summary>
            /// <param name="qrCode">The QR code data structure to modify.</param>
            /// <param name="versionStr">The bit array containing the version information.</param>
            public static void PlaceVersion(QRCodeData qrCode, BitArray versionStr)
            {
                var size = qrCode.ModuleMatrix.Count; // The size of the QR code matrix.

                // Loop through each module position intended for version information, placed adjacent to the separators.
                for (var x = 0; x < 6; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        // Apply the version bits to the corresponding modules on the matrix, mapping the bits from the versionStr array.
                        qrCode.ModuleMatrix[y + size - 11][x] = versionStr[17 - (x * 3 + y)];
                        qrCode.ModuleMatrix[x][y + size - 11] = versionStr[17 - (x * 3 + y)];
                    }
                }
            }

            /// <summary>
            /// Places the format information on the QR code, encoding the error correction level and mask pattern used.
            /// </summary>
            /// <param name="qrCode">The QR code data structure to modify.</param>
            /// <param name="formatStr">The bit array containing the format information.</param>
            public static void PlaceFormat(QRCodeData qrCode, BitArray formatStr)
            {
                var size = qrCode.ModuleMatrix.Count;

                //    { x1, y1, x2, y2 }          i
                //    ===============================
                //    { 8, 0, size - 1, 8 },   // 0
                //    { 8, 1, size - 2, 8 },   // 1
                //    { 8, 2, size - 3, 8 },   // 2
                //    { 8, 3, size - 4, 8 },   // 3
                //    { 8, 4, size - 5, 8 },   // 4
                //    { 8, 5, size - 6, 8 },   // 5
                //    { 8, 7, size - 7, 8 },   // 6
                //    { 8, 8, size - 8, 8 },   // 7
                //    { 7, 8, 8, size - 7 },   // 8
                //    { 5, 8, 8, size - 6 },   // 9
                //    { 4, 8, 8, size - 5 },   // 10
                //    { 3, 8, 8, size - 4 },   // 11
                //    { 2, 8, 8, size - 3 },   // 12
                //    { 1, 8, 8, size - 2 },   // 13
                //    { 0, 8, 8, size - 1 } }; // 14

                for (var i = 0; i < 15; i++)
                {
                    // values computed to follow table above
                    var x1 = i < 8 ? 8 : i == 8 ? 7 : 14 - i;
                    var y1 = i < 6 ? i : i < 7 ? i + 1 : 8;
                    var x2 = i < 8 ? size - 1 - i : 8;
                    var y2 = i < 8 ? 8 : size - (15 - i);

                    qrCode.ModuleMatrix[y1][x1] = formatStr[14 - i];
                    qrCode.ModuleMatrix[y2][x2] = formatStr[14 - i];
                }
            }

            /// <summary>
            /// Applies the most effective mask pattern to the QR code based on minimizing the penalty score,
            /// which evaluates how well the pattern will work for QR scanners.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the mask will be applied.</param>
            /// <param name="version">The version of the QR code, which determines the size and complexity.</param>
            /// <param name="blockedModules">List of rectangles representing areas that must not be overwritten.</param>
            /// <param name="eccLevel">The error correction level of the QR code, which affects format string values.</param>
            /// <returns>The index of the selected mask pattern.</returns>
            public static int MaskCode(QRCodeData qrCode, int version, List<Rectangle> blockedModules, ECCLevel eccLevel)
            {
                int? selectedPattern = null;
                var patternScore = 0;

                var size = qrCode.ModuleMatrix.Count;

                // Temporary QRCodeData object to test different mask patterns without altering the original.
                var qrTemp = new QRCodeData(version);
                foreach (var pattern in MaskPattern.Patterns)
                {
                    // Reset the temporary QR code to the current state of the actual QR code.
                    for (var y = 0; y < size; y++)
                    {
                        for (var x = 0; x < size; x++)
                        {
                            qrTemp.ModuleMatrix[y][x] = qrCode.ModuleMatrix[y][x];
                        }
                    }

                    // Place format information using the current mask pattern.
                    var formatStr = GetFormatString(eccLevel, pattern.Key - 1);
                    ModulePlacer.PlaceFormat(qrTemp, formatStr);

                    // Place version information if applicable.
                    if (version >= 7)
                    {
                        var versionString = GetVersionString(version);
                        ModulePlacer.PlaceVersion(qrTemp, versionString);
                    }

                    // Apply the mask pattern and calculate the score.
                    for (var x = 0; x < size; x++)
                    {
                        for (var y = 0; y < x; y++)
                        {
                            if (!IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            {
                                qrTemp.ModuleMatrix[y][x] ^= pattern.Value(x, y);
                                qrTemp.ModuleMatrix[x][y] ^= pattern.Value(y, x);
                            }
                        }

                        if (!IsBlocked(new Rectangle(x, x, 1, 1), blockedModules))
                        {
                            qrTemp.ModuleMatrix[x][x] ^= pattern.Value(x, x);
                        }
                    }

                    var score = MaskPattern.Score(qrTemp);

                    // Select the pattern with the lowest score, indicating better QR code readability.
                    if (!selectedPattern.HasValue || patternScore > score)
                    {
                        selectedPattern = pattern.Key;
                        patternScore = score;
                    }
                }

                // Apply the best mask pattern to the actual QR code.
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < x; y++)
                    {
                        if (!IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                        {
                            qrCode.ModuleMatrix[y][x] ^= MaskPattern.Patterns[selectedPattern.Value](x, y);
                            qrCode.ModuleMatrix[x][y] ^= MaskPattern.Patterns[selectedPattern.Value](y, x);
                        }
                    }

                    if (!IsBlocked(new Rectangle(x, x, 1, 1), blockedModules))
                    {
                        qrCode.ModuleMatrix[x][x] ^= MaskPattern.Patterns[selectedPattern.Value](x, x);
                    }
                }
                return selectedPattern.Value - 1;
            }

            /// <summary>
            /// Places data bits into the QR code's module matrix following a specific pattern that navigates around blocked modules.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the data bits are to be placed.</param>
            /// <param name="data">The data bits to be placed within the QR code matrix.</param>
            /// <param name="blockedModules">A list of rectangles representing areas within the QR code matrix that should not be modified because they contain other necessary information like format and version info.</param>
            public static void PlaceDataWords(QRCodeData qrCode, BitArray data, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count; // Get the size of the QR code matrix.
                var up = true; // A boolean flag used to alternate the direction of filling data: up or down.
                var index = 0; // Index to track the current bit position in the data BitArray.
                var count = data.Length; // Total number of data bits to place.

                // Loop from the rightmost column to the leftmost column, skipping one column each time.
                for (var x = size - 1; x >= 0; x -= 2)
                {
                    // Skip the timing pattern column at position 6.
                    if (x == 6)
                        x = 5;

                    // Loop through each row in the current column set.
                    for (var yMod = 1; yMod <= size; yMod++)
                    {
                        int y; // Actual y position to place data in the matrix.

                        // Determine the actual y position based on the current fill direction.
                        if (up)
                        {
                            y = size - yMod; // Calculate y for upward direction.
                                             // Place data if within data length, current position is not blocked, and leftward column is in bounds.
                            if (index < count && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x] = data[index++];
                            if (index < count && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x - 1] = data[index++];
                        }
                        else
                        {
                            y = yMod - 1; // Calculate y for downward direction.
                                          // Similar checks and data placement for the downward direction.
                            if (index < count && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x] = data[index++];
                            if (index < count && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x - 1] = data[index++];
                        }
                    }
                    // Switch the fill direction after completing each column set.
                    up = !up;
                }
            }

            /// <summary>
            /// Reserves separator areas around the positioning patterns of a QR code to ensure that these crucial areas remain unmodified during data placement.
            /// </summary>
            /// <param name="size">The size of the QR code matrix.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten.</param>
            public static void ReserveSeperatorAreas(int size, List<Rectangle> blockedModules)
            {
                // Block areas around the finder patterns, which are located near three corners of the QR code.
                blockedModules.Add(new Rectangle(7, 0, 1, 8));        // Vertical block near the top left finder pattern
                blockedModules.Add(new Rectangle(0, 7, 7, 1));        // Horizontal block near the top left finder pattern
                blockedModules.Add(new Rectangle(0, size - 8, 8, 1)); // Horizontal block near the bottom left finder pattern
                blockedModules.Add(new Rectangle(7, size - 7, 1, 7)); // Vertical block near the bottom left finder pattern
                blockedModules.Add(new Rectangle(size - 8, 0, 1, 8)); // Vertical block near the top right finder pattern
                blockedModules.Add(new Rectangle(size - 7, 7, 7, 1)); // Horizontal block near the top right finder pattern
            }

            /// <summary>
            /// Reserves areas for version information on QR codes that are version 7 or higher. Also reserves space for format information.
            /// </summary>
            /// <param name="size">The size of the QR code matrix.</param>
            /// <param name="version">The version number of the QR code, which determines the placement of version information.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten.</param>
            public static void ReserveVersionAreas(int size, int version, List<Rectangle> blockedModules)
            {
                // Reserve areas near the timing patterns for version and format information.
                blockedModules.Add(new Rectangle(8, 0, 1, 6));        // Near the top timing pattern
                blockedModules.Add(new Rectangle(8, 7, 1, 1));        // Small square near the top left finder pattern
                blockedModules.Add(new Rectangle(0, 8, 6, 1));        // Near the left timing pattern
                blockedModules.Add(new Rectangle(7, 8, 2, 1));        // Extension of the above block
                blockedModules.Add(new Rectangle(size - 8, 8, 8, 1)); // Near the right timing pattern
                blockedModules.Add(new Rectangle(8, size - 7, 1, 7)); // Near the bottom timing pattern

                // If the version is 7 or higher, additional blocks for version information are added.
                if (version >= 7)
                {
                    blockedModules.Add(new Rectangle(size - 11, 0, 3, 6)); // Top right version information block
                    blockedModules.Add(new Rectangle(0, size - 11, 6, 3)); // Bottom left version information block
                }
            }

            /// <summary>
            /// Places a dark module on the QR code matrix as per QR code specification, which requires a dark module at a specific position for all QR codes.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the dark module is to be placed.</param>
            /// <param name="version">The version number of the QR code, which determines the specific location of the dark module.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten, updated to include the dark module.</param>
            public static void PlaceDarkModule(QRCodeData qrCode, int version, List<Rectangle> blockedModules)
            {
                // Place the dark module, which is always required to be black.
                qrCode.ModuleMatrix[4 * version + 9][8] = true; // Calculated position for the dark module based on the version
                // Block the dark module area to prevent overwriting during further QR code generation steps.
                blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
            }

            /// <summary>
            /// Places finder patterns on the QR code. Finder patterns are critical for QR code scanners to correctly orient and recognize the QR code.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the finder patterns will be placed.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten. This is updated with the areas occupied by the finder patterns.</param>
            public static void PlaceFinderPatterns(QRCodeData qrCode, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count; // Get the size of the QR code matrix.

                // Loop to place three finder patterns in the top-left, top-right, and bottom-left corners of the QR code.
                for (var i = 0; i < 3; i++)
                {
                    // Calculate the x and y starting positions for each finder pattern based on the index.
                    var locationX = i == 1 ? size - 7 : 0; // Place at top-right if i is 1, otherwise at left side (top or bottom).
                    var locationY = i == 2 ? size - 7 : 0; // Place at bottom-left if i is 2, otherwise at top (left or right).

                    // Nested loops to draw the 7x7 finder pattern at the calculated location.
                    for (var x = 0; x < 7; x++)
                    {
                        for (var y = 0; y < 7; y++)
                        {
                            // Condition to form the characteristic 5x5 black/white border of the finder pattern.
                            // The center 3x3 area is filled, bordered by a line of white modules, enclosed by a 7x7 black border.
                            if (!(((x == 1 || x == 5) && y > 0 && y < 6) || (x > 0 && x < 6 && (y == 1 || y == 5))))
                            {
                                qrCode.ModuleMatrix[y + locationY][x + locationX] = true;
                            }
                        }
                    }

                    // Add the area covered by the current finder pattern to the list of blocked modules, preventing any data from being placed there.
                    blockedModules.Add(new Rectangle(locationX, locationY, 7, 7));
                }
            }

            /// <summary>
            /// Places alignment patterns on the QR code matrix. Alignment patterns help ensure the scanner can correctly interpret the QR code at various scales and orientations.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the alignment patterns will be placed.</param>
            /// <param name="alignmentPatternLocations">A list of points representing the centers of where alignment patterns should be placed.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten. Updated with the areas occupied by alignment patterns.</param>
            public static void PlaceAlignmentPatterns(QRCodeData qrCode, List<Point> alignmentPatternLocations, List<Rectangle> blockedModules)
            {
                // Iterate through each specified location for alignment patterns.
                foreach (var loc in alignmentPatternLocations)
                {
                    // Define a 5x5 rectangle for the alignment pattern based on the center point provided.
                    var alignmentPatternRect = new Rectangle(loc.X, loc.Y, 5, 5);
                    var blocked = false; // Flag to check if the current location overlaps with any blocked modules.

                    // Check if the proposed alignment pattern rectangle intersects with any already blocked rectangles.
                    foreach (var blockedRect in blockedModules)
                    {
                        if (Intersects(alignmentPatternRect, blockedRect))
                        {
                            blocked = true;
                            break; // Stop checking if an intersection is found and mark this location as blocked.
                        }
                    }

                    // Skip the current location if it is blocked to prevent overwriting crucial information.
                    if (blocked)
                        continue;

                    // Place the alignment pattern by setting modules within the 5x5 area.
                    // The pattern consists of a 3x3 center block with a single module border.
                    for (var x = 0; x < 5; x++)
                    {
                        for (var y = 0; y < 5; y++)
                        {
                            // Create the pattern: a 3x3 block surrounded by a border, with the very center module set.
                            if (y == 0 || y == 4 || x == 0 || x == 4 || (x == 2 && y == 2))
                            {
                                qrCode.ModuleMatrix[loc.Y + y][loc.X + x] = true;
                            }
                        }
                    }

                    // Add the alignment pattern's area to the list of blocked modules to prevent future overwrites.
                    blockedModules.Add(new Rectangle(loc.X, loc.Y, 5, 5));
                }
            }

            /// <summary>
            /// Places timing patterns in the QR code. Timing patterns are alternating dark and light modules that help scanners determine the coordinates of modules within the QR code.
            /// </summary>
            /// <param name="qrCode">The QR code data structure where the timing patterns will be placed.</param>
            /// <param name="blockedModules">A list of rectangles representing areas that must not be overwritten. Updated with the areas occupied by timing patterns.</param>
            public static void PlaceTimingPatterns(QRCodeData qrCode, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count; // Get the size of the QR code matrix.

                // Place timing patterns starting from the 8th module to the size - 8 to avoid overlapping with finder patterns.
                for (var i = 8; i < size - 8; i++)
                {
                    if (i % 2 == 0) // Place a dark module every other module to create the alternating pattern.
                    {
                        qrCode.ModuleMatrix[6][i] = true; // Horizontal timing pattern
                        qrCode.ModuleMatrix[i][6] = true; // Vertical timing pattern
                    }
                }

                // Add the areas occupied by the timing patterns to the list of blocked modules.
                blockedModules.Add(new Rectangle(6, 8, 1, size - 16)); // Horizontal timing pattern area
                blockedModules.Add(new Rectangle(8, 6, size - 16, 1)); // Vertical timing pattern area
            }

            /// <summary>
            /// Determines if two rectangles intersect with each other.
            /// </summary>
            /// <param name="r1">The first rectangle.</param>
            /// <param name="r2">The second rectangle to check for intersection with the first.</param>
            /// <returns>True if the rectangles intersect; otherwise, false.</returns>
            private static bool Intersects(Rectangle r1, Rectangle r2)
            {
                // Check if any part of the rectangles overlap.
                return r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height;
            }

            /// <summary>
            /// Checks if a given rectangle is blocked by any rectangle in a list of blocked modules.
            /// </summary>
            /// <param name="r1">The rectangle to check.</param>
            /// <param name="blockedModules">The list of rectangles representing blocked areas.</param>
            /// <returns>True if the rectangle is blocked; otherwise, false.</returns>
            private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
            {
                // Iterate through the list of blocked modules to check for any intersection.
                foreach (var blockedMod in blockedModules)
                {
                    if (Intersects(blockedMod, r1))
                        return true; // Return true if an intersection is found, indicating the area is blocked.
                }
                return false; // Return false if no intersections are found, indicating the area is not blocked.
            }
        }
    }
}
