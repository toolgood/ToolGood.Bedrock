using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 获取 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
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
