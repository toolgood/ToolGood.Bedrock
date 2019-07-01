using Microsoft.AspNetCore.Mvc.Filters;

namespace ToolGood.Bedrock.Web.Loggers
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                LogUtil.QueryArgs = context.HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgsBase;
            }
            LogUtil.Error(context.Exception);
        }
    }
}
