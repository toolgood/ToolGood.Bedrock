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
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool camelCase = true, bool indented = false) where T : class
        {
            if (object.Equals(null, obj)) { return ""; }

            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (camelCase) {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented) {
                settings.Formatting = Formatting.Indented;
            }
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            settings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
            settings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,

            return JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 将对象转换为json格式的字符串(忽略null值)
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns>json格式的字符串</returns>
        public static string ToJsonIgnoreNull<T>(this T obj, bool camelCase = true, bool indented = false) where T : class
        {
            if (object.Equals(null, obj)) { return ""; }

            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (camelCase) {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented) {
                settings.Formatting = Formatting.Indented;
            }
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            settings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
            settings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,

            return JsonConvert.SerializeObject(obj, settings);
        }
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, params string[] propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, true, false, false, (IEnumerable<string>)propertyNames);
        }
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, IEnumerable<string> propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, true, false, false, (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, params string[] propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, camelCase, false, false, (IEnumerable<string>)propertyNames);
        }
        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, IEnumerable<string> propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, camelCase, false, false, (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, bool indented, params string[] propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, camelCase, indented, false, (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, bool indented, IEnumerable<string> propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, camelCase, indented, false, (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, bool indented, bool ignoreNull, params string[] propertyNames)
        {
            return ToJsonIgnorePropertyNames(obj, camelCase, indented, ignoreNull, (IEnumerable<string>)propertyNames);
        }

        /// <summary>
        /// 序列化 ,忽略指定的字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        public static string ToJsonIgnorePropertyNames<T>(this T obj, bool camelCase, bool indented, bool ignoreNull, IEnumerable<string> propertyNames)
        {
            if (object.Equals(null, obj)) { return ""; }

            var settings = new JsonSerializerSettings();
            settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            if (camelCase) {
                var ContractResolver = new CamelCasePropertyNamesWithIgnoreContractResolver();
                ContractResolver.PropertyNames = propertyNames?.ToList();
                settings.ContractResolver = ContractResolver;
            } else {
                var ContractResolver = new PropertyNamesWithIgnoreContractResolver();
                ContractResolver.PropertyNames = propertyNames?.ToList();
                settings.ContractResolver = ContractResolver;
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
