using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace QRCoder;

/// <summary>
/// Represents the data structure of a QR code.
/// </summary>
public class QRCodeData : IDisposable
{
    /// <summary>
    /// Gets or sets the module matrix of the QR code.
    /// </summary>
    public List<BitArray> ModuleMatrix { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="QRCodeData"/> class with the specified version.
    /// </summary>
    /// <param name="version">The version of the QR code.</param>
    public QRCodeData(int version)
    {
        Version = version;
        var size = ModulesPerSideFromVersion(version);
        ModuleMatrix = new List<BitArray>(size);
        for (var i = 0; i < size; i++)
            ModuleMatrix.Add(new BitArray(size));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QRCodeData"/> class with the specified version and padding option.
    /// </summary>
    /// <param name="version">The version of the QR code.</param>
    /// <param name="addPadding">Indicates whether padding should be added to the QR code.</param>
    public QRCodeData(int version, bool addPadding)
    {
        Version = version;
        var size = ModulesPerSideFromVersion(version) + (addPadding ? 8 : 0);
        ModuleMatrix = new List<BitArray>(size);
        for (var i = 0; i < size; i++)
            ModuleMatrix.Add(new BitArray(size));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QRCodeData"/> class with raw data from a specified path and compression mode.
    /// </summary>
    /// <param name="pathToRawData">The path to the raw data file.</param>
    /// <param name="compressMode">The compression mode used for the raw data.</param>
    public QRCodeData(string pathToRawData, Compression compressMode) : this(File.ReadAllBytes(pathToRawData), compressMode)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QRCodeData"/> class with raw data and compression mode.
    /// </summary>
    /// <param name="rawData">The raw data of the QR code.</param>
    /// <param name="compressMode">The compression mode used for the raw data.</param>
    public QRCodeData(byte[] rawData, Compression compressMode)
    {
        //Decompress
        if (compressMode == Compression.Deflate)
        {
            using var input = new MemoryStream(rawData);
            using var output = new MemoryStream();
            using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            rawData = output.ToArray();
        }
        else if (compressMode == Compression.GZip)
        {
            using var input = new MemoryStream(rawData);
            using var output = new MemoryStream();
            using (var dstream = new GZipStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            rawData = output.ToArray();
        }

        var count = rawData.Length;

        if (count < 5)
            throw new Exception("Invalid raw data file. File too short.");
        if (rawData[0] != 0x51 || rawData[1] != 0x52 || rawData[2] != 0x52)
            throw new Exception("Invalid raw data file. Filetype doesn't match \"QRR\".");

        //Set QR code version
        var sideLen = (int)rawData[4];
        Version = (sideLen - 21 - 8) / 4 + 1;

        //Unpack
        var modules = new Queue<bool>(8 * (count - 5));
        for (int j = 5; j < count; j++)
        {
            var b = rawData[j];
            for (int i = 7; i >= 0; i--)
            {
                modules.Enqueue((b & (1 << i)) != 0);
            }
        }

        //Build module matrix
        ModuleMatrix = new List<BitArray>(sideLen);
        for (int y = 0; y < sideLen; y++)
        {
            ModuleMatrix.Add(new BitArray(sideLen));
            for (int x = 0; x < sideLen; x++)
            {
                ModuleMatrix[y][x] = modules.Dequeue();
            }
        }
    }

    /// <summary>
    /// Gets the raw data of the QR code with the specified compression mode.
    /// </summary>
    /// <param name="compressMode">The compression mode used for the raw data.</param>
    /// <returns>Returns the raw data of the QR code as a byte array.</returns>
    public byte[] GetRawData(Compression compressMode)
    {
        using var output = new MemoryStream();
        Stream targetStream = output;
        DeflateStream? deflateStream = null;
        GZipStream? gzipStream = null;

        //Set up compression stream if needed
        if (compressMode == Compression.Deflate)
        {
            deflateStream = new DeflateStream(output, CompressionMode.Compress, true);
            targetStream = deflateStream;
        }
        else if (compressMode == Compression.GZip)
        {
            gzipStream = new GZipStream(output, CompressionMode.Compress, true);
            targetStream = gzipStream;
        }

        try
        {
            //Add header - signature ("QRR")
            targetStream.Write(new byte[] { 0x51, 0x52, 0x52, 0x00 }, 0, 4);

            //Add header - rowsize
            targetStream.WriteByte((byte)ModuleMatrix.Count);

            //Build data queue
            var dataQueue = new Queue<int>();
            foreach (var row in ModuleMatrix)
            {
                foreach (var module in row)
                {
                    dataQueue.Enqueue((bool)module ? 1 : 0);
                }
            }
            for (int i = 0; i < 8 - (ModuleMatrix.Count * ModuleMatrix.Count) % 8; i++)
            {
                dataQueue.Enqueue(0);
            }

            //Process queue
            while (dataQueue.Count > 0)
            {
                byte b = 0;
                for (int i = 7; i >= 0; i--)
                {
                    b += (byte)(dataQueue.Dequeue() << i);
                }
                targetStream.WriteByte(b);
            }
        }
        finally
        {
            //Close compression streams to flush data
            deflateStream?.Dispose();
            gzipStream?.Dispose();
        }

        return output.ToArray();
    }

    /// <summary>
    /// Saves the raw data of the QR code to a specified file with the specified compression mode.
    /// </summary>
    /// <param name="filePath">The path to the file where the raw data will be saved.</param>
    /// <param name="compressMode">The compression mode used for the raw data.</param>
    public void SaveRawData(string filePath, Compression compressMode)
        => File.WriteAllBytes(filePath, GetRawData(compressMode));

    /// <summary>
    /// Gets the version of the QR code.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Gets the number of modules per side from the specified version.
    /// </summary>
    /// <param name="version">The version of the QR code (1 to 40, or -1 to -4 for Micro QR codes).</param>
    /// <returns>Returns the number of modules per side.</returns>
    private static int ModulesPerSideFromVersion(int version)
        => version > 0
            ? 21 + (version - 1) * 4
            : 11 + (-version - 1) * 2;

    /// <summary>
    /// Releases all resources used by the <see cref="QRCodeData"/>.
    /// </summary>
    public void Dispose()
    {
        ModuleMatrix = null!;
        Version = 0;

    }

    /// <summary>
    /// Specifies the compression mode used for the raw data.
    /// </summary>
    public enum Compression
    {
        /// <summary>
        /// No compression.
        /// </summary>
        Uncompressed,
        /// <summary>
        /// Deflate compression.
        /// </summary>
        Deflate,
        /// <summary>
        /// GZip compression.
        /// </summary>
        GZip
    }
}
