using System.IO;
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
                    using (DeflateStream zStream = new DeflateStream(stream, CompressionMode.Compress)) {
                        zStream.Write(data, 0, data.Length);
                        zStream.Close();
                    }
                    return stream.ToArray();
                }
            } catch {
                return data;
            }
        }

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DeflateDecompression(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream(data)) {
                    using (DeflateStream zStream = new DeflateStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            return resultStream.ToArray();
                        }
                    }
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
                    using (GZipStream zStream = new GZipStream(stream, CompressionMode.Compress)) {
                        zStream.Write(data, 0, data.Length);
                        zStream.Close();
                    }
                    return stream.ToArray();
                }
            } catch {
                return data;
            }
        }

        /// <summary>
        /// Gzip解压
        /// </summary>
        /// <param name="data">要压缩的字节数组</param>
        /// <returns>Gzip压缩后的数组</returns>
        public static byte[] GzipDecompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream(data)) {
                    using (GZipStream zStream = new GZipStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            return resultStream.ToArray();
                        }
                    }
                }
            } catch {
                return data;
            }
        }


#if NETCOREAPP3_0
        /// <summary>
        /// Br压缩
        /// </summary>
        /// <param name="data">要压缩的字节数组</param>
        /// <returns>Gzip压缩后的数组</returns>
        public static byte[] BrCompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream()) {
                    
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionLevel.Optimal)) {
                        zStream.Write(data, 0, data.Length);
                        zStream.Close();
                    }
                    return stream.ToArray();
                }
            } catch {
                return data;
            }
        }

        /// <summary>
        /// Br压缩
        /// </summary>
        /// <param name="data">要压缩的字节数组</param>
        /// <returns>Gzip压缩后的数组</returns>
        public static byte[] BrDecompress(byte[] data)
        {
            if (data == null || data.Length < 1)
                return data;
            try {
                using (MemoryStream stream = new MemoryStream(data)) {
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            return resultStream.ToArray();
                        }
                    }
                }
            } catch {
                return data;
            }
        }

#endif



    }

}
