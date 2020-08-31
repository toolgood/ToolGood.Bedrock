using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SevenZip;

namespace ToolGood.Bedrock.Utils
{
    /// <summary>
    /// 7z Dll
    /// </summary>
    public class SevenZipUtil
    {
        /// <summary>
        /// 解压单文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] DecompressSingleFile(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (SevenZipExtractor extractor = new SevenZipExtractor(stream))
                {
                    using (var ms = new MemoryStream())
                    {
                        extractor.ExtractFile(0, ms);
                        return ms.ToArray();
                    }
                }
            }
            throw new Exception("");
        }
        /// <summary>
        /// 压缩单文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] CompressionSingleFile(byte[] data, string fileName)
        {
            if (data == null || data.Length == 0)
                return data;
            using (MemoryStream outms = new MemoryStream())
            {
                using (MemoryStream inms = new MemoryStream(data))
                {
                    SevenZipCompressor compressor = new SevenZipCompressor();
                    Dictionary<string, Stream> dict = new Dictionary<string, Stream> { { fileName, inms } };
                    compressor.CompressStreamDictionary(dict, outms);
                    return outms.ToArray();
                }
            }
        }
        /// <summary>
        /// 重命名压缩的文件名
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static byte[] RenameSingleFile(byte[] data, string fileName)
        {
            var bytes = DecompressSingleFile(data);
            return CompressionSingleFile(bytes, fileName);
        }
    }
}
