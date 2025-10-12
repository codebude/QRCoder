namespace QRCoder;

public partial class QRCodeGenerator
{
    /// <summary>
    /// Provides QR code capacity and error correction data for each version and encoding mode.
    /// Used to determine how much data can be stored in a QR code and which version is required.
    /// </summary>
    private static class CapacityTables
    {
        private static readonly int[] _capacityBaseValues = { 41, 25, 17, 10, 34, 20, 14, 8, 27, 16, 11, 7, 17, 10, 7, 4, 77, 47, 32, 20, 63, 38, 26, 16, 48, 29, 20, 12, 34, 20, 14, 8, 127, 77, 53, 32, 101, 61, 42, 26, 77, 47, 32, 20, 58, 35, 24, 15, 187, 114, 78, 48, 149, 90, 62, 38, 111, 67, 46, 28, 82, 50, 34, 21, 255, 154, 106, 65, 202, 122, 84, 52, 144, 87, 60, 37, 106, 64, 44, 27, 322, 195, 134, 82, 255, 154, 106, 65, 178, 108, 74, 45, 139, 84, 58, 36, 370, 224, 154, 95, 293, 178, 122, 75, 207, 125, 86, 53, 154, 93, 64, 39, 461, 279, 192, 118, 365, 221, 152, 93, 259, 157, 108, 66, 202, 122, 84, 52, 552, 335, 230, 141, 432, 262, 180, 111, 312, 189, 130, 80, 235, 143, 98, 60, 652, 395, 271, 167, 513, 311, 213, 131, 364, 221, 151, 93, 288, 174, 119, 74, 772, 468, 321, 198, 604, 366, 251, 155, 427, 259, 177, 109, 331, 200, 137, 85, 883, 535, 367, 226, 691, 419, 287, 177, 489, 296, 203, 125, 374, 227, 155, 96, 1022, 619, 425, 262, 796, 483, 331, 204, 580, 352, 241, 149, 427, 259, 177, 109, 1101, 667, 458, 282, 871, 528, 362, 223, 621, 376, 258, 159, 468, 283, 194, 120, 1250, 758, 520, 320, 991, 600, 412, 254, 703, 426, 292, 180, 530, 321, 220, 136, 1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470, 322, 198, 602, 365, 250, 154, 1548, 938, 644, 397, 1212, 734, 504, 310, 876, 531, 364, 224, 674, 408, 280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345, 948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153, 792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272, 813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970, 666, 410, 1159, 702, 482, 297, 919, 557, 382, 235, 2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742, 509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618, 1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640, 439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528, 1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704, 1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407, 1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451, 997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330, 3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094, 751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902, 2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910, 625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732, 2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369, 1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559, 1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994, 1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457, 4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499, 1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201, 3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226, 842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002, 2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183, 2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756, 2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632, 1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605, 5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966, 1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577, 4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591, 1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292, 3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087, 2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972, 2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391, 2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784 };
        private static readonly int[] _capacityECCBaseValues = { 19, 7, 1, 19, 0, 0, 16, 10, 1, 16, 0, 0, 13, 13, 1, 13, 0, 0, 9, 17, 1, 9, 0, 0, 34, 10, 1, 34, 0, 0, 28, 16, 1, 28, 0, 0, 22, 22, 1, 22, 0, 0, 16, 28, 1, 16, 0, 0, 55, 15, 1, 55, 0, 0, 44, 26, 1, 44, 0, 0, 34, 18, 2, 17, 0, 0, 26, 22, 2, 13, 0, 0, 80, 20, 1, 80, 0, 0, 64, 18, 2, 32, 0, 0, 48, 26, 2, 24, 0, 0, 36, 16, 4, 9, 0, 0, 108, 26, 1, 108, 0, 0, 86, 24, 2, 43, 0, 0, 62, 18, 2, 15, 2, 16, 46, 22, 2, 11, 2, 12, 136, 18, 2, 68, 0, 0, 108, 16, 4, 27, 0, 0, 76, 24, 4, 19, 0, 0, 60, 28, 4, 15, 0, 0, 156, 20, 2, 78, 0, 0, 124, 18, 4, 31, 0, 0, 88, 18, 2, 14, 4, 15, 66, 26, 4, 13, 1, 14, 194, 24, 2, 97, 0, 0, 154, 22, 2, 38, 2, 39, 110, 22, 4, 18, 2, 19, 86, 26, 4, 14, 2, 15, 232, 30, 2, 116, 0, 0, 182, 22, 3, 36, 2, 37, 132, 20, 4, 16, 4, 17, 100, 24, 4, 12, 4, 13, 274, 18, 2, 68, 2, 69, 216, 26, 4, 43, 1, 44, 154, 24, 6, 19, 2, 20, 122, 28, 6, 15, 2, 16, 324, 20, 4, 81, 0, 0, 254, 30, 1, 50, 4, 51, 180, 28, 4, 22, 4, 23, 140, 24, 3, 12, 8, 13, 370, 24, 2, 92, 2, 93, 290, 22, 6, 36, 2, 37, 206, 26, 4, 20, 6, 21, 158, 28, 7, 14, 4, 15, 428, 26, 4, 107, 0, 0, 334, 22, 8, 37, 1, 38, 244, 24, 8, 20, 4, 21, 180, 22, 12, 11, 4, 12, 461, 30, 3, 115, 1, 116, 365, 24, 4, 40, 5, 41, 261, 20, 11, 16, 5, 17, 197, 24, 11, 12, 5, 13, 523, 22, 5, 87, 1, 88, 415, 24, 5, 41, 5, 42, 295, 30, 5, 24, 7, 25, 223, 24, 11, 12, 7, 13, 589, 24, 5, 98, 1, 99, 453, 28, 7, 45, 3, 46, 325, 24, 15, 19, 2, 20, 253, 30, 3, 15, 13, 16, 647, 28, 1, 107, 5, 108, 507, 28, 10, 46, 1, 47, 367, 28, 1, 22, 15, 23, 283, 28, 2, 14, 17, 15, 721, 30, 5, 120, 1, 121, 563, 26, 9, 43, 4, 44, 397, 28, 17, 22, 1, 23, 313, 28, 2, 14, 19, 15, 795, 28, 3, 113, 4, 114, 627, 26, 3, 44, 11, 45, 445, 26, 17, 21, 4, 22, 341, 26, 9, 13, 16, 14, 861, 28, 3, 107, 5, 108, 669, 26, 3, 41, 13, 42, 485, 30, 15, 24, 5, 25, 385, 28, 15, 15, 10, 16, 932, 28, 4, 116, 4, 117, 714, 26, 17, 42, 0, 0, 512, 28, 17, 22, 6, 23, 406, 30, 19, 16, 6, 17, 1006, 28, 2, 111, 7, 112, 782, 28, 17, 46, 0, 0, 568, 30, 7, 24, 16, 25, 442, 24, 34, 13, 0, 0, 1094, 30, 4, 121, 5, 122, 860, 28, 4, 47, 14, 48, 614, 30, 11, 24, 14, 25, 464, 30, 16, 15, 14, 16, 1174, 30, 6, 117, 4, 118, 914, 28, 6, 45, 14, 46, 664, 30, 11, 24, 16, 25, 514, 30, 30, 16, 2, 17, 1276, 26, 8, 106, 4, 107, 1000, 28, 8, 47, 13, 48, 718, 30, 7, 24, 22, 25, 538, 30, 22, 15, 13, 16, 1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46, 4, 47, 754, 28, 28, 22, 6, 23, 596, 30, 33, 16, 4, 17, 1468, 30, 8, 122, 4, 123, 1128, 28, 22, 45, 3, 46, 808, 30, 8, 23, 26, 24, 628, 30, 12, 15, 28, 16, 1531, 30, 3, 117, 10, 118, 1193, 28, 3, 45, 23, 46, 871, 30, 4, 24, 31, 25, 661, 30, 11, 15, 31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28, 21, 45, 7, 46, 911, 30, 1, 23, 37, 24, 701, 30, 19, 15, 26, 16, 1735, 30, 5, 115, 10, 116, 1373, 28, 19, 47, 10, 48, 985, 30, 15, 24, 25, 25, 745, 30, 23, 15, 25, 16, 1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46, 29, 47, 1033, 30, 42, 24, 1, 25, 793, 30, 23, 15, 28, 16, 1955, 30, 17, 115, 0, 0, 1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24, 35, 25, 845, 30, 19, 15, 35, 16, 2071, 30, 17, 115, 1, 116, 1631, 28, 14, 46, 21, 47, 1171, 30, 29, 24, 19, 25, 901, 30, 11, 15, 46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28, 14, 46, 23, 47, 1231, 30, 44, 24, 7, 25, 961, 30, 59, 16, 1, 17, 2306, 30, 12, 121, 7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30, 39, 24, 14, 25, 986, 30, 22, 15, 41, 16, 2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47, 34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30, 2, 15, 64, 16, 2566, 30, 17, 122, 4, 123, 1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24, 10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30, 4, 122, 18, 123, 2102, 28, 13, 46, 32, 47, 1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15, 32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28, 40, 47, 7, 48, 1582, 30, 43, 24, 22, 25, 1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118, 6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30, 34, 24, 34, 25, 1276, 30, 20, 15, 61, 16 };
        private static readonly int[] _remainderBits = { 0, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0 };

