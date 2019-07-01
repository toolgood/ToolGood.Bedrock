﻿using System.IO;
using System.IO.Compression;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 压缩
    /// </summary>
    public static class CompressionUtil
    {
        /// <summary>
        /// 默认压缩
        /// </summary>
        /// <param name="data">要压缩的字节数组</param>
        /// <returns>压缩后的数组</returns>
        public static byte[] DeflateCompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream()) {
                    using (DeflateStream gZipStream = new DeflateStream(stream, CompressionMode.Compress)) {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();
                    }
                    return stream.ToArray();
                }
            } catch {
                return data;
            }
        }

        /// <summary>
        /// Gzip压缩
        /// </summary>
        /// <param name="data">要压缩的字节数组</param>
        /// <returns>Gzip压缩后的数组</returns>
        public static byte[] GzipCompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream()) {
                    using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Compress)) {
                        gZipStream.Write(data, 0, data.Length);
                        gZipStream.Close();
                    }
                    return stream.ToArray();
                }
            } catch {
                return data;
            }
        }
    }

}
