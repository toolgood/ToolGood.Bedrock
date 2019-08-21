using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// Base62
    /// </summary>
    public static class Base62
    {
        private const string ALPHABET = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 转成Base62String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase62String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[] 
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase62String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }

    /// <summary>
    /// Base58
    /// </summary>
    public static class Base58
    {
        private const string ALPHABET = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        /// <summary>
        /// 转成 Base58String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase58String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[]
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase58String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }
    /// <summary>
    /// Base52
    /// </summary>
    public static class Base52
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 转成 Base52String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase52String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[]
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase52String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }

    /// <summary>
    /// Base36
    /// </summary>
    public static class Base36
    {
        private const string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 转成 Base36String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase36String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[]
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase36String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }
    /// <summary>
    /// Base32
    /// </summary>
    public static class Base32
    {
        private const string ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 转成 Base32String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase32String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[]
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase32String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray);
            return bytes.Reverse().ToArray();
        }
    }
    /// <summary>
    /// Base26
    /// </summary>
    public static class Base26
    {
        private const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// 转成 Base26String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToBase26String(byte[] input)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = input.Reverse().ToArray();
            return baseEncoding.Encode(bytes);
        }
        /// <summary>
        /// 转成 byte[] 
        /// </summary>
        /// <param name="baseArray"></param>
        /// <returns></returns>
        public static byte[] FromBase26String(string baseArray)
        {
            var baseEncoding = new BaseEncoding(ALPHABET, BaseEncoding.EndianFormat.Little);
            var bytes = baseEncoding.Decode(baseArray.ToLower());
            return bytes.Reverse().ToArray();
        }
        /// <summary>
        /// Excel中的列字母转换为数字,从0开始
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int ColumnToIndex(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+")) { throw new Exception("invalid parameter"); }

            int index = 0;
            char[] chars = columnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++) {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }

        /// <summary>
        ///  数字转换为Excel中的列字母,从0开始
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string IndexToColumn(int index)
        {
            if (index < 0) { throw new Exception("invalid parameter"); }

            List<string> chars = new List<string>();
            do {
                if (chars.Count > 0) index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }

    }

}