        /// <summary>
        /// A list containing detailed capacity information for each version of QR codes.
        /// The index in the capacity table corresponds to one less than the version number.
        /// </summary>
        private static readonly List<VersionInfo> _capacityTable = CreateCapacityTable(_capacityBaseValues);
        private static readonly List<VersionInfo> _microCapacityTable = CreateMicroCapacityTable();

        /// <summary>
        /// A table containing the error correction capacities and data codeword information for different combinations of QR code versions and error correction levels.
        /// </summary>
        private static readonly List<ECCInfo> _capacityECCTable = CreateCapacityECCTable(_capacityECCBaseValues);

        /// <summary>
        /// Retrieves the error correction information for a specific QR code version and error correction level.
        /// </summary>
        /// <param name="version">The version of the QR code (1 to 40, or -1 to -4 for M1 to M4).</param>
        /// <param name="eccLevel">The desired error correction level (L, M, Q, or H). Do not supply <see cref="ECCLevel.Default"/>.</param>
        /// <returns>
        /// An <see cref="ECCInfo"/> object containing the total number of data codewords, ECC per block, 
        /// block group details, and other parameters required for encoding error correction data.
        /// </returns>
        public static ECCInfo GetEccInfo(int version, ECCLevel eccLevel)
        {
            foreach (var item in _capacityECCTable)
            {
                if (item.Version == version && item.ErrorCorrectionLevel == eccLevel)
                {
                    return item;
                }
            }

            throw new InvalidOperationException("No item found");   // same exception type as Linq would throw
        }

