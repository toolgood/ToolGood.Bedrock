using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Web
{
    public static partial class ActionResultUtil
    {
        /// <summary>
        /// 跳转Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IActionResult JumpTopUrl(string url)
        {
            var content = new ContentResult();
            content.Content = $"<script>top.location.href='{url}'</script>";
            content.ContentType = "text/html";
            return content;
        }
        /// <summary>
        /// 跳转Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IActionResult JumpUrl(string url)
        {
            var content = new ContentResult();
            content.Content = $"<script>location.href='{url}'</script>";
            content.ContentType = "text/html";
            return content;
        }
    }
}
