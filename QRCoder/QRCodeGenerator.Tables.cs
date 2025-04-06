using System.Collections.Generic;

namespace QRCoder;

/// <summary>
/// Partial class containing table generation methods for QR code generation.
/// </summary>
public partial class QRCodeGenerator
{
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
        for (int i = 0; i < _alphanumEncTable.Length; i++)
            localAlphanumEncDict.Add(_alphanumEncTable[i], localAlphanumEncDict.Count);
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

        for (var i = 0; i < (7 * 40); i += 7)
        {
            var points = new List<Point>(50);
            for (var x = 0; x < 7; x++)
            {
                if (_alignmentPatternBaseValues[i + x] != 0)
                {
                    for (var y = 0; y < 7; y++)
                    {
                        if (_alignmentPatternBaseValues[i + y] != 0)
                        {
                            var p = new Point(_alignmentPatternBaseValues[i + x] - 2, _alignmentPatternBaseValues[i + y] - 2);
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
        for (var i = 0; i < (4 * 6 * 40); i += (4 * 6))
        {
            localCapacityECCTable.AddRange(
            new[]
            {
                new ECCInfo(
                    (i+24) / 24,
                    ECCLevel.L,
                    _capacityECCBaseValues[i],
                    _capacityECCBaseValues[i+1],
                    _capacityECCBaseValues[i+2],
                    _capacityECCBaseValues[i+3],
                    _capacityECCBaseValues[i+4],
                    _capacityECCBaseValues[i+5]),
                new ECCInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ECCLevel.M,
                    totalDataCodewords: _capacityECCBaseValues[i+6],
                    eccPerBlock: _capacityECCBaseValues[i+7],
                    blocksInGroup1: _capacityECCBaseValues[i+8],
                    codewordsInGroup1: _capacityECCBaseValues[i+9],
                    blocksInGroup2: _capacityECCBaseValues[i+10],
                    codewordsInGroup2: _capacityECCBaseValues[i+11]
                ),
                new ECCInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ECCLevel.Q,
                    totalDataCodewords: _capacityECCBaseValues[i+12],
                    eccPerBlock: _capacityECCBaseValues[i+13],
                    blocksInGroup1: _capacityECCBaseValues[i+14],
                    codewordsInGroup1: _capacityECCBaseValues[i+15],
                    blocksInGroup2: _capacityECCBaseValues[i+16],
                    codewordsInGroup2: _capacityECCBaseValues[i+17]
                ),
                new ECCInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ECCLevel.H,
                    totalDataCodewords: _capacityECCBaseValues[i+18],
                    eccPerBlock: _capacityECCBaseValues[i+19],
                    blocksInGroup1: _capacityECCBaseValues[i+20],
                    codewordsInGroup1: _capacityECCBaseValues[i+21],
                    blocksInGroup2: _capacityECCBaseValues[i+22],
                    codewordsInGroup2: _capacityECCBaseValues[i+23]
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
        for (var i = 0; i < (16 * 40); i += 16)
        {
            localCapacityTable.Add(new VersionInfo(

                (i + 16) / 16,
                new List<VersionInfoDetails>(4)
                {
                    new VersionInfoDetails(
                         ECCLevel.L,
                         new Dictionary<EncodingMode,int>(){
                             { EncodingMode.Numeric, _capacityBaseValues[i] },
                             { EncodingMode.Alphanumeric, _capacityBaseValues[i+1] },
                             { EncodingMode.Byte, _capacityBaseValues[i+2] },
                             { EncodingMode.Kanji, _capacityBaseValues[i+3] },
                        }
                    ),
                    new VersionInfoDetails(
                         ECCLevel.M,
                         new Dictionary<EncodingMode,int>(){
                             { EncodingMode.Numeric, _capacityBaseValues[i+4] },
                             { EncodingMode.Alphanumeric, _capacityBaseValues[i+5] },
                             { EncodingMode.Byte, _capacityBaseValues[i+6] },
                             { EncodingMode.Kanji, _capacityBaseValues[i+7] },
                         }
                    ),
                    new VersionInfoDetails(
                         ECCLevel.Q,
                         new Dictionary<EncodingMode,int>(){
                             { EncodingMode.Numeric, _capacityBaseValues[i+8] },
                             { EncodingMode.Alphanumeric, _capacityBaseValues[i+9] },
                             { EncodingMode.Byte, _capacityBaseValues[i+10] },
                             { EncodingMode.Kanji, _capacityBaseValues[i+11] },
                         }
                    ),
                    new VersionInfoDetails(
                         ECCLevel.H,
                         new Dictionary<EncodingMode,int>(){
                             { EncodingMode.Numeric, _capacityBaseValues[i+12] },
                             { EncodingMode.Alphanumeric, _capacityBaseValues[i+13] },
                             { EncodingMode.Byte, _capacityBaseValues[i+14] },
                             { EncodingMode.Kanji, _capacityBaseValues[i+15] },
                         }
                    )
                }
            ));
        }
        return localCapacityTable;
    }
}