        /// <summary>
        /// Retrieves the capacity information for a specific QR code version.
        /// The returned structure contains detailed data capacity values for each error correction level (L, M, Q, H)
        /// and encoding mode (Numeric, Alphanumeric, Byte, Kanji), indicating the maximum number of characters
        /// that can be stored in a QR code of the specified version under each configuration.
        /// </summary>
        /// <param name="version">The version of the QR code (1 to 40, or -1 to -4 for M1 to M4).</param>
        /// <returns>
        /// A <see cref="VersionInfo"/> object containing data capacity details for all error correction levels 
        /// and encoding modes for the specified version.
        /// </returns>
        public static VersionInfo GetVersionInfo(int version)
            => version < 0 ? _microCapacityTable[-version - 1] : _capacityTable[version - 1];

        /// <summary>
        /// Retrieves the number of remainder bits required for a specific QR code version.
        /// Remainder bits are added to the final bit stream to ensure proper alignment with byte boundaries,
        /// as required by the QR code specification.
        /// </summary>
        /// <param name="version">The version of the QR code (1 to 40, or -1 to -4 for M1 to M4).</param>
        /// <returns>
        /// The number of remainder bits (0 to 7) that must be appended to the encoded bit stream.
        /// </returns>
        public static int GetRemainderBits(int version)
            => version < 0 ? 0 : _remainderBits[version - 1];

