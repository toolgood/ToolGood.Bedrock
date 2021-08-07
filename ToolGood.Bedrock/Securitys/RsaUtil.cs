using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// RSA 加解密
    /// </summary>
    public class RsaUtil
    {

        #region RSA 的密钥产生
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="Modulus"></param>
        /// <param name="Exponent"></param>
        public static void GetParams(string xmlKeys, out string Modulus, out string Exponent)
        {
            RSA rsa = new RSACryptoServiceProvider();
            try {
                LoadPrivateKey(rsa, xmlKeys);
            } catch {
                LoadPublicKey(rsa, xmlKeys);
            }
            var p = rsa.ExportParameters(false);
            Modulus = BitConverter.ToString(p.Modulus).Replace("-", "");
            Exponent = BitConverter.ToString(p.Exponent).Replace("-", "");
        }

        /// <summary>
        /// RSA 的密钥产生 产生私钥
        /// </summary>
        /// <param name="xmlKeys"></param>
        public static void CreateKey(out string xmlKeys)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = SaveXmlString(rsa, true);
        }

        /// <summary>
        /// RSA 的密钥产生 产生私钥
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="xmlKeys"></param>
        public static void CreateKey(int keySize, out string xmlKeys)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
            xmlKeys = SaveXmlString(rsa, true);
        }

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="publicKey"></param>
        public static void CreateKey(out string xmlKeys, out string publicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            xmlKeys = SaveXmlString(rsa, true);
            publicKey = SaveXmlString(rsa, false);
        }

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="xmlKeys"></param>
        /// <param name="publicKey"></param>
        public static void CreateKey(int keySize, out string xmlKeys, out string publicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize);
            xmlKeys = SaveXmlString(rsa, true);
            publicKey = SaveXmlString(rsa, false);
        }

        #endregion RSA 的密钥产生

        #region 公钥加密 私钥解密
        /// <summary>
        /// 公钥加密 返回Base64
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="EncryptString"></param>
        /// <returns></returns>
        public static string PublicEncrypt(string publicKey, string EncryptString)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPublicKey(rsa, publicKey);
                var bs = Encoding.UTF8.GetBytes(EncryptString);
                return Convert.ToBase64String(publicEncrypt(rsa, bs));
            }
        }

        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] PublicEncrypt(string publicKey, byte[] bytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPublicKey(rsa, publicKey);
                return publicEncrypt(rsa, bytes);
            }
        }
        private static byte[] publicEncrypt(RSACryptoServiceProvider rsa, byte[] bytes)
        {
            var keySize = rsa.KeySize / 8 - 11;
            if (bytes.Length <= keySize) {
                return rsa.Encrypt(bytes, false);
            }
            using (MemoryStream ms = new MemoryStream()) {
                var index = 0;
                while (index < bytes.Length) {
                    var length = Math.Min(keySize, bytes.Length - index);
                    var bs = new byte[length];
                    Array.Copy(bytes, index, bs, 0, length);
                    var bs2 = rsa.Encrypt(bs, false);
                    ms.Write(bs2, 0, bs2.Length);
                    index += keySize;
                }
                return ms.ToArray();
            }
        }


        /// <summary>
        /// 私钥解密 返回 UTF8格式
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="DecryptString">Base64格式</param>
        /// <returns></returns>
        public static string PrivateDecrypt(string publicKey, string DecryptString)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPrivateKey(rsa, publicKey);

                var bytes = Base64.FromBase64ForUrlString(DecryptString);
                var b2 = privateDecrypt(rsa, bytes);
                return Encoding.UTF8.GetString(b2);
            }
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] PrivateDecrypt(string privateKey, byte[] bytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPrivateKey(rsa, privateKey);
                return privateDecrypt(rsa, bytes);
            }
        }
        private static byte[] privateDecrypt(RSACryptoServiceProvider rsa, byte[] bytes)
        {
            var keySize = rsa.KeySize / 8;
            if (bytes.Length < keySize) {//修复 js base64化bug
                byte[] bs = new byte[keySize];
                Array.Copy(bytes, 0, bs, keySize - bytes.Length, bytes.Length);
                return rsa.Decrypt(bs, false);
            } else if (bytes.Length == keySize) {
                return rsa.Decrypt(bytes, false);
            }
            using (MemoryStream ms = new MemoryStream()) {
                var index = 0;
                while (index < bytes.Length) {
                    var length = Math.Min(keySize, bytes.Length - index);
                    var bs = new byte[length];
                    Array.Copy(bytes, index, bs, 0, length);
                    byte[] bs2 = rsa.Decrypt(bs, false);
                    ms.Write(bs2, 0, bs2.Length);
                    index += keySize;
                }
                return ms.ToArray();
            }
        }

        #endregion 公钥加密 私钥解密

        #region 私钥加密 公钥解密
        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="EncryptString"></param>
        /// <returns></returns>
        public static string PrivateEncrypt(string privateKey, string EncryptString)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPrivateKey(rsa, privateKey);
                var bs = Encoding.UTF8.GetBytes(EncryptString);
                var bytes = RSAPrivateEncryption.PrivareEncryption(rsa, bs);
                return Base64.ToBase64ForUrlString(bytes);
            }
        }
        /// <summary>
        /// 私钥加密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] PrivateEncrypt(string privateKey, byte[] bytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPrivateKey(rsa, privateKey);
                return RSAPrivateEncryption.PrivareEncryption(rsa, bytes);
            }
        }
        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="DecryptString"></param>
        /// <returns></returns>
        public static string PublicDecrypt(string publicKey, string DecryptString)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPublicKey(rsa, publicKey);
                var bs = Encoding.UTF8.GetBytes(DecryptString);
                var bytes = RSAPrivateEncryption.PublicDecryption(rsa, bs);
                return Base64.ToBase64ForUrlString(bytes);
            }
        }
        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] PublicDecrypt(string publicKey, byte[] bytes)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider()) {
                LoadPublicKey(rsa, publicKey);
                return RSAPrivateEncryption.PublicDecryption(rsa, bytes);
            }
        }

        #endregion 私钥加密 公钥解密


        #region RSA签名 验签

        /// <summary>
        /// RSA签名,默认SHA256
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="EncryptedSignatureData"></param>
        /// <returns></returns>
        public static bool Sign(string privateKey, byte[] HashbyteSignature, out byte[] EncryptedSignatureData)
        {
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            LoadPrivateKey(RSA, privateKey);

            SHA256 sh = new SHA256CryptoServiceProvider();
            EncryptedSignatureData = RSA.SignData(HashbyteSignature, sh);
            sh.Dispose();
            RSA.Dispose();
            return true;
        }


        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="hashType">md2,md5,sha1,sha224,sha256,sha384,sha512,ripemd160</param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="EncryptedSignatureData"></param>
        /// <returns></returns>
        public static bool Sign(string privateKey, string hashType, byte[] HashbyteSignature, out byte[] EncryptedSignatureData)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            LoadPrivateKey(RSA, privateKey);

            EncryptedSignatureData = RSA.SignData(HashbyteSignature, hashType);
            RSA.Dispose();
            return true;
        }

        /// <summary>
        /// 签名验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="signedData"></param>
        /// <param name="input_charset">编码格式</param>
        /// <returns>true(通过)，false(不通过)</returns>
        public static bool Verify(string publicKey, byte[] HashbyteSignature, byte[] signedData)
        {
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            LoadPublicKey(rsaPub, publicKey);
            SHA256 sh = new SHA256CryptoServiceProvider();
            return rsaPub.VerifyData(HashbyteSignature, sh, signedData);
        }

        /// <summary>
        /// 签名验签
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="hashType">md2,md5,sha1,sha224,sha256,sha384,sha512,ripemd160</param>
        /// <param name="HashbyteSignature"></param>
        /// <param name="signedData"></param>
        /// <returns>true(通过)，false(不通过)</returns>
        /// 各种签名 填充值
        /// md2:"3020300c06082a864886f70d020205000410",
        /// md5:"3020300c06082a864886f70d020505000410",
        /// sha1:"3021300906052b0e03021a05000414",
        /// sha224:"302d300d06096086480165030402040500041c",
        /// sha256:"3031300d060960864801650304020105000420",
        /// sha384:"3041300d060960864801650304020205000430",
        /// sha512:"3051300d060960864801650304020305000440",
        /// ripemd160: "3021300906052b2403020105000414"       
        public static bool Verify(string publicKey, string hashType, byte[] HashbyteSignature, byte[] signedData)
        {
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            LoadPublicKey(rsaPub, publicKey);
            return rsaPub.VerifyData(HashbyteSignature, hashType, signedData);
        }

        #endregion

        #region 密钥解析
        public static string ConvertToXml(string key, string password = null)
        {
            if (key.StartsWith("<")) { return key; }
            try {
                return SaveXmlString(LoadPemPrivateKey(key), true);
            } catch (Exception) { }
            try {
                return SaveXmlString(LoadPemPublicKey(key), false);
            } catch (Exception) { }

            var bytes = Encoding.Default.GetBytes(key);
            X509Certificate2 certificate2;
            if (string.IsNullOrEmpty(password)) {
                certificate2 = new X509Certificate2(bytes);
            } else {
                certificate2 = new X509Certificate2(bytes, password);
            }
            if (certificate2.HasPrivateKey) {
                return certificate2.PrivateKey.ToXmlString(true);
            }
            return certificate2.PublicKey.Key.ToXmlString(false);
        }

        private static string SaveXmlString(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
            return SaveXmlString(parameters, includePrivateParameters);
        }
        private static string SaveXmlString(RSAParameters parameters, bool includePrivateParameters)
        {
            if (includePrivateParameters) {
                return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                    Convert.ToBase64String(parameters.Modulus),
                    Convert.ToBase64String(parameters.Exponent),
                    Convert.ToBase64String(parameters.P),
                    Convert.ToBase64String(parameters.Q),
                    Convert.ToBase64String(parameters.DP),
                    Convert.ToBase64String(parameters.DQ),
                    Convert.ToBase64String(parameters.InverseQ),
                    Convert.ToBase64String(parameters.D));
            }
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(parameters.Modulus),
                Convert.ToBase64String(parameters.Exponent));
        }

        private static void LoadPrivateKey(RSA rsa, string key)
        {
            RSAParameters parameters;
            if (key.StartsWith("<")) {
                parameters = LoadXmlString(key);
            } else {
                parameters = LoadPemPrivateKey(key);
            }
            rsa.ImportParameters(parameters);
        }

        private static void LoadPublicKey(RSA rsa, string key)
        {
            RSAParameters parameters;
            if (key.StartsWith("<")) {
                parameters = LoadXmlString(key);
            } else {
                try {
                    parameters = LoadPemPublicKey(key);
                } catch {
                    parameters = LoadCertPublicKey(key);
                }
            }
            rsa.ImportParameters(parameters);
        }

        private static RSAParameters LoadCertPublicKey(string certString)
        {
            var bytes = Encoding.Default.GetBytes(certString);
            X509Certificate2 c1 = new X509Certificate2(bytes);
            string keyPublic = c1.PublicKey.Key.ToXmlString(false);  // 公钥
            return LoadXmlString(keyPublic);
        }

        private static RSAParameters LoadXmlString(string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue")) {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes) {
                    switch (node.Name) {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            } else {
                throw new Exception("Invalid XML RSA key.");
            }
            return parameters;
        }

        private static RSAParameters LoadPemPublicKey(string pemFileConent)
        {

            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 162) {
                throw new ArgumentException("pem file content is incorrect.");
            }
            byte[] pemModulus = new byte[128];
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, 29, pemModulus, 0, 128);
            Array.Copy(keyData, 159, pemPublicExponent, 0, 3);
            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            return para;
        }

        private static RSAParameters LoadPemPrivateKey(string pemFileConent)
        {
            byte[] keyData = Convert.FromBase64String(pemFileConent);
            if (keyData.Length < 609) {
                throw new ArgumentException("pem file content is incorrect.");
            }

            int index = 11;
            byte[] pemModulus = new byte[128];
            Array.Copy(keyData, index, pemModulus, 0, 128);

            index += 128;
            index += 2;//141
            byte[] pemPublicExponent = new byte[3];
            Array.Copy(keyData, index, pemPublicExponent, 0, 3);

            index += 3;
            index += 4;//148
            byte[] pemPrivateExponent = new byte[128];
            Array.Copy(keyData, index, pemPrivateExponent, 0, 128);

            index += 128;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//279
            byte[] pemPrime1 = new byte[64];
            Array.Copy(keyData, index, pemPrime1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//346
            byte[] pemPrime2 = new byte[64];
            Array.Copy(keyData, index, pemPrime2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//412/413
            byte[] pemExponent1 = new byte[64];
            Array.Copy(keyData, index, pemExponent1, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//479/480
            byte[] pemExponent2 = new byte[64];
            Array.Copy(keyData, index, pemExponent2, 0, 64);

            index += 64;
            index += ((int)keyData[index + 1] == 64 ? 2 : 3);//545/546
            byte[] pemCoefficient = new byte[64];
            Array.Copy(keyData, index, pemCoefficient, 0, 64);

            RSAParameters para = new RSAParameters();
            para.Modulus = pemModulus;
            para.Exponent = pemPublicExponent;
            para.D = pemPrivateExponent;
            para.P = pemPrime1;
            para.Q = pemPrime2;
            para.DP = pemExponent1;
            para.DQ = pemExponent2;
            para.InverseQ = pemCoefficient;
            return para;
        }
        #endregion

        public static class RSAPrivateEncryption
        {
            public static byte[] PrivareEncryption(RSACryptoServiceProvider rsa, byte[] data)
            {
                if (data == null)
                    throw new ArgumentNullException("data");
                if (rsa.PublicOnly)
                    throw new InvalidOperationException("Private key is not loaded");

                int len = (rsa.KeySize / 8) - 11;
                if (data.Length > len) {
                    using (var ms = new MemoryStream()) {
                        MemoryStream msInput = new MemoryStream(data);
                        byte[] buffer = new byte[len];
                        int readLen = msInput.Read(buffer, 0, len);

                        while (readLen > 0) {
                            byte[] dataToEnc = new byte[readLen];
                            Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                            var bytes = PrivareEncryption2(rsa, dataToEnc);
                            if (bytes.Length == rsa.KeySize / 8 + 1) {
                                ms.Write(bytes, 0, bytes.Length);
                            } else {
                                ms.Write(bytes, 0, bytes.Length);
                                var l = rsa.KeySize / 8 + 1 - bytes.Length;
                                byte[] bs = new byte[l];
                                ms.Write(bs, 0, l);
                            }

                            readLen = msInput.Read(buffer, 0, len);
                        }
                        msInput.Close();
                        return ms.ToArray();
                    }
                }
                return PrivareEncryption2(rsa, data);
            }



            public static byte[] PublicDecryption(RSACryptoServiceProvider rsa, byte[] cipherData)
            {
                if (cipherData == null)
                    throw new ArgumentNullException("cipherData");

                int len = (rsa.KeySize / 8) + 1;
                if (cipherData.Length > len) {
                    using (var ms = new MemoryStream()) {
                        MemoryStream msInput = new MemoryStream(cipherData);
                        byte[] buffer = new byte[len];
                        int readLen = msInput.Read(buffer, 0, len);

                        while (readLen > 0) {
                            byte[] dataToEnc = new byte[readLen];
                            Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                            var bytes = PublicDecryption2(rsa, dataToEnc);
                            ms.Write(bytes, 0, bytes.Length);
                            readLen = msInput.Read(buffer, 0, len);
                        }
                        msInput.Close();
                        return ms.ToArray();
                    }
                }
                return PublicDecryption2(rsa, cipherData);
            }

            private static byte[] PrivareEncryption2(RSACryptoServiceProvider rsa, byte[] data)
            {
                BigInteger numData = GetBig(AddPadding(data));

                RSAParameters rsaParams = rsa.ExportParameters(true);
                BigInteger D = GetBig(rsaParams.D);
                BigInteger Modulus = GetBig(rsaParams.Modulus);
                BigInteger encData = BigInteger.ModPow(numData, D, Modulus);

                return encData.ToByteArray();
            }

            private static byte[] PublicDecryption2(RSACryptoServiceProvider rsa, byte[] cipherData)
            {
                BigInteger numEncData = new BigInteger(cipherData);

                RSAParameters rsaParams = rsa.ExportParameters(false);
                BigInteger Exponent = GetBig(rsaParams.Exponent);
                BigInteger Modulus = GetBig(rsaParams.Modulus);

                BigInteger decData = BigInteger.ModPow(numEncData, Exponent, Modulus);

                byte[] data = decData.ToByteArray();
                byte[] result = new byte[data.Length - 1];
                Array.Copy(data, result, result.Length);
                result = RemovePadding(result);

                Array.Reverse(result);
                return result;
            }


            private static BigInteger GetBig(byte[] data)
            {
                byte[] inArr = (byte[])data.Clone();
                Array.Reverse(inArr);  // Reverse the byte order
                byte[] final = new byte[inArr.Length + 1];  // Add an empty byte at the end, to simulate unsigned BigInteger (no negatives!)
                Array.Copy(inArr, final, inArr.Length);

                return new BigInteger(final);
            }

            private static byte[] AddPadding(byte[] data)
            {
                Random rnd = new Random();
                byte[] paddings = new byte[4];
                rnd.NextBytes(paddings);
                paddings[0] = (byte)(paddings[0] | 128);

                byte[] results = new byte[data.Length + 4];

                Array.Copy(paddings, results, 4);
                Array.Copy(data, 0, results, 4, data.Length);
                return results;
            }
            private static byte[] RemovePadding(byte[] data)
            {
                byte[] results = new byte[data.Length - 4];
                Array.Copy(data, results, results.Length);
                return results;
            }

        }

    }

}
