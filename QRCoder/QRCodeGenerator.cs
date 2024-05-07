using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace QRCoder
{
    public partial class QRCodeGenerator : IDisposable
    {
        private static readonly char[] alphanumEncTable = { ' ', '$', '%', '*', '+', '-', '.', '/', ':' };
        private static readonly int[] capacityBaseValues = { 41, 25, 17, 10, 34, 20, 14, 8, 27, 16, 11, 7, 17, 10, 7, 4, 77, 47, 32, 20, 63, 38, 26, 16, 48, 29, 20, 12, 34, 20, 14, 8, 127, 77, 53, 32, 101, 61, 42, 26, 77, 47, 32, 20, 58, 35, 24, 15, 187, 114, 78, 48, 149, 90, 62, 38, 111, 67, 46, 28, 82, 50, 34, 21, 255, 154, 106, 65, 202, 122, 84, 52, 144, 87, 60, 37, 106, 64, 44, 27, 322, 195, 134, 82, 255, 154, 106, 65, 178, 108, 74, 45, 139, 84, 58, 36, 370, 224, 154, 95, 293, 178, 122, 75, 207, 125, 86, 53, 154, 93, 64, 39, 461, 279, 192, 118, 365, 221, 152, 93, 259, 157, 108, 66, 202, 122, 84, 52, 552, 335, 230, 141, 432, 262, 180, 111, 312, 189, 130, 80, 235, 143, 98, 60, 652, 395, 271, 167, 513, 311, 213, 131, 364, 221, 151, 93, 288, 174, 119, 74, 772, 468, 321, 198, 604, 366, 251, 155, 427, 259, 177, 109, 331, 200, 137, 85, 883, 535, 367, 226, 691, 419, 287, 177, 489, 296, 203, 125, 374, 227, 155, 96, 1022, 619, 425, 262, 796, 483, 331, 204, 580, 352, 241, 149, 427, 259, 177, 109, 1101, 667, 458, 282, 871, 528, 362, 223, 621, 376, 258, 159, 468, 283, 194, 120, 1250, 758, 520, 320, 991, 600, 412, 254, 703, 426, 292, 180, 530, 321, 220, 136, 1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470, 322, 198, 602, 365, 250, 154, 1548, 938, 644, 397, 1212, 734, 504, 310, 876, 531, 364, 224, 674, 408, 280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345, 948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153, 792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272, 813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970, 666, 410, 1159, 702, 482, 297, 919, 557, 382, 235, 2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742, 509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618, 1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640, 439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528, 1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704, 1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407, 1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451, 997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330, 3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094, 751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902, 2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910, 625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732, 2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369, 1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559, 1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994, 1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457, 4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499, 1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201, 3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226, 842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002, 2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183, 2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756, 2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632, 1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605, 5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966, 1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577, 4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591, 1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292, 3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087, 2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972, 2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391, 2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784 };
        private static readonly int[] capacityECCBaseValues = { 19, 7, 1, 19, 0, 0, 16, 10, 1, 16, 0, 0, 13, 13, 1, 13, 0, 0, 9, 17, 1, 9, 0, 0, 34, 10, 1, 34, 0, 0, 28, 16, 1, 28, 0, 0, 22, 22, 1, 22, 0, 0, 16, 28, 1, 16, 0, 0, 55, 15, 1, 55, 0, 0, 44, 26, 1, 44, 0, 0, 34, 18, 2, 17, 0, 0, 26, 22, 2, 13, 0, 0, 80, 20, 1, 80, 0, 0, 64, 18, 2, 32, 0, 0, 48, 26, 2, 24, 0, 0, 36, 16, 4, 9, 0, 0, 108, 26, 1, 108, 0, 0, 86, 24, 2, 43, 0, 0, 62, 18, 2, 15, 2, 16, 46, 22, 2, 11, 2, 12, 136, 18, 2, 68, 0, 0, 108, 16, 4, 27, 0, 0, 76, 24, 4, 19, 0, 0, 60, 28, 4, 15, 0, 0, 156, 20, 2, 78, 0, 0, 124, 18, 4, 31, 0, 0, 88, 18, 2, 14, 4, 15, 66, 26, 4, 13, 1, 14, 194, 24, 2, 97, 0, 0, 154, 22, 2, 38, 2, 39, 110, 22, 4, 18, 2, 19, 86, 26, 4, 14, 2, 15, 232, 30, 2, 116, 0, 0, 182, 22, 3, 36, 2, 37, 132, 20, 4, 16, 4, 17, 100, 24, 4, 12, 4, 13, 274, 18, 2, 68, 2, 69, 216, 26, 4, 43, 1, 44, 154, 24, 6, 19, 2, 20, 122, 28, 6, 15, 2, 16, 324, 20, 4, 81, 0, 0, 254, 30, 1, 50, 4, 51, 180, 28, 4, 22, 4, 23, 140, 24, 3, 12, 8, 13, 370, 24, 2, 92, 2, 93, 290, 22, 6, 36, 2, 37, 206, 26, 4, 20, 6, 21, 158, 28, 7, 14, 4, 15, 428, 26, 4, 107, 0, 0, 334, 22, 8, 37, 1, 38, 244, 24, 8, 20, 4, 21, 180, 22, 12, 11, 4, 12, 461, 30, 3, 115, 1, 116, 365, 24, 4, 40, 5, 41, 261, 20, 11, 16, 5, 17, 197, 24, 11, 12, 5, 13, 523, 22, 5, 87, 1, 88, 415, 24, 5, 41, 5, 42, 295, 30, 5, 24, 7, 25, 223, 24, 11, 12, 7, 13, 589, 24, 5, 98, 1, 99, 453, 28, 7, 45, 3, 46, 325, 24, 15, 19, 2, 20, 253, 30, 3, 15, 13, 16, 647, 28, 1, 107, 5, 108, 507, 28, 10, 46, 1, 47, 367, 28, 1, 22, 15, 23, 283, 28, 2, 14, 17, 15, 721, 30, 5, 120, 1, 121, 563, 26, 9, 43, 4, 44, 397, 28, 17, 22, 1, 23, 313, 28, 2, 14, 19, 15, 795, 28, 3, 113, 4, 114, 627, 26, 3, 44, 11, 45, 445, 26, 17, 21, 4, 22, 341, 26, 9, 13, 16, 14, 861, 28, 3, 107, 5, 108, 669, 26, 3, 41, 13, 42, 485, 30, 15, 24, 5, 25, 385, 28, 15, 15, 10, 16, 932, 28, 4, 116, 4, 117, 714, 26, 17, 42, 0, 0, 512, 28, 17, 22, 6, 23, 406, 30, 19, 16, 6, 17, 1006, 28, 2, 111, 7, 112, 782, 28, 17, 46, 0, 0, 568, 30, 7, 24, 16, 25, 442, 24, 34, 13, 0, 0, 1094, 30, 4, 121, 5, 122, 860, 28, 4, 47, 14, 48, 614, 30, 11, 24, 14, 25, 464, 30, 16, 15, 14, 16, 1174, 30, 6, 117, 4, 118, 914, 28, 6, 45, 14, 46, 664, 30, 11, 24, 16, 25, 514, 30, 30, 16, 2, 17, 1276, 26, 8, 106, 4, 107, 1000, 28, 8, 47, 13, 48, 718, 30, 7, 24, 22, 25, 538, 30, 22, 15, 13, 16, 1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46, 4, 47, 754, 28, 28, 22, 6, 23, 596, 30, 33, 16, 4, 17, 1468, 30, 8, 122, 4, 123, 1128, 28, 22, 45, 3, 46, 808, 30, 8, 23, 26, 24, 628, 30, 12, 15, 28, 16, 1531, 30, 3, 117, 10, 118, 1193, 28, 3, 45, 23, 46, 871, 30, 4, 24, 31, 25, 661, 30, 11, 15, 31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28, 21, 45, 7, 46, 911, 30, 1, 23, 37, 24, 701, 30, 19, 15, 26, 16, 1735, 30, 5, 115, 10, 116, 1373, 28, 19, 47, 10, 48, 985, 30, 15, 24, 25, 25, 745, 30, 23, 15, 25, 16, 1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46, 29, 47, 1033, 30, 42, 24, 1, 25, 793, 30, 23, 15, 28, 16, 1955, 30, 17, 115, 0, 0, 1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24, 35, 25, 845, 30, 19, 15, 35, 16, 2071, 30, 17, 115, 1, 116, 1631, 28, 14, 46, 21, 47, 1171, 30, 29, 24, 19, 25, 901, 30, 11, 15, 46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28, 14, 46, 23, 47, 1231, 30, 44, 24, 7, 25, 961, 30, 59, 16, 1, 17, 2306, 30, 12, 121, 7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30, 39, 24, 14, 25, 986, 30, 22, 15, 41, 16, 2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47, 34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30, 2, 15, 64, 16, 2566, 30, 17, 122, 4, 123, 1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24, 10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30, 4, 122, 18, 123, 2102, 28, 13, 46, 32, 47, 1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15, 32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28, 40, 47, 7, 48, 1582, 30, 43, 24, 22, 25, 1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118, 6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30, 34, 24, 34, 25, 1276, 30, 20, 15, 61, 16 };
        private static readonly int[] alignmentPatternBaseValues = { 0, 0, 0, 0, 0, 0, 0, 6, 18, 0, 0, 0, 0, 0, 6, 22, 0, 0, 0, 0, 0, 6, 26, 0, 0, 0, 0, 0, 6, 30, 0, 0, 0, 0, 0, 6, 34, 0, 0, 0, 0, 0, 6, 22, 38, 0, 0, 0, 0, 6, 24, 42, 0, 0, 0, 0, 6, 26, 46, 0, 0, 0, 0, 6, 28, 50, 0, 0, 0, 0, 6, 30, 54, 0, 0, 0, 0, 6, 32, 58, 0, 0, 0, 0, 6, 34, 62, 0, 0, 0, 0, 6, 26, 46, 66, 0, 0, 0, 6, 26, 48, 70, 0, 0, 0, 6, 26, 50, 74, 0, 0, 0, 6, 30, 54, 78, 0, 0, 0, 6, 30, 56, 82, 0, 0, 0, 6, 30, 58, 86, 0, 0, 0, 6, 34, 62, 90, 0, 0, 0, 6, 28, 50, 72, 94, 0, 0, 6, 26, 50, 74, 98, 0, 0, 6, 30, 54, 78, 102, 0, 0, 6, 28, 54, 80, 106, 0, 0, 6, 32, 58, 84, 110, 0, 0, 6, 30, 58, 86, 114, 0, 0, 6, 34, 62, 90, 118, 0, 0, 6, 26, 50, 74, 98, 122, 0, 6, 30, 54, 78, 102, 126, 0, 6, 26, 52, 78, 104, 130, 0, 6, 30, 56, 82, 108, 134, 0, 6, 34, 60, 86, 112, 138, 0, 6, 30, 58, 86, 114, 142, 0, 6, 34, 62, 90, 118, 146, 0, 6, 30, 54, 78, 102, 126, 150, 6, 24, 50, 76, 102, 128, 154, 6, 28, 54, 80, 106, 132, 158, 6, 32, 58, 84, 110, 136, 162, 6, 26, 54, 82, 110, 138, 166, 6, 30, 58, 86, 114, 142, 170 };
        private static readonly int[] remainderBits = { 0, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0 };

        private static readonly Dictionary<int, AlignmentPattern> alignmentPatternTable = CreateAlignmentPatternTable();
        private static readonly List<ECCInfo> capacityECCTable = CreateCapacityECCTable();
        private static readonly List<VersionInfo> capacityTable = CreateCapacityTable();
        private static readonly int[] galoisFieldByExponentAlpha = { 1, 2, 4, 8, 16, 32, 64, 128, 29, 58, 116, 232, 205, 135, 19, 38, 76, 152, 45, 90, 180, 117, 234, 201, 143, 3, 6, 12, 24, 48, 96, 192, 157, 39, 78, 156, 37, 74, 148, 53, 106, 212, 181, 119, 238, 193, 159, 35, 70, 140, 5, 10, 20, 40, 80, 160, 93, 186, 105, 210, 185, 111, 222, 161, 95, 190, 97, 194, 153, 47, 94, 188, 101, 202, 137, 15, 30, 60, 120, 240, 253, 231, 211, 187, 107, 214, 177, 127, 254, 225, 223, 163, 91, 182, 113, 226, 217, 175, 67, 134, 17, 34, 68, 136, 13, 26, 52, 104, 208, 189, 103, 206, 129, 31, 62, 124, 248, 237, 199, 147, 59, 118, 236, 197, 151, 51, 102, 204, 133, 23, 46, 92, 184, 109, 218, 169, 79, 158, 33, 66, 132, 21, 42, 84, 168, 77, 154, 41, 82, 164, 85, 170, 73, 146, 57, 114, 228, 213, 183, 115, 230, 209, 191, 99, 198, 145, 63, 126, 252, 229, 215, 179, 123, 246, 241, 255, 227, 219, 171, 75, 150, 49, 98, 196, 149, 55, 110, 220, 165, 87, 174, 65, 130, 25, 50, 100, 200, 141, 7, 14, 28, 56, 112, 224, 221, 167, 83, 166, 81, 162, 89, 178, 121, 242, 249, 239, 195, 155, 43, 86, 172, 69, 138, 9, 18, 36, 72, 144, 61, 122, 244, 245, 247, 243, 251, 235, 203, 139, 11, 22, 44, 88, 176, 125, 250, 233, 207, 131, 27, 54, 108, 216, 173, 71, 142, 1 };
        private static readonly int[] galoisFieldByIntegerValue = { 0, 0, 1, 25, 2, 50, 26, 198, 3, 223, 51, 238, 27, 104, 199, 75, 4, 100, 224, 14, 52, 141, 239, 129, 28, 193, 105, 248, 200, 8, 76, 113, 5, 138, 101, 47, 225, 36, 15, 33, 53, 147, 142, 218, 240, 18, 130, 69, 29, 181, 194, 125, 106, 39, 249, 185, 201, 154, 9, 120, 77, 228, 114, 166, 6, 191, 139, 98, 102, 221, 48, 253, 226, 152, 37, 179, 16, 145, 34, 136, 54, 208, 148, 206, 143, 150, 219, 189, 241, 210, 19, 92, 131, 56, 70, 64, 30, 66, 182, 163, 195, 72, 126, 110, 107, 58, 40, 84, 250, 133, 186, 61, 202, 94, 155, 159, 10, 21, 121, 43, 78, 212, 229, 172, 115, 243, 167, 87, 7, 112, 192, 247, 140, 128, 99, 13, 103, 74, 222, 237, 49, 197, 254, 24, 227, 165, 153, 119, 38, 184, 180, 124, 17, 68, 146, 217, 35, 32, 137, 46, 55, 63, 209, 91, 149, 188, 207, 205, 144, 135, 151, 178, 220, 252, 190, 97, 242, 86, 211, 171, 20, 42, 93, 158, 132, 60, 57, 83, 71, 109, 65, 162, 31, 45, 67, 216, 183, 123, 164, 118, 196, 23, 73, 236, 127, 12, 111, 246, 108, 161, 59, 82, 41, 157, 85, 170, 251, 96, 134, 177, 187, 204, 62, 90, 203, 89, 95, 176, 156, 169, 160, 81, 11, 245, 22, 235, 122, 117, 44, 215, 79, 174, 213, 233, 230, 231, 173, 232, 116, 214, 244, 234, 168, 80, 88, 175 };
        private static readonly Dictionary<char, int> alphanumEncDict = CreateAlphanumEncDict();

        /// <summary>
        /// Initializes the QR code generator
        /// </summary>
        public QRCodeGenerator()
        {
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
            int minVersion = GetVersion(dataInputLength + (eciMode != EciMode.Default ? 2 : 0), encoding, eccLevel);
            if (version == -1)
            {
                version = minVersion;
            }
            else
            {
                //Version was passed as fixed version via parameter. Thus let's check if chosen version is valid.
                if (minVersion > version)
                {
                    var maxSizeByte = capacityTable[version - 1].Details.First(x => x.ErrorCorrectionLevel == eccLevel).CapacityDict[encoding];
                    throw new QRCoder.Exceptions.DataTooLongException(eccLevel.ToString(), encoding.ToString(), version, maxSizeByte);
                }
            }

            var modeIndicatorLength = eciMode != EciMode.Default ? 16 : 4;
            var countIndicatorLength = GetCountIndicatorLength(version, encoding);
            var completeBitArrayLength = modeIndicatorLength + countIndicatorLength + codedText.Length;

            var completeBitArray = new BitArray(completeBitArrayLength);

            // write mode indicator
            var completeBitArrayIndex = 0;
            if (eciMode != EciMode.Default)
            {
                completeBitArrayIndex = DecToBin((int)EncodingMode.ECI, 4, completeBitArray, completeBitArrayIndex);
                completeBitArrayIndex = DecToBin((int)eciMode, 8, completeBitArray, completeBitArrayIndex);
            }
            completeBitArrayIndex = DecToBin((int)encoding, 4, completeBitArray, completeBitArrayIndex);
            // write count indicator
            completeBitArrayIndex = DecToBin(dataInputLength, countIndicatorLength, completeBitArray, completeBitArrayIndex);
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
            // Convert byte array to bit array, with prefix padding for mode indicator and count indicator
            var bitArray = ToBitArray(binaryData, prefixZeros: 4 + countIndicatorLen);
            // Add mode indicator and count indicator
            var index = DecToBin((int)EncodingMode.Byte, 4, bitArray, 0);
            DecToBin(binaryData.Length, countIndicatorLen, bitArray, index);

            return GenerateQrCode(bitArray, eccLevel, version);
        }

        private static readonly BitArray _repeatingPattern = new BitArray(
            new[] { true, true, true, false, true, true, false, false, false, false, false, true, false, false, false, true });

        /// <summary>
        /// Generates a QR code data structure using the provided BitArray, error correction level, and version.
        /// The BitArray provided is assumed to already include the count, encoding mode, and/or ECI mode information.
        /// </summary>
        /// <param name="bitArray">The BitArray containing the binary-encoded data to be included in the QR code. It should already contain the count, encoding mode, and/or ECI mode information.</param>
        /// <param name="eccLevel">The desired error correction level for the QR code. This impacts how much data can be recovered if damaged.</param>
        /// <param name="version">The version of the QR code, determining the size and complexity of the QR code data matrix.</param>
        /// <returns>A QRCodeData structure containing the full QR code matrix, which can be used for rendering or analysis.</returns>
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
                if ((uint)index % 8 != 0)
                    index += 8 - (int)((uint)index % 8);
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
            AddCodeWordBlocks(1, eccInfo.BlocksInGroup1, eccInfo.CodewordsInGroup1, 0, bitArray.Length);
            int offset = eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8;
            AddCodeWordBlocks(2, eccInfo.BlocksInGroup2, eccInfo.CodewordsInGroup2, offset, bitArray.Length - offset);


            //Calculate interleaved code word lengths
            int interleavedLength = 0;
            for (var i = 0; i < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.CodeWordsLength / 8 > i)
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
                {
                    if (codeBlock.CodeWordsLength / 8 > i)
                        pos = bitArray.CopyTo(interleavedData, i * 8 + codeBlock.CodeWordsOffset, pos, 8);
                }
            }
            for (var i = 0; i < eccInfo.ECCPerBlock; i++)
            {
                foreach (var codeBlock in codeWordWithECC)
                    if (codeBlock.ECCWords.Length > i)
                        pos = DecToBin(codeBlock.ECCWords[i], 8, interleavedData, pos);
            }

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

            void AddCodeWordBlocks(int blockNum, int blocksInGroup, int codewordsInGroup, int offset2, int count)
            {
                var groupLength = codewordsInGroup * 8;
                for (var i = 0; i < blocksInGroup; i++)
                {
                    var eccWordList = CalculateECCWords(bitArray, offset2, groupLength, eccInfo);
                    codeWordWithECC.Add(new CodewordBlock(offset2, groupLength, eccWordList));
                    offset2 += groupLength;
                }
            }
        }

        private static readonly BitArray _getFormatGenerator = new BitArray(new bool[] { true, false, true, false, false, true, true, false, true, true, true });
        private static readonly BitArray _getFormatMask = new BitArray(new bool[] { true, false, true, false, true, false, false, false, false, false, true, false, false, true, false });
        /// <summary>
        /// Generates a BitArray containing the format string for a QR code based on the error correction level and mask pattern version.
        /// The format string includes the error correction level, mask pattern version, and error correction coding.
        /// </summary>
        /// <param name="level">The error correction level to be encoded in the format string.</param>
        /// <param name="maskVersion">The mask pattern version to be encoded in the format string.</param>
        /// <returns>A BitArray containing the 15-bit format string used in QR code generation.</returns>
        private static BitArray GetFormatString(ECCLevel level, int maskVersion)
        {
            var fStrEcc = new BitArray(15); // Total length including space for mask version and padding
            WriteEccLevelAndVersion();

            // Apply the format generator polynomial to add error correction to the format string.
            int index = 0;
            int count = 15;
            TrimLeadingZeros(fStrEcc, ref index, ref count);
            while (count > 10)
            {
                for (var i = 0; i < _getFormatGenerator.Length; i++)
                    fStrEcc[index + i] ^= _getFormatGenerator[i];
                TrimLeadingZeros(fStrEcc, ref index, ref count);
            }

            // Align bits with the start of the array.
            ShiftTowardsBit0(fStrEcc, index);

            // Prefix the error correction bits with the ECC level and version number.
            fStrEcc.Length = 10 + 5;
            ShiftAwayFromBit0(fStrEcc, (10 - count) + 5);
            WriteEccLevelAndVersion();

            // XOR the format string with a predefined mask to add robustness against errors.
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
                
                // Insert the 3-bit mask version directly after the error correction level bits.
                DecToBin(maskVersion, 3, fStrEcc, 2);
            }
        }

