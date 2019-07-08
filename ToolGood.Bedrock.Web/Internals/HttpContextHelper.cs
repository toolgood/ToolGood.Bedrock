using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ToolGood.Bedrock.Web.Internals
{
    internal class HttpContextHelper
    {
        public static QueryArgsBase BuildQueryArgs(HttpContext context, IDictionary<string, object> args)
        {
            foreach (var item in args) {
                var queryArgsBase = item.Value as QueryArgsBase;
                if (queryArgsBase != null) {
                    queryArgsBase.SetHttpContext(context);
                    context.Items["ToolGood.Bedrock.QueryArgsBase"] = queryArgsBase;
                    LogUtil.QueryArgs = queryArgsBase;
                    return queryArgsBase;
                 }
            }
            if (context.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                var queryArgsBase = context.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgsBase;
                LogUtil.QueryArgs = queryArgsBase;
                return queryArgsBase;
            }

            QueryArgsBase queryArgs = new QueryArgsBase();
            if (context.Request.Headers["Content-Type"].ToSafeString().Contains("json") == false) {
                queryArgs.UseLog = GetBool(context, "UseLog");
                queryArgs.UseToday = GetDateTime(context, "UseToday");
                queryArgs.UseNow = GetDateTime(context, "UseNow");
                queryArgs.BatchNum = GetString(context, "BatchNum");
            } else {
                using (var buffer = new MemoryStream()) {
                    context.Request.Body.Position = 0;
                    context.Request.Body.CopyTo(buffer);
                    context.Request.Body.Position = 0;
                    var json = JObject.Parse(Encoding.UTF8.GetString(buffer.ToArray()));
                    queryArgs.UseLog = GetBool(json, "UseLog");
                    queryArgs.UseToday = GetDateTime(json, "UseToday");
                    queryArgs.UseNow = GetDateTime(json, "UseNow");
                    queryArgs.BatchNum = GetString(json, "BatchNum");
                }
            }

            queryArgs.SetHttpContext(context);
            context.Items["ToolGood.Bedrock.QueryArgsBase"] = queryArgs;
            LogUtil.QueryArgs = queryArgs;

            return queryArgs;
        }

        private static bool? GetBool(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeBool(false);
                }
            } else if (context.Request.Method == "POST" && context.Request.HasFormContentType) {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeBool(false);
                    }
                }
            }
            return null;
        }
        private static DateTime? GetDateTime(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeDateTime(DateTime.Now);
                }
            } else if (context.Request.Method == "POST" && context.Request.HasFormContentType) {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeDateTime(DateTime.Now);
                    }
                }
            }
            return null;
        }
        private static int? GetInt(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeInt32(0);
                }
            } else if (context.Request.Method == "POST" && context.Request.HasFormContentType) {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeInt32(0);
                    }
                }
            }
            return null;
        }
        private static string GetString(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                return context.Request.Query[key].ToString();
            } else if (context.Request.Method == "POST" && context.Request.HasFormContentType) {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    return context.Request.Form[key].ToString();
                }
            }
            return null;
        }


        private static bool? GetBool(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeBool(false);
            }
            return null;
        }
        private static DateTime? GetDateTime(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeDateTime(DateTime.Now);
            }
            return null;
        }
        private static int? GetInt(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeInt32(0);
            }
            return null;
        }
        private static string GetString(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString();
            }
            return null;
        }
    }
}
