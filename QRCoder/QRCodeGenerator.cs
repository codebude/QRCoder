using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Globalization;

namespace QRCoder
{
    public class QRCodeGenerator : IDisposable
    {
        private static readonly char[] alphanumEncTable = { ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
        private static readonly int[] capacityBaseValues = { 41, 25, 17, 10, 34, 20, 14, 8, 27, 16, 11, 7, 17, 10, 7, 4, 77, 47, 32, 20, 63, 38, 26, 16, 48, 29, 20, 12, 34, 20, 14, 8, 127, 77, 53, 32, 101, 61, 42, 26, 77, 47, 32, 20, 58, 35, 24, 15, 187, 114, 78, 48, 149, 90, 62, 38, 111, 67, 46, 28, 82, 50, 34, 21, 255, 154, 106, 65, 202, 122, 84, 52, 144, 87, 60, 37, 106, 64, 44, 27, 322, 195, 134, 82, 255, 154, 106, 65, 178, 108, 74, 45, 139, 84, 58, 36, 370, 224, 154, 95, 293, 178, 122, 75, 207, 125, 86, 53, 154, 93, 64, 39, 461, 279, 192, 118, 365, 221, 152, 93, 259, 157, 108, 66, 202, 122, 84, 52, 552, 335, 230, 141, 432, 262, 180, 111, 312, 189, 130, 80, 235, 143, 98, 60, 652, 395, 271, 167, 513, 311, 213, 131, 364, 221, 151, 93, 288, 174, 119, 74, 772, 468, 321, 198, 604, 366, 251, 155, 427, 259, 177, 109, 331, 200, 137, 85, 883, 535, 367, 226, 691, 419, 287, 177, 489, 296, 203, 125, 374, 227, 155, 96, 1022, 619, 425, 262, 796, 483, 331, 204, 580, 352, 241, 149, 427, 259, 177, 109, 1101, 667, 458, 282, 871, 528, 362, 223, 621, 376, 258, 159, 468, 283, 194, 120, 1250, 758, 520, 320, 991, 600, 412, 254, 703, 426, 292, 180, 530, 321, 220, 136, 1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470, 322, 198, 602, 365, 250, 154, 1548, 938, 644, 397, 1212, 734, 504, 310, 876, 531, 364, 224, 674, 408, 280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345, 948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153, 792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272, 813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970, 666, 410, 1159, 702, 482, 297, 919, 557, 382, 235, 2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742, 509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618, 1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640, 439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528, 1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704, 1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407, 1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451, 997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330, 3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094, 751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902, 2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910, 625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732, 2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369, 1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559, 1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994, 1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457, 4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499, 1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201, 3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226, 842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002, 2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183, 2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756, 2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632, 1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605, 5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966, 1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577, 4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591, 1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292, 3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087, 2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972, 2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391, 2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784 };
        private static readonly int[] capacityECCBaseValues = { 19, 7, 1, 19, 0, 0, 16, 10, 1, 16, 0, 0, 13, 13, 1, 13, 0, 0, 9, 17, 1, 9, 0, 0, 34, 10, 1, 34, 0, 0, 28, 16, 1, 28, 0, 0, 22, 22, 1, 22, 0, 0, 16, 28, 1, 16, 0, 0, 55, 15, 1, 55, 0, 0, 44, 26, 1, 44, 0, 0, 34, 18, 2, 17, 0, 0, 26, 22, 2, 13, 0, 0, 80, 20, 1, 80, 0, 0, 64, 18, 2, 32, 0, 0, 48, 26, 2, 24, 0, 0, 36, 16, 4, 9, 0, 0, 108, 26, 1, 108, 0, 0, 86, 24, 2, 43, 0, 0, 62, 18, 2, 15, 2, 16, 46, 22, 2, 11, 2, 12, 136, 18, 2, 68, 0, 0, 108, 16, 4, 27, 0, 0, 76, 24, 4, 19, 0, 0, 60, 28, 4, 15, 0, 0, 156, 20, 2, 78, 0, 0, 124, 18, 4, 31, 0, 0, 88, 18, 2, 14, 4, 15, 66, 26, 4, 13, 1, 14, 194, 24, 2, 97, 0, 0, 154, 22, 2, 38, 2, 39, 110, 22, 4, 18, 2, 19, 86, 26, 4, 14, 2, 15, 232, 30, 2, 116, 0, 0, 182, 22, 3, 36, 2, 37, 132, 20, 4, 16, 4, 17, 100, 24, 4, 12, 4, 13, 274, 18, 2, 68, 2, 69, 216, 26, 4, 43, 1, 44, 154, 24, 6, 19, 2, 20, 122, 28, 6, 15, 2, 16, 324, 20, 4, 81, 0, 0, 254, 30, 1, 50, 4, 51, 180, 28, 4, 22, 4, 23, 140, 24, 3, 12, 8, 13, 370, 24, 2, 92, 2, 93, 290, 22, 6, 36, 2, 37, 206, 26, 4, 20, 6, 21, 158, 28, 7, 14, 4, 15, 428, 26, 4, 107, 0, 0, 334, 22, 8, 37, 1, 38, 244, 24, 8, 20, 4, 21, 180, 22, 12, 11, 4, 12, 461, 30, 3, 115, 1, 116, 365, 24, 4, 40, 5, 41, 261, 20, 11, 16, 5, 17, 197, 24, 11, 12, 5, 13, 523, 22, 5, 87, 1, 88, 415, 24, 5, 41, 5, 42, 295, 30, 5, 24, 7, 25, 223, 24, 11, 12, 7, 13, 589, 24, 5, 98, 1, 99, 453, 28, 7, 45, 3, 46, 325, 24, 15, 19, 2, 20, 253, 30, 3, 15, 13, 16, 647, 28, 1, 107, 5, 108, 507, 28, 10, 46, 1, 47, 367, 28, 1, 22, 15, 23, 283, 28, 2, 14, 17, 15, 721, 30, 5, 120, 1, 121, 563, 26, 9, 43, 4, 44, 397, 28, 17, 22, 1, 23, 313, 28, 2, 14, 19, 15, 795, 28, 3, 113, 4, 114, 627, 26, 3, 44, 11, 45, 445, 26, 17, 21, 4, 22, 341, 26, 9, 13, 16, 14, 861, 28, 3, 107, 5, 108, 669, 26, 3, 41, 13, 42, 485, 30, 15, 24, 5, 25, 385, 28, 15, 15, 10, 16, 932, 28, 4, 116, 4, 117, 714, 26, 17, 42, 0, 0, 512, 28, 17, 22, 6, 23, 406, 30, 19, 16, 6, 17, 1006, 28, 2, 111, 7, 112, 782, 28, 17, 46, 0, 0, 568, 30, 7, 24, 16, 25, 442, 24, 34, 13, 0, 0, 1094, 30, 4, 121, 5, 122, 860, 28, 4, 47, 14, 48, 614, 30, 11, 24, 14, 25, 464, 30, 16, 15, 14, 16, 1174, 30, 6, 117, 4, 118, 914, 28, 6, 45, 14, 46, 664, 30, 11, 24, 16, 25, 514, 30, 30, 16, 2, 17, 1276, 26, 8, 106, 4, 107, 1000, 28, 8, 47, 13, 48, 718, 30, 7, 24, 22, 25, 538, 30, 22, 15, 13, 16, 1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46, 4, 47, 754, 28, 28, 22, 6, 23, 596, 30, 33, 16, 4, 17, 1468, 30, 8, 122, 4, 123, 1128, 28, 22, 45, 3, 46, 808, 30, 8, 23, 26, 24, 628, 30, 12, 15, 28, 16, 1531, 30, 3, 117, 10, 118, 1193, 28, 3, 45, 23, 46, 871, 30, 4, 24, 31, 25, 661, 30, 11, 15, 31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28, 21, 45, 7, 46, 911, 30, 1, 23, 37, 24, 701, 30, 19, 15, 26, 16, 1735, 30, 5, 115, 10, 116, 1373, 28, 19, 47, 10, 48, 985, 30, 15, 24, 25, 25, 745, 30, 23, 15, 25, 16, 1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46, 29, 47, 1033, 30, 42, 24, 1, 25, 793, 30, 23, 15, 28, 16, 1955, 30, 17, 115, 0, 0, 1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24, 35, 25, 845, 30, 19, 15, 35, 16, 2071, 30, 17, 115, 1, 116, 1631, 28, 14, 46, 21, 47, 1171, 30, 29, 24, 19, 25, 901, 30, 11, 15, 46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28, 14, 46, 23, 47, 1231, 30, 44, 24, 7, 25, 961, 30, 59, 16, 1, 17, 2306, 30, 12, 121, 7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30, 39, 24, 14, 25, 986, 30, 22, 15, 41, 16, 2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47, 34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30, 2, 15, 64, 16, 2566, 30, 17, 122, 4, 123, 1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24, 10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30, 4, 122, 18, 123, 2102, 28, 13, 46, 32, 47, 1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15, 32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28, 40, 47, 7, 48, 1582, 30, 43, 24, 22, 25, 1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118, 6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30, 34, 24, 34, 25, 1276, 30, 20, 15, 61, 16 };
        private static readonly int[] alignmentPatternBaseValues = { 0, 0, 0, 0, 0, 0, 0, 6, 18, 0, 0, 0, 0, 0, 6, 22, 0, 0, 0, 0, 0, 6, 26, 0, 0, 0, 0, 0, 6, 30, 0, 0, 0, 0, 0, 6, 34, 0, 0, 0, 0, 0, 6, 22, 38, 0, 0, 0, 0, 6, 24, 42, 0, 0, 0, 0, 6, 26, 46, 0, 0, 0, 0, 6, 28, 50, 0, 0, 0, 0, 6, 30, 54, 0, 0, 0, 0, 6, 32, 58, 0, 0, 0, 0, 6, 34, 62, 0, 0, 0, 0, 6, 26, 46, 66, 0, 0, 0, 6, 26, 48, 70, 0, 0, 0, 6, 26, 50, 74, 0, 0, 0, 6, 30, 54, 78, 0, 0, 0, 6, 30, 56, 82, 0, 0, 0, 6, 30, 58, 86, 0, 0, 0, 6, 34, 62, 90, 0, 0, 0, 6, 28, 50, 72, 94, 0, 0, 6, 26, 50, 74, 98, 0, 0, 6, 30, 54, 78, 102, 0, 0, 6, 28, 54, 80, 106, 0, 0, 6, 32, 58, 84, 110, 0, 0, 6, 30, 58, 86, 114, 0, 0, 6, 34, 62, 90, 118, 0, 0, 6, 26, 50, 74, 98, 122, 0, 6, 30, 54, 78, 102, 126, 0, 6, 26, 52, 78, 104, 130, 0, 6, 30, 56, 82, 108, 134, 0, 6, 34, 60, 86, 112, 138, 0, 6, 30, 58, 86, 114, 142, 0, 6, 34, 62, 90, 118, 146, 0, 6, 30, 54, 78, 102, 126, 150, 6, 24, 50, 76, 102, 128, 154, 6, 28, 54, 80, 106, 132, 158, 6, 32, 58, 84, 110, 136, 162, 6, 26, 54, 82, 110, 138, 166, 6, 30, 58, 86, 114, 142, 170 };
        private static readonly int[] remainderBits = { 0, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0 };

        private static readonly Dictionary<int, AlignmentPattern> alignmentPatternTable = CreateAlignmentPatternTable();
        private static readonly List<ECCInfo> capacityECCTable = CreateCapacityECCTable();
        private static readonly List<VersionInfo> capacityTable = CreateCapacityTable();
        private static readonly int[] galoisFieldByExponentAlpha;
        private static readonly int[] galoisFieldByIntegerValue;
        private static readonly Dictionary<char, int> alphanumEncDict = CreateAlphanumEncDict();

        public enum EciMode
        {
            Default = 0,
            Iso8859_1 = 3,
            Iso8859_2 = 4,
            Utf8 = 26
        }

        /// <summary>
        /// Initializes the QR code generator
        /// </summary>
        public QRCodeGenerator()
        {
        }

        static QRCodeGenerator()
        {
            var galoisField = CreateAntilogTable();
            galoisFieldByExponentAlpha = new int[256];
            galoisFieldByIntegerValue = new int[256];
            for (int i = 255; i >= 0; i--) // the value 1 occurs twice in the galois field, so we start from the end to get the correct value
            {
                var entry = galoisField[i];
                galoisFieldByExponentAlpha[entry.ExponentAlpha] = checked((byte)entry.IntegerValue);
                galoisFieldByIntegerValue[entry.IntegerValue] = checked((byte)entry.ExponentAlpha);
            }
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="payload">A payload object, generated by the PayloadGenerator-class</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public QRCodeData CreateQrCode(PayloadGenerator.Payload payload)
        {
            return GenerateQrCode(payload);
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="payload">A payload object, generated by the PayloadGenerator-class</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public QRCodeData CreateQrCode(PayloadGenerator.Payload payload, ECCLevel eccLevel)
        {
            return GenerateQrCode(payload, eccLevel);
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="plainText">The payload which shall be encoded in the QR code</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <param name="forceUtf8">Shall the generator be forced to work in UTF-8 mode?</param>
        /// <param name="utf8BOM">Should the byte-order-mark be used?</param>
        /// <param name="eciMode">Which ECI mode shall be used?</param>
        /// <param name="requestedVersion">Set fixed QR code target version.</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public QRCodeData CreateQrCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1)
        {
            return GenerateQrCode(plainText, eccLevel, forceUtf8, utf8BOM, eciMode, requestedVersion);
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="binaryData">A byte array which shall be encoded/stored in the QR code</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public QRCodeData CreateQrCode(byte[] binaryData, ECCLevel eccLevel)
        {
            return GenerateQrCode(binaryData, eccLevel);
        }


        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="payload">A payload object, generated by the PayloadGenerator-class</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public static QRCodeData GenerateQrCode(PayloadGenerator.Payload payload)
        {
            return GenerateQrCode(payload.ToString(), payload.EccLevel, false, false, payload.EciMode, payload.Version);
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="payload">A payload object, generated by the PayloadGenerator-class</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public static QRCodeData GenerateQrCode(PayloadGenerator.Payload payload, ECCLevel eccLevel)
        {
            return GenerateQrCode(payload.ToString(), eccLevel, false, false, payload.EciMode, payload.Version);
        }

        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="plainText">The payload which shall be encoded in the QR code</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <param name="forceUtf8">Shall the generator be forced to work in UTF-8 mode?</param>
        /// <param name="utf8BOM">Should the byte-order-mark be used?</param>
        /// <param name="eciMode">Which ECI mode shall be used?</param>
        /// <param name="requestedVersion">Set fixed QR code target version.</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public static QRCodeData GenerateQrCode(string plainText, ECCLevel eccLevel, bool forceUtf8 = false, bool utf8BOM = false, EciMode eciMode = EciMode.Default, int requestedVersion = -1)
        {
            EncodingMode encoding = GetEncodingFromPlaintext(plainText, forceUtf8);
            var codedText = PlainTextToBinary(plainText, encoding, eciMode, utf8BOM, forceUtf8);
            var dataInputLength = GetDataLength(encoding, plainText, codedText, forceUtf8);
            int version = requestedVersion;
            if (version == -1)
            {
                version = GetVersion(dataInputLength + (eciMode != EciMode.Default ? 2 : 0), encoding, eccLevel);
            }
            else
            {
                //Version was passed as fixed version via parameter. Thus let's check if chosen version is valid.
                var minVersion = GetVersion(dataInputLength + (eciMode != EciMode.Default ? 2 : 0), encoding, eccLevel);
                if (minVersion > version)
                {
                    var maxSizeByte = capacityTable[version - 1].Details.First(x => x.ErrorCorrectionLevel == eccLevel).CapacityDict[encoding];
                    throw new QRCoder.Exceptions.DataTooLongException(eccLevel.ToString(), encoding.ToString(), version, maxSizeByte);
                }
            }

            var countIndicatorLength = GetCountIndicatorLength(version, encoding);

            var completeBitArrayLength =
                (eciMode != EciMode.Default ? 16 : 4) +  // Mode indicator
                countIndicatorLength + // Count indicator
                codedText.Length;                            // Data

            var completeBitArray = new BitArray(completeBitArrayLength);
            // write mode indicator
            var completeBitArrayIndex = 0;
            if (eciMode != EciMode.Default)
            {
                DecToBin((int)EncodingMode.ECI, 4, completeBitArray, ref completeBitArrayIndex);
                DecToBin((int)eciMode, 8, completeBitArray, ref completeBitArrayIndex);
            }
            DecToBin((int)encoding, 4, completeBitArray, ref completeBitArrayIndex);
            // write count indicator
            DecToBin(dataInputLength, countIndicatorLength, completeBitArray, ref completeBitArrayIndex);
            // write data
            for (int i = 0; i < codedText.Length; i++)
            {
                completeBitArray[completeBitArrayIndex++] = codedText[i];
            }

            return GenerateQrCode(completeBitArray, eccLevel, version);
        }


        /// <summary>
        /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
        /// </summary>
        /// <param name="binaryData">A byte array which shall be encoded/stored in the QR code</param>
        /// <param name="eccLevel">The level of error correction data</param>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
        /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
        public static QRCodeData GenerateQrCode(byte[] binaryData, ECCLevel eccLevel)
        {
            int version = GetVersion(binaryData.Length, EncodingMode.Byte, eccLevel);

            int countIndicatorLen = GetCountIndicatorLength(version, EncodingMode.Byte);
            var bitArray = ToBitArray(binaryData, 4 + countIndicatorLen);
            var index = 0;
            DecToBin((int)EncodingMode.Byte, 4, bitArray, ref index);
            DecToBin(binaryData.Length, countIndicatorLen, bitArray, ref index);

            return GenerateQrCode(bitArray, eccLevel, version);
        }

        private static readonly BitArray _repeatingPattern = new BitArray(
            new[] { true, true, true, false, true, true, false, false, false, false, false, true, false, false, false, true });

        private static QRCodeData GenerateQrCode(BitArray bitArray, ECCLevel eccLevel, int version)
        {
            //Fill up data code word
            var eccInfo = capacityECCTable.Single(x => x.Version == version && x.ErrorCorrectionLevel == eccLevel);
            var dataLength = eccInfo.TotalDataCodewords * 8;
            var lengthDiff = dataLength - bitArray.Length;
            if (lengthDiff > 0)
            {
                // set 'write index' to end of existing bit array
                var index = bitArray.Length;
                // extend bit array to required length
                bitArray.Length = dataLength;
                // pad with 4 zeros (or less if lengthDiff < 4)
                index += Math.Min(lengthDiff, 4);
                // pad to nearest 8 bit boundary
                if (index % 8 != 0)
                    index += 8 - (index % 8);
                // pad with repeating pattern
                var repeatingPatternIndex = 0;
                while (index < dataLength)
                {
                    bitArray[index++] = _repeatingPattern[repeatingPatternIndex++];
                    if (repeatingPatternIndex >= _repeatingPattern.Length)
                        repeatingPatternIndex = 0;
                }
            }

            //Calculate error correction words
            var codeWordWithECC = new List<CodewordBlock>(eccInfo.BlocksInGroup1 + eccInfo.BlocksInGroup2);
            AddCodeWordBlocks(1, eccInfo.BlocksInGroup1, eccInfo.CodewordsInGroup1, bitArray, 0, bitArray.Length);
            int offset = eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8;
            AddCodeWordBlocks(2, eccInfo.BlocksInGroup2, eccInfo.CodewordsInGroup2, bitArray, offset, bitArray.Length - offset);


            //Calculate interleaved code word lengths
            int interleavedLength = 0;
            for (var i = 0; i < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.CodeWords.Length > i)
                        interleavedLength += 8;
            }
            for (var i = 0; i < eccInfo.ECCPerBlock; i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.ECCWords.Length > i)
                        interleavedLength += 8;
            }
            interleavedLength += remainderBits[version - 1];

            //Interleave code words
            var interleavedData = new BitArray(interleavedLength);
            int pos = 0;
            for (var i = 0; i < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.CodeWords.Length > i)
                        DecToBin(codeBlock.CodeWords[i], 8, interleavedData, ref pos);
            }
            for (var i = 0; i < eccInfo.ECCPerBlock; i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.ECCWords.Length > i)
                        DecToBin(codeBlock.ECCWords[i], 8, interleavedData, ref pos);
            }


            /*
            var interleavedWordsSb = new StringBuilder();
            for (var i = 0; i < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.CodeWords.Count > i)
                        interleavedWordsSb.Append(codeBlock.CodeWords[i]);
            }


            for (var i = 0; i < eccInfo.ECCPerBlock; i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.ECCWords.Count > i)
                        interleavedWordsSb.Append(codeBlock.ECCWords[i]);
            }
            interleavedWordsSb.Append(new string('0', remainderBits[version - 1]));
            var interleavedData = ToBitArray(interleavedWordsSb.ToString());
            */


            //Place interleaved data on module matrix
            var qr = new QRCodeData(version);
            var blockedModules = new List<Rectangle>(17);
            ModulePlacer.PlaceFinderPatterns(qr, blockedModules);
            ModulePlacer.ReserveSeperatorAreas(qr.ModuleMatrix.Count, blockedModules);
            ModulePlacer.PlaceAlignmentPatterns(qr, alignmentPatternTable[version].PatternPositions, blockedModules);
            ModulePlacer.PlaceTimingPatterns(qr, blockedModules);
            ModulePlacer.PlaceDarkModule(qr, version, blockedModules);
            ModulePlacer.ReserveVersionAreas(qr.ModuleMatrix.Count, version, blockedModules);
            ModulePlacer.PlaceDataWords(qr, interleavedData, blockedModules);
            var maskVersion = ModulePlacer.MaskCode(qr, version, blockedModules, eccLevel);
            var formatStr = GetFormatString(eccLevel, maskVersion);

            ModulePlacer.PlaceFormat(qr, formatStr);
            if (version >= 7)
            {
                var versionString = GetVersionString(version);
                ModulePlacer.PlaceVersion(qr, versionString);
            }


            ModulePlacer.AddQuietZone(qr);
            return qr;

            void AddCodeWordBlocks(int blockNum, int blocksInGroup, int codewordsInGroup, BitArray bitArray2, int offset2, int count)
            {
                var groupLength = codewordsInGroup * 8;
                for (var i = 0; i < blocksInGroup; i++)
                {
                    var bitBlockList = BinaryStringToBitBlockByteList(bitArray2, offset2, groupLength);
                    var eccWordList = CalculateECCWords(bitArray2, offset2, groupLength, eccInfo);
                    codeWordWithECC.Add(new CodewordBlock(
                                          bitBlockList,
                                          eccWordList)
                                    );
                    offset2 += groupLength;
                }
            }
        }

        private static readonly BitArray _getFormatGenerator = new BitArray(new bool[] { true, false, true, false, false, true, true, false, true, true, true });
        private static readonly BitArray _getFormatMask = new BitArray(new bool[] { true, false, true, false, true, false, false, false, false, false, true, false, false, true, false });
        private static BitArray GetFormatString(ECCLevel level, int maskVersion)
        {
            var fStrEcc = new BitArray(15); // Total length including space for mask version and padding
            WriteEccLevelAndVersion();

            int index = 0;
            int count = 15;
            TrimLeadingZeros(); // modifies index and count
            while (count > 10)
            {
                for (var i = 0; i < _getFormatGenerator.Length; i++)
                    fStrEcc[index + i] ^= _getFormatGenerator[i];
                TrimLeadingZeros(); // modifies index and count
            }
            ShiftTowardsBit0(fStrEcc, index);
            fStrEcc.Length = 10 + 5;
            ShiftAwayFromBit0(fStrEcc, (10 - count) + 5);
            WriteEccLevelAndVersion();
            fStrEcc.Xor(_getFormatMask);
            return fStrEcc;

            void WriteEccLevelAndVersion()
            {
                switch (level)
                {
                    case ECCLevel.L: // 01
                        fStrEcc[1] = true;
                        break;
                    case ECCLevel.H: // 10
                        fStrEcc[0] = true;
                        break;
                    case ECCLevel.Q: // 11
                        fStrEcc[0] = true;
                        fStrEcc[1] = true;
                        break;
                    default: // M: 00
                        break;
                }
                int indexTemp = 2;
                DecToBin(maskVersion, 3, fStrEcc, ref indexTemp);
            }

            void TrimLeadingZeros()
            {
                while (!fStrEcc[index])
                {
                    index++;
                    count--;
                }
            }

        }

        private static void ShiftTowardsBit0(BitArray fStrEcc, int num)
        {
#if NETCOREAPP
            fStrEcc.RightShift(num); // Shift towards bit 0
#else
            for (var i = 0; i < fStrEcc.Length - num; i++)
                fStrEcc[i] = fStrEcc[i + num];
            for (var i = fStrEcc.Length - num; i < fStrEcc.Length; i++)
                fStrEcc[i] = false;
#endif
        }

        private static void ShiftAwayFromBit0(BitArray fStrEcc, int num)
        {
#if NETCOREAPP
            fStrEcc.LeftShift(num); // Shift away from bit 0
#else
            for (var i = fStrEcc.Length - 1; i >= num; i--)
                fStrEcc[i] = fStrEcc[i - num];
            for (var i = 0; i < num; i++)
                fStrEcc[i] = false;
#endif
        }

        private static readonly BitArray _getVersionGenerator = new BitArray(new bool[] { true, true, true, true, true, false, false, true, false, false, true, false, true });
        private static BitArray GetVersionString(int version)
        {
            var vStr = new BitArray(18);
            var index = 0;
            DecToBin(version, 6, vStr, ref index);
            var count = vStr.Length;
            index = 0;
            while (!vStr[index])
            {
                index++;
                count--;
            }
            while (count > 12)
            {
                for (var i = 0; i < _getVersionGenerator.Length; i++)
                    vStr[index + i] ^= _getVersionGenerator[i];
                while (!vStr[index])
                {
                    index++;
                    count--;
                }
            }
            ShiftTowardsBit0(vStr, index);
            vStr.Length = 12 + 6;
            ShiftAwayFromBit0(vStr, (12 - count) + 6);
            index = 0;
            DecToBin(version, 6, vStr, ref index);
            return vStr;
        }

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
                    double blackModules = 0;
                    foreach (var row in qrCode.ModuleMatrix)
                        foreach (bool bit in row)
                            if (bit)
                                blackModules++;

                    var percent = (blackModules / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count)) * 100;
                    var prevMultipleOf5 = Math.Abs((int)Math.Floor(percent / 5) * 5 - 50) / 5;
                    var nextMultipleOf5 = Math.Abs((int)Math.Floor(percent / 5) * 5 - 45) / 5;
                    score4 = Math.Min(prevMultipleOf5, nextMultipleOf5) * 10;

                    return score1 + score2 + score3 + score4;
                }
            }

        }

        private static byte[] CalculateECCWords(BitArray bitArray, int offset, int count, ECCInfo eccInfo)
        {
            var eccWords = eccInfo.ECCPerBlock;
            var messagePolynom = CalculateMessagePolynom(bitArray, offset, count);
            var generatorPolynom = CalculateGeneratorPolynom(eccWords);

            for (var i = 0; i < messagePolynom.PolyItems.Count; i++)
                messagePolynom.PolyItems[i] = new PolynomItem(messagePolynom.PolyItems[i].Coefficient,
                    messagePolynom.PolyItems[i].Exponent + eccWords);

            for (var i = 0; i < generatorPolynom.PolyItems.Count; i++)
                generatorPolynom.PolyItems[i] = new PolynomItem(generatorPolynom.PolyItems[i].Coefficient,
                    generatorPolynom.PolyItems[i].Exponent + (messagePolynom.PolyItems.Count - 1));

            var leadTermSource = messagePolynom;
            for (var i = 0; (leadTermSource.PolyItems.Count > 0 && leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent > 0); i++)
            {
                if (leadTermSource.PolyItems[0].Coefficient == 0)
                {
                    leadTermSource.PolyItems.RemoveAt(0);
                    leadTermSource.PolyItems.Add(new PolynomItem(0, leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent - 1));
                }
                else
                {
                    var resPoly = MultiplyGeneratorPolynomByLeadterm(generatorPolynom, ConvertToAlphaNotation(leadTermSource).PolyItems[0], i);
                    ConvertToDecNotationInPlace(resPoly);
                    resPoly = XORPolynoms(leadTermSource, resPoly);
                    leadTermSource = resPoly;
                }
            }
            var ret = new byte[leadTermSource.PolyItems.Count];
            for (var i = 0; i < leadTermSource.PolyItems.Count; i++)
                ret[i] = (byte)leadTermSource.PolyItems[i].Coefficient;
            return ret;
        }

        private static Polynom ConvertToAlphaNotation(Polynom poly)
        {
            var newPoly = new Polynom(poly.PolyItems.Count);
            for (var i = 0; i < poly.PolyItems.Count; i++)
                newPoly.PolyItems.Add(
                    new PolynomItem(
                        (poly.PolyItems[i].Coefficient != 0
                            ? GetAlphaExpFromIntVal(poly.PolyItems[i].Coefficient)
                            : 0), poly.PolyItems[i].Exponent));
            return newPoly;
        }

        private static void ConvertToDecNotationInPlace(Polynom poly)
        {
            for (var i = 0; i < poly.PolyItems.Count; i++)
                poly.PolyItems[i] = new PolynomItem(GetIntValFromAlphaExp(poly.PolyItems[i].Coefficient), poly.PolyItems[i].Exponent);
        }

        private static int GetVersion(int length, EncodingMode encMode, ECCLevel eccLevel)
        {

            var fittingVersions = capacityTable.Where(
                x => x.Details.Any(
                    y => (y.ErrorCorrectionLevel == eccLevel
                          && y.CapacityDict[encMode] >= Convert.ToInt32(length)
                          )
                    )
              ).Select(x => new
              {
                  version = x.Version,
                  capacity = x.Details.Single(y => y.ErrorCorrectionLevel == eccLevel)
                                            .CapacityDict[encMode]
              });

            if (fittingVersions.Any())
                return fittingVersions.Min(x => x.version);

            var maxSizeByte = capacityTable.Where(
                x => x.Details.Any(
                    y => (y.ErrorCorrectionLevel == eccLevel))
                ).Max(x => x.Details.Single(y => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]);
            throw new QRCoder.Exceptions.DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
        }

        private static EncodingMode GetEncodingFromPlaintext(string plainText, bool forceUtf8)
        {
            if (forceUtf8) return EncodingMode.Byte;
            EncodingMode result = EncodingMode.Numeric; // assume numeric
            foreach (char c in plainText)
            {
                if (IsInRange(c, '0', '9')) continue;   // numeric - char.IsDigit() for Latin1
                result = EncodingMode.Alphanumeric;     // not numeric, assume alphanumeric
                if (IsInRange(c, 'A', 'Z') || alphanumEncTable.Contains(c)) continue; // alphanumeric
                return EncodingMode.Byte;               // not numeric or alphanumeric, assume byte
            }
            return result;                              // either numeric or alphanumeric
        }

        private static bool IsInRange(char c, char min, char max)
        {
            return (uint)(c - min) <= (uint)(max - min);
        }

        private static Polynom CalculateMessagePolynom(BitArray bitArray, int offset, int count)
        {
            var messagePol = new Polynom(count / 8);
            for (var i = count / 8 - 1; i >= 0; i--)
            {
                messagePol.PolyItems.Add(new PolynomItem(BinToDec(bitArray, offset, 8), i));
                offset += 8;
            }
            return messagePol;
        }


        private static Polynom CalculateGeneratorPolynom(int numEccWords)
        {
            var generatorPolynom = new Polynom(2);
            generatorPolynom.PolyItems.Add(new PolynomItem(0, 1));
            generatorPolynom.PolyItems.Add(new PolynomItem(0, 0));
            var multiplierPolynom = new Polynom(numEccWords * 2);
            for (var i = 1; i <= numEccWords - 1; i++)
            {
                multiplierPolynom.PolyItems.Clear();
                multiplierPolynom.PolyItems.Add(new PolynomItem(0, 1));
                multiplierPolynom.PolyItems.Add(new PolynomItem(i, 0));

                generatorPolynom = MultiplyAlphaPolynoms(generatorPolynom, multiplierPolynom);
            }

            return generatorPolynom;
        }

        private static byte[] BinaryStringToBitBlockByteList(BitArray bitString, int offset, int count)
        {
            const int blockSize = 8;
            if (count % blockSize != 0)
                throw new ArgumentException("Count must be a multiple of 8.", nameof(count));
            var numberOfBlocks = count / blockSize;
            var blocklist = new byte[numberOfBlocks];

            int j = 0;
            count += offset;
            for (int i = offset; i < count; i += blockSize)
            {
                blocklist[j++] = (byte)(
                    (bitString[i] ? 128 : 0) + 
                    (bitString[i + 1] ? 64 : 0) + 
                    (bitString[i + 2] ? 32 : 0) + 
                    (bitString[i + 3] ? 16 : 0) + 
                    (bitString[i + 4] ? 8 : 0) + 
                    (bitString[i + 5] ? 4 : 0) + 
                    (bitString[i + 6] ? 2 : 0) + 
                    (bitString[i + 7] ? 1 : 0));
            }

            return blocklist;
        }

        private static List<int> BinaryStringListToDecList(List<string> binaryStringList)
        {
            return binaryStringList.Select(binaryString => BinToDec(binaryString)).ToList();
        }

        private static int BinToDec(string binStr)
        {
            return Convert.ToInt32(binStr, 2);
        }

        private static int BinToDec(BitArray bitArray, int offset, int count)
        {
            var ret = 0;
            for (int i = 0; i < count; i++)
            {
                ret ^= bitArray[offset + i] ? 1 << (count - i - 1) : 0;
            }
            return ret;
        }

        private static void DecToBin(int decNum, int bits, BitArray bitList, ref int index)
        {
            // Convert decNum to binary using a bitwise operation
            for (int i = bits - 1; i >= 0; i--)
            {
                // Check each bit from most significant to least significant
                bool bit = (decNum & (1 << i)) != 0;
                bitList[index++] = bit;
            }
        }

        private static string DecToBin(int decNum)
        {
            return Convert.ToString(decNum, 2);
        }

        private static string DecToBin(int decNum, int padLeftUpTo)
        {
            var binStr = DecToBin(decNum);
            return binStr.PadLeft(padLeftUpTo, '0');
        }

        private static int GetCountIndicatorLength(int version, EncodingMode encMode)
        {
            if (version < 10)
            {
                if (encMode == EncodingMode.Numeric)
                    return 10;
                else if (encMode == EncodingMode.Alphanumeric)
                    return 9;
                else
                    return 8;
            }
            else if (version < 27)
            {
                if (encMode == EncodingMode.Numeric)
                    return 12;
                else if (encMode == EncodingMode.Alphanumeric)
                    return 11;
                else if (encMode == EncodingMode.Byte)
                    return 16;
                else
                    return 10;
            }
            else
            {
                if (encMode == EncodingMode.Numeric)
                    return 14;
                else if (encMode == EncodingMode.Alphanumeric)
                    return 13;
                else if (encMode == EncodingMode.Byte)
                    return 16;
                else
                    return 12;
            }
        }

        private static int GetDataLength(EncodingMode encoding, string plainText, BitArray codedText, bool forceUtf8)
        {
            return forceUtf8 || IsUtf8(encoding, plainText, forceUtf8) ? (codedText.Length / 8) : plainText.Length;
        }

        private static bool IsUtf8(EncodingMode encoding, string plainText, bool forceUtf8)
        {
            return (encoding == EncodingMode.Byte && (!IsValidISO(plainText) || forceUtf8));
        }

        private static bool IsValidISO(string input)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
            var result = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
            return String.Equals(input, result);
        }

        private static BitArray PlainTextToBinary(string plainText, EncodingMode encMode, EciMode eciMode, bool utf8BOM, bool forceUtf8)
        {
            switch (encMode)
            {
                case EncodingMode.Alphanumeric:
                    return PlainTextToBinaryAlphanumeric(plainText);
                case EncodingMode.Numeric:
                    return PlainTextToBinaryNumeric(plainText);
                case EncodingMode.Byte:
                    return PlainTextToBinaryByte(plainText, eciMode, utf8BOM, forceUtf8);
                case EncodingMode.Kanji:
                case EncodingMode.ECI:
                default:
                    return _emptyBitArray;
            }
        }

        private static readonly BitArray _emptyBitArray = new BitArray(0);

        private static BitArray PlainTextToBinaryNumeric(string plainText)
        {
            var bitArray = new BitArray(plainText.Length / 3 * 10 + (plainText.Length % 3 == 1 ? 4 : plainText.Length % 3 == 2 ? 7 : 0));
            var index = 0;
            for (int i = 0; (i + 2) < plainText.Length; i += 3)
            {
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                DecToBin(dec, 10, bitArray, ref index);
            }
            if (plainText.Length % 3 == 2)
            {
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                DecToBin(dec, 7, bitArray, ref index);
            }
            else if (plainText.Length % 3 == 1)
            {
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                DecToBin(dec, 4, bitArray, ref index);
            }
            return bitArray;
        }

        private static BitArray PlainTextToBinaryAlphanumeric(string plainText)
        {
            var codeText = new BitArray((plainText.Length / 2) * 11 + (plainText.Length & 1) * 6);
            var codeIndex = 0;
            var index = 0;
            var count = plainText.Length;
            while (count >= 2)
            {
                var dec = alphanumEncDict[plainText[index++]] * 45 + alphanumEncDict[plainText[index++]];
                DecToBin(dec, 11, codeText, ref codeIndex);
                count -= 2;

            }
            if (count > 0)
            {
                DecToBin(alphanumEncDict[plainText[index]], 6, codeText, ref codeIndex);
            }
            return codeText;
        }

        private BitArray PlainTextToBinaryECI(string plainText)
        {
            var codeText = string.Empty;
            byte[] _bytes = Encoding.GetEncoding("ascii").GetBytes(plainText);
            return ToBitArray(_bytes);
        }

        private static string ConvertToIso8859(string value, string Iso = "ISO-8859-2")
        {
            Encoding iso = Encoding.GetEncoding(Iso);
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(value);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }

        private static BitArray PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
        {
            byte[] codeBytes;

            if (IsValidISO(plainText) && !forceUtf8)
                codeBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText);
            else
            {
                switch (eciMode)
                {
                    case EciMode.Iso8859_1:
                        codeBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(ConvertToIso8859(plainText, "ISO-8859-1"));
                        break;
                    case EciMode.Iso8859_2:
                        codeBytes = Encoding.GetEncoding("ISO-8859-2").GetBytes(ConvertToIso8859(plainText, "ISO-8859-2"));
                        break;
                    case EciMode.Default:
                    case EciMode.Utf8:
                    default:
                        codeBytes = utf8BOM ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(plainText)).ToArray() : Encoding.UTF8.GetBytes(plainText);
                        break;
                }
            }

            return ToBitArray(codeBytes);
        }

        private static BitArray ToBitArray(byte[] byteArray, int prefixZeros = 0)
        {
            var bitArray = new BitArray(byteArray.Length * 8 + prefixZeros);
            for (var i = 0; i < byteArray.Length; i++)
            {
                var byteVal = byteArray[i];
                for (var j = 0; j < 8; j++)
                {
                    bitArray[i * 8 + j + prefixZeros] = (byteVal & (1 << (7 - j))) != 0;
                }
            }
            return bitArray;
        }

        private static Polynom XORPolynoms(Polynom messagePolynom, Polynom resPolynom)
        {
            var resultPolynom = new Polynom(Math.Max(messagePolynom.PolyItems.Count, resPolynom.PolyItems.Count));
            Polynom longPoly, shortPoly;
            if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
            {
                longPoly = messagePolynom;
                shortPoly = resPolynom;
            }
            else
            {
                longPoly = resPolynom;
                shortPoly = messagePolynom;
            }

            for (var i = 0; i < longPoly.PolyItems.Count; i++)
            {
                var polItemRes = new PolynomItem
                (

                        longPoly.PolyItems[i].Coefficient ^
                        (shortPoly.PolyItems.Count > i ? shortPoly.PolyItems[i].Coefficient : 0),
                    messagePolynom.PolyItems[0].Exponent - i
                );
                resultPolynom.PolyItems.Add(polItemRes);
            }
            resultPolynom.PolyItems.RemoveAt(0);
            return resultPolynom;
        }


        private static Polynom MultiplyGeneratorPolynomByLeadterm(Polynom genPolynom, PolynomItem leadTerm, int lowerExponentBy)
        {
            var resultPolynom = new Polynom(genPolynom.PolyItems.Count);
            foreach (var polItemBase in genPolynom.PolyItems)
            {
                var polItemRes = new PolynomItem(

                    (polItemBase.Coefficient + leadTerm.Coefficient) % 255,
                    polItemBase.Exponent - lowerExponentBy
                );
                resultPolynom.PolyItems.Add(polItemRes);
            }
            return resultPolynom;
        }


        private static Polynom MultiplyAlphaPolynoms(Polynom polynomBase, Polynom polynomMultiplier)
        {
            var resultPolynom = new Polynom(polynomMultiplier.PolyItems.Count * polynomBase.PolyItems.Count);
            foreach (var polItemBase in polynomMultiplier.PolyItems)
            {
                foreach (var polItemMulti in polynomBase.PolyItems)
                {
                    var polItemRes = new PolynomItem
                    (
                        ShrinkAlphaExp(polItemBase.Coefficient + polItemMulti.Coefficient),
                        (polItemBase.Exponent + polItemMulti.Exponent)
                    );
                    resultPolynom.PolyItems.Add(polItemRes);
                }
            }
            var toGlue = GetNotUniqueExponents(resultPolynom.PolyItems);
            var gluedPolynoms = new PolynomItem[toGlue.Length];
            var gluedPolynomsIndex = 0;
            foreach (var exponent in toGlue)
            {
                var coefficient = 0;
                foreach (var polynomOld in resultPolynom.PolyItems)
                {
                    if (polynomOld.Exponent == exponent)
                        coefficient ^= GetIntValFromAlphaExp(polynomOld.Coefficient);
                }

                var polynomFixed = new PolynomItem(GetAlphaExpFromIntVal(coefficient), exponent);
                gluedPolynoms[gluedPolynomsIndex++] = polynomFixed;
            }
            for (int i = resultPolynom.PolyItems.Count - 1; i >= 0; i--)
                if (toGlue.Contains(resultPolynom.PolyItems[i].Exponent))
                    resultPolynom.PolyItems.RemoveAt(i);
            foreach (var polynom in gluedPolynoms)
                resultPolynom.PolyItems.Add(polynom);
            resultPolynom.PolyItems.Sort((x, y) => -x.Exponent.CompareTo(y.Exponent));
            return resultPolynom;

            int[] GetNotUniqueExponents(List<PolynomItem> list)
            {
                var dic = new Dictionary<int, bool>();
                foreach (var row in list)
                {
#if NETCOREAPP
                    if (dic.TryAdd(row.Exponent, false))
                        dic[row.Exponent] = true;
#else
                    if (!dic.ContainsKey(row.Exponent))
                        dic.Add(row.Exponent, false);
                    else
                        dic[row.Exponent] = true;
#endif
                }

                int count = 0;
                foreach (var row in dic)
                {
                    if (row.Value)
                        count++;
                }

                var result = new int[count];
                int i = 0;
                foreach (var row in dic)
                {
                    if (row.Value)
                        result[i++] = row.Key;
                }

                return result;
            }
        }

        private static int GetIntValFromAlphaExp(int exp)
        {
            return galoisFieldByExponentAlpha[exp];
        }

        private static int GetAlphaExpFromIntVal(int intVal)
        {
            if (intVal == 0)
                throw new ArgumentOutOfRangeException(nameof(intVal));
            return galoisFieldByIntegerValue[intVal];
        }

        private static int ShrinkAlphaExp(int alphaExp)
        {
            // ReSharper disable once PossibleLossOfFraction
            return (int)((alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
        }

        private static Dictionary<char, int> CreateAlphanumEncDict()
        {
            var localAlphanumEncDict = new Dictionary<char, int>(45);
            //Add numbers
            for (int i = 0; i < 10; i++)
                localAlphanumEncDict.Add($"{i}"[0], i);
            //Add chars
            for (char c = 'A'; c <= 'Z'; c++)
                localAlphanumEncDict.Add(c, localAlphanumEncDict.Count());
            //Add special chars
            for (int i = 0; i < alphanumEncTable.Length; i++)
                localAlphanumEncDict.Add(alphanumEncTable[i], localAlphanumEncDict.Count());
            return localAlphanumEncDict;
        }

        private static Dictionary<int, AlignmentPattern> CreateAlignmentPatternTable()
        {
            var localAlignmentPatternTable = new Dictionary<int, AlignmentPattern>(40);

            for (var i = 0; i < (7 * 40); i = i + 7)
            {
                var points = new List<Point>();
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
                }
                );
            }
            return localAlignmentPatternTable;
        }


        private static List<ECCInfo> CreateCapacityECCTable()
        {
            var localCapacityECCTable = new List<ECCInfo>(160);
            for (var i = 0; i < (4 * 6 * 40); i = i + (4 * 6))
            {
                localCapacityECCTable.AddRange(
                new[]
                {
                    new ECCInfo(
                        (i+24) / 24,
                        ECCLevel.L,
                        capacityECCBaseValues[i],
                        capacityECCBaseValues[i+1],
                        capacityECCBaseValues[i+2],
                        capacityECCBaseValues[i+3],
                        capacityECCBaseValues[i+4],
                        capacityECCBaseValues[i+5]),
                    new ECCInfo
                    (
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.M,
                        totalDataCodewords: capacityECCBaseValues[i+6],
                        eccPerBlock: capacityECCBaseValues[i+7],
                        blocksInGroup1: capacityECCBaseValues[i+8],
                        codewordsInGroup1: capacityECCBaseValues[i+9],
                        blocksInGroup2: capacityECCBaseValues[i+10],
                        codewordsInGroup2: capacityECCBaseValues[i+11]
                    ),
                    new ECCInfo
                    (
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.Q,
                        totalDataCodewords: capacityECCBaseValues[i+12],
                        eccPerBlock: capacityECCBaseValues[i+13],
                        blocksInGroup1: capacityECCBaseValues[i+14],
                        codewordsInGroup1: capacityECCBaseValues[i+15],
                        blocksInGroup2: capacityECCBaseValues[i+16],
                        codewordsInGroup2: capacityECCBaseValues[i+17]
                    ),
                    new ECCInfo
                    (
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.H,
                        totalDataCodewords: capacityECCBaseValues[i+18],
                        eccPerBlock: capacityECCBaseValues[i+19],
                        blocksInGroup1: capacityECCBaseValues[i+20],
                        codewordsInGroup1: capacityECCBaseValues[i+21],
                        blocksInGroup2: capacityECCBaseValues[i+22],
                        codewordsInGroup2: capacityECCBaseValues[i+23]
                    )
                });
            }
            return localCapacityECCTable;
        }

        private static List<VersionInfo> CreateCapacityTable()
        {
            var localCapacityTable = new List<VersionInfo>(40);
            for (var i = 0; i < (16 * 40); i = i + 16)
            {
                localCapacityTable.Add(new VersionInfo(

                    (i + 16) / 16,
                    new List<VersionInfoDetails>(4)
                    {
                        new VersionInfoDetails(
                             ECCLevel.L,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i+1] },
                                 { EncodingMode.Byte, capacityBaseValues[i+2] },
                                 { EncodingMode.Kanji, capacityBaseValues[i+3] },
                            }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.M,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i+4] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i+5] },
                                 { EncodingMode.Byte, capacityBaseValues[i+6] },
                                 { EncodingMode.Kanji, capacityBaseValues[i+7] },
                             }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.Q,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i+8] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i+9] },
                                 { EncodingMode.Byte, capacityBaseValues[i+10] },
                                 { EncodingMode.Kanji, capacityBaseValues[i+11] },
                             }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.H,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i+12] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i+13] },
                                 { EncodingMode.Byte, capacityBaseValues[i+14] },
                                 { EncodingMode.Kanji, capacityBaseValues[i+15] },
                             }
                        )
                    }
                ));
            }
            return localCapacityTable;
        }

        private static List<Antilog> CreateAntilogTable()
        {
            var localGaloisField = new List<Antilog>(256);

            int gfItem = 1;
            for (var i = 0; i < 256; i++)
            {
                localGaloisField.Add(new Antilog(i, gfItem));
                gfItem *= 2;
                if (gfItem > 255)
                    gfItem ^= 285;
            }
            return localGaloisField;
        }

        /// <summary>
        /// Error correction level. These define the tolerance levels for how much of the code can be lost before the code cannot be recovered.
        /// </summary>
        public enum ECCLevel
        {
            /// <summary>
            /// 7% may be lost before recovery is not possible
            /// </summary>
            L,
            /// <summary>
            /// 15% may be lost before recovery is not possible
            /// </summary>
            M,
            /// <summary>
            /// 25% may be lost before recovery is not possible
            /// </summary>
            Q,
            /// <summary>
            /// 30% may be lost before recovery is not possible
            /// </summary>
            H
        }

        private enum EncodingMode
        {
            Numeric = 1,
            Alphanumeric = 2,
            Byte = 4,
            Kanji = 8,
            ECI = 7
        }

        private struct AlignmentPattern
        {
            public int Version;
            public List<Point> PatternPositions;
        }

        private struct CodewordBlock
        {
            public CodewordBlock(byte[] codeWords, byte[] eccWords)
            {
                this.CodeWords = codeWords;
                this.ECCWords = eccWords;
            }

            public byte[] CodeWords { get; }
            public byte[] ECCWords { get; }
        }

        private struct ECCInfo
        {
            public ECCInfo(int version, ECCLevel errorCorrectionLevel, int totalDataCodewords, int eccPerBlock, int blocksInGroup1,
                int codewordsInGroup1, int blocksInGroup2, int codewordsInGroup2)
            {
                this.Version = version;
                this.ErrorCorrectionLevel = errorCorrectionLevel;
                this.TotalDataCodewords = totalDataCodewords;
                this.ECCPerBlock = eccPerBlock;
                this.BlocksInGroup1 = blocksInGroup1;
                this.CodewordsInGroup1 = codewordsInGroup1;
                this.BlocksInGroup2 = blocksInGroup2;
                this.CodewordsInGroup2 = codewordsInGroup2;
            }
            public int Version { get; }
            public ECCLevel ErrorCorrectionLevel { get; }
            public int TotalDataCodewords { get; }
            public int ECCPerBlock { get; }
            public int BlocksInGroup1 { get; }
            public int CodewordsInGroup1 { get; }
            public int BlocksInGroup2 { get; }
            public int CodewordsInGroup2 { get; }
        }

        private struct VersionInfo
        {
            public VersionInfo(int version, List<VersionInfoDetails> versionInfoDetails)
            {
                this.Version = version;
                this.Details = versionInfoDetails;
            }
            public int Version { get; }
            public List<VersionInfoDetails> Details { get; }
        }

        private struct VersionInfoDetails
        {
            public VersionInfoDetails(ECCLevel errorCorrectionLevel, Dictionary<EncodingMode, int> capacityDict)
            {
                this.ErrorCorrectionLevel = errorCorrectionLevel;
                this.CapacityDict = capacityDict;
            }

            public ECCLevel ErrorCorrectionLevel { get; }
            public Dictionary<EncodingMode, int> CapacityDict { get; }
        }

        private struct Antilog
        {
            public Antilog(int exponentAlpha, int integerValue)
            {
                this.ExponentAlpha = exponentAlpha;
                this.IntegerValue = integerValue;
            }
            public int ExponentAlpha { get; }
            public int IntegerValue { get; }
        }

        private struct PolynomItem
        {
            public PolynomItem(int coefficient, int exponent)
            {
                this.Coefficient = coefficient;
                this.Exponent = exponent;
            }

            public int Coefficient { get; }
            public int Exponent { get; }
        }

        private struct Polynom
        {
            public Polynom(int count)
            {
                this.PolyItems = new List<PolynomItem>(count);
            }

            public List<PolynomItem> PolyItems { get; set; }

            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var polyItem in this.PolyItems)
                {
                    sb.Append("a^" + polyItem.Coefficient + "*x^" + polyItem.Exponent + " + ");
                }

                if (sb.Length > 0)
                    sb.Length -= 3;

                return sb.ToString();
            }
        }

        private readonly struct Point
        {
            public int X { get; }
            public int Y { get; }
            public Point(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        private readonly struct Rectangle
        {
            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }

            public Rectangle(int x, int y, int w, int h)
            {
                this.X = x;
                this.Y = y;
                this.Width = w;
                this.Height = h;
            }
        }

        public void Dispose()
        {
            // left for back-compat
        }
    }
}
