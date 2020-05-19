using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HtmlTest.Cores
{
    /// <summary>
    /// 生成Html静态文件
    /// </summary>
    public class HtmlStaticFileAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 静态文件保存路径, 默认放在 根目录\html 文件夹下
        /// </summary>
        public static string OutputFolder;
        /// <summary>
        /// 页面更新参数，用于更新页面,更新文件 如 https://localhost:44345/?__update__
        /// </summary>
        public static string UpdateFileQueryString = "__update__";
        /// <summary>
        /// 页面测试参数，测试页面，不更新文件 如  https://localhost:44345/?__test__
        /// </summary>
        public static string TestQueryString = "__test__";
        /// <summary>
        /// 支持Url参数
        /// </summary>
        public static bool UseQueryString = false;
        /// <summary>
        /// 页面缓存时间 1分钟
        /// </summary>
        public static int ExpireMinutes = 1;

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (IsTest(context))
            {
                await base.OnResultExecutionAsync(context, next);
                return;
            }

            var filePath = GetOutputFilePath(context);
            var response = context.HttpContext.Response;
            if (IsUpdateOutputFile(context) == false && File.Exists(filePath))
            {
                var fi = new FileInfo(filePath);
                var etag = fi.LastWriteTimeUtc.Ticks.ToString();
                if (context.HttpContext.Request.Headers["If-None-Match"] == etag)
                {
                    response.StatusCode = 304;
                    return;
                }
                context.HttpContext.Response.Headers["Cache-Control"] = "max-age=" + ExpireMinutes * 60;
                context.HttpContext.Response.Headers["Etag"] = etag;
                context.HttpContext.Response.Headers["Date"] = DateTime.Now.ToString("r");
                context.HttpContext.Response.Headers["Expires"] = DateTime.Now.AddMinutes(ExpireMinutes).ToString("r");

                response.ContentType = "text/html; charset=utf-8";
                var text = await File.ReadAllTextAsync(filePath);
                StreamWriter writer = new StreamWriter(response.Body);
                await writer.WriteAsync(text);
                return;
            }

            if (!response.Body.CanRead || !response.Body.CanSeek)
            {
                using (var ms = new MemoryStream())
                {
                    var old = response.Body;
                    response.Body = ms;

                    await base.OnResultExecutionAsync(context, next);

                    if (response.StatusCode == 200)
                    {
                        await SaveHtmlResult(response.Body, filePath);
                    }
                    ms.Position = 0;
                    await ms.CopyToAsync(old);
                    response.Body = old;
                }
            }
            else
            {
                await base.OnResultExecutionAsync(context, next);
                var old = response.Body.Position;
                if (response.StatusCode == 200)
                {
                    await SaveHtmlResult(response.Body, filePath);
                }
                response.Body.Position = old;
            }
            {
                var fi = new FileInfo(filePath);
                var etag = fi.LastWriteTimeUtc.Ticks.ToString();
                context.HttpContext.Response.Headers["Cache-Control"] = "max-age=" + ExpireMinutes * 60;
                context.HttpContext.Response.Headers["Etag"] = etag;
                context.HttpContext.Response.Headers["Date"] = DateTime.Now.ToString("r");
                context.HttpContext.Response.Headers["Expires"] = DateTime.Now.AddMinutes(ExpireMinutes).ToString("r");
            }
        }


        private static async Task SaveHtmlResult(Stream stream, string filePath)
        {
            stream.Position = 0;
            var responseReader = new StreamReader(stream);
            var responseContent = await responseReader.ReadToEndAsync();
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            await File.WriteAllTextAsync(filePath, responseContent);
        }


        private bool IsTest(ResultExecutingContext context)
        {
            return context.HttpContext.Request.Query.Keys.Contains(TestQueryString);
        }

        private bool IsUpdateOutputFile(ResultExecutingContext context)
        {
            return context.HttpContext.Request.Query.Keys.Contains(UpdateFileQueryString);
        }

        private string GetOutputFilePath(ResultExecutingContext context)
        {
            string dir = OutputFolder;
            if (string.IsNullOrEmpty(dir))
            {
                dir = Path.Combine(Path.GetDirectoryName(typeof(HtmlStaticFileAttribute).Assembly.Location), "html");
                OutputFolder = dir;
            }

            var t = context.HttpContext.Request.Path.ToString().Replace("//", Path.DirectorySeparatorChar.ToString()).Replace("/", Path.DirectorySeparatorChar.ToString());
            if (t.EndsWith(Path.DirectorySeparatorChar))
            {
                t += "index";
            }
            if (UseQueryString)
            {
                var list = new HashSet<string>();
                foreach (var item in context.HttpContext.Request.Query.Keys)
                {
                    if (item != UpdateFileQueryString)
                    {
                        var value = context.HttpContext.Request.Query[item];
                        if (string.IsNullOrEmpty(value))
                        {
                            list.Add($"{list}_");
                        }
                        else
                        {
                            list.Add($"{list}_{value}");
                        }
                    }
                }
                t += string.Join(",", list);
            }

            t = t.TrimStart(Path.DirectorySeparatorChar);
            return Path.Combine(dir, t) + ".html";
        }


    }
}
