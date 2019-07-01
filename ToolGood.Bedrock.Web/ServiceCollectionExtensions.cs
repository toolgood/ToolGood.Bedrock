using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using System.IO;
using ToolGood.Bedrock.Web.ResumeFiles.Executor;
using ToolGood.Bedrock.Web.ResumeFiles.ResumeFileResult;

namespace ToolGood.Bedrock.Web
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void UselMyLetsEncrypt(this IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Run(async (context) => {
                if (context.Request.Path.Value.ToLower().StartsWith("/.well-known/acme-challenge/")) {
                    var id = context.Request.Path.Value.Substring("/.well-known/acme-challenge/".Length);
                    var file = Path.Combine(env.ContentRootPath, ".well-known", "acme-challenge", id);
                    var text = await File.ReadAllTextAsync(file);
                    context.Response.StatusCode = 200;
                    context.Response.Headers[HeaderNames.ContentType] = "text/plain";
                    await context.Response.WriteAsync(text);
                    return;
                }
            });
        }
        
        /// <summary>
        /// 注入断点续传服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMyResumeFileResult(this IServiceCollection services)
        {
            services.TryAddSingleton<IActionResultExecutor<ResumePhysicalFileResult>, ResumePhysicalFileResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeVirtualFileResult>, ResumeVirtualFileResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeFileStreamResult>, ResumeFileStreamResultExecutor>();
            services.TryAddSingleton<IActionResultExecutor<ResumeFileContentResult>, ResumeFileContentResultExecutor>();
            return services;
        }

        /// <summary>
        /// 注入HttpContext静态对象，方便在任意地方获取HttpContext，services.AddHttpContextAccessor();
        /// </summary>
        /// <param name="services"></param>
        public static void AddMyStaticHttpContext(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }



 
    }
}
