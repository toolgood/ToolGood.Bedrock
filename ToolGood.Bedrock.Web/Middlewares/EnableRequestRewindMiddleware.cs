﻿//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using System.Threading.Tasks;

//namespace ToolGood.Bedrock.Web.Middlewares
//{
//    public class EnableRequestRewindMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public EnableRequestRewindMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context)
//        {
//            if (!context.Request.Body.CanSeek) { context.Request.EnableBuffering(); }
//            //context.Request.EnableBuffering();
//            await _next(context);
//        }
//    }

//    public static class EnableRequestRewindExtension
//    {
//        public static IApplicationBuilder UseEnableRequestRewind(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<EnableRequestRewindMiddleware>();
//        }
//    }
//}