#if !NETFRAMEWORK || NET45_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void TrimLeadingZeros(BitArray fStrEcc, ref int index, ref int count)
        {
            while (!fStrEcc[index])
            {
                index++;
                count--;
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

        /// <summary>
        /// Encodes the version information of a QR code into a BitArray using error correction coding similar to format information encoding.
        /// This method is used for QR codes version 7 and above.
        /// </summary>
        /// <param name="version">The version number of the QR code (7-40).</param>
        /// <returns>A BitArray containing the encoded version information, which includes error correction bits.</returns>
        private static BitArray GetVersionString(int version)
        {
            var vStr = new BitArray(18);
            DecToBin(version, 6, vStr, 0); // Convert the version number to a 6-bit binary representation.

            var count = vStr.Length;
            var index = 0;
            TrimLeadingZeros(vStr, ref index, ref count); // Trim leading zeros to normalize the version bit sequence.

            // Perform error correction encoding using a polynomial generator (specified by _getVersionGenerator).
            while (count > 12) // The target length of the version information error correction information is 12 bits.
            {
                for (var i = 0; i < _getVersionGenerator.Length; i++)
                    vStr[index + i] ^= _getVersionGenerator[i]; // XOR the current bits with the generator sequence.

                TrimLeadingZeros(vStr, ref index, ref count); // Trim leading zeros after each XOR operation to maintain the proper sequence.
            }

            ShiftTowardsBit0(vStr, index); // Align the bit array so the data starts at index 0.

            // Prefix the error correction encoding with 6 bits containing the version number
            vStr.Length = 12 + 6;
            ShiftAwayFromBit0(vStr, (12 - count) + 6);
            DecToBin(version, 6, vStr, 0);

            return vStr;
        }

        /// <summary>
        /// Calculates the Error Correction Codewords (ECC) for a segment of data using the provided ECC information.
        /// This method applies polynomial division, using the message polynomial and a generator polynomial,
        /// to compute the remainder which forms the ECC codewords.
        /// </summary>
        private static byte[] CalculateECCWords(BitArray bitArray, int offset, int count, ECCInfo eccInfo)
        {
            var eccWords = eccInfo.ECCPerBlock;
            // Calculate the message polynomial from the bit array data.
            var messagePolynom = CalculateMessagePolynom(bitArray, offset, count);
            // Generate the generator polynomial using the number of ECC words.
            var generatorPolynom = CalculateGeneratorPolynom(eccWords);

            // Adjust the exponents in the message polynomial to account for ECC length.
            for (var i = 0; i < messagePolynom.PolyItems.Count; i++)
                messagePolynom.PolyItems[i] = new PolynomItem(messagePolynom.PolyItems[i].Coefficient,
                    messagePolynom.PolyItems[i].Exponent + eccWords);

            // Adjust the generator polynomial exponents based on the message polynomial.
            for (var i = 0; i < generatorPolynom.PolyItems.Count; i++)
                generatorPolynom.PolyItems[i] = new PolynomItem(generatorPolynom.PolyItems[i].Coefficient,
                    generatorPolynom.PolyItems[i].Exponent + (messagePolynom.PolyItems.Count - 1));

            // Divide the message polynomial by the generator polynomial to find the remainder.
            var leadTermSource = messagePolynom;
            for (var i = 0; (leadTermSource.PolyItems.Count > 0 && leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent > 0); i++)
            {
                if (leadTermSource.PolyItems[0].Coefficient == 0)  // Simplify the polynomial if the leading coefficient is zero.
                {
                    leadTermSource.PolyItems.RemoveAt(0);
                    leadTermSource.PolyItems.Add(new PolynomItem(0, leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent - 1));
                }
                else  // Otherwise, perform polynomial reduction using XOR and multiplication with the generator polynomial.
                {
                    var resPoly = MultiplyGeneratorPolynomByLeadterm(generatorPolynom, ConvertToAlphaNotation(leadTermSource).PolyItems[0], i);
                    ConvertToDecNotationInPlace(resPoly);
                    resPoly = XORPolynoms(leadTermSource, resPoly);
                    leadTermSource = resPoly;
                }
            }

            // Convert the resulting polynomial into a byte array representing the ECC codewords.
            var ret = new byte[leadTermSource.PolyItems.Count];
            for (var i = 0; i < leadTermSource.PolyItems.Count; i++)
                ret[i] = (byte)leadTermSource.PolyItems[i].Coefficient;
            return ret;
        }

        /// <summary>
        /// Converts the coefficients of a polynomial from integer values to their corresponding alpha exponent notation
        /// based on a Galois field mapping. This is typically used in error correction calculations where
        /// operations are performed on exponents rather than coefficients directly.
        /// </summary>
        private static Polynom ConvertToAlphaNotation(Polynom poly)
        {
            var newPoly = new Polynom(poly.PolyItems.Count);

            for (var i = 0; i < poly.PolyItems.Count; i++)
            {
                // Convert each coefficient to its corresponding alpha exponent unless it's zero.
                // Coefficients that are zero remain zero because log(0) is undefined.
                newPoly.PolyItems.Add(
                    new PolynomItem(
                        (poly.PolyItems[i].Coefficient != 0
                            ? GetAlphaExpFromIntVal(poly.PolyItems[i].Coefficient)
                            : 0),
                        poly.PolyItems[i].Exponent)); // The exponent remains unchanged.
            }

            return newPoly;
        }

        /// <summary>
        /// Converts all polynomial item coefficients from their alpha exponent notation to decimal representation in place.
        /// This conversion facilitates operations that require polynomial coefficients in their integer forms.
        /// </summary>
        private static void ConvertToDecNotationInPlace(Polynom poly)
        {
            for (var i = 0; i < poly.PolyItems.Count; i++)
            {
                // Convert the alpha exponent of the coefficient to its decimal value and create a new polynomial item with the updated coefficient.
                poly.PolyItems[i] = new PolynomItem(GetIntValFromAlphaExp(poly.PolyItems[i].Coefficient), poly.PolyItems[i].Exponent);
            }
        }

        /// <summary>
        /// Determines the minimum QR code version required to encode a given amount of data with a specific encoding mode and error correction level.
        /// If no suitable version is found, it throws an exception indicating that the data length exceeds the maximum capacity for the given settings.
        /// </summary>
        /// <param name="length">The length of the data to be encoded.</param>
        /// <param name="encMode">The encoding mode (e.g., Numeric, Alphanumeric, Byte).</param>
        /// <param name="eccLevel">The error correction level (e.g., Low, Medium, Quartile, High).</param>
        /// <returns>The minimum version of the QR code that can accommodate the given data and settings.</returns>
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

        /// <summary>
        /// Determines the most efficient encoding mode for the given plain text based on its character content
        /// and a flag indicating whether to force UTF-8 encoding.
        /// </summary>
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

        /// <summary>
        /// Checks if a character falls within a specified range.
        /// </summary>
        private static bool IsInRange(char c, char min, char max)
        {
            return (uint)(c - min) <= (uint)(max - min);
        }

        /// <summary>
        /// Calculates the message polynomial from a bit array which represents the encoded data.
        /// </summary>
        /// <returns>A polynomial representation of the message.</returns>
        private static Polynom CalculateMessagePolynom(BitArray bitArray, int offset, int count)
        {
            var messagePol = new Polynom(count /= 8);
            for (var i = count - 1; i >= 0; i--)
            {
                messagePol.PolyItems.Add(new PolynomItem(BinToDec(bitArray, offset, 8), i));
                offset += 8;
            }
            return messagePol;
        }

        /// <summary>
        /// Calculates the generator polynomial used for creating error correction codewords.
        /// </summary>
        /// <param name="numEccWords">The number of error correction codewords to generate.</param>
        /// <returns>A polynomial that can be used to generate ECC codewords.</returns>
        private static Polynom CalculateGeneratorPolynom(int numEccWords)
        {
            var generatorPolynom = new Polynom(2); // Start with the simplest form of the polynomial
            generatorPolynom.PolyItems.Add(new PolynomItem(0, 1));
            generatorPolynom.PolyItems.Add(new PolynomItem(0, 0));

            var multiplierPolynom = new Polynom(numEccWords * 2); // Used for polynomial multiplication
            for (var i = 1; i <= numEccWords - 1; i++)
            {
                // Clear and set up the multiplier polynomial for the current multiplication
                multiplierPolynom.PolyItems.Clear();
                multiplierPolynom.PolyItems.Add(new PolynomItem(0, 1));
                multiplierPolynom.PolyItems.Add(new PolynomItem(i, 0));

                // Multiply the generator polynomial by the current multiplier polynomial
                generatorPolynom = MultiplyAlphaPolynoms(generatorPolynom, multiplierPolynom);
            }

            return generatorPolynom; // Return the completed generator polynomial
        }

        /// <summary>
        /// Converts a segment of a BitArray into its decimal (integer) equivalent.
        /// </summary>
        /// <returns>The integer value that represents the specified binary data.</returns>
        private static int BinToDec(BitArray bitArray, int offset, int count)
        {
            var ret = 0;
            for (int i = 0; i < count; i++)
            {
                ret ^= bitArray[offset + i] ? 1 << (count - i - 1) : 0;
            }
            return ret;
        }

        /// <summary>
        /// Converts a decimal number to binary and stores the result in a BitArray starting from a specific index.
        /// </summary>
        /// <param name="decNum">The decimal number to convert to binary.</param>
        /// <param name="bits">The number of bits to use for the binary representation (ensuring fixed-width like 8, 16, 32 bits).</param>
        /// <param name="bitList">The BitArray where the binary bits will be stored.</param>
        /// <param name="index">The starting index in the BitArray where the bits will be stored.</param>
        /// <returns>The next index in the BitArray after the last bit placed.</returns>
        private static int DecToBin(int decNum, int bits, BitArray bitList, int index)
        {
            // Convert decNum to binary using a bitwise operation
            for (int i = bits - 1; i >= 0; i--)
            {
                // Check each bit from most significant to least significant
                bool bit = (decNum & (1 << i)) != 0;
                bitList[index++] = bit;
            }
            return index;
        }

        /// <summary>
        /// Determines the number of bits used to indicate the count of characters in a segment, depending on the QR code version and the encoding mode.
        /// </summary>
        /// <param name="version">The version of the QR code, which influences the number of bits due to increasing data capacity.</param>
        /// <param name="encMode">The encoding mode (e.g., Numeric, Alphanumeric, Byte) used for the data segment.</param>
        /// <returns>The number of bits needed to represent the character count in the specified encoding mode and version.</returns>
        private static int GetCountIndicatorLength(int version, EncodingMode encMode)
        {
            // Different versions and encoding modes require different lengths of bits to represent the character count efficiently
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

        /// <summary>
        /// Calculates the data length based on the encoding mode, text content, and whether UTF-8 is forced.
        /// </summary>
        /// <param name="encoding">The encoding mode used for the QR code data.</param>
        /// <param name="plainText">The plain text input to be encoded.</param>
        /// <param name="codedText">A BitArray representing the binary data of the encoded text.</param>
        /// <param name="forceUtf8">Flag to determine if UTF-8 encoding should be enforced.</param>
        /// <returns>The length of data in units appropriate to the encoding (bytes or characters).</returns>
        private static int GetDataLength(EncodingMode encoding, string plainText, BitArray codedText, bool forceUtf8)
        {
            // If UTF-8 is forced or the text is detected as UTF-8, return the number of bytes, otherwise return the character count.
            return forceUtf8 || IsUtf8() ? (int)((uint)codedText.Length / 8) : plainText.Length;

            bool IsUtf8()
            {
                return (encoding == EncodingMode.Byte && (forceUtf8 || !IsValidISO(plainText)));
            }
        }

        /// <summary>
        /// Checks if the given string can be accurately represented and retrieved in ISO-8859-1 encoding.
        /// </summary>
        private static bool IsValidISO(string input)
        {
            var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
            var result = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
            return String.Equals(input, result);
        }

        /// <summary>
        /// Converts plain text to a binary format suitable for QR code generation, based on the specified encoding mode.
        /// </summary>
        /// <param name="plainText">The text to be encoded.</param>
        /// <param name="encMode">The encoding mode.</param>
        /// <param name="eciMode">The ECI mode specifying the character encoding to use.</param>
        /// <param name="utf8BOM">Flag indicating whether to prepend a UTF-8 Byte Order Mark.</param>
        /// <param name="forceUtf8">Flag indicating whether UTF-8 encoding is forced.</param>
        /// <returns>A BitArray containing the binary representation of the encoded data.</returns>
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

        /// <summary>
        /// Converts numeric plain text into a binary format specifically optimized for QR codes.
        /// Numeric compression groups up to 3 digits into 10 bits, less for remaining digits if they do not complete a group of three.
        /// </summary>
        /// <param name="plainText">The numeric text to be encoded, which should only contain digit characters.</param>
        /// <returns>A BitArray representing the binary data of the encoded numeric text.</returns>
        private static BitArray PlainTextToBinaryNumeric(string plainText)
        {
            // Calculate the length of the BitArray needed to encode the text.
            // Groups of three digits are encoded in 10 bits, remaining groups of two or one digits take 7 or 4 bits respectively.
            var bitArray = new BitArray(plainText.Length / 3 * 10 + (plainText.Length % 3 == 1 ? 4 : plainText.Length % 3 == 2 ? 7 : 0));
            var index = 0;

            // Process each group of three digits.
            for (int i = 0; i < plainText.Length - 2; i += 3)
            {
                // Parse the next three characters as a decimal integer.
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(i, 3), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                // Convert the decimal to binary and store it in the BitArray.
                index = DecToBin(dec, 10, bitArray, index);
            }

            // Handle any remaining digits if the total number is not a multiple of three.
            if (plainText.Length % 3 == 2)  // Two remaining digits are encoded in 7 bits.
            {
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 2), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                index = DecToBin(dec, 7, bitArray, index);
            }
            else if (plainText.Length % 3 == 1)  // One remaining digit is encoded in 4 bits.
            {
#if NET5_0_OR_GREATER
                var dec = int.Parse(plainText.AsSpan(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#else
                var dec = int.Parse(plainText.Substring(plainText.Length / 3 * 3, 1), NumberStyles.None, CultureInfo.InvariantCulture);
#endif
                index = DecToBin(dec, 4, bitArray, index);
            }

            return bitArray;
        }

        /// <summary>
        /// Converts alphanumeric plain text into a binary format optimized for QR codes.
        /// Alphanumeric encoding packs characters into 11-bit groups for each pair of characters,
        /// and 6 bits for a single remaining character if the total count is odd.
        /// </summary>
        /// <param name="plainText">The alphanumeric text to be encoded, which should only contain characters valid in QR alphanumeric mode.</param>
        /// <returns>A BitArray representing the binary data of the encoded alphanumeric text.</returns>
        private static BitArray PlainTextToBinaryAlphanumeric(string plainText)
        {
            // Calculate the length of the BitArray needed based on the number of character pairs.
            var codeText = new BitArray((plainText.Length / 2) * 11 + (plainText.Length & 1) * 6);
            var codeIndex = 0;
            var index = 0;
            var count = plainText.Length;

            // Process each pair of characters.
            while (count >= 2)
            {
                // Convert each pair of characters to a number by looking them up in the alphanumeric dictionary and calculating.
                var dec = alphanumEncDict[plainText[index++]] * 45 + alphanumEncDict[plainText[index++]];
                // Convert the number to binary and store it in the BitArray.
                codeIndex = DecToBin(dec, 11, codeText, codeIndex);
                count -= 2;
            }

            // Handle the last character if the length is odd.
            if (count > 0)
            {
                DecToBin(alphanumEncDict[plainText[index]], 6, codeText, codeIndex);
            }

            return codeText;
        }

        /// <summary>
        /// Returns a string that contains the original string, with characters that cannot be encoded by a
        /// specified encoding (default of ISO-8859-2) with a replacement character.
        /// </summary>
        private static string ConvertToIso8859(string value, string Iso = "ISO-8859-2")
        {
            Encoding iso = Encoding.GetEncoding(Iso);
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(value);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            return iso.GetString(isoBytes);
        }

        /// <summary>
        /// Converts plain text into a binary format using byte mode encoding, which supports various character encodings through ECI (Extended Channel Interpretations).
        /// </summary>
        /// <param name="plainText">The text to be encoded.</param>
        /// <param name="eciMode">The ECI mode that specifies the character encoding to use.</param>
        /// <param name="utf8BOM">Specifies whether to include a Byte Order Mark (BOM) for UTF-8 encoding.</param>
        /// <param name="forceUtf8">Forces UTF-8 encoding regardless of the text content's compatibility with ISO-8859-1.</param>
        /// <returns>A BitArray representing the binary data of the encoded text.</returns>
        /// <remarks>
        /// The returned text is always encoded as ISO-8859-1 unless either the text contains a non-ISO-8859-1 character or
        /// UTF-8 encoding is forced. This does not meet the QR Code standard, which requires the use of ECI to specify the encoding
        /// when not ISO-8859-1.
        /// </remarks>
        private static BitArray PlainTextToBinaryByte(string plainText, EciMode eciMode, bool utf8BOM, bool forceUtf8)
        {
            byte[] codeBytes;

            // Check if the text is valid ISO-8859-1 and UTF-8 is not forced, then encode using ISO-8859-1.
            if (IsValidISO(plainText) && !forceUtf8)
                codeBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText);
            else
            {
                // Determine the encoding based on the specified ECI mode.
                switch (eciMode)
                {
                    case EciMode.Iso8859_1:
                        // Convert text to ISO-8859-1 and encode.
                        codeBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(ConvertToIso8859(plainText, "ISO-8859-1"));
                        break;
                    case EciMode.Iso8859_2:
                        // Convert text to ISO-8859-2 and encode.
                        codeBytes = Encoding.GetEncoding("ISO-8859-2").GetBytes(ConvertToIso8859(plainText, "ISO-8859-2"));
                        break;
                    case EciMode.Default:
                    case EciMode.Utf8:
                    default:
                        // Handle UTF-8 encoding, optionally adding a BOM if specified.
                        codeBytes = utf8BOM ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(plainText)).ToArray() : Encoding.UTF8.GetBytes(plainText);
                        break;
                }
            }

            // Convert the array of bytes into a BitArray.
            return ToBitArray(codeBytes);
        }

        /// <summary>
        /// Converts an array of bytes into a BitArray, considering the proper bit order within each byte.
        /// Unlike the constructor of BitArray, this function preserves the MSB-to-LSB order within each byte.
        /// </summary>
        /// <param name="byteArray">The byte array to convert into a BitArray.</param>
        /// <param name="prefixZeros">The number of leading zeros to prepend to the resulting BitArray.</param>
        /// <returns>A BitArray representing the bits of the input byteArray, with optional leading zeros.</returns>
        private static BitArray ToBitArray(byte[] byteArray, int prefixZeros = 0)
        {
            // Calculate the total number of bits in the resulting BitArray including the prefix zeros.
            var bitArray = new BitArray((int)((uint)byteArray.Length * 8) + prefixZeros);
            for (var i = 0; i < byteArray.Length; i++)
            {
                var byteVal = byteArray[i];
                for (var j = 0; j < 8; j++)
                {
                    // Set each bit in the BitArray based on the corresponding bit in the byte array.
                    // It shifts bits within the byte to align with the MSB-to-LSB order.
                    bitArray[(int)((uint)i * 8) + j + prefixZeros] = (byteVal & (1 << (7 - j))) != 0;
                }
            }
            return bitArray;
        }

        /// <summary>
        /// Performs a bitwise XOR operation between two polynomials, commonly used in QR code error correction coding.
        /// </summary>
        /// <returns>The resultant polynomial after performing the XOR operation.</returns>
        private static Polynom XORPolynoms(Polynom messagePolynom, Polynom resPolynom)
        {
            // Determine the larger of the two polynomials to guide the XOR operation.
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

            // XOR the coefficients of the two polynomials.
            for (var i = 0; i < longPoly.PolyItems.Count; i++)
            {
                var polItemRes = new PolynomItem(
                    longPoly.PolyItems[i].Coefficient ^
                    (shortPoly.PolyItems.Count > i ? shortPoly.PolyItems[i].Coefficient : 0),
                    messagePolynom.PolyItems[0].Exponent - i
                );
                resultPolynom.PolyItems.Add(polItemRes);
            }
            resultPolynom.PolyItems.RemoveAt(0);
            return resultPolynom;
        }

        /// <summary>
        /// Multiplies a generator polynomial by a leading term polynomial, reducing the result by a specified lower exponent,
        /// used in constructing QR code error correction codewords.
        /// </summary>
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

        /// <summary>
        /// Multiplies two polynomials, treating coefficients as exponents of a primitive element (alpha), which is common in error correction algorithms such as Reed-Solomon.
        /// </summary>
        /// <param name="polynomBase">The first polynomial to multiply.</param>
        /// <param name="polynomMultiplier">The second polynomial to multiply.</param>
        /// <returns>A new polynomial which is the result of the multiplication of the two input polynomials.</returns>
        private static Polynom MultiplyAlphaPolynoms(Polynom polynomBase, Polynom polynomMultiplier)
        {
            // Initialize a new polynomial with a size based on the product of the sizes of the two input polynomials.
            var resultPolynom = new Polynom(polynomMultiplier.PolyItems.Count * polynomBase.PolyItems.Count);

            // Multiply each term of the first polynomial by each term of the second polynomial.
            foreach (var polItemBase in polynomMultiplier.PolyItems)
            {
                foreach (var polItemMulti in polynomBase.PolyItems)
                {
                    // Create a new polynomial term with the coefficients added (as exponents) and exponents summed.
                    var polItemRes = new PolynomItem
                    (
                        ShrinkAlphaExp(polItemBase.Coefficient + polItemMulti.Coefficient),
                        (polItemBase.Exponent + polItemMulti.Exponent)
                    );
                    resultPolynom.PolyItems.Add(polItemRes);
                }
            }

            // Identify and merge terms with the same exponent.
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

                // Fix the polynomial terms by recalculating the coefficients based on XORed results.
                var polynomFixed = new PolynomItem(GetAlphaExpFromIntVal(coefficient), exponent);
                gluedPolynoms[gluedPolynomsIndex++] = polynomFixed;
            }

            // Remove duplicated exponents and add the corrected ones back.
            for (int i = resultPolynom.PolyItems.Count - 1; i >= 0; i--)
                if (toGlue.Contains(resultPolynom.PolyItems[i].Exponent))
                    resultPolynom.PolyItems.RemoveAt(i);
            foreach (var polynom in gluedPolynoms)
                resultPolynom.PolyItems.Add(polynom);

            // Sort the polynomial terms by exponent in descending order.
            resultPolynom.PolyItems.Sort((x, y) => -x.Exponent.CompareTo(y.Exponent));
            return resultPolynom;

            // Auxiliary function to identify exponents that appear more than once in the polynomial.
            int[] GetNotUniqueExponents(List<PolynomItem> list)
            {
                var dic = new Dictionary<int, bool>(list.Count);
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

                // Collect all exponents that appeared more than once.
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

        /// <summary>
        /// Retrieves the integer value from the Galois field that corresponds to a given exponent.
        /// This is used in Reed-Solomon and other error correction calculations involving Galois fields.
        /// </summary>
        private static int GetIntValFromAlphaExp(int exp)
        {
            return galoisFieldByExponentAlpha[exp];
        }

        /// <summary>
        /// Retrieves the exponent from the Galois field that corresponds to a given integer value.
        /// Throws an exception if the integer value is zero, as zero does not have a logarithmic representation in the field.
        /// </summary>
        private static int GetAlphaExpFromIntVal(int intVal)
        {
            if (intVal == 0)
                ThrowIntValOutOfRangeException(); // Zero is not valid as it does not have an exponent representation.
            return galoisFieldByIntegerValue[intVal];

            void ThrowIntValOutOfRangeException() => throw new ArgumentOutOfRangeException(nameof(intVal), "The provided integer value is out of range, as zero is not representable.");
        }

        /// <summary>
        /// Normalizes a Galois field exponent to ensure it remains within the bounds of the field's size.
        /// This is particularly necessary when performing multiplications in the field which can result in exponents exceeding the field's maximum.
        /// </summary>
        private static int ShrinkAlphaExp(int alphaExp)
        {
            return (int)((alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
        }

        /// <summary>
        /// Creates a dictionary mapping alphanumeric characters to their respective positions used in QR code encoding.
        /// This includes digits 0-9, uppercase letters A-Z, and some special characters.
        /// </summary>
        /// <returns>A dictionary mapping each supported alphanumeric character to its corresponding value.</returns>
        private static Dictionary<char, int> CreateAlphanumEncDict()
        {
            var localAlphanumEncDict = new Dictionary<char, int>(45);
            for (int i = 0; i < 10; i++)
                localAlphanumEncDict.Add($"{i}"[0], i);
            // Add uppercase alphabetic characters.
            for (char c = 'A'; c <= 'Z'; c++)
                localAlphanumEncDict.Add(c, localAlphanumEncDict.Count);
            // Add special characters from a predefined table.
            for (int i = 0; i < alphanumEncTable.Length; i++)
                localAlphanumEncDict.Add(alphanumEncTable[i], localAlphanumEncDict.Count);
            return localAlphanumEncDict;
        }

        /// <summary>
        /// Creates a lookup table mapping QR code versions to their corresponding alignment patterns.
        /// Alignment patterns are used in QR codes to help scanners accurately read the code at high speeds and when partially obscured.
        /// This table provides the necessary patterns based on the QR code version which dictates the size and complexity of the QR code.
        /// </summary>
        /// <returns>A dictionary where keys are QR code version numbers and values are AlignmentPattern structures detailing the positions of alignment patterns for each version.</returns>
        private static Dictionary<int, AlignmentPattern> CreateAlignmentPatternTable()
        {
            var localAlignmentPatternTable = new Dictionary<int, AlignmentPattern>(40);

            for (var i = 0; i < (7 * 40); i = i + 7)
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
                }
                );
            }
            return localAlignmentPatternTable;
        }

        /// <summary>
        /// Generates a table containing the error correction capacities and data codeword information for different QR code versions and error correction levels.
        /// This table is essential for determining how much data can be encoded in a QR code of a specific version and ECC level,
        /// as well as how robust the QR code will be against distortions or obstructions.
        /// </summary>
        /// <returns>A list of ECCInfo structures, each representing the ECC data and capacities for different combinations of QR code versions and ECC levels.</returns>
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

        /// <summary>
        /// Generates a list containing detailed capacity information for various versions of QR codes.
        /// This table includes capacities for different encoding modes (numeric, alphanumeric, byte, etc.) under each error correction level.
        /// The capacity table is crucial for QR code generation, as it determines how much data each QR code version can store depending on the encoding mode and error correction level used.
        /// </summary>
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

        public void Dispose()
        {
            // left for back-compat
        }
    }
}
