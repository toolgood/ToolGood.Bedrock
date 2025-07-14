using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace ToolGood.Bedrock.Utils
{
    public class JsonUtil
    {
        public static string ToJson(object obj, bool camelCase = true, bool indented = false, bool ignoreNull = false)
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
            if (ignoreNull) {
                settings.NullValueHandling = NullValueHandling.Ignore;
            }
            settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            settings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
            settings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,
            return JsonConvert.SerializeObject(obj, settings);
        }
        public static T ToObject<T>(string json) where T : class
        {
            if (object.Equals(null, json)) { return default(T); }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static JObject ParseJObject(string json)
        {
            return JObject.Parse(json);
        }
        public static JArray ParseJArray(string json)
        {
            return JArray.Parse(json);
        }

        class JsonCustomDoubleConvert : CustomCreationConverter<double>
        {
            public override bool CanWrite { get { return true; } }
            public override double Create(Type objectType) { return 0.0; }
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { return reader.Value; }
            /// <summary>
            /// 重载序列化方法
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="serializer"></param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null) {
                    writer.WriteNull();
                } else {
                    var d = Math.Round((double)value, 10);
                    writer.WriteValue(d);
                }
            }
        }
        class JsonCustomDoubleNullConvert : CustomCreationConverter<double?>
        {
            public override bool CanWrite { get { return true; } }
            public override double? Create(Type objectType) { return null; }
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) { return reader.Value; }
            /// <summary>
            /// 重载序列化方法
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="serializer"></param>
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value == null || ((double?)value).HasValue == false) {
                    writer.WriteNull();
                } else {
                    var d = Math.Round(((double?)value).Value, 10);
                    writer.WriteValue(d);
                }
            }
        }
    }
}
