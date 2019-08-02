using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Bedrock.Web.Constants;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.Web
{
    public static partial class ActionResultUtil
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success(object obj, bool usePassword = false)
        {
            return Success(obj, null, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ignoreNames"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success(object obj, IEnumerable<string> ignoreNames, bool usePassword = false)
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                Data = obj,
                State = "SUCCESS",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result, ignoreNames);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success<T>(List<T> objs, bool usePassword = false)
        {
            return Success(objs, null, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="ignoreNames"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success<T>(List<T> objs, IEnumerable<string> ignoreNames, bool usePassword = false)
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
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password, ignoreNames);
                }
            }
            return CamelCaseJson(result, ignoreNames);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success<T>(Page<T> page, bool usePassword = false)
        {
            return Success(page, null, usePassword);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="ignoreNames"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success<T>(Page<T> page, IEnumerable<string> ignoreNames, bool usePassword = false)
        {
            QueryResult result = new QueryResult() {
                Code = SuccessCode,
                Data = page,
                State = "SUCCESS",
            };
            if (QueryArgs != null) {
                if (QueryArgs.SqlTimes != null && QueryArgs.SqlTimes.Count > 0) {
                    result.SqlTimes = QueryArgs.SqlTimes;
                }
                if (QueryArgs.Logs != null && QueryArgs.Logs.Count > 0) {
                    result.Logs = QueryArgs.Logs;
                }
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password, ignoreNames);
                }
            }
            return CamelCaseJson(result, ignoreNames);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success(string msg = "SUCCESS", bool usePassword = false)
        {
            return Success(msg, null, usePassword);
        }
 

    }
}
