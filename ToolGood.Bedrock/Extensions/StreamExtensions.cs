using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{
    public static partial class StreamExtensions
    {
        public static byte[] GetAllBytes(this Stream stream)
        {
            byte[] streamBytes;

            using (var memoryStream = new MemoryStream()) {
                stream.CopyTo(memoryStream);
                streamBytes = memoryStream.ToArray();
            }

            return streamBytes;
        }


    }
}
