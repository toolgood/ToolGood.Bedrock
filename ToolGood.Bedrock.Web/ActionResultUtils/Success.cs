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
        /// <param name="extendData"></param>
        /// <returns></returns>
        public static IActionResult Success(object obj, bool usePassword = false)
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
            return CamelCaseJson(result);
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
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
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
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Success(string msg = "SUCCESS", bool usePassword = false)
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
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
        }

    }
}
