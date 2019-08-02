using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ToolGood.Bedrock.Extensions.Internals
{
    public class CamelCasePropertyNamesWithIgnoreContractResolver : CamelCasePropertyNamesContractResolver
    {
        public List<string> PropertyNames;
 
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var list = base.CreateProperties(type, memberSerialization);
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
