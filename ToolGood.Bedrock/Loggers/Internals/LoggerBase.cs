using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ToolGood.Bedrock.Attributes;

namespace ToolGood.Bedrock.Internals
{
    public abstract class LoggerBase : ILogger
    {
        public virtual bool UseDebug { get; set; } = false;
        public virtual bool UseInfo { get; set; } = true;
        public virtual bool UseWarn { get; set; } = false;
        public virtual bool UseError { get; set; } = true;
        public virtual bool UseFatal { get; set; } = true;
        public virtual bool UseSql { get; set; } = true;
        public virtual string Log_Prefix { get; set; } = "{yyyy}-{MM}-{dd} {HH}:{mm}:{ss}|{type}|{class}.{method} >>> \r\n";


        public virtual void WriteLog(LogType type, string msg)
        {
            if (type == LogType.Debug && UseDebug == false) return;
            if (type == LogType.Error && UseError == false) return;
            if (type == LogType.Fatal && UseFatal == false) return;
            if (type == LogType.Info && UseInfo == false) return;
            if (type == LogType.Sql && UseSql == false) return;
            if (type == LogType.Warn && UseWarn == false) return;

            var context = FormatLog(type.ToString(), msg, null);
            WriteLog(type.ToString(), context);
        }

        public virtual void WriteLog(QueryArgsBase queryArgs, LogType type, string msg)
        {
            if (queryArgs == null || queryArgs.UseDebuggingMode == null || queryArgs.UseDebuggingMode == false) {
                if (type == LogType.Debug && UseDebug == false) return;
                if (type == LogType.Error && UseError == false) return;
                if (type == LogType.Fatal && UseFatal == false) return;
                if (type == LogType.Info && UseInfo == false) return;
                if (type == LogType.Warn && UseWarn == false) return;
            } else {
                if (type == LogType.Debug && ((queryArgs.UseDebugLog == null && UseDebug == false) || queryArgs.UseDebugLog == false)) return;
                if (type == LogType.Error && ((queryArgs.UseErrorLog == null && UseError == false) || queryArgs.UseErrorLog == false)) return;
                if (type == LogType.Fatal && ((queryArgs.UseFatalLog == null && UseFatal == false) || queryArgs.UseFatalLog == false)) return;
                if (type == LogType.Info && ((queryArgs.UseInfoLog == null && UseInfo == false) || queryArgs.UseInfoLog == false)) return;
                if (type == LogType.Warn && ((queryArgs.UseWarnLog == null && UseWarn == false) || queryArgs.UseWarnLog == false)) return;
            }

            var context = FormatLog(type.ToString(), msg, queryArgs);
            WriteLog(type.ToString(), context);
            if (queryArgs != null && queryArgs.UseDebuggingMode != null && queryArgs.UseDebuggingMode == true) {
                WriteLog("DebuggingMode", context);
            }
        }

        public abstract void WriteLog(string type, string content);

        protected virtual string FormatLog(string type, string content, QueryArgsBase queryArgs)
        {
            var Time = DateTime.Now;
            var log = new StringBuilder(Log_Prefix);
            log.Replace("{time}", (Time.Ticks / 1000).ToString());
            log.Replace("{yyyy}", Time.Year.ToString());
            log.Replace("{yy}", (Time.Year % 100).ToString("D2"));
            log.Replace("{MM}", Time.Month.ToString("D2"));
            log.Replace("{dd}", Time.Day.ToString("D2"));
            log.Replace("{HH}", Time.Hour.ToString("D2"));
            log.Replace("{hh}", Time.Hour.ToString("D2"));
            log.Replace("{mm}", Time.Minute.ToString("D2"));
            log.Replace("{ss}", Time.Second.ToString("D2"));
            log.Replace("{type}", type);

            if (Log_Prefix.Contains("{class}") || Log_Prefix.Contains("{method}")) {
                var mi = GetMethodInfo();
                log.Replace("{class}", mi.DeclaringType.FullName);
                log.Replace("{method}", mi.Name);
            }
            log.Replace("{threadId}", Thread.CurrentThread.ManagedThreadId.ToString());

            if (queryArgs != null) {
                if (string.IsNullOrEmpty(queryArgs.BatchNum) == false) {
                    log.Insert(0, queryArgs.BatchNum + "|");
                }
                if (queryArgs.HttpContext != null) {
                    var ip = GetRealIP(queryArgs.HttpContext);
                    var request = queryArgs.HttpContext.Request;
                    log.Append($"{ip}|{request.Method}|{request.Scheme}://{request.Host}{request.Path}{request.QueryString}\r\n");
                    if (request.Method == "POST") {
                        if (request.ContentType.ToLower().StartsWith("multipart/form-data;") == false || request.ContentType.ToLower().Contains("json") == false) {
                            using (var buffer = new MemoryStream()) {
                                //request.EnableRewind();
                                request.Body.Position = 0;
                                request.Body.CopyTo(buffer);
                                request.Body.Position = 0;
                                var bs = buffer.ToArray();
                                var post = Encoding.UTF8.GetString(bs);
                                log.Append("Body:" + post + "\r\n");
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(content)==false) {
                log.Append(content);
            }
            log.Append("\r\n\r\n");
            return log.ToString();

        }


        /// <summary>
        /// 获取真ip
        /// </summary>
        /// <returns></returns>
        protected string GetRealIP(HttpContext context)
        {
            string result = String.Empty;
            result = context.Request.Headers["HTTP_X_FORWARDED_FOR"];
            //可能有代理 
            if (!string.IsNullOrWhiteSpace(result)) {
                //没有"." 肯定是非IP格式
                if (result.IndexOf(".") == -1) {
                    result = null;
                } else {
                    //有","，估计多个代理。取第一个不是内网的IP。
                    if (result.IndexOf(",") != -1) {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

                        string[] temparyip = result.Split(",;".ToCharArray());

                        if (temparyip != null && temparyip.Length > 0) {
                            for (int i = 0; i < temparyip.Length; i++) {
                                //找到不是内网的地址
                                if (IsIPAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.") {
                                    return temparyip[i];
                                }
                            }
                        }
                    }
                    //代理即是IP格式
                    else if (IsIPAddress(result)) {
                        return result;
                    }
                    //代理中的内容非IP
                    else {
                        result = null;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result)) {
                result = context.Request.Headers["REMOTE_ADDR"];
            }

            if (string.IsNullOrWhiteSpace(result)) {
                result = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return result;
        }
        private bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;

            string regformat = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str);
        }

        protected MethodBase GetMethodInfo()
        {
            var st = new StackTrace();
            int index = 1;
            var mi = st.GetFrame(index++).GetMethod();
            var classType = mi.DeclaringType;
            while (MatchType(classType, mi)) {
                mi = st.GetFrame(index++).GetMethod();
                classType = mi.DeclaringType;
            }
            return mi;
        }
        private bool MatchType(Type type, MethodBase mi)
        {
            if (type == typeof(TextLogger) || type == typeof(AssertUtil) || type == typeof(LogUtil)) {
                return true;
            }
            var ts = type.GetInterfaces();
            foreach (var item in ts) {
                if (item == typeof(ILogger)) {
                    return true;
                }
            }
            return mi.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(IgnoreLogAttribute)));
        }


    }


}
