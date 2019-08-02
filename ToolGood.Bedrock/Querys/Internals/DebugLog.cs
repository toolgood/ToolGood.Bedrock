using System;
using Newtonsoft.Json;
using ToolGood.Bedrock.Attributes;

namespace ToolGood.Bedrock.Internals
{
    /// <summary>
    /// 调试日志
    /// </summary>
    [JsonRequireAttribute]
    public class DebugLog
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// log类型
        /// </summary>
        [JsonProperty("logType", NullValueHandling = NullValueHandling.Ignore)]
        public string LogType { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
