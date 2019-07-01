using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace ToolGood.Bedrock.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ResponseLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            //跳过忽略日志的方法
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null) {
                var isskip = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                if (isskip) return;
                isskip = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                if (isskip) return;
            } else {
                var compiledPageActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (compiledPageActionDescriptor != null) {
                    var isskip = compiledPageActionDescriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true)
                                          .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                    if (isskip) return;
                    isskip = compiledPageActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                        .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                    if (isskip) return;
                }
            }




            if (context.HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                LogUtil.QueryArgs = context.HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgsBase;
            }
            if (context.Exception != null) {
                LogUtil.Error(context.Exception);
            } else {
                var result = context.Result;
                if (result is ContentResult) {
                    if (((ContentResult)result).ContentType.Contains("json")) {
                        var content = ((ContentResult)result).Content;
                        LogUtil.Response(content);
                    }
                } else if (result is EmptyResult) {
                    LogUtil.Response("[NULL]");
                } else if (result is JsonResult) {
                    var json = ((JsonResult)result).Value.ToJson();
                    LogUtil.Response(json);
                } else if (result is ObjectResult) {
                    var json = ((JsonResult)result).Value.ToJson();
                    LogUtil.Response(json);
                }

            }
            base.OnResultExecuted(context);
        }

    }
}