        /// <summary>
        /// Determines the minimum QR code version required to encode a given amount of data with a specific encoding mode and error correction level.
        /// If no suitable version is found, it throws an exception indicating that the data length exceeds the maximum capacity for the given settings.
        /// </summary>
        /// <param name="length">The length of the data to be encoded.</param>
        /// <param name="encMode">The encoding mode (e.g., Numeric, Alphanumeric, Byte).</param>
        /// <param name="eccLevel">The error correction level (e.g., Low, Medium, Quartile, High).</param>
        /// <returns>The minimum version of the QR code that can accommodate the given data and settings.</returns>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">
        /// Thrown when the data length exceeds the maximum capacity for the specified encoding mode and error correction level.
        /// </exception>
        public static int CalculateMinimumVersion(int length, EncodingMode encMode, ECCLevel eccLevel)
        {
            // only iterates through non-micro QR codes
            // capacity table is already sorted by version number ascending, so the smallest version that can hold the data is the first one found
            foreach (var x in _capacityTable)
            {
                // find the requested ECC level and encoding mode in the capacity table
                foreach (var y in x.Details)
                {
                    if (y.ErrorCorrectionLevel == eccLevel && y.CapacityDict[encMode] >= length)
                    {
                        // if the capacity of the current version is enough, return the version number
                        return x.Version;
                    }
                }
            }

            // if no version was found, throw an exception
            // In order to get the maxSizeByte we use a throw-helper method to avoid the allocation of a closure
            Throw(encMode, eccLevel);
            throw null!;    // this is needed to make the compiler happy

            static void Throw(EncodingMode encMode, ECCLevel eccLevel)
            {
                var maxSizeByte = _capacityTable.Where(
                    x => x.Details.Any(
                        y => (y.ErrorCorrectionLevel == eccLevel))
                    ).Max(x => x.Details.Single(y => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]);

                throw new Exceptions.DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
            }
        }

        /// <summary>
        /// Attempts to determine the minimum QR code version required to encode a data segment with a specific error correction level.
        /// This method accounts for the version-dependent size of mode and count indicators when calculating the total bit length.
        /// </summary>
        /// <param name="segment">The data segment to be encoded (includes encoding mode, character count, and data bits).</param>
        /// <param name="eccLevel">The error correction level (e.g., Low, Medium, Quartile, High).</param>
        /// <param name="version">When this method returns, contains the minimum version number (1-40) that can accommodate the data segment if a suitable version was found; otherwise, 0.</param>
        /// <returns><see langword="true"/> if a suitable QR code version was found; otherwise, <see langword="false"/>.</returns>
        public static bool TryCalculateMinimumVersion(DataSegment segment, ECCLevel eccLevel, out int version)
        {
            // Versions 1-9: Count indicator length is constant within this range
            var segmentBitLength = segment.GetBitLength(1);
            for (version = 1; version <= 9; version++)
            {
                var eccInfo = GetEccInfo(version, eccLevel);
                // Check if this version has enough capacity for the segment's total bits
                if (eccInfo.TotalDataBits >= segmentBitLength)
                {
                    return true;
                }
            }

            // Versions 10-26: Count indicator length is constant within this range
            segmentBitLength = segment.GetBitLength(10);
            for (version = 10; version <= 26; version++)
            {
                var eccInfo = GetEccInfo(version, eccLevel);
                // Check if this version has enough capacity for the segment's total bits
                if (eccInfo.TotalDataBits >= segmentBitLength)
                {
                    return true;
                }
            }

            // Versions 27-40: Count indicator length is constant within this range
            segmentBitLength = segment.GetBitLength(27);
            for (version = 27; version <= 40; version++)
            {
                var eccInfo = GetEccInfo(version, eccLevel);
                // Check if this version has enough capacity for the segment's total bits
                if (eccInfo.TotalDataBits >= segmentBitLength)
                {
                    return true;
                }
            }

            version = 0;
            return false;
        }

