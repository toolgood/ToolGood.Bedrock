using Newtonsoft.Json;
using System.Collections.Generic;
using ToolGood.Bedrock.Internals;
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
        [JsonProperty("code",NullValueHandling = NullValueHandling.Ignore)]
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
        /// 
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [JsonProperty("ciphertext", NullValueHandling = NullValueHandling.Ignore)]
        public string Ciphertext { get; set; }
        /// <summary>
        /// 扩展数据
        /// </summary>
        [JsonProperty("extendData", NullValueHandling = NullValueHandling.Ignore)]
        public object ExtendData { get; set; }
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
        /// 页面
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public Page Page { get; set; }

    }
}
