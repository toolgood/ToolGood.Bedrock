﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToolGood.Bedrock.Web.Extensions
{
    public static partial class HttpContextExtensions
    {

        /// <summary>
        /// 获取 Cookie
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(this HttpContext HttpContext, string key)
        {
            return HttpContext.Request.Cookies[key];
        }
        /// <summary>
        /// 设置 Cookie
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void SetCookie(this HttpContext HttpContext, string key, string val)
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
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="minutes"></param>
        public static void SetCookie(this HttpContext HttpContext, string key, string val, int minutes)
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
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="dateTime"></param>
        public static void SetCookie(this HttpContext HttpContext, string key, string val, DateTime dateTime)
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
        /// <param name="HttpContext"></param>
        /// <param name="cookieName"></param>
        public static void DeleteCookie(this HttpContext HttpContext, string cookieName)
        {
            var val = HttpContext.Request.Cookies[cookieName];
            if (val != null) {
                SetCookie(HttpContext, cookieName, "", DateTime.Now.AddYears(-1));
            }
        }
        /// <summary>
        /// 判断  Cookie是否包含cookieName 
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static bool HasCookie(this HttpContext HttpContext, string cookieName)
        {
            return HttpContext.Request.Cookies.ContainsKey(cookieName);
        }
    }
}
