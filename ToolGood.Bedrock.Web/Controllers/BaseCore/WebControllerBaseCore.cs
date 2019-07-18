using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Bedrock.Web.Constants;
using ToolGood.Bedrock.Web.Internals;
using ToolGood.Bedrock.Web.Mime;
using ToolGood.Bedrock.Web.ResumeFiles.ResumeFileResult;
using ToolGood.Bedrock.Web.Theme;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.Web.Controllers.BaseCore
{
    public abstract class WebControllerBaseCore : Controller
    {
        protected int SuccessCode { get { return CommonConstants.SuccessCode; } }
        protected int ErrorCode { get { return CommonConstants.ErrorCode; } }
        protected QueryArgs QueryArgs { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["SuccessCode"] = SuccessCode;
            ViewData["ErrorCode"] = ErrorCode;

            QueryArgs = HttpContextHelper.BuildQueryArgs(context.HttpContext, context.ActionArguments);
            ViewData["QueryArgs"] = QueryArgs;
 
            base.OnActionExecuting(context);
        }


        /// <summary>
        /// 首字母小写json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected IActionResult CamelCaseJson(object data)
        {
            var json = data.ToJson();
            return Content(json, "application/json");
        }

        #region Success
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="extendData"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(object obj, object extendData = null)
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                Data = obj,
                ExtendData = extendData,
                State = "SUCCESS",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected virtual IActionResult Success<T>(List<T> objs)
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                Data = objs,
                State = "SUCCESS",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <returns></returns>
        protected virtual IActionResult Success<T>(Page<T> page)
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                Page = page,
                State = "SUCCESS",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual IActionResult Success(string msg = "SUCCESS")
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                State = "SUCCESS",
                Message = msg
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        #endregion

        #region Error
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        protected IActionResult Error(ModelStateDictionary ms)
        {
            List<string> sb = new List<string>();
            //获取所有错误的Key
            List<string> Keys = ModelState.Keys.ToList();
            //获取每一个key对应的ModelStateDictionary
            foreach (var key in Keys) {
                var errors = ModelState[key].Errors.ToList();
                //将错误描述添加到sb中
                foreach (var error in errors) {
                    sb.Add(error.ErrorMessage);
                }
            }
            QueryResult result = new QueryResult() {
                Code = ErrorCode,
                Message = string.Join(",", sb),
                State = "ERROR",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual IActionResult Error(string msg = "ERROR")
        {
            QueryResult result = new QueryResult() {
                Code = ErrorCode,
                Message = msg,
                State = "ERROR",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected virtual IActionResult Error(int code, string msg)
        {
            QueryResult result = new QueryResult() {
                Code = code,
                Message = msg,
                State = "ERROR",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual IActionResult Error(object obj)
        {
            QueryResult result = new QueryResult() {
                Code = ErrorCode,
                Data = obj,
                State = "ERROR",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
            }
            return CamelCaseJson(result);
        }

        #endregion

        #region Other CreatePassword GetUserAgent MapPath
        /// <summary>
        /// 获取 UserAgent
        /// </summary>
        /// <returns></returns>
        protected string GetUserAgent()
        {
            return HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();
        }

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        protected string MapPath(string path)
        {
            return MyHostingEnvironment.MapPath(path);
        }
        /// <summary>
        /// 获取文件绝对路径 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected string MapWebRootPath(string path)
        {
            return MyHostingEnvironment.MapWebRootPath(path);
        }
        #endregion

        #region Session
        /// <summary>
        /// 获取 SessionId
        /// </summary>
        /// <returns></returns>
        protected string GetSessionId()
        {
            return HttpContext.Session.Id;
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void SetSession(string key, string val)
        {
            HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(val));
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetSession(string key, object value)
        {
            HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetSession(string key)
        {
            return HttpContext.Session.GetString(key);
        }
        /// <summary>
        /// 判断session是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool HasSession(string key)
        {
            return HttpContext.Session.Keys.Any(q => q == key);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetSession<T>(string key)
        {
            var value = HttpContext.Session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 依据key删除Session
        /// </summary>
        /// <param name="key"></param>
        protected void DeleteSession(string key)
        {
            HttpContext.Session.Remove(key);
        }

        /// <summary>
        /// 核对Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected bool CheckSession(string key, string val)
        {
            var sessionCode = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(sessionCode) || sessionCode != val) {
                return false;
            }
            return true;
        }

        #endregion

        #region Cookie 操作
        /// <summary>
        /// 获取 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key];
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void SetCookie(string key, string val)
        {
            HttpContext.Response.Cookies.Append(key, val, new CookieOptions() {
                Path = "/",
                IsEssential = true,
                HttpOnly = true,
                //Secure = true, //非https会无效
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
            });
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        protected void SetCookie(string key, string val, int minutes)
        {
            HttpContext.Response.Cookies.Append(key, val, new Microsoft.AspNetCore.Http.CookieOptions() {
                Path = "/",
                Expires = DateTime.Now.AddMinutes(minutes),
                IsEssential = true,
                HttpOnly = true,
                //Secure = true, //非https会无效
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
            });
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dateTime"></param>
        protected void SetCookie(string key, string val, DateTime dateTime)
        {
            HttpContext.Response.Cookies.Append(key, val, new Microsoft.AspNetCore.Http.CookieOptions() {
                Path = "/",
                Expires = dateTime,
                IsEssential = true,
                HttpOnly = true,
                //Secure = true,  //非https会无效
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None,
            });
        }
        /// <summary>
        /// 依据cookieName 删除 cookie
        /// </summary>
        /// <param name="cookieName"></param>
        protected void DeleteCookie(string cookieName)
        {
            var val = Request.Cookies[cookieName];
            if (val != null) {
                SetCookie(cookieName, "", DateTime.Now.AddYears(-1));
            }
        }
        /// <summary>
        /// 判断  Cookie是否包含cookieName 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        protected bool HasCookie(string cookieName)
        {
            return Request.Cookies.ContainsKey(cookieName);
        }
        #endregion

        #region JumpUrl
        /// <summary>
        /// 跳转Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected IActionResult JumpTopUrl(string url)
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
        protected IActionResult JumpUrl(string url)
        {
            var content = new ContentResult();
            content.Content = $"<script>location.href='{url}'</script>";
            content.ContentType = "text/html";
            return content;
        }
        #endregion

        #region 获取真ip
        /// <summary>
        /// 获取真ip
        /// </summary>
        /// <returns></returns>
        protected string GetRealIP()
        {
            return HttpContext.GetRealIP();
        }
        #endregion


        #region 可断点续传和多线程下载的FileResult

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileContents">文件二进制流</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeFileContentResult ResumeFile(byte[] fileContents, string contentType, string fileDownloadName)
        {
            return ResumeFile(fileContents, contentType, fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileContents">文件二进制流</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeFileContentResult ResumeFile(byte[] fileContents, string fileDownloadName)
        {
            return ResumeFile(fileContents, new MimeMapper().GetMimeFromPath(fileDownloadName), fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileContents">文件二进制流</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <param name="etag">ETag</param>
        /// <returns></returns>
        protected ResumeFileContentResult ResumeFile(byte[] fileContents, string contentType, string fileDownloadName, string etag)
        {
            return new ResumeFileContentResult(fileContents, contentType, etag) {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileStream">文件二进制流</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeFileStreamResult ResumeFile(FileStream fileStream, string contentType, string fileDownloadName)
        {
            return ResumeFile(fileStream, contentType, fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileStream">文件二进制流</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeFileStreamResult ResumeFile(FileStream fileStream, string fileDownloadName)
        {
            return ResumeFile(fileStream, new MimeMapper().GetMimeFromPath(fileDownloadName), fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="fileStream">文件二进制流</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <param name="etag">ETag</param>
        /// <returns></returns>
        protected ResumeFileStreamResult ResumeFile(FileStream fileStream, string contentType, string fileDownloadName, string etag)
        {
            return new ResumeFileStreamResult(fileStream, contentType, etag) {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="virtualPath">服务端本地文件的虚拟路径</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeVirtualFileResult ResumeFile(string virtualPath, string contentType, string fileDownloadName)
        {
            return ResumeFile(virtualPath, contentType, fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="virtualPath">服务端本地文件的虚拟路径</param>
        /// <returns></returns>
        protected ResumeVirtualFileResult ResumeFile(string virtualPath)
        {
            var fileDownloadName = Path.GetFileName(virtualPath);
            return ResumeFile(virtualPath, new MimeMapper().GetMimeFromPath(virtualPath), fileDownloadName, null);
        }


        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="virtualPath">服务端本地文件的虚拟路径</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumeVirtualFileResult ResumeFile(string virtualPath, string fileDownloadName)
        {
            return ResumeFile(virtualPath, new MimeMapper().GetMimeFromPath(virtualPath), fileDownloadName, null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="virtualPath">服务端本地文件的虚拟路径</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <param name="etag">ETag</param>
        /// <returns></returns>
        protected ResumeVirtualFileResult ResumeFile(string virtualPath, string contentType, string fileDownloadName, string etag)
        {
            return new ResumeVirtualFileResult(virtualPath, contentType, etag) {
                FileDownloadName = fileDownloadName
            };
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="physicalPath">服务端本地文件的物理路径</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumePhysicalFileResult ResumePhysicalFile(string physicalPath, string contentType, string fileDownloadName)
        {
            return ResumePhysicalFile(physicalPath, contentType, fileDownloadName, etag: null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="physicalPath">服务端本地文件的物理路径</param>
        /// <returns></returns>
        protected ResumePhysicalFileResult ResumePhysicalFile(string physicalPath)
        {
            var fileDownloadName = Path.GetFileName(physicalPath);
            return ResumePhysicalFile(physicalPath, new MimeMapper().GetMimeFromPath(physicalPath), fileDownloadName, etag: null);
        }


        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="physicalPath">服务端本地文件的物理路径</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <returns></returns>
        protected ResumePhysicalFileResult ResumePhysicalFile(string physicalPath, string fileDownloadName)
        {
            return ResumePhysicalFile(physicalPath, new MimeMapper().GetMimeFromPath(physicalPath), fileDownloadName, etag: null);
        }

        /// <summary>
        /// 可断点续传和多线程下载的FileResult
        /// </summary>
        /// <param name="physicalPath">服务端本地文件的物理路径</param>
        /// <param name="contentType">Content-Type</param>
        /// <param name="fileDownloadName">下载的文件名</param>
        /// <param name="etag">ETag</param>
        /// <returns></returns>
        protected ResumePhysicalFileResult ResumePhysicalFile(string physicalPath, string contentType, string fileDownloadName, string etag)
        {
            return new ResumePhysicalFileResult(physicalPath, contentType, etag) {
                FileDownloadName = fileDownloadName
            };
        }
        #endregion

        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="themeName"></param>
        protected void SetThemeName(string themeName)
        {
            HttpContext.Request.HttpContext.Items[WebConstants.Theme] = themeName;
        }
    }
}
