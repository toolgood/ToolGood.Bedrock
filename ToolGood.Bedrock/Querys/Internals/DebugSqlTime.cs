using System;
using Newtonsoft.Json;

namespace ToolGood.Bedrock.Internals
{
    /// <summary>
    /// sql调试时间
    /// </summary>
    public class DebugSqlTime
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }
        /// <summary>
        /// SQL
        /// </summary>
        [JsonProperty("sql", NullValueHandling = NullValueHandling.Ignore)]
        public string Sql { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Args { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        [JsonProperty("queryTimes", NullValueHandling = NullValueHandling.Ignore)]
        public int? QueryTimes { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
    }
}
