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
        [JsonProperty("code",NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        /// <summary>
        /// 密文
        /// </summary>
        [JsonProperty("ciphertext", NullValueHandling = NullValueHandling.Ignore)]
        public string Ciphertext { get; set; }

        [JsonProperty("extendData", NullValueHandling = NullValueHandling.Ignore)]
        public object ExtendData { get; set; }

        [JsonProperty("sqlTimes", NullValueHandling = NullValueHandling.Ignore)]
        public List<DebugSqlTime> SqlTimes { get; set; }

        [JsonProperty("logs", NullValueHandling = NullValueHandling.Ignore)]
        public List<DebugLog> Logs { get; set; }

        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public Page Page { get; set; }

    }
}
