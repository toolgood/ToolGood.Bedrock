using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Bedrock.Web
{
    public static partial class ActionResultUtil
    {

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static IActionResult Error(ModelStateDictionary ms)
        {
            List<string> sb = new List<string>();
            //获取所有错误的Key
            List<string> Keys = ms.Keys.ToList();
            //获取每一个key对应的ModelStateDictionary
            foreach (var key in Keys) {
                var errors = ms[key].Errors.ToList();
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
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Error(string msg = "ERROR", bool usePassword = false)
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
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Error(int code, string msg, bool usePassword = false)
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
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
        }
        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="usePassword"></param>
        /// <returns></returns>
        public static IActionResult Error(object obj, bool usePassword = false)
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
                if (usePassword && QueryArgs is EncryptedQueryArgs args) {
                    result.EncryptData(args.Password);
                }
            }
            return CamelCaseJson(result);
        }

    }
}
