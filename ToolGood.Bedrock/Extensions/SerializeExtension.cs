using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ToolGood.Bedrock.Internals;

namespace System
{
    public static class SerializeExtension
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="camelCase"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool camelCase = true, bool indented = false)
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
        public static string ToJsonIgnoreNull<T>(this T obj, bool camelCase = true, bool indented = false)
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
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            if (object.Equals(null, json)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(json);
        }



    }
}
