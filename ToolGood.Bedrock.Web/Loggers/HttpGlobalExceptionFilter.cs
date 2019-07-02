using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using ToolGood.Bedrock.Web.Constants;

namespace ToolGood.Bedrock.Web.Loggers
{
    /// <summary>
    /// HTTP全局异常处理
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                LogUtil.QueryArgs = context.HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgsBase;
            }
            LogUtil.Error(context.Exception);

            if (context.HttpContext.Request.Method.ToLower() != "get") {
                QueryResult result = new QueryResult() {
                    Code = CommonConstants.ErrorCode,
                    Message = "系统出了个小差！",
                    State = "ERROR",
                };

                context.Result = new ContentResult() {
                    ContentType = "application/json",
                    StatusCode = 503,
                    Content = result.ToJson()
                };
            }

        }
    }
}
