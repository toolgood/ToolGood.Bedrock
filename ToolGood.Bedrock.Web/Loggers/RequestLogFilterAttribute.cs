using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class RequestLogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
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
            LogUtil.Request(GetActionArguments(context.HttpContext, context.ActionArguments));
            base.OnActionExecuting(context);
        }
        private string GetActionArguments(HttpContext context, IDictionary<string, object> actionArguments)
        {
            var request = context.Request;
            StringBuilder actionParm = new StringBuilder();
            if (request.Method == "POST") {
                if (request.ContentType.ToLower().StartsWith("multipart/form-data;") == false || request.ContentType.ToLower().Contains("json") == false) {
                    using (var buffer = new MemoryStream()) {
                        //request.EnableRewind();
                        request.Body.Position = 0;
                        request.Body.CopyTo(buffer);
                        request.Body.Position = 0;
                        var bs = buffer.ToArray();
                        var post = Encoding.UTF8.GetString(bs);
                        actionParm.Append("Body:" + post + "\r\n");
                    }
                }
            }
            if (actionArguments.Count > 0) {
                actionParm.Append("Parms:\r\n");
                foreach (KeyValuePair<string, object> kv in actionArguments) {
                    actionParm.Append(kv.Key + ":" + kv.Value.ToJson() + "\r\n");
                }
            }
            actionParm.AppendLine();
            return actionParm.ToString();
        }

    }
}
