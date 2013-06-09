using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Utils
{
    public class DataCompression
    {
        public byte[] Compress(MemoryStream uncompressed)
        {
            var outStream = new System.IO.MemoryStream();
            using (var tinyStream = new GZipStream(uncompressed, CompressionMode.Compress))
            {
                uncompressed.CopyTo(outStream);
            }
            return outStream.ToArray();
        }

        public byte[] Decompress(byte[] compressed)
        {
            var outStream = new System.IO.MemoryStream(compressed);
            //Decompress                
            var bigStream = new GZipStream(outStream, CompressionMode.Decompress);
            var bigStreamOut = new System.IO.MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            return bigStreamOut.ToArray();
        }
    }
}
