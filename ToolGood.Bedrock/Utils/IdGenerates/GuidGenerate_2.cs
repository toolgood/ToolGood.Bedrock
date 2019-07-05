using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ToolGood.Bedrock.Utils.IdGenerates
{
    public class GuidGenerate_2
    {
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// 获取有序的唯一ID。
        /// http://www.codeproject.com/Articles/388157/GUIDs-as-fast-primary-keys-under-multiple-database
        /// </summary>
        /// <param name="guidType">有序GUID的类型（sqlServer用AtEnd，mysql用AsString或者AsBinary，oracle用AsBinary，postgresql用AsString或者AsBinary）</param>
        /// <returns></returns>
        public static Guid Generate(SequentialGuidType guidType = SequentialGuidType.SequentialAtEnd)
        {
            var randomBytes = new byte[10];
            Rng.GetBytes(randomBytes);

            var timestampBytes = BitConverter.GetBytes(DateTime.UtcNow.Ticks / 10000L);
            if (BitConverter.IsLittleEndian) { Array.Reverse(timestampBytes); }

            var guidBytes = new byte[16];
            switch (guidType) {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to reverse the order
                    // of the Data1 and Data2 blocks on little-endian systems.
                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian) {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;
                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("guidType", guidType, null);
            }
            return new Guid(guidBytes);
        }
    
    }
    /// <summary>
    /// 有序GUID的类型（sqlServer用AtEnd，mysql用AsString或者AsBinary，oracle用AsBinary，postgresql用AsString或者AsBinary）
    /// </summary>
    public enum SequentialGuidType
    {
        SequentialAsString,
        SequentialAsBinary,
        SequentialAtEnd
    }
}
