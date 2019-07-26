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
using ToolGood.Bedrock.Web.Constants;
using ToolGood.Bedrock.Web.Extensions;
using ToolGood.Bedrock.Web.Internals;
using ToolGood.Bedrock.Web.Mime;
using ToolGood.Bedrock.Web.ResumeFiles.ResumeFileResult;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.Web.Controllers.BaseCore
{
    public abstract class WebControllerBaseCore : Controller
    {
        protected int SuccessCode { get { return CommonConstants.SuccessCode; } }
        protected string SuccessStr { get { return CommonConstants.SuccessStr; } }
        protected int ErrorCode { get { return CommonConstants.ErrorCode; } }
        protected string ErrorStr { get { return CommonConstants.ErrorStr; } }


        protected QueryArgs QueryArgs { get; set; }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["SuccessCode"] = SuccessCode;
            ViewData["ErrorCode"] = ErrorCode;
            ViewData["SuccessStr"] = SuccessStr;
            ViewData["ErrorStr"] = ErrorStr;

            QueryArgs = HttpContextHelper.BuildQueryArgs(context.HttpContext, context.ActionArguments);
            ActionResultUtil.QueryArgs = QueryArgs;
            ViewData["QueryArgs"] = QueryArgs;

            base.OnActionExecuting(context);
        }


        #region Success
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="usePassword"></param>
        /// <param name="extendData"></param>
        /// <returns></returns>
        protected IActionResult Success(object obj, bool usePassword = false)
        {
            return ActionResultUtil.Success(obj, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Success<T>(List<T> objs, bool usePassword = false)
        {
            return ActionResultUtil.Success(objs, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Success<T>(Page<T> page, bool usePassword = false)
        {
            return ActionResultUtil.Success(page, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Success(string msg = "SUCCESS", bool usePassword = false)
        {
            return ActionResultUtil.Success(msg, usePassword);
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
            return ActionResultUtil.Error(ms);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Error(string msg = "ERROR", bool usePassword = false)
        {
            return ActionResultUtil.Error(msg, usePassword);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Error(int code, string msg, bool usePassword = false)
        {
            return ActionResultUtil.Error(code, msg, usePassword);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        protected IActionResult Error(object obj, bool usePassword = false)
        {
            return ActionResultUtil.Error(obj, usePassword);
        }

        #endregion

        #region Other CreatePassword GetUserAgent MapPath
        /// <summary>
        /// 获取 UserAgent
        /// </summary>
        /// <returns></returns>
        protected string GetUserAgent()
        {
            return HttpContext.GetUserAgent();
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
            return HttpContext.GetSessionId();
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void SetSession(string key, string val)
        {
            HttpContext.SetSession(key, val);
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected void SetSession(string key, object value)
        {
            HttpContext.SetSession(key, value);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string GetSession(string key)
        {
            return HttpContext.GetSession(key);
        }
        /// <summary>
        /// 判断session是否存在key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected bool HasSession(string key)
        {
            return HttpContext.HasSession(key);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T GetSession<T>(string key)
        {
            return HttpContext.GetSession<T>(key);
        }
        /// <summary>
        /// 依据key删除Session
        /// </summary>
        /// <param name="key"></param>
        protected void DeleteSession(string key)
        {
            HttpContext.DeleteSession(key);
        }

        /// <summary>
        /// 核对Session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        protected bool CheckSession(string key, string val)
        {
            return HttpContext.CheckSession(key, val);
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
            return HttpContext.GetCookie(key);
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        protected void SetCookie(string key, string val)
        {
            HttpContext.SetCookie(key, val);
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        protected void SetCookie(string key, string val, int minutes)
        {
            HttpContext.SetCookie(key, val, minutes);
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dateTime"></param>
        protected void SetCookie(string key, string val, DateTime dateTime)
        {
            HttpContext.SetCookie(key, val, dateTime);
        }
        /// <summary>
        /// 依据cookieName 删除 cookie
        /// </summary>
        /// <param name="cookieName"></param>
        protected void DeleteCookie(string cookieName)
        {
            HttpContext.DeleteCookie(cookieName);

        }
        /// <summary>
        /// 判断  Cookie是否包含cookieName 
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        protected bool HasCookie(string cookieName)
        {
            return HttpContext.HasCookie(cookieName);
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
            return ActionResultUtil.JumpTopUrl(url);
        }
        /// <summary>
        /// 跳转Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected IActionResult JumpUrl(string url)
        {
            return ActionResultUtil.JumpUrl(url);
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
