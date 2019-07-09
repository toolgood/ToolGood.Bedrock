using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ToolGood.RcxCrypto;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 加密过的查询参数
    /// </summary>
    public abstract class EncryptedQueryArgs : QueryArgs
    {
        /// <summary>
        /// 钥匙（RSA公匙加密过的）
        /// </summary>
        [JsonProperty("rsaKey")]
        [Required(ErrorMessage = "rsaKey is null.")]
        public string RsaKey { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [JsonProperty("ciphertext")]
        [Required(ErrorMessage = "ciphertext is null.")]
        public string Ciphertext { get; set; }

        /// <summary>
        /// 时间截
        /// </summary>
        [JsonProperty("timestamp")]
        [Required(ErrorMessage = "timestamp is null.")]
        public long Timestamp { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        [JsonProperty("sign")]
        [Required(ErrorMessage = "sign is null.")]
        public string Sign { get; set; }

        /// <summary>
        /// 核对签名
        /// </summary>
        /// <returns></returns>
        public bool CheckSign()
        {
            var st = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Timestamp);
            if (Math.Abs(st.Seconds) > 15) { return false; }

            var txt = $"{Ciphertext}|{RsaKey}|{Timestamp}";
            var hash = HashUtil.GetMd5String(txt);
            return Sign.ToUpper() == hash;
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="xmlKey"></param>
        /// <returns></returns>
        public abstract bool DecryptData(string xmlKey);

        /// <summary>
        /// 核对数据
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckDate(out string errMsg);
    }
    /// <summary>
    /// 加密过的查询参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EncryptedQueryArgs<T> : EncryptedQueryArgs
    {
        /// <summary>
        /// 解密后的参数
        /// </summary>
        [JsonIgnore]
        public T Data { get; set; }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="xmlKey"></param>
        /// <returns></returns>
        public override bool DecryptData(string xmlKey)
        {
            try {
                var bytes = Base64.FromBase64ForUrlString(RsaKey);
                var key = RsaUtil.PrivateDecrypt(xmlKey, bytes);//解密

                var bs = ThreeRCX.Encrypt(Base64.FromBase64ForUrlString(Ciphertext), key);//解密
                var json = Encoding.UTF8.GetString(bs);
                Data = JsonConvert.DeserializeObject<T>(json);
                return true;
            } catch { }
            return false;
        }

        public override bool CheckDate(out string errMsg)
        {
            var type = typeof(T);
            var pis = type.GetProperties().ToList();
            pis.RemoveAll(q => q.CanRead == false || q.CanWrite == false);

            foreach (var pi in pis) {
                var atts = pi.GetCustomAttributes(true);
                if (atts.Length == 0) { continue; }

                //TODO:




            }



            errMsg = "";
            return false;
        }

    }
}
