using System.Collections;
using System.Collections.Generic;

namespace QRCoder
{
    using QRCoder.Framework4._0Methods;
    using System;

    public class QRCodeData : IDisposable
    {
        public List<BitArray> ModuleMatrix { get; set; }
        
        public QRCodeData(int version)
        {
            this.Version = version;
            var size = ModulesPerSideFromVersion(version);
            this.ModuleMatrix = new List<BitArray>();
            for (var i = 0; i < size; i++)
                this.ModuleMatrix.Add(new BitArray(size));
        }
#if !PCL
        public QRCodeData(string pathToRawData, bool isRawCompressed) : this(System.IO.File.ReadAllBytes(pathToRawData), isRawCompressed)
        {
        }
#endif
        public QRCodeData(byte[] rawData, bool isRawCompressed)
        {
            var bytes = new List<byte>(rawData);

            //Decompress
            if (isRawCompressed)
            {
                var input = new System.IO.MemoryStream(bytes.ToArray());
                var output = new System.IO.MemoryStream();
                using (var dstream = new System.IO.Compression.DeflateStream(input, System.IO.Compression.CompressionMode.Decompress))
                {
                    Stream4Methods.CopyTo(dstream, output);
                }
                bytes = new List<byte>(output.ToArray());
            }

            //Set QR code version
            var sideLen = (int)bytes[0];
            bytes.RemoveAt(0);
            this.Version = (sideLen - 21 - 8) / 4 + 1;

            //Unpack
            var modules = new Queue<bool>();
            foreach (var b in bytes)
            {
                var bArr = new BitArray(new byte[] { b });
                for (int i = 7; i >= 0; i--)
                {
                    modules.Enqueue((b & (1 << i)) != 0);
                }               
            }
            
            //Build module matrix
            this.ModuleMatrix = new List<BitArray>();
            for (int y = 0; y < sideLen; y++)
            {
                this.ModuleMatrix.Add(new BitArray(sideLen));
                for (int x = 0; x < sideLen; x++)
                {
                    this.ModuleMatrix[y][x] = modules.Dequeue();
                }
            }
            
        }

        public byte[] GetRawData(bool compress)
        {
            var bytes = new List<byte>();

            //Add header
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
            if (compress)
            {
                var output = new System.IO.MemoryStream();
                using (var dstream = new System.IO.Compression.DeflateStream(output, System.IO.Compression.CompressionMode.Compress))
                {
                    dstream.Write(rawData, 0, rawData.Length);
                }
                rawData = output.ToArray();
            }
            return rawData;
        }

#if !PCL
        public void SaveRawData(string filePath, bool compress)
        {         
            System.IO.File.WriteAllBytes(filePath, GetRawData(compress));
        }
#endif

        public int Version { get; private set; }
        
        private static int ModulesPerSideFromVersion(int version)
        {
            return 21 + (version - 1) * 4;
        }

        public void Dispose()
        {
            this.ModuleMatrix = null;
            this.Version = 0;
            
        }
    }
}