        /// <summary>
        /// Determines the minimum Micro QR code version required to encode a given amount of data with a specific encoding mode and error correction level.
        /// If no suitable version is found, it throws an exception indicating that the data length exceeds the maximum capacity for the given settings.
        /// </summary>
        /// <param name="length">The length of the data to be encoded.</param>
        /// <param name="encMode">The encoding mode (e.g., Numeric, Alphanumeric, Byte).</param>
        /// <param name="eccLevel">The error correction level (e.g., Default, Low, Medium, Quartile, High).</param>
        /// <returns>The minimum version of the QR code (-1 to -4) that can accommodate the given data and settings.</returns>
        /// <exception cref="QRCoder.Exceptions.DataTooLongException">
        /// Thrown when the data length exceeds the maximum capacity for the specified encoding mode and error correction level.
        /// </exception>
        public static int CalculateMinimumMicroVersion(int length, EncodingMode encMode, ECCLevel eccLevel)
        {
            // only iterates through non-micro QR codes
            // capacity table is already sorted by version number ascending, so the smallest version that can hold the data is the first one found
            foreach (var x in _microCapacityTable)
            {
                // find the requested ECC level and encoding mode in the capacity table
                foreach (var y in x.Details)
                {
                    // Use ECC level L for Micro QR Code versions 2, 3 and 4 when Default is specified
                    if (y.ErrorCorrectionLevel == eccLevel || (eccLevel == ECCLevel.Default && y.ErrorCorrectionLevel == ECCLevel.L))
                    {
                        // Not all versions support all encoding modes, so check if the encoding mode is supported
                        if (y.CapacityDict.TryGetValue(encMode, out int maxLength) && maxLength >= length)
                        {
                            // if the capacity of the current version is enough, return the version number
                            return x.Version;
                        }
                    }
                }
            }

            // if no version was found, throw an exception
            var maxSizeByte = _microCapacityTable
                .SelectMany(x => x.Details)
                .Where(y => (y.ErrorCorrectionLevel == eccLevel || (eccLevel == ECCLevel.Default && y.ErrorCorrectionLevel == ECCLevel.L))
                    && y.CapacityDict.ContainsKey(encMode))
                .Max(y => y.CapacityDict[encMode]);

            throw new QRCoder.Exceptions.DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
        }

