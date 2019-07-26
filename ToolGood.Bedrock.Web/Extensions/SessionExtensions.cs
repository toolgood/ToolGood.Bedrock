using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock.Web.Extensions
{
    public static partial class HttpContextExtensions
    {
 
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void SetSession(this HttpContext HttpContext,string key, string val)
        {
            HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(val));
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession(this HttpContext HttpContext, string key, object value)
        {
            HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSession(this HttpContext HttpContext, string key)
        {
            return HttpContext.Session.GetString(key);
        }
        /// <summary>
        /// 判断session是否存在key
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasSession(this HttpContext HttpContext, string key)
        {
            return HttpContext.Session.Keys.Any(q => q == key);
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSession<T>(this HttpContext HttpContext, string key)
        {
            var value = HttpContext.Session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 依据key删除Session
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        public static void DeleteSession(this HttpContext HttpContext, string key)
        {
            HttpContext.Session.Remove(key);
        }

        /// <summary>
        /// 核对Session
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool CheckSession(this HttpContext HttpContext, string key, string val)
        {
            var sessionCode = HttpContext.Session.GetString(key);
            if (string.IsNullOrEmpty(sessionCode) || sessionCode != val) {
                return false;
            }
            return true;
        }
    }
}
