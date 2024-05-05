using System;
using System.Collections.Generic;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static partial class ModulePlacer
        {
            private static class MaskPattern
            {
                public static readonly Dictionary<int, Func<int, int, bool>> Patterns =
                    new Dictionary<int, Func<int, int, bool>>(8) {
                        { 1, MaskPattern.Pattern1 }, {2, MaskPattern.Pattern2 }, {3, MaskPattern.Pattern3 }, {4, MaskPattern.Pattern4 },
                        { 5, MaskPattern.Pattern5 }, {6, MaskPattern.Pattern6 }, {7, MaskPattern.Pattern7 }, {8, MaskPattern.Pattern8 }
                    };

                public static bool Pattern1(int x, int y)
                {
                    return (x + y) % 2 == 0;
                }

                public static bool Pattern2(int x, int y)
                {
                    return y % 2 == 0;
                }

                public static bool Pattern3(int x, int y)
                {
                    return x % 3 == 0;
                }

                public static bool Pattern4(int x, int y)
                {
                    return (x + y) % 3 == 0;
                }

                public static bool Pattern5(int x, int y)
                {
                    return ((int)(Math.Floor(y / 2d) + Math.Floor(x / 3d)) % 2) == 0;
                }

                public static bool Pattern6(int x, int y)
                {
                    return ((x * y) % 2) + ((x * y) % 3) == 0;
                }

                public static bool Pattern7(int x, int y)
                {
                    return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

                public static bool Pattern8(int x, int y)
                {
                    return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

                public static int Score(QRCodeData qrCode)
                {
                    int score1 = 0,
                        score2 = 0,
                        score3 = 0,
                        score4 = 0;
                    var size = qrCode.ModuleMatrix.Count;

                    //Penalty 1
                    for (var y = 0; y < size; y++)
                    {
                        var modInRow = 0;
                        var modInColumn = 0;
                        var lastValRow = qrCode.ModuleMatrix[y][0];
                        var lastValColumn = qrCode.ModuleMatrix[0][y];
                        for (var x = 0; x < size; x++)
                        {
                            if (qrCode.ModuleMatrix[y][x] == lastValRow)
                                modInRow++;
                            else
                                modInRow = 1;
                            if (modInRow == 5)
                                score1 += 3;
                            else if (modInRow > 5)
                                score1++;
                            lastValRow = qrCode.ModuleMatrix[y][x];


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


                    //Penalty 2
                    for (var y = 0; y < size - 1; y++)
                    {
                        for (var x = 0; x < size - 1; x++)
                        {
                            if (qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y][x + 1] &&
                                qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x] &&
                                qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x + 1])
                                score2 += 3;
                        }
                    }

                    //Penalty 3
                    for (var y = 0; y < size; y++)
                    {
                        for (var x = 0; x < size - 10; x++)
                        {
                            if ((qrCode.ModuleMatrix[y][x] &&
                                !qrCode.ModuleMatrix[y][x + 1] &&
                                qrCode.ModuleMatrix[y][x + 2] &&
                                qrCode.ModuleMatrix[y][x + 3] &&
                                qrCode.ModuleMatrix[y][x + 4] &&
                                !qrCode.ModuleMatrix[y][x + 5] &&
                                qrCode.ModuleMatrix[y][x + 6] &&
                                !qrCode.ModuleMatrix[y][x + 7] &&
                                !qrCode.ModuleMatrix[y][x + 8] &&
                                !qrCode.ModuleMatrix[y][x + 9] &&
                                !qrCode.ModuleMatrix[y][x + 10]) ||
                                (!qrCode.ModuleMatrix[y][x] &&
                                !qrCode.ModuleMatrix[y][x + 1] &&
                                !qrCode.ModuleMatrix[y][x + 2] &&
                                !qrCode.ModuleMatrix[y][x + 3] &&
                                qrCode.ModuleMatrix[y][x + 4] &&
                                !qrCode.ModuleMatrix[y][x + 5] &&
                                qrCode.ModuleMatrix[y][x + 6] &&
                                qrCode.ModuleMatrix[y][x + 7] &&
                                qrCode.ModuleMatrix[y][x + 8] &&
                                !qrCode.ModuleMatrix[y][x + 9] &&
                                qrCode.ModuleMatrix[y][x + 10]))
                            {
                                score3 += 40;
                            }

                            if ((qrCode.ModuleMatrix[x][y] &&
                                !qrCode.ModuleMatrix[x + 1][y] &&
                                qrCode.ModuleMatrix[x + 2][y] &&
                                qrCode.ModuleMatrix[x + 3][y] &&
                                qrCode.ModuleMatrix[x + 4][y] &&
                                !qrCode.ModuleMatrix[x + 5][y] &&
                                qrCode.ModuleMatrix[x + 6][y] &&
                                !qrCode.ModuleMatrix[x + 7][y] &&
                                !qrCode.ModuleMatrix[x + 8][y] &&
                                !qrCode.ModuleMatrix[x + 9][y] &&
                                !qrCode.ModuleMatrix[x + 10][y]) ||
                                (!qrCode.ModuleMatrix[x][y] &&
                                !qrCode.ModuleMatrix[x + 1][y] &&
                                !qrCode.ModuleMatrix[x + 2][y] &&
                                !qrCode.ModuleMatrix[x + 3][y] &&
                                qrCode.ModuleMatrix[x + 4][y] &&
                                !qrCode.ModuleMatrix[x + 5][y] &&
                                qrCode.ModuleMatrix[x + 6][y] &&
                                qrCode.ModuleMatrix[x + 7][y] &&
                                qrCode.ModuleMatrix[x + 8][y] &&
                                !qrCode.ModuleMatrix[x + 9][y] &&
                                qrCode.ModuleMatrix[x + 10][y]))
                            {
                                score3 += 40;
                            }
                        }
                    }

                    //Penalty 4
                    int blackModules = 0;
                    foreach (var bitArray in qrCode.ModuleMatrix)
                        for (var x = 0; x < size; x++)
                            if (bitArray[x])
                                blackModules++;

                    var percentDiv5 = blackModules * 20 / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count);
                    var prevMultipleOf5 = Math.Abs(percentDiv5 - 10);
                    var nextMultipleOf5 = Math.Abs(percentDiv5 - 9);
                    score4 = Math.Min(prevMultipleOf5, nextMultipleOf5) * 10;

                    return (score1 + score2) + (score3 + score4);
                }
            }
        }
    }
}