        /// <summary>
        /// Generates a table containing the error correction capacities and data codeword information for different QR code versions and error correction levels.
        /// This table is essential for determining how much data can be encoded in a QR code of a specific version and ECC level,
        /// as well as how robust the QR code will be against distortions or obstructions.
        /// </summary>
        /// <returns>A list of ECCInfo structures, each representing the ECC data and capacities for different combinations of QR code versions and ECC levels.</returns>
        private static List<ECCInfo> CreateCapacityECCTable(int[] capacityECCBaseValues)
        {
            var localCapacityECCTable = new List<ECCInfo>(160 + 8);
            for (var i = 0; i < (4 * 6 * 40); i += (4 * 6))
            {
                localCapacityECCTable.AddRange(new[]
                {
                    new ECCInfo(
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.L,
                        totalDataCodewords: capacityECCBaseValues[i],
                        eccPerBlock: capacityECCBaseValues[i + 1],
                        blocksInGroup1: capacityECCBaseValues[i + 2],
                        codewordsInGroup1: capacityECCBaseValues[i + 3],
                        blocksInGroup2: capacityECCBaseValues[i + 4],
                        codewordsInGroup2: capacityECCBaseValues[i + 5]),
                    new ECCInfo(
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.M,
                        totalDataCodewords: capacityECCBaseValues[i + 6],
                        eccPerBlock: capacityECCBaseValues[i + 7],
                        blocksInGroup1: capacityECCBaseValues[i + 8],
                        codewordsInGroup1: capacityECCBaseValues[i + 9],
                        blocksInGroup2: capacityECCBaseValues[i + 10],
                        codewordsInGroup2: capacityECCBaseValues[i + 11]
                    ),
                    new ECCInfo(
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.Q,
                        totalDataCodewords: capacityECCBaseValues[i + 12],
                        eccPerBlock: capacityECCBaseValues[i + 13],
                        blocksInGroup1: capacityECCBaseValues[i + 14],
                        codewordsInGroup1: capacityECCBaseValues[i + 15],
                        blocksInGroup2: capacityECCBaseValues[i + 16],
                        codewordsInGroup2: capacityECCBaseValues[i + 17]
                    ),
                    new ECCInfo(
                        version: (i + 24) / 24,
                        errorCorrectionLevel: ECCLevel.H,
                        totalDataCodewords: capacityECCBaseValues[i + 18],
                        eccPerBlock: capacityECCBaseValues[i + 19],
                        blocksInGroup1: capacityECCBaseValues[i + 20],
                        codewordsInGroup1: capacityECCBaseValues[i + 21],
                        blocksInGroup2: capacityECCBaseValues[i + 22],
                        codewordsInGroup2: capacityECCBaseValues[i + 23]
                    )
                });
            }

            localCapacityECCTable.AddRange(new ECCInfo[]
            {
                // Micro QR Code Version M1 - only supports ECCLevel.Default (none)
                new ECCInfo(
                    version: -1,
                    errorCorrectionLevel: ECCLevel.Default,
                    totalDataCodewords: 3,
                    totalDataBits: 20,
                    eccPerBlock: 2),

                // Micro QR Code Version M2
                new ECCInfo(-2, ECCLevel.L, 5, 40, 5),
                new ECCInfo(-2, ECCLevel.M, 4, 32, 6),

                // Micro QR Code Version M3
                new ECCInfo(-3, ECCLevel.L, 11, 84, 6),
                new ECCInfo(-3, ECCLevel.M, 9, 68, 8),

                // Micro QR Code Version M4
                new ECCInfo(-4, ECCLevel.L, 16, 128, 8),
                new ECCInfo(-4, ECCLevel.M, 14, 112, 10),
                new ECCInfo(-4, ECCLevel.Q, 10, 80, 14),
            });

            return localCapacityECCTable;
        }

