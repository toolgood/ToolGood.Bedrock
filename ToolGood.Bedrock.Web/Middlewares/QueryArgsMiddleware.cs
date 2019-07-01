using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ToolGood.Bedrock.Web.Middlewares
{
    public class QueryArgsMiddleware
    {
        private readonly RequestDelegate _next;
        public QueryArgsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            QueryArgsBase queryArgs = new QueryArgsBase();
            if (context.Request.Headers["Content-Type"].ToString().Contains("json") == false) {
                queryArgs.UseDebuggingMode = GetBool(context, "UseDebuggingMode");
                queryArgs.UseDebugLog = GetBool(context, "UseDebugLog");
                queryArgs.UseErrorLog = GetBool(context, "UseErrorLog");
                queryArgs.UseFatalLog = GetBool(context, "UseFatalLog");
                queryArgs.UseInfoLog = GetBool(context, "UseInfoLog");
                queryArgs.UseSqlLog = GetBool(context, "UseSqlLog");
                queryArgs.UseWarnLog = GetBool(context, "UseWarnLog");
                queryArgs.UseToday = GetDateTime(context, "UseToday");
                queryArgs.UseNow = GetDateTime(context, "UseNow");
                queryArgs.BatchNum = GetString(context, "BatchNum");
                queryArgs.SqlQueryTimeout = GetInt(context, "SqlQueryTimeout");
            } else {
                using (var buffer = new MemoryStream()) {
                    context.Request.Body.Position = 0;
                    context.Request.Body.CopyTo(buffer);
                    context.Request.Body.Position = 0;
                    var json = JObject.Parse(Encoding.UTF8.GetString(buffer.ToArray()));
                    queryArgs.UseDebuggingMode = GetBool(json, "UseDebuggingMode");
                    queryArgs.UseDebugLog = GetBool(json, "UseDebugLog");
                    queryArgs.UseErrorLog = GetBool(json, "UseErrorLog");
                    queryArgs.UseFatalLog = GetBool(json, "UseFatalLog");
                    queryArgs.UseInfoLog = GetBool(json, "UseInfoLog");
                    queryArgs.UseSqlLog = GetBool(json, "UseSqlLog");
                    queryArgs.UseWarnLog = GetBool(json, "UseWarnLog");
                    queryArgs.UseToday = GetDateTime(json, "UseToday");
                    queryArgs.UseNow = GetDateTime(json, "UseNow");
                    queryArgs.BatchNum = GetString(json, "BatchNum");
                    queryArgs.SqlQueryTimeout = GetInt(json, "SqlQueryTimeout");
                }
            }

            queryArgs.SetHttpContext(context);
            context.Items["ToolGood.Bedrock.QueryArgsBase"] = queryArgs;
            LogUtil.QueryArgs = queryArgs;
            await _next.Invoke(context);
        }

        private bool? GetBool(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeBool(false);
                }
            } else if (context.Request.Method == "POST") {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeBool(false);
                    }
                }
            }
            return null;
        }
        private DateTime? GetDateTime(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeDateTime(DateTime.Now);
                }
            } else if (context.Request.Method == "POST") {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeDateTime(DateTime.Now);
                    }
                }
            }
            return null;
        }
        private int? GetInt(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                var value = context.Request.Query[key].ToString();
                if (string.IsNullOrEmpty(value) == false) {
                    return value.ToSafeInt32(0);
                }
            } else if (context.Request.Method == "POST") {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    var value = context.Request.Form[key].ToString();
                    if (string.IsNullOrEmpty(value) == false) {
                        return value.ToSafeInt32(0);
                    }
                }
            }
            return null;
        }
        private string GetString(HttpContext context, string key)
        {
            if (context.Request.Query.ContainsKey(key)) {
                return context.Request.Query[key].ToString();
            } else if (context.Request.Method == "POST") {
                if (context.Request.Form != null && context.Request.Form.ContainsKey(key)) {
                    return context.Request.Form[key].ToString();
                }
            }
            return null;
        }

        private bool? GetBool(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeBool(false);
            }
            return null;
        }
        private DateTime? GetDateTime(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeDateTime(DateTime.Now);
            }
            return null;
        }
        private int? GetInt(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString().ToSafeInt32(0);
            }
            return null;
        }
        private string GetString(JObject context, string key)
        {
            if (context.ContainsKey(key)) {
                return context[key].ToString();
            }
            return null;
        }

    }

    public static class QueryArgsMiddlewareExtensions
    {
        public static IApplicationBuilder UseQueryArgs(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<QueryArgsMiddleware>();
        }
    }
}
