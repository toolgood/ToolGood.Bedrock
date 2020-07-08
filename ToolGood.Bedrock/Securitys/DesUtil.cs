using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Security;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// DES 加解密
    /// </summary>
    public static class DesUtil
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
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
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
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
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


        #region Java

        public static string EncryptForJava(string input, string key)
        {
            var inCipher = CreateCipher(true, key);

            var inputArray = Encoding.UTF8.GetBytes(input);

            byte[] cipherData = inCipher.DoFinal(inputArray);

            return Convert.ToBase64String(cipherData);
        }

        public static string DecryptForJava(string input, string key)
        {
            var inputArrary = Convert.FromBase64String(input);

            var outCipher = CreateCipher(false, key);

            var encryptedDataStream = new MemoryStream(inputArrary, false);

            var dataStream = new MemoryStream();

            var outCipherStream = new CipherStream(dataStream, null, outCipher);

            int ch;
            while ((ch = encryptedDataStream.ReadByte()) >= 0)
            {
                outCipherStream.WriteByte((byte) ch);
            }

            outCipherStream.Close();
            encryptedDataStream.Close();

            var dataBytes = dataStream.ToArray();

            return Encoding.UTF8.GetString(dataBytes);
        }

        static IBufferedCipher CreateCipher(bool forEncryption, string key,
            string cipMode = "DESede/ECB/PKCS5Padding")
        {
            var algorithmName = cipMode;
            if (cipMode.IndexOf('/') >= 0)
            {
                algorithmName = cipMode.Substring(0, cipMode.IndexOf('/'));
            }

            var cipher = CipherUtilities.GetCipher(cipMode);

            var keyBytes = Encoding.UTF8.GetBytes(key);

            var keyParameter = ParameterUtilities.CreateKeyParameter(algorithmName, keyBytes);

            cipher.Init(forEncryption, keyParameter);

            return cipher;
        }

        #endregion
    }

}
