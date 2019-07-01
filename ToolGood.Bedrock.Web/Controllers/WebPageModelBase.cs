using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToolGood.Bedrock.Attributes;
using ToolGood.Bedrock.Web.Controllers.BaseCore;

namespace ToolGood.Bedrock.Web
{
    public abstract class WebPageModelBase : PageModelBaseCore
    {
        [IgnoreLog]
        protected IActionResult Execute(Func<IActionResult> action)
        {
            try {
                var result = action();
                if (result is ContentResult) {
                    var content = ((ContentResult)result).Content;
                    LogUtil.Response(content);
                } else if (result is EmptyResult) {
                    LogUtil.Response("[NULL]");
                } else if (result is JsonResult) {
                    var json = ((JsonResult)result).Value.ToJson();
                    LogUtil.Response(json);
                    return Content(json, "application/json");
                } else if (result is ObjectResult) {
                    var json = ((ObjectResult)result).Value.ToJson();
                    LogUtil.Response(json);
                    return Content(json, "application/json");
                }
                return result;
            } catch (Exception ex) {
                LogUtil.Error(ex);
            }
            return Content("500 Server Error.");
        }

        [IgnoreLog]
        protected IActionResult Execute(Func<object> action)
        {
            try {
                var result = action();
                var json = result.ToJson();
                LogUtil.Response(json);
                return Content(json, "application/json");
            } catch (Exception ex) {
                LogUtil.Error(ex);
            }
            return Content("500 Server Error.");
        }

        [IgnoreLog]
        protected async Task<IActionResult> Execute(Func<Task<IActionResult>> action)
        {
            try {
                var result = await action();
                if (result is ContentResult) {
                    var content = ((ContentResult)result).Content;
                    LogUtil.Response(content);
                } else if (result is EmptyResult) {
                    LogUtil.Response("[NULL]");
                } else if (result is JsonResult) {
                    var json = ((JsonResult)result).Value.ToJson();
                    LogUtil.Response(json);
                    return Content(json, "application/json");
                } else if (result is ObjectResult) {
                    var json = ((ObjectResult)result).Value.ToJson();
                    LogUtil.Response(json);
                    return Content(json, "application/json");
                }
                return result;
            } catch (Exception ex) {
                LogUtil.Error(ex);
            }
            return Content("500 Server Error.");
        }

        [IgnoreLog]
        protected async Task<IActionResult> Execute(Func<Task<object>> action)
        {
            try {
                var result = await action();
                var json = result.ToJson();
                LogUtil.Response(json);
                return Content(json, "application/json");
            } catch (Exception ex) {
                LogUtil.Error(ex);
            }
            return Content("500 Server Error.");
        }

    }
}
