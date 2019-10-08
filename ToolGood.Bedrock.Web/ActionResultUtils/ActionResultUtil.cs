using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Web.Constants;
using ToolGood.Bedrock;

namespace ToolGood.Bedrock.Web
{
    public static partial class ActionResultUtil
    {
        private static int SuccessCode { get { return CommonConstants.SuccessCode; } }
        private static string SuccessStr { get { return CommonConstants.SuccessStr; } }
        private static int ErrorCode { get { return CommonConstants.ErrorCode; } }
        private static string ErrorStr { get { return CommonConstants.ErrorStr; } }
        [ThreadStatic]
        internal static QueryArgs QueryArgs;


        /// <summary>
        /// 首字母小写json
        /// </summary>
        /// <param name="data">原数据</param>
        /// <param name="ignoreNames">忽略的字段名</param>
        /// <returns></returns>
        private static IActionResult CamelCaseJson(object data, IEnumerable<string> ignoreNames = null)
        {
            var json = data.ToJson(ignoreNames);
            return new ContentResult() {
                Content = json,
                StatusCode = 200,
                ContentType = "application/json"
            };
        }

    }
}
