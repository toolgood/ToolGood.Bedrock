using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Internals;
using ToolGood.RcxCrypto;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 查询结果
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// code码
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }
        /// <summary>
        /// code描术
        /// </summary>
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [JsonProperty("ciphertext", NullValueHandling = NullValueHandling.Ignore)]
        public string Ciphertext { get; set; }

        /// <summary>
        /// sql执行日志
        /// </summary>
        [JsonProperty("sqlTimes", NullValueHandling = NullValueHandling.Ignore)]
        public List<DebugSqlTime> SqlTimes { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public List<DebugLog> Logs { get; set; }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="password"></param>
        public void EncryptData(byte[] password)
        {
            if (password!= null && password.Length>0) {
                var json = JsonConvert.SerializeObject(this.Data);
                var bytes = Encoding.UTF8.GetBytes(json);

                var bs = ThreeRCX.Encrypt(bytes, password);//解密
                Ciphertext = Base64.ToBase64String(bs);
                Data = null;
            }
        }

    }
}
