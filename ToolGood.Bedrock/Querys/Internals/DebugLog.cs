using System;
using Newtonsoft.Json;

namespace ToolGood.Bedrock.Internals
{
    
    public class DebugLog
    {
        [JsonProperty("startTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartTime { get; set; }

        [JsonProperty("logType", NullValueHandling = NullValueHandling.Ignore)]
        public string LogType { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
