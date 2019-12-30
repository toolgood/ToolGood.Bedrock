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
            using (RsaEncryption rsa = new RsaEncryption()) {
                rsa.LoadPrivateFromXml(privateKey);
                var bs = Encoding.UTF8.GetBytes(EncryptString);
                return Convert.ToBase64String(rsa.PrivateEncryption(bs));
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
            using (RsaEncryption rsa = new RsaEncryption()) {
                rsa.LoadPrivateFromXml(privateKey);
                return rsa.PrivateEncryption(bytes);
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
            using (RsaEncryption rsa = new RsaEncryption()) {
                rsa.LoadPublicFromXml(publicKey);
                var bs = rsa.PublicDecryption(Convert.FromBase64String(DecryptString));
                return Encoding.UTF8.GetString(bs);
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
            using (RsaEncryption rsa = new RsaEncryption()) {
                rsa.LoadPublicFromXml(publicKey);
                return rsa.PublicDecryption(bytes);
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

        private static string SaveXmlString(RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

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

        class RsaEncryption : IDisposable
        {
            private BigInteger D;
            private BigInteger Exponent;
            private BigInteger Modulus;
            private int bufferSize = 0;
            private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            private bool isPrivateKeyLoaded = false;
            private bool isPublicKeyLoaded = false;

            public void LoadPublicFromXml(string publicString)
            {
                //if (!File.Exists(publicPath))
                //    throw new FileNotFoundException("File not exists: " + publicPath);
                // Using the .NET RSA class to load a key from an Xml file, and populating the relevant members
                // of my class with it's RSAParameters
                try {
                    LoadPublicKey(rsa, publicString);

                    //rsa.FromXmlString(publicPath);
                    RSAParameters rsaParams = rsa.ExportParameters(false);
                    Modulus = FromBytes(rsaParams.Modulus);
                    Exponent = FromBytes(rsaParams.Exponent);
                    isPublicKeyLoaded = true;
                    isPrivateKeyLoaded = false;
                    bufferSize = rsa.KeySize / 8 - 11;
                }
                // Examle for the proper use of try - catch blocks: Informing the main app where and why the Exception occurred
                //catch (XmlSyntaxException ex)  // Not an xml file
                //{
                //    string excReason = "Exception occurred at LoadPublicFromXml(), Selected file is not a valid xml file.";
                //    System.Diagnostics.Debug.WriteLine(excReason + " Exception Message: " + ex.Message);
                //    throw new Exception(excReason, ex);
                //}
                catch (CryptographicException ex)  // Not a Key file
                {
                    string excReason = "Exception occurred at LoadPublicFromXml(), Selected xml file is not a public key file.";
                    System.Diagnostics.Debug.WriteLine(excReason + " Exception Message: " + ex.Message);
                    throw new Exception(excReason, ex);
                } catch (Exception ex)  // other exception, hope the ex.message will help
                {
                    string excReason = "General Exception occurred at LoadPublicFromXml().";
                    System.Diagnostics.Debug.WriteLine(excReason + " Exception Message: " + ex.Message);
                    throw new Exception(excReason, ex);
                }
                // You might want to replace the Diagnostics.Debug with your Log statement
            }

            public void LoadPrivateFromXml(string privateString)
            {
                try {
                    LoadPrivateKey(rsa, privateString);

                    RSAParameters rsaParams = rsa.ExportParameters(true);
                    D = FromBytes(rsaParams.D);  // This parameter is only for private key
                    Exponent = FromBytes(rsaParams.Exponent);
                    Modulus = FromBytes(rsaParams.Modulus);
                    isPrivateKeyLoaded = true;
                    isPublicKeyLoaded = true;
                    bufferSize = rsa.KeySize / 8 - 11;
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine("Exception occurred at LoadPrivateFromXml()\nMessage: " + ex.Message);
                    throw ex;
                }
            }

            public byte[] PrivateEncryption(byte[] data)
            {
                if (!isPrivateKeyLoaded)  // is the private key has been loaded?
                    throw new CryptographicException
                        ("Private Key must be loaded before using the Private Encryption method!");
                if (data.Length > bufferSize) {
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[bufferSize];
                    MemoryStream msInput = new MemoryStream(data);
                    int readLen = msInput.Read(buffer, 0, bufferSize);
                    while (readLen > 0) {
                        byte[] dataToEnc = new byte[readLen];
                        Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                        bytes.AddRange(PrivateEncryption(dataToEnc));
                        readLen = msInput.Read(buffer, 0, bufferSize);
                    }
                    msInput.Close();
                    return bytes.ToArray();
                }
                return BigInteger.ModPow(FromBytes(data), D, Modulus).ToByteArray().Reverse().ToArray();
                //return new BigInteger(data).ModPow(D, Modulus).getBytes();
            }

            public byte[] PublicEncryption(byte[] data)
            {
                if (!isPublicKeyLoaded)  // is the public key has been loaded?
                    throw new CryptographicException
                        ("Public Key must be loaded before using the Public Encryption method!");
                if (data.Length > bufferSize) {
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[bufferSize];
                    MemoryStream msInput = new MemoryStream(data);
                    int readLen = msInput.Read(buffer, 0, bufferSize);

                    while (readLen > 0) {
                        byte[] dataToEnc = new byte[readLen];
                        Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                        bytes.AddRange(PublicEncryption(dataToEnc));

                        readLen = msInput.Read(buffer, 0, bufferSize);
                    }
                    msInput.Close();
                    return bytes.ToArray();
                }
                // Converting the byte array data into a BigInteger instance
                return BigInteger.ModPow(FromBytes(data), Exponent, Modulus).ToByteArray().Reverse().ToArray();
                //return new BigInteger(data).modPow(Exponent, Modulus).getBytes();
            }

            public byte[] PrivateDecryption(byte[] encryptedData)
            {
                if (!isPrivateKeyLoaded)  // is the private key has been loaded?
                    throw new CryptographicException
                        ("Private Key must be loaded before using the Private Decryption method!");
                if (encryptedData.Length > rsa.KeySize / 8) {
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[rsa.KeySize / 8];
                    MemoryStream msInput = new MemoryStream(encryptedData);
                    int readLen = msInput.Read(buffer, 0, rsa.KeySize / 8);

                    while (readLen > 0) {
                        byte[] dataToEnc = new byte[readLen];
                        Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                        bytes.AddRange(PrivateDecryption(dataToEnc));

                        readLen = msInput.Read(buffer, 0, rsa.KeySize / 8);
                    }
                    msInput.Close();
                    return bytes.ToArray();
                }
                return BigInteger.ModPow(FromBytes(encryptedData), D, Modulus).ToByteArray().Reverse().ToArray();
                // Converting the encrypted data byte array data into a BigInteger instance
                //return new BigInteger(encryptedData).modPow(D, Modulus).getBytes();
            }

            public byte[] PublicDecryption(byte[] encryptedData)
            {
                if (!isPublicKeyLoaded)  // is the public key has been loaded?
                    throw new CryptographicException
                        ("Public Key must be loaded before using the Public Deccryption method!");
                if (encryptedData.Length > rsa.KeySize / 8) {
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[rsa.KeySize / 8];
                    MemoryStream msInput = new MemoryStream(encryptedData);
                    int readLen = msInput.Read(buffer, 0, rsa.KeySize / 8);

                    while (readLen > 0) {
                        byte[] dataToEnc = new byte[readLen];
                        Array.Copy(buffer, 0, dataToEnc, 0, readLen);

                        bytes.AddRange(PublicDecryption(dataToEnc));

                        readLen = msInput.Read(buffer, 0, rsa.KeySize / 8);
                    }
                    msInput.Close();
                    return bytes.ToArray();
                }
                return BigInteger.ModPow(FromBytes(encryptedData), Exponent, Modulus).ToByteArray().Reverse().ToArray();

                //return new BigInteger(encryptedData).modPow(Exponent, Modulus).getBytes();
            }

            private static BigInteger FromBytes(byte[] beBytes)
            {
                // 1、BigInteger的构造函数接受byte[]的格式是“低位在前（Litter Endian）”。所以以下两行是等价的：
                //    new BigInteger(new byte[]{1, 2, 3, 4})
                //    new BitInteger(new byte[]{1, 2, 3, 4, 0, 0, 0})
                // 2、BigInteger支持负数，如果byte[]的最高二进制位非零，则表示为负数，比如new byte[]{1,2,3, 0x80}就是负数。
                //    而RSA中的参数都是正整数，因此，Concat(0)用来保证正整数。
                // 3、如果输入的byte[]的格式是“高位在前(Big Endian)”，那么要先用Reverse翻转一次。
                return new BigInteger(beBytes.Reverse().Concat(new byte[] { 0 }).ToArray());
            }
            public void Dispose()
            {
                rsa.Clear();
            }
        }






        ///// <summary>
        ///// 解密 Pem 的私钥
        ///// </summary>
        ///// <param name="pemstr"></param>
        ///// <returns></returns>
        //public static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        //{
        //    byte[] pkcs8privatekey;
        //    pkcs8privatekey = Convert.FromBase64String(pemstr);
        //    if (pkcs8privatekey != null) {
        //        RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
        //        return rsa;
        //    } else
        //        return null;
        //}

        ///// <summary>
        ///// 解密 Pem 的公钥
        ///// </summary>
        ///// <param name="pkcs8"></param>
        ///// <returns></returns>
        //public static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        //{
        //    byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
        //    byte[] seq = new byte[15];

        //    MemoryStream mem = new MemoryStream(pkcs8);
        //    int lenstream = (int)mem.Length;
        //    BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        //    byte bt = 0;
        //    ushort twobytes = 0;

        //    try {
        //        twobytes = binr.ReadUInt16();
        //        if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
        //            binr.ReadByte();	//advance 1 byte
        //        else if (twobytes == 0x8230)
        //            binr.ReadInt16();	//advance 2 bytes
        //        else
        //            return null;

        //        bt = binr.ReadByte();
        //        if (bt != 0x02)
        //            return null;

        //        twobytes = binr.ReadUInt16();

        //        if (twobytes != 0x0001)
        //            return null;

        //        seq = binr.ReadBytes(15);		//read the Sequence OID
        //        if (!CompareBytearrays(seq, SeqOID))	//make sure Sequence for OID is correct
        //            return null;

        //        bt = binr.ReadByte();
        //        if (bt != 0x04)	//expect an Octet string 
        //            return null;

        //        bt = binr.ReadByte();		//read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
        //        if (bt == 0x81)
        //            binr.ReadByte();
        //        else
        //            if (bt == 0x82)
        //            binr.ReadUInt16();
        //        //------ at this stage, the remaining sequence should be the RSA private key

        //        byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
        //        RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
        //        return rsacsp;
        //    } catch (Exception) {
        //        return null;
        //    } finally { binr.Close(); }

        //}

        //private static bool CompareBytearrays(byte[] a, byte[] b)
        //{
        //    if (a.Length != b.Length)
        //        return false;
        //    int i = 0;
        //    foreach (byte c in a) {
        //        if (c != b[i])
        //            return false;
        //        i++;
        //    }
        //    return true;
        //}
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="privkey"></param>
        ///// <returns></returns>
        //public static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        //{
        //    byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

        //    // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
        //    MemoryStream mem = new MemoryStream(privkey);
        //    BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading
        //    byte bt = 0;
        //    ushort twobytes = 0;
        //    int elems = 0;
        //    try {
        //        twobytes = binr.ReadUInt16();
        //        if (twobytes == 0x8130)	//data read as little endian order (actual data order for Sequence is 30 81)
        //            binr.ReadByte();	//advance 1 byte
        //        else if (twobytes == 0x8230)
        //            binr.ReadInt16();	//advance 2 bytes
        //        else
        //            return null;

        //        twobytes = binr.ReadUInt16();
        //        if (twobytes != 0x0102)	//version number
        //            return null;
        //        bt = binr.ReadByte();
        //        if (bt != 0x00)
        //            return null;


        //        //------  all private key components are Integer sequences ----
        //        elems = GetIntegerSize(binr);
        //        MODULUS = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        E = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        D = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        P = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        Q = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        DP = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        DQ = binr.ReadBytes(elems);

        //        elems = GetIntegerSize(binr);
        //        IQ = binr.ReadBytes(elems);

        //        // ------- create RSACryptoServiceProvider instance and initialize with public key -----
        //        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        //        RSAParameters RSAparams = new RSAParameters();
        //        RSAparams.Modulus = MODULUS;
        //        RSAparams.Exponent = E;
        //        RSAparams.D = D;
        //        RSAparams.P = P;
        //        RSAparams.Q = Q;
        //        RSAparams.DP = DP;
        //        RSAparams.DQ = DQ;
        //        RSAparams.InverseQ = IQ;
        //        RSA.ImportParameters(RSAparams);
        //        return RSA;
        //    } catch (Exception) {
        //        return null;
        //    } finally { binr.Close(); }
        //}

        //private static int GetIntegerSize(BinaryReader binr)
        //{
        //    byte bt = 0;
        //    byte lowbyte = 0x00;
        //    byte highbyte = 0x00;
        //    int count = 0;
        //    bt = binr.ReadByte();
        //    if (bt != 0x02)		//expect integer
        //        return 0;
        //    bt = binr.ReadByte();

        //    if (bt == 0x81)
        //        count = binr.ReadByte();	// data size in next byte
        //    else
        //        if (bt == 0x82) {
        //        highbyte = binr.ReadByte(); // data size in next 2 bytes
        //        lowbyte = binr.ReadByte();
        //        byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
        //        count = BitConverter.ToInt32(modint, 0);
        //    } else {
        //        count = bt;     // we already have the data size
        //    }



        //    while (binr.ReadByte() == 0x00) {	//remove high order zeros in data
        //        count -= 1;
        //    }
        //    binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
        //    return count;
        //}

        //#endregion
    }
}
