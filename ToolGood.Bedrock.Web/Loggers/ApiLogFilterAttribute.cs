using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolGood.Bedrock.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ApiLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsShip(context)) { return; }

            if (context.HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
                LogUtil.QueryArgs = context.HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgsBase;
            }
            LogUtil.Request(GetActionArguments(context.HttpContext, context.ActionArguments));
            base.OnActionExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (IsShip(context)) { return; }

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
                } else if (result is FileResult) {
                    var file = ((FileResult)result).FileDownloadName;
                    LogUtil.Response("Download File:" + file);
                }
            }
            base.OnResultExecuted(context);
        }

        private bool IsShip(FilterContext context)
        {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null) {
                var isskip = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                if (isskip) return true;
                isskip = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                    .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                if (isskip) return true;
            } else {
                var compiledPageActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (compiledPageActionDescriptor != null) {
                    var isskip = compiledPageActionDescriptor.ControllerTypeInfo.GetCustomAttributes(inherit: true)
                                          .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                    if (isskip) return true;
                    isskip = compiledPageActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                        .Any(a => a.GetType().Equals(typeof(IgnoreLogFilterAttribute)));
                    if (isskip) return true;
                }
            }
            return false;
        }


        private string GetActionArguments(HttpContext context, IDictionary<string, object> actionArguments)
        {
            var request = context.Request;
            StringBuilder actionParm = new StringBuilder();
            if (actionArguments.Count > 0) {
                actionParm.Append("Parms:\r\n");
                foreach (KeyValuePair<string, object> kv in actionArguments) {
                    actionParm.Append(kv.Key + ":" + kv.Value.ToJson() + "\r\n");
                }
                actionParm.AppendLine();
            }
            return actionParm.ToString();
        }


    }
}
