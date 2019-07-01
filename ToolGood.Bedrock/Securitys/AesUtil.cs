using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// AES 加解密
    /// </summary>
    public static class AesUtil
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="encryptString">要加密的字符串</param>
        /// <param name="key">加密密钥</param>
        /// <param name="defaultEncode">默认编码格式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string encryptString, string key, Encoding defaultEncode = null)
        {
            byte[] inputByteArray = (defaultEncode ?? Encoding.ASCII).GetBytes(encryptString);
            var encodeBytes = Encrypt(inputByteArray, key, defaultEncode: defaultEncode);
            return Convert.ToBase64String(encodeBytes);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sourceBytes">要加密的字节数组</param>
        /// <param name="key">加密密钥</param>
        /// <param name="defaultEncode">默认编码格式</param>
        /// <returns>加密后的字节数组</returns>
        public static byte[] Encrypt(byte[] sourceBytes, string key, Encoding defaultEncode = null)
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider();
            string providerKey = GetProviderKey(key);
            provider.Key = (defaultEncode ?? Encoding.ASCII).GetBytes(providerKey);
            provider.IV = (defaultEncode ?? Encoding.ASCII).GetBytes(providerKey);
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cryStream = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write)) {
                cryStream.Write(sourceBytes, 0, sourceBytes.Length);
                cryStream.FlushFinalBlock();
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encodeString">要解密的字符串</param>
        /// <param name="key">解密密钥</param>
        /// <param name="defaultEncode">默认编码格式</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string encodeString, string key, Encoding defaultEncode = null)
        {
            var sourceBytes = Convert.FromBase64String(encodeString);
            var decodeBytes = Decrypt(sourceBytes, key, defaultEncode: defaultEncode);
            return (defaultEncode ?? Encoding.ASCII).GetString(decodeBytes);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sourceBytes">要解密的字节数组</param>
        /// <param name="key">解密密钥</param>
        /// <param name="defaultEncode">默认编码格式</param>
        /// <returns>解密后的字节数组</returns>
        public static byte[] Decrypt(byte[] sourceBytes, string key, Encoding defaultEncode = null)
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider();
            string providerKey = GetProviderKey(key);
            provider.Key = (defaultEncode ?? Encoding.ASCII).GetBytes(providerKey);
            provider.IV = (defaultEncode ?? Encoding.ASCII).GetBytes(providerKey);
            MemoryStream stream = new MemoryStream();
            using (CryptoStream cryStream = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write)) {
                cryStream.Write(sourceBytes, 0, sourceBytes.Length);
                cryStream.FlushFinalBlock();
                return stream.ToArray();
            }
        }



        /// <summary>
        /// 获取ProviderKey的值
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns>ProviderKey的值</returns>
        private static string GetProviderKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 8) return "01234567";
            return key.Substring(0, 8);
        }
         


    }
}
