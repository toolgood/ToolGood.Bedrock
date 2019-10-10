using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ToolGood.Bedrock.Web.Middlewares
{
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;


        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var msg = "";
            if (request.Method == "POST") {
                if (request.ContentType.ToLower().Contains("form-data") == false || request.ContentType.ToLower().Contains("json") == false) {
                    using (var buffer = new MemoryStream()) {
                        request.EnableBuffering();
                        request.Body.Position = 0;
                        request.Body.CopyTo(buffer);
                        request.Body.Position = 0;
                        var bs = buffer.ToArray();
                        var post = Encoding.UTF8.GetString(bs);
                        msg += "Body:" + post + "\r\n";
                    }
                }
            }  
            if (context.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                LogUtil.QueryArgs = context.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgs;
            }
            LogUtil.Request(msg);
            await _next.Invoke(context);
        }
    }
    public static class RequestInfoLoggerExtensions
    {
        public static IApplicationBuilder UseRequestLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggerMiddleware>();
        }
    }
}
