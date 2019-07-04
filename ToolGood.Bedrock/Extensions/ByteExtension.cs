using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System
{
    /// <summary>
    /// 字节数组操作扩展类
    /// </summary>
    public static class ByteExtension
    {
        #region 将字符串转为字节数组

        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string @str)
        {
            return Encoding.UTF8.GetBytes(@str);
        }

        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="encode">转换编码格式</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string @str, Encoding encode)
        {
            return encode.GetBytes(@str);
        }

        /// <summary>
        /// 将字符串转为字节数组
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="encodeName">转换编码名字</param>
        /// <returns>字节数组</returns>
        public static byte[] ToBytes(this string @str, string encodeName)
        {
            return Encoding.GetEncoding(encodeName).GetBytes(@str);
        }

        #endregion 将字符串转为字节数组


        public static byte[] ToBytesForHexString(this string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2) {
                string s = hexString.Substring(i, 2);
                bytes[i / 2] = byte.Parse(s, NumberStyles.HexNumber, null);
            }
            return bytes;
        }

        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);

            foreach (byte b in bytes) {
                builder.Append(b.ToString("X2"));
            }
            return builder.ToString();
        }

        public static byte[] SubBytes(this byte[] bytes, int startIndex, int length)
        {
            byte[] res = new byte[length];
            Array.Copy(bytes, startIndex, res, 0, length);
            return res;
        }


        public static void SaveFile(this byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }

    }
}
