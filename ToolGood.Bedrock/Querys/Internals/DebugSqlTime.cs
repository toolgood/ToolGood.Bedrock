using System;
using Newtonsoft.Json;

namespace ToolGood.Bedrock.Internals
{
    public class DebugSqlTime
    {
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }

        [JsonProperty("sql", NullValueHandling = NullValueHandling.Ignore)]
        public string Sql { get; set; }

        [JsonProperty("args", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Args { get; set; }

        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndTime { get; set; }

        [JsonProperty("queryTimes", NullValueHandling = NullValueHandling.Ignore)]
        public int? QueryTimes { get; set; }

        [JsonProperty("errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
    }
}
