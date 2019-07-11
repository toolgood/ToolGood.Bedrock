using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using ToolGood.AntiDuplication;
using ToolGood.Bedrock.Internals;
using Newtonsoft.Json;

namespace ToolGood.Bedrock
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class QueryArgs
    {
        public QueryArgs()
        {
            SqlTimes = new List<DebugSqlTime>();
            Logs = new List<DebugLog>();
        }


        internal HttpContext HttpContext;

        public HttpContext GetHttpContext() { return HttpContext; }

        public void SetHttpContext(HttpContext value) { HttpContext = value; }

        /// <summary>
        /// 使用调试模式
        /// </summary>
        public bool? UseLog { get; set; }

        private DateTime? _UseToday;

        /// <summary>
        /// 使用今天
        /// </summary>
        public DateTime? UseToday {
            set {
                if (value != null) {
                    DateTimeUtil.UseToday(value.Value);
                }
                _UseToday = value;
            }
            get { return _UseToday; }
        }
        private DateTime? _UseNow;

        /// <summary>
        /// 使用当前时间
        /// </summary>
        public DateTime? UseNow {
            set {
                if (value != null) {
                    DateTimeUtil.UseNow(value.Value);
                }
                _UseNow = value;
            }
            get { return _UseNow; }
        }

        /// <summary>
        /// 批序号
        /// </summary>
        public string BatchNum { get; set; }


        /// <summary>
        /// sql执行时间
        /// </summary>
        [JsonIgnore]
        public List<DebugSqlTime> SqlTimes { get; set; }

        /// <summary>
        /// log记录
        /// </summary>
        [JsonIgnore]
        public List<DebugLog> Logs { get; set; }

         

    }

}
