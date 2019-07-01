using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolGood.Bedrock.Attributes;
using ToolGood.Bedrock.Web.Controllers.BaseCore;

namespace ToolGood.Bedrock.Web
{
    public abstract class WebControllerBase : WebControllerBaseCore
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



        #region JumpUrl
        protected IActionResult JumpTopUrl(string url)
        {
            var content = new ContentResult();
            content.Content = $"<script>top.location.href='{url}'</script>";
            content.ContentType = "text/html";
            return content;
        }
        protected IActionResult JumpUrl(string url)
        {
            var content = new ContentResult();
            content.Content = $"<script>location.href='{url}'</script>";
            content.ContentType = "text/html";
            return content;
        }
        #endregion

        #region 断点下载

        /// <summary>
        /// 断点下载
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        protected async Task<long> RangeDownload(string fullpath)
        {
            long size, start, end, length, fp = 0;
            using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(fullpath))) {

                size = reader.BaseStream.Length;
                start = 0;
                end = size - 1;
                length = size;
                // Now that we've gotten so far without errors we send the accept range header
                /* At the moment we only support single ranges.
                 * Multiple ranges requires some more work to ensure it works correctly
                 * and comply with the spesifications: http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2
                 *
                 * Multirange support annouces itself with:
                 * header('Accept-Ranges: bytes');
                 *
                 * Multirange content must be sent with multipart/byteranges mediatype,
                 * (mediatype = mimetype)
                 * as well as a boundry header to indicate the various chunks of data.
                 */
                Response.Headers.Add("Accept-Ranges", "0-" + size);
                // header('Accept-Ranges: bytes');
                // multipart/byteranges
                // http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2

                if (!String.IsNullOrEmpty(Request.Headers["HTTP_RANGE"])) {
                    long anotherStart = start;
                    long anotherEnd = end;
                    string[] arr_split = Request.Headers["HTTP_RANGE"].FirstOrDefault().Split(new char[] { Convert.ToChar("=") });
                    string range = arr_split[1];

                    // Make sure the client hasn't sent us a multibyte range
                    if (range.IndexOf(",") > -1) {
                        // (?) Shoud this be issued here, or should the first
                        // range be used? Or should the header be ignored and
                        // we output the whole content?
                        Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        Response.StatusCode = 416;
                        await Response.WriteAsync("Requested Range Not Satisfiable");
                    }

                    // If the range starts with an '-' we start from the beginning
                    // If not, we forward the file pointer
                    // And make sure to get the end byte if spesified
                    if (range.StartsWith("-")) {
                        // The n-number of the last bytes is requested
                        anotherStart = size - Convert.ToInt64(range.Substring(1));
                    } else {
                        arr_split = range.Split(new char[] { Convert.ToChar("-") });
                        anotherStart = Convert.ToInt64(arr_split[0]);
                        long temp = 0;
                        anotherEnd = (arr_split.Length > 1 && Int64.TryParse(arr_split[1].ToString(), out temp)) ? Convert.ToInt64(arr_split[1]) : size;
                    }
                    /* Check the range and make sure it's treated according to the specs.
                     * http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
                     */
                    // End bytes can not be larger than $end.
                    anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                    // Validate the requested range and return an error if it's not correct.
                    if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size) {

                        Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        Response.StatusCode = 416;
                        await Response.WriteAsync("Requested Range Not Satisfiable");
                    }
                    start = anotherStart;
                    end = anotherEnd;

                    length = end - start + 1; // Calculate new content length
                    fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    Response.StatusCode = 206;
                }
            }
            // Notify the client the byte range we'll be outputting
            Response.Headers.Add("Content-Range", "bytes " + start + "-" + end + "/" + size);
            Response.Headers.Add("Content-Length", length.ToString());
            // Start buffered download
            await Response.SendFileAsync(fullpath, fp, length);
            return fp;
        }

        #endregion
    }
}
