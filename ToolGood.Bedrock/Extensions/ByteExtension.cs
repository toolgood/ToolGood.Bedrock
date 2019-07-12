using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System
{
    /// <summary>
    /// 字节数组操作扩展类
    /// </summary>
    public static partial class ObjectExtension
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

        /// <summary>
        /// 截取byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] SubBytes(this byte[] bytes, int startIndex, int length)
        {
            byte[] res = new byte[length];
            Array.Copy(bytes, startIndex, res, 0, length);
            return res;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="filePath"></param>
        public static void SaveFile(this byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }

    }
}
