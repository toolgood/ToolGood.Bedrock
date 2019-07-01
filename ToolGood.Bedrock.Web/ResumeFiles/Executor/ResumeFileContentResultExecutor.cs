using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using ToolGood.Bedrock.Web.ResumeFiles.ResumeFileResult;

namespace ToolGood.Bedrock.Web.ResumeFiles.Executor
{
    /// <summary>
    /// 断点续传文件FileResult执行器
    /// </summary>
    internal class ResumeFileContentResultExecutor : FileContentResultExecutor, IActionResultExecutor<ResumeFileContentResult>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loggerFactory"></param>
        public ResumeFileContentResultExecutor(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        /// <summary>
        /// 执行Result
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public virtual Task ExecuteAsync(ActionContext context, ResumeFileContentResult result)
        {
            if (context == null) {
                throw new ArgumentNullException(nameof(context));
            }

            if (result == null) {
                throw new ArgumentNullException(nameof(result));
            }

            SetContentDispositionHeaderInline(context, result);
            return base.ExecuteAsync(context, result);
        }

        /// <summary>
        /// 设置响应头ContentDispositionHeader
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        private void SetContentDispositionHeaderInline(ActionContext context, IResumeFileResult result)
        {
            context.HttpContext.Response.Headers[HeaderNames.AccessControlExposeHeaders] = HeaderNames.ContentDisposition;
            if (string.IsNullOrEmpty(result.FileDownloadName)) {
                var contentDisposition = new ContentDispositionHeaderValue("inline");

                if (!string.IsNullOrWhiteSpace(result.FileInlineName)) {
                    contentDisposition.SetHttpFileName(result.FileInlineName);
                }

                context.HttpContext.Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
            }
        }

    }
}