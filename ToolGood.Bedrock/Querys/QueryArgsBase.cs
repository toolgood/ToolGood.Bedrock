﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using ToolGood.AntiDuplication;
using ToolGood.Bedrock.Internals;

namespace ToolGood.Bedrock
{
    public class QueryArgsBase
    {
        private static AntiDupQueue<string, CancellationTokenSource> antiDupQueue = new AntiDupQueue<string, CancellationTokenSource>(10000);

        public QueryArgsBase()
        {
            SqlTimes = new List<DebugSqlTime>();
            Logs = new List<DebugLog>();
        }
        public QueryArgsBase(QueryArgsBase inArgs)
        {
            CancellationTokenSource = inArgs.CancellationTokenSource; //必须放在前面

            UseNow = inArgs.UseNow;
            UseToday = inArgs.UseToday;

            BatchNum = inArgs.BatchNum;

            UseSqlLog = inArgs.UseSqlLog;
            SqlQueryTimeout = inArgs.SqlQueryTimeout;

            UseDebugLog = inArgs.UseDebugLog;
            UseErrorLog = inArgs.UseErrorLog;
            UseFatalLog = inArgs.UseFatalLog;
            UseInfoLog = inArgs.UseInfoLog;
            UseWarnLog = inArgs.UseWarnLog;

            SqlTimes = inArgs.SqlTimes;
            Logs = inArgs.Logs;
        }
 

        internal HttpContext HttpContext;
        public HttpContext GetHttpContext() { return HttpContext; }
        public void SetHttpContext(HttpContext value) { HttpContext = value; }

        /// <summary>
        /// 使用调试模式
        /// </summary>
        public bool? UseDebuggingMode { get; set; }

        #region 日期 时间 
        /// <summary>
        /// 使用今天
        /// </summary>
        public DateTime? UseToday { set; get; }

        /// <summary>
        /// 使用当前时间
        /// </summary>
        public DateTime? UseNow { set; get; }

        #endregion

        /// <summary>
        /// 批序号
        /// </summary>
        public string BatchNum { get; set; }

        #region  日志

        #region SQL
        /// <summary>
        /// sql查询超时，毫秒，设置后，超过该值的sql语句才被记录
        /// </summary>
        public int? SqlQueryTimeout { get; set; }

        /// <summary>
        /// 使用sql 监视
        /// </summary>
        public bool? UseSqlLog { get; set; }

        #endregion

        #region 日志

        /// <summary>
        /// 
        /// </summary>
        public bool? UseDebugLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? UseInfoLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? UseWarnLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? UseErrorLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? UseFatalLog { get; set; }


        #endregion

        #endregion

        private CancellationTokenSource CancellationTokenSource;
        public CancellationTokenSource GetCancellationTokenSource()
        {
            if (CancellationTokenSource != null) {
                return CancellationTokenSource;
            }

            if (string.IsNullOrWhiteSpace(BatchNum)) {
                return null;
            }
            if (antiDupQueue.TryGetValue(BatchNum, out CancellationTokenSource)) {
                return CancellationTokenSource;
            }
            return null;
        }
        public void SetCancellationTokenSource(CancellationTokenSource value)
        {
            if (value != null) {
                CancellationTokenSource = value;
                if (string.IsNullOrWhiteSpace(BatchNum) == false) {
                    antiDupQueue.TryAdd(BatchNum, value);
                }
            }
        }


        /// <summary>
        /// sql执行时间
        /// </summary>
        public List<DebugSqlTime> SqlTimes { get; set; }

        /// <summary>
        /// log记录
        /// </summary>
        public List<DebugLog> Logs { get; set; }

        #region DateTime function

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <returns></returns>
        public DateTime GetNow(bool isRealNow = false)
        {
            if (UseDebuggingMode == false) { return DateTime.Now; }

            if (isRealNow) {
                return DateTime.Now;
            }
            if (UseNow != null && UseNow.HasValue) {
                return UseNow.Value;
            }
            if (UseToday != null && UseToday.HasValue) {
                var now = DateTime.Now;
                var date = UseToday.Value.Date;
                return new DateTime(date.Year, date.Month, date.Day, now.Hour, now.Minute, now.Second);
            }
            return DateTime.Now;
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <returns></returns>
        public DateTime GetUtcNow(bool isRealNow = false)
        {
            if (UseDebuggingMode == false) { return DateTime.UtcNow; }

            if (isRealNow) {
                return DateTime.UtcNow;
            }
            if (UseNow != null && UseNow.HasValue) {
                var date = UseToday.Value.Date;
                return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
            }
            if (UseToday != null && UseToday.HasValue) {
                var now = DateTime.Now;
                var date = UseToday.Value.Date;
                return new DateTime(date.Year, date.Month, date.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);
            }
            return DateTime.UtcNow;
        }

        /// <summary>
        /// 获取真实的Now
        /// </summary>
        /// <returns></returns>
        public DateTime GetRealNow()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 获取真实的UtcNow
        /// </summary>
        /// <returns></returns>
        public DateTime GetRealUtcNow()
        {
            return DateTime.UtcNow;
        }


        /// <summary>
        /// 获取当前时间的时间戳
        /// </summary>
        /// <param name="isRealNow">是否取真实的电脑时间</param>
        /// <param name="isUtcNow">是否为UtcNow</param>
        /// <returns></returns>
        public long GetTimestamp(bool isRealNow = false, bool isUtcNow = false)
        {
            var now = isUtcNow ? GetUtcNow(isRealNow) : GetNow(isRealNow);
            return (long)(now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local)).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转为时间
        /// </summary>
        /// <param name="totalSeconds">秒</param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public DateTime ToDateTime(long totalSeconds, DateTimeKind kind = DateTimeKind.Local)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, kind);
            return start.AddSeconds(totalSeconds);
        }

        #endregion

        public void CopyTo(QueryArgsBase inArgs)
        {
            inArgs.CancellationTokenSource = CancellationTokenSource;//必须放在前面

            inArgs.UseNow = UseNow;
            inArgs.UseToday = UseToday;

            inArgs.BatchNum = BatchNum;

            inArgs.UseSqlLog = UseSqlLog;
            inArgs.SqlQueryTimeout = SqlQueryTimeout;

            inArgs.UseDebugLog = UseDebugLog;
            inArgs.UseErrorLog = UseErrorLog;
            inArgs.UseFatalLog = UseFatalLog;
            inArgs.UseInfoLog = UseInfoLog;
            inArgs.UseWarnLog = UseWarnLog;

            inArgs.SqlTimes = SqlTimes;
            inArgs.Logs = Logs;
        }

    }

}
