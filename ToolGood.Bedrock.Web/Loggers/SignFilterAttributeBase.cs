//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Newtonsoft.Json.Linq;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using ToolGood.Bedrock.Web.Constants;

//namespace ToolGood.Bedrock.Web
//{
//    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
//    public abstract class SignFilterAttributeBase : ActionFilterAttribute
//    {
//        protected abstract string GetSlat();
//        protected virtual string GetHashType()
//        {
//            return "md5";
//        }

//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
//            dict["slat"] = GetSlat();
//            string sign = "";
//            if (context.HttpContext.Request.Method == "GET") {
//                foreach (var item in context.HttpContext.Request.Query) {
//                    if (item.Key.ToLower() == "sign") {
//                        sign = item.Value.ToSafeString();
//                    } else {
//                        dict[item.Key] = item.Value.ToSafeString();
//                    }
//                }
//            } else if (context.HttpContext.Request.Method == "POST") {
//                if (context.HttpContext.Request.ContentType.Contains("json")) {
//                    var request = context.HttpContext.Request;
//                    using (var buffer = new MemoryStream()) {
//                        //request.EnableRewind();
//                        request.Body.Position = 0;
//                        request.Body.CopyTo(buffer);
//                        request.Body.Position = 0;
//                        var bs = buffer.ToArray();
//                        var post = Encoding.UTF8.GetString(bs);
//                        var json = JObject.Parse(post);
//                        foreach (var item in json) {
//                            if (item.Key.ToLower() == "sign") {
//                                sign = item.Value.ToSafeString();
//                            } else {
//                                dict[item.Key] = item.Value.ToSafeString();
//                            }
//                        }
//                    }
//                } else {
//                    foreach (var item in context.HttpContext.Request.Form) {
//                        if (item.Key.ToLower() == "sign") {
//                            sign = item.Value.ToSafeString();
//                        } else {
//                            dict[item.Key] = item.Value.ToSafeString();
//                        }
//                    }
//                }
//            }

//            StringBuilder stringBuilder = new StringBuilder();
//            foreach (var item in dict) {
//                stringBuilder.Append("&");
//                stringBuilder.Append(item.Key);
//                stringBuilder.Append("=");
//                stringBuilder.Append(item.Value);
//            }
//            if (stringBuilder.Length>0) {
//                stringBuilder.Remove(0, 1);
//            }

//            string hashType = GetHashType();
//            string hash = "";
//            if (hashType.ToLower() == "md5") {
//                hash = HashUtil.GetMd5String(stringBuilder.ToString());
//            } else if (hashType.ToLower() == "sha1") {
//                hash = HashUtil.GetSha1String(stringBuilder.ToString());
//            } else if (hashType.ToLower() == "sha256") {
//                hash = HashUtil.GetSha256String(stringBuilder.ToString());
//            } else if (hashType.ToLower() == "sha384") {
//                hash = HashUtil.GetSha384String(stringBuilder.ToString());
//            } else if (hashType.ToLower() == "sha512") {
//                hash = HashUtil.GetSha512String(stringBuilder.ToString());
//            } else {
//                hash = HashUtil.GetMd5String(stringBuilder.ToString());
//            }
//            if (hash.ToLower() != sign.ToLower()) {
//                if (context.HttpContext.Items.ContainsKey("ToolGood.Bedrock.QueryArgsBase")) {
//                    LogUtil.QueryArgs = context.HttpContext.Items["ToolGood.Bedrock.QueryArgsBase"] as QueryArgs;
//                }
//                LogUtil.Error("Sign is error.");
//                context.Result = Error("Sign is error.");
//            }
//        }
//        private IActionResult Error(string msg)
//        {
//            QueryResult result = new QueryResult() {
//                Code = CommonConstants.ErrorCode,
//                Message = msg,
//                State = "ERROR",
//            };
//            var json = result.ToJson();
//            return new ContentResult() {
//                Content = json,
//                ContentType = "application/json"
//            };
//        }
//    }
//}