        /// <summary>
        /// Generates a list containing detailed capacity information for various versions of QR codes.
        /// This table includes capacities for different encoding modes (numeric, alphanumeric, byte, etc.) under each error correction level.
        /// The capacity table is crucial for QR code generation, as it determines how much data each QR code version can store depending on the encoding mode and error correction level used.
        /// The index in the capacity table corresponds to one less than the version number.
        /// </summary>
        private static List<VersionInfo> CreateCapacityTable(int[] capacityBaseValues)
        {
            var localCapacityTable = new List<VersionInfo>(40);
            for (var i = 0; i < (16 * 40); i += 16)
            {
                localCapacityTable.Add(new VersionInfo(
                    version: (i + 16) / 16,
                    versionInfoDetails: new List<VersionInfoDetails>(4)
                    {
                        new VersionInfoDetails(
                             ECCLevel.L,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i + 1] },
                                 { EncodingMode.Byte, capacityBaseValues[i + 2] },
                                 { EncodingMode.Kanji, capacityBaseValues[i + 3] },
                            }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.M,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i + 4] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i + 5] },
                                 { EncodingMode.Byte, capacityBaseValues[i + 6] },
                                 { EncodingMode.Kanji, capacityBaseValues[i + 7] },
                             }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.Q,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i + 8] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i + 9] },
                                 { EncodingMode.Byte, capacityBaseValues[i + 10] },
                                 { EncodingMode.Kanji, capacityBaseValues[i + 11] },
                             }
                        ),
                        new VersionInfoDetails(
                             ECCLevel.H,
                             new Dictionary<EncodingMode,int>(){
                                 { EncodingMode.Numeric, capacityBaseValues[i + 12] },
                                 { EncodingMode.Alphanumeric, capacityBaseValues[i + 13] },
                                 { EncodingMode.Byte, capacityBaseValues[i + 14] },
                                 { EncodingMode.Kanji, capacityBaseValues[i + 15] },
                             }
                        )
                    }
                ));
            }
            return localCapacityTable;
        }

        /// <inheritdoc cref="CreateCapacityTable(int[])"/>
        private static List<VersionInfo> CreateMicroCapacityTable()
        {
            var tbl = new List<VersionInfo>(4);

            var m1details = new List<VersionInfoDetails>(1)
            {
                new VersionInfoDetails(
                    ECCLevel.Default, // none
                    new Dictionary<EncodingMode, int>(1) {
                        { EncodingMode.Numeric, 5 },
                    }
                )
            };
            tbl.Add(new VersionInfo(-1, m1details));

            var m2details = new List<VersionInfoDetails>(2)
            {
                new VersionInfoDetails(
                    ECCLevel.L,
                    new Dictionary<EncodingMode, int>(2) {
                        { EncodingMode.Numeric, 10 },
                        { EncodingMode.Alphanumeric, 6 },
                    }
                ),
                new VersionInfoDetails(
                    ECCLevel.M,
                    new Dictionary<EncodingMode, int>(2) {
                        { EncodingMode.Numeric, 8 },
                        { EncodingMode.Alphanumeric, 5 },
                    }
                ),
            };
            tbl.Add(new VersionInfo(-2, m2details));

            var m3details = new List<VersionInfoDetails>(2)
            {
                new VersionInfoDetails(
                    ECCLevel.L,
                    new Dictionary<EncodingMode, int>(4) {
                        { EncodingMode.Numeric, 23 },
                        { EncodingMode.Alphanumeric, 14 },
                        { EncodingMode.Byte, 9 },
                        { EncodingMode.Kanji, 6 },
                    }
                ),
                new VersionInfoDetails(
                    ECCLevel.M,
                    new Dictionary<EncodingMode, int>(4) {
                        { EncodingMode.Numeric, 18 },
                        { EncodingMode.Alphanumeric, 11 },
                        { EncodingMode.Byte, 7 },
                        { EncodingMode.Kanji, 4 },
                    }
                ),
            };
            tbl.Add(new VersionInfo(-3, m3details));

            var m4details = new List<VersionInfoDetails>(3)
            {
                new VersionInfoDetails(
                    ECCLevel.L,
                    new Dictionary<EncodingMode, int>(4) {
                        { EncodingMode.Numeric, 35 },
                        { EncodingMode.Alphanumeric, 21 },
                        { EncodingMode.Byte, 15 },
                        { EncodingMode.Kanji, 9 },
                    }
                ),
                new VersionInfoDetails(
                    ECCLevel.M,
                    new Dictionary<EncodingMode, int>(4) {
                        { EncodingMode.Numeric, 30 },
                        { EncodingMode.Alphanumeric, 18 },
                        { EncodingMode.Byte, 13 },
                        { EncodingMode.Kanji, 8 },
                    }
                ),
                new VersionInfoDetails(
                    ECCLevel.Q,
                    new Dictionary<EncodingMode, int>(4) {
                        { EncodingMode.Numeric, 21 },
                        { EncodingMode.Alphanumeric, 13 },
                        { EncodingMode.Byte, 9 },
                        { EncodingMode.Kanji, 5 },
                    }
                ),
            };
            tbl.Add(new VersionInfo(-4, m4details));

            return tbl;
        }
    }
}
