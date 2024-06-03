using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace QRCoder
{
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
            this.Version = version;
            var size = ModulesPerSideFromVersion(version);
            this.ModuleMatrix = new List<BitArray>(size);
            for (var i = 0; i < size; i++)
                this.ModuleMatrix.Add(new BitArray(size));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QRCodeData"/> class with the specified version and padding option.
        /// </summary>
        /// <param name="version">The version of the QR code.</param>
        /// <param name="addPadding">Indicates whether padding should be added to the QR code.</param>
        public QRCodeData(int version, bool addPadding)
        {
            this.Version = version;
            var size = ModulesPerSideFromVersion(version) + (addPadding ? 8 : 0);
            this.ModuleMatrix = new List<BitArray>(size);
            for (var i = 0; i < size; i++)
                this.ModuleMatrix.Add(new BitArray(size));
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
            var bytes = new List<byte>(rawData);

            //Decompress
            if (compressMode == Compression.Deflate)
            {
                using (var input = new MemoryStream(bytes.ToArray()))
                {
                    using (var output = new MemoryStream())
                    {
                        using (var dstream = new DeflateStream(input, CompressionMode.Decompress))
                        {
                            dstream.CopyTo(output);
                        }
                        bytes = new List<byte>(output.ToArray());
                    }
                }
            }
            else if (compressMode == Compression.GZip)
            {
                using (var input = new MemoryStream(bytes.ToArray()))
                {
                    using (var output = new MemoryStream())
                    {
                        using (var dstream = new GZipStream(input, CompressionMode.Decompress))
                        {
                            dstream.CopyTo(output);
                        }
                        bytes = new List<byte>(output.ToArray());
                    }
                }
            }

            if (bytes[0] != 0x51 || bytes[1] != 0x52 || bytes[2] != 0x52)
                throw new Exception("Invalid raw data file. Filetype doesn't match \"QRR\".");

            //Set QR code version
            var sideLen = (int)bytes[4];
            bytes.RemoveRange(0, 5);
            this.Version = (sideLen - 21 - 8) / 4 + 1;

            //Unpack
            var modules = new Queue<bool>(8 * bytes.Count);
            foreach (var b in bytes)
            {
                var bArr = new BitArray(new byte[] { b });
                for (int i = 7; i >= 0; i--)
                {
                    modules.Enqueue((b & (1 << i)) != 0);
                }
            }

            //Build module matrix
            this.ModuleMatrix = new List<BitArray>(sideLen);
            for (int y = 0; y < sideLen; y++)
            {
                this.ModuleMatrix.Add(new BitArray(sideLen));
                for (int x = 0; x < sideLen; x++)
                {
                    this.ModuleMatrix[y][x] = modules.Dequeue();
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
            var bytes = new List<byte>();

            //Add header - signature ("QRR")
            bytes.AddRange(new byte[]{ 0x51, 0x52, 0x52, 0x00 });

            //Add header - rowsize
            bytes.Add((byte)ModuleMatrix.Count);

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
                bytes.Add(b);
            }
            var rawData = bytes.ToArray();

            //Compress stream (optional)
            if (compressMode == Compression.Deflate)
            {
                using (var output = new MemoryStream())
                {
                    using (var dstream = new DeflateStream(output, CompressionMode.Compress))
                    {
                        dstream.Write(rawData, 0, rawData.Length);
                    }
                    rawData = output.ToArray();
                }
            }
            else if (compressMode == Compression.GZip)
            {
                using (var output = new MemoryStream())
                {
                    using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress, true))
                    {
                        gzipStream.Write(rawData, 0, rawData.Length);
                    }
                    rawData = output.ToArray();
                }
            }
            return rawData;
        }

        /// <summary>
        /// Saves the raw data of the QR code to a specified file with the specified compression mode.
        /// </summary>
        /// <param name="filePath">The path to the file where the raw data will be saved.</param>
        /// <param name="compressMode">The compression mode used for the raw data.</param>
        public void SaveRawData(string filePath, Compression compressMode)
        {
            File.WriteAllBytes(filePath, GetRawData(compressMode));
        }

        /// <summary>
        /// Gets the version of the QR code.
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the number of modules per side from the specified version.
        /// </summary>
        /// <param name="version">The version of the QR code.</param>
        /// <returns>Returns the number of modules per side.</returns>
        private static int ModulesPerSideFromVersion(int version)
        {
            return 21 + (version - 1) * 4;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="QRCodeData"/>.
        /// </summary>
        public void Dispose()
        {
            this.ModuleMatrix = null!;
            this.Version = 0;

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
}
