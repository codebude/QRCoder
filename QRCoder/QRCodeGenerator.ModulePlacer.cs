using System;
using System.Collections.Generic;
using System.Collections;

namespace QRCoder
{
    public partial class QRCodeGenerator
    {
        private static class ModulePlacer
        {
            public static void AddQuietZone(QRCodeData qrCode)
            {
                var quietLine = new bool[qrCode.ModuleMatrix.Count + 8];
                for (var i = 0; i < quietLine.Length; i++)
                    quietLine[i] = false;
                for (var i = 0; i < 4; i++)
                    qrCode.ModuleMatrix.Insert(0, new BitArray(quietLine));
                for (var i = 0; i < 4; i++)
                    qrCode.ModuleMatrix.Add(new BitArray(quietLine));
                for (var i = 4; i < qrCode.ModuleMatrix.Count - 4; i++)
                {
                    qrCode.ModuleMatrix[i].Length += 8;
                    ShiftAwayFromBit0(qrCode.ModuleMatrix[i], 4);
                }
            }

            public static void PlaceVersion(QRCodeData qrCode, BitArray versionStr)
            {
                var size = qrCode.ModuleMatrix.Count;

                for (var x = 0; x < 6; x++)
                {
                    for (var y = 0; y < 3; y++)
                    {
                        qrCode.ModuleMatrix[y + size - 11][x] = versionStr[17 - (x * 3 + y)];
                        qrCode.ModuleMatrix[x][y + size - 11] = versionStr[17 - (x * 3 + y)];
                    }
                }
            }

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

            public static int MaskCode(QRCodeData qrCode, int version, List<Rectangle> blockedModules, ECCLevel eccLevel)
            {
                int? selectedPattern = null;
                var patternScore = 0;

                var size = qrCode.ModuleMatrix.Count;

                var qrTemp = new QRCodeData(version);
                foreach (var pattern in MaskPattern.Patterns)
                {
                    // reset qrTemp to qrCode
                    for (var y = 0; y < size; y++)
                    {
                        for (var x = 0; x < size; x++)
                        {
                            qrTemp.ModuleMatrix[y][x] = qrCode.ModuleMatrix[y][x];
                        }

                    }

                    var formatStr = GetFormatString(eccLevel, pattern.Key - 1);
                    ModulePlacer.PlaceFormat(qrTemp, formatStr);
                    if (version >= 7)
                    {
                        var versionString = GetVersionString(version);
                        ModulePlacer.PlaceVersion(qrTemp, versionString);
                    }

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
                    if (!selectedPattern.HasValue || patternScore > score)
                    {
                        selectedPattern = pattern.Key;
                        patternScore = score;
                    }
                }

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


            public static void PlaceDataWords(QRCodeData qrCode, BitArray data, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count;
                var up = true;
                var index = 0;
                var count = data.Length;
                for (var x = size - 1; x >= 0; x = x - 2)
                {
                    if (x == 6)
                        x = 5;
                    for (var yMod = 1; yMod <= size; yMod++)
                    {
                        int y;
                        if (up)
                        {
                            y = size - yMod;
                            if (index < count && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x] = data[index++];
                            if (index < count && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x - 1] = data[index++];
                        }
                        else
                        {
                            y = yMod - 1;
                            if (index < count && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x] = data[index++];
                            if (index < count && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                                qrCode.ModuleMatrix[y][x - 1] = data[index++];
                        }
                    }
                    up = !up;
                }
            }

            public static void ReserveSeperatorAreas(int size, List<Rectangle> blockedModules)
            {
                blockedModules.Add(new Rectangle(7, 0, 1, 8));
                blockedModules.Add(new Rectangle(0, 7, 7, 1));
                blockedModules.Add(new Rectangle(0, size - 8, 8, 1));
                blockedModules.Add(new Rectangle(7, size - 7, 1, 7));
                blockedModules.Add(new Rectangle(size - 8, 0, 1, 8));
                blockedModules.Add(new Rectangle(size - 7, 7, 7, 1));
            }

            public static void ReserveVersionAreas(int size, int version, List<Rectangle> blockedModules)
            {
                blockedModules.Add(new Rectangle(8, 0, 1, 6));
                blockedModules.Add(new Rectangle(8, 7, 1, 1));
                blockedModules.Add(new Rectangle(0, 8, 6, 1));
                blockedModules.Add(new Rectangle(7, 8, 2, 1));
                blockedModules.Add(new Rectangle(size - 8, 8, 8, 1));
                blockedModules.Add(new Rectangle(8, size - 7, 1, 7));

                if (version >= 7)
                {
                    blockedModules.Add(new Rectangle(size - 11, 0, 3, 6));
                    blockedModules.Add(new Rectangle(0, size - 11, 6, 3));
                }
            }
            public static void PlaceDarkModule(QRCodeData qrCode, int version, List<Rectangle> blockedModules)
            {
                qrCode.ModuleMatrix[4 * version + 9][8] = true;
                blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
            }

            public static void PlaceFinderPatterns(QRCodeData qrCode, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count;

                for (var i = 0; i < 3; i++)
                {
                    var locationX = i == 1 ? size - 7 : 0;
                    var locationY = i == 2 ? size - 7 : 0;
                    for (var x = 0; x < 7; x++)
                    {
                        for (var y = 0; y < 7; y++)
                        {
                            if (!(((x == 1 || x == 5) && y > 0 && y < 6) || (x > 0 && x < 6 && (y == 1 || y == 5))))
                            {
                                qrCode.ModuleMatrix[y + locationY][x + locationX] = true;
                            }
                        }
                    }
                    blockedModules.Add(new Rectangle(locationX, locationY, 7, 7));
                }
            }

            public static void PlaceAlignmentPatterns(QRCodeData qrCode, List<Point> alignmentPatternLocations, List<Rectangle> blockedModules)
            {
                foreach (var loc in alignmentPatternLocations)
                {
                    var alignmentPatternRect = new Rectangle(loc.X, loc.Y, 5, 5);
                    var blocked = false;
                    foreach (var blockedRect in blockedModules)
                    {
                        if (Intersects(alignmentPatternRect, blockedRect))
                        {
                            blocked = true;
                            break;
                        }
                    }
                    if (blocked)
                        continue;

                    for (var x = 0; x < 5; x++)
                    {
                        for (var y = 0; y < 5; y++)
                        {
                            if (y == 0 || y == 4 || x == 0 || x == 4 || (x == 2 && y == 2))
                            {
                                qrCode.ModuleMatrix[loc.Y + y][loc.X + x] = true;
                            }
                        }
                    }
                    blockedModules.Add(new Rectangle(loc.X, loc.Y, 5, 5));
                }
            }

            public static void PlaceTimingPatterns(QRCodeData qrCode, List<Rectangle> blockedModules)
            {
                var size = qrCode.ModuleMatrix.Count;
                for (var i = 8; i < size - 8; i++)
                {
                    if (i % 2 == 0)
                    {
                        qrCode.ModuleMatrix[6][i] = true;
                        qrCode.ModuleMatrix[i][6] = true;
                    }
                }
                blockedModules.Add(new Rectangle(6, 8, 1, size - 16));
                blockedModules.Add(new Rectangle(8, 6, size - 16, 1));
            }

            private static bool Intersects(Rectangle r1, Rectangle r2)
            {
                return r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height;
            }

            private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
            {
                foreach (var blockedMod in blockedModules)
                {
                    if (Intersects(blockedMod, r1))
                        return true;
                }
                return false;
            }

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
