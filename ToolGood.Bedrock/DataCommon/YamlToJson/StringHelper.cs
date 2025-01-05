using Newtonsoft.Json;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToolGood.Bedrock.DataCommon.YamlToJson
{
    public class StringHelper
    {
        /// <summary>
        /// Converts the YAML string to JSON string.
        /// </summary>
        /// <param name="yaml">YAML string.</param>
        /// <param name="ignoreXYarm">Value indicating whether to ignore the top level properties is or starts with <c>x-yarm</c>, or not. Default value is <c>true</c>.</param>
        /// <returns>Returns JSON string converted.</returns>
        public static string ToJson(string yaml, bool ignoreXYarm = true)
        {
            if (string.IsNullOrWhiteSpace(yaml)) {
                return null;
            }
            yaml = yaml.Trim();
            yaml= Regex.Replace(yaml, @"^(\s*[a-zA-Z0-9_\u3400-\u9fff]+:) *", "$1 ", RegexOptions.Multiline);

            object deserialized;
            try {
                deserialized = new Serializer().Deserialize(yaml);
            } catch (Exception ex) {
                throw new InvalidYamlException(ex.Message);
            }

            var json = ignoreXYarm
                ? JsonConvert.SerializeObject(deserialized, Formatting.Indented, new IgnoreXYarmJsonConverter())
                : JsonConvert.SerializeObject(deserialized, Formatting.Indented);

            return json;
        }
    }
}
