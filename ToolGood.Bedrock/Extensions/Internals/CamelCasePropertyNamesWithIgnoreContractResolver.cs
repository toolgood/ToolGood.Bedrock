using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ToolGood.Bedrock.Attributes;
using ToolGood.ReadyGo3;

namespace ToolGood.Bedrock.Extensions.Internals
{
    public class CamelCasePropertyNamesWithIgnoreContractResolver : CamelCasePropertyNamesContractResolver
    {
        public List<string> PropertyNames;

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, memberSerialization);
            if (typeof(Page).IsAssignableFrom(type)) return list;
            if (type.GetCustomAttributes(typeof(JsonRequireAttribute), true).Length > 0) return list;

            if (PropertyNames != null) {
                JsonPropertyCollection jsonProperties = new JsonPropertyCollection(type);
                foreach (var item in list) {
                    if (PropertyNames.ContainsIgnoreCase(item.PropertyName) == false) {
                        jsonProperties.Add(item);
                    }
                }
                return jsonProperties;
            }
            return list;
        }

 


    }
}
