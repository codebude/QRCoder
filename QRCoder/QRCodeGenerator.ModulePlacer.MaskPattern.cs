using System;
using System.Collections;
using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static partial class ModulePlacer
        {
            /// <summary>
            /// Provides static methods and properties to handle mask patterns used in QR code generation.
            /// Mask patterns are applied to QR codes to break up patterns in the data matrix that might confuse scanners.
            /// </summary>
            private static class MaskPattern
            {
                /// <summary>
                /// A dictionary mapping each mask pattern index to its corresponding function that calculates whether a given pixel should be masked.
                /// </summary>
                public static readonly Dictionary<int, Func<int, int, bool>> Patterns =
                    new Dictionary<int, Func<int, int, bool>>(8) {
                        { 1, MaskPattern.Pattern1 }, {2, MaskPattern.Pattern2 }, {3, MaskPattern.Pattern3 }, {4, MaskPattern.Pattern4 },
                        { 5, MaskPattern.Pattern5 }, {6, MaskPattern.Pattern6 }, {7, MaskPattern.Pattern7 }, {8, MaskPattern.Pattern8 }
                    };

                /// <summary>
                /// Mask pattern 1: (x + y) % 2 == 0
                /// Applies a checkerboard mask on the QR code.
                /// </summary>
                public static bool Pattern1(int x, int y)
                {
                    return (x + y) % 2 == 0;
                }

                /// <summary>
                /// Mask pattern 2: y % 2 == 0
                /// Applies a horizontal striping mask on the QR code.
                /// </summary>
                public static bool Pattern2(int x, int y)
                {
                    return y % 2 == 0;
                }

                /// <summary>
                /// Mask pattern 3: x % 3 == 0
                /// Applies a vertical striping mask on the QR code.
                /// </summary>
                public static bool Pattern3(int x, int y)
                {
                    return x % 3 == 0;
                }

                /// <summary>
                /// Mask pattern 4: (x + y) % 3 == 0
                /// Applies a diagonal striping mask on the QR code.
                /// </summary>
                public static bool Pattern4(int x, int y)
                {
                    return (x + y) % 3 == 0;
                }

                /// <summary>
                /// Mask pattern 5: ((y / 2) + (x / 3)) % 2 == 0
                /// Applies a complex pattern mask on the QR code, mixing horizontal and vertical rules.
                /// </summary>
                public static bool Pattern5(int x, int y)
                {
                    return ((int)(Math.Floor(y / 2d) + Math.Floor(x / 3d)) % 2) == 0;
                }

                /// <summary>
                /// Mask pattern 6: ((x * y) % 2 + (x * y) % 3) == 0
                /// Applies a mask based on the product of x and y coordinates modulo 2 and 3.
                /// </summary>
                public static bool Pattern6(int x, int y)
                {
                    return ((x * y) % 2) + ((x * y) % 3) == 0;
                }

                /// <summary>
                /// Mask pattern 7: (((x * y) % 2 + (x * y) % 3) % 2) == 0
                /// Applies a mask based on a more complex function involving the product of x and y coordinates.
                /// </summary>
                public static bool Pattern7(int x, int y)
                {
                    return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

                /// <summary>
                /// Mask pattern 8: (((x + y) % 2) + ((x * y) % 3) % 2) == 0
                /// Combines rules of checkers and complex multiplicative masks.
                /// </summary>
                public static bool Pattern8(int x, int y)
                {
                    return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

                /// <summary>
                /// Calculates a penalty score for a QR code to evaluate the effectiveness of a mask pattern.
                /// A lower score indicates a QR code that is easier for decoders to read accurately.
                /// The score is the sum of four penalty rules applied to the QR code.
                /// </summary>
                /// <param name="qrCode">The QR code data structure to be evaluated.</param>
                /// <returns>The total penalty score of the QR code.</returns>
                public static int Score(QRCodeData qrCode)
                {
                    int score1 = 0,  // Penalty for groups of five or more same-color modules in a row (or column)
                        score2 = 0,  // Penalty for blocks of modules in the same color
                        score3 = 0,  // Penalty for specific patterns found within the QR code
                        score4 = 0;  // Penalty for having more than 50% black modules or more than 50% white modules
                    var size = qrCode.ModuleMatrix.Count;

                    //Penalty 1: Checking for consecutive modules of the same color in rows and columns
                    for (var y = 0; y < size; y++)
                    {
                        var modInRow = 0;
                        var modInColumn = 0;
                        var lastValRow = qrCode.ModuleMatrix[y][0];
                        var lastValColumn = qrCode.ModuleMatrix[0][y];
                        for (var x = 0; x < size; x++)
                        {
                            // Check rows for consecutive modules
                            if (qrCode.ModuleMatrix[y][x] == lastValRow)
                                modInRow++;
                            else
                                modInRow = 1;
                            if (modInRow == 5)
                                score1 += 3;
                            else if (modInRow > 5)
                                score1++;
                            lastValRow = qrCode.ModuleMatrix[y][x];

                            // Check columns for consecutive modules
                            if (qrCode.ModuleMatrix[x][y] == lastValColumn)
                                modInColumn++;
                            else
                                modInColumn = 1;
                            if (modInColumn == 5)
                                score1 += 3;
                            else if (modInColumn > 5)
                                score1++;
                            lastValColumn = qrCode.ModuleMatrix[x][y];
                        }
                    }

                    //Penalty 2: Checking for blocks of modules in the same color
                    for (var y = 0; y < size - 1; y++)
                    {
                        for (var x = 0; x < size - 1; x++)
                        {
                            if (qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y][x + 1] &&
                                qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x] &&
                                qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x + 1])
                            {
                                score2 += 3;
                            }
                        }
                    }

                    //Penalty 3: Checking for specific patterns within the QR code (patterns that should be avoided)
                    for (var y = 0; y < size; y++)
                    {
                        for (var x = 0; x < size - 10; x++)
                        {
                            // Horizontal pattern matching
                            if (MatchesPattern1(qrCode.ModuleMatrix, x, y))
                                score3 += 40;
                            // Vertical pattern matching
                            if (MatchesPattern1(qrCode.ModuleMatrix, y, x))
                                score3 += 40;
                        }
                    }

                    //Penalty 4: Proportions of dark and light modules
                    int blackModules = 0;
                    foreach (var bitArray in qrCode.ModuleMatrix)
                        for (var x = 0; x < size; x++)
                            if (bitArray[x])
                                blackModules++;

                    var percentDiv5 = blackModules * 20 / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count);
                    var prevMultipleOf5 = Math.Abs(percentDiv5 - 10);
                    var nextMultipleOf5 = Math.Abs(percentDiv5 - 9);
                    score4 = Math.Min(prevMultipleOf5, nextMultipleOf5) * 10;

                    // Return the sum of all four penalties
                    return (score1 + score2) + (score3 + score4);
                }

                /// <summary>
                /// Matches the specified pattern in QR code evaluation rules (1:1:3:1:1 ratio).
                /// </summary>
                private static bool MatchesPattern1(List<BitArray> matrix, int x, int y)
                {
                    return (matrix[y][x] && 
                        !matrix[y][x + 1] && 
                        matrix[y][x + 2] && 
                        matrix[y][x + 3] &&
                        matrix[y][x + 4] && 
                        !matrix[y][x + 5] && 
                        matrix[y][x + 6] && 
                        !matrix[y][x + 7] &&
                        !matrix[y][x + 8] && 
                        !matrix[y][x + 9] && 
                        !matrix[y][x + 10]) ||
                        (!matrix[y][x] &&
                        !matrix[y][x + 1] &&
                        !matrix[y][x + 2] &&
                        !matrix[y][x + 3] &&
                        matrix[y][x + 4] &&
                        !matrix[y][x + 5] &&
                        matrix[y][x + 6] &&
                        matrix[y][x + 7] &&
                        matrix[y][x + 8] &&
                        !matrix[y][x + 9] &&
                        matrix[y][x + 10]);
                }
            }
        }
    }
}
