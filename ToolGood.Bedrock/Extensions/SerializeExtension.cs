using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using ToolGood.Bedrock.Extensions.Internals;
using ToolGood.Bedrock.Internals;

namespace System
{
    public static partial class ObjectExtension
    {
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, params string[] ignoreNames) where T : class 
        {
            return ToJson(obj, true, false, false, (IEnumerable<string>)ignoreNames);
        }
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, IEnumerable<string> ignoreNames) where T : class
        {
            return ToJson(obj, true, false, false, (IEnumerable<string>)ignoreNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, params string[] ignoreNames) where T : class
        {
            return ToJson(obj, camelCase, false, false, (IEnumerable<string>)ignoreNames);
        }
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, IEnumerable<string> ignoreNames) where T : class
        {
            return ToJson(obj, camelCase, false, false, (IEnumerable<string>)ignoreNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, bool indented, params string[] ignoreNames) where T : class
        {
            return ToJson(obj, camelCase, indented, false, (IEnumerable<string>)ignoreNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, bool indented, IEnumerable<string> ignoreNames) where T : class
        {
            return ToJson(obj, camelCase, indented, false, (IEnumerable<string>)ignoreNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, bool indented, bool ignoreNull, params string[] ignoreNames) where T : class
        {
            return ToJson(obj, camelCase, indented, ignoreNull, (IEnumerable<string>)ignoreNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="ignoreNames"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase, bool indented, bool ignoreNull, IEnumerable<string> ignoreNames) where T : class
        {
            if (object.Equals(null, obj)) { return ""; }

            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (ignoreNames != null && ignoreNames.Count() > 0) {
                if (camelCase) {
                    var ContractResolver = new CamelCasePropertyNamesWithIgnoreContractResolver();
                    ContractResolver.PropertyNames = ignoreNames?.ToList();
                    settings.ContractResolver = ContractResolver;
                } else {
                    var ContractResolver = new PropertyNamesWithIgnoreContractResolver();
                    ContractResolver.PropertyNames = ignoreNames?.ToList();
                    settings.ContractResolver = ContractResolver;
                }
            } else if (camelCase) {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            if (indented) {
                settings.Formatting = Formatting.Indented;
            }
            if (ignoreNull) {
                settings.NullValueHandling = NullValueHandling.Ignore;
            }
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            settings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
            settings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) where T : class
        {
            if (object.Equals(null, json)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(json);
        }

    }
}
