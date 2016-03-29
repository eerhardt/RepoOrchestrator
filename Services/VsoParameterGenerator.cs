using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RepoOrchestrator.Services
{
    public static class VsoParameterGenerator
    {
        public static string GetParameters(IDictionary<string, JToken> vsoParameters)
        {
            if (!vsoParameters.Any())
            {
                return null;
            }

            StringBuilder builder = new StringBuilder("{");
            bool firstTime = true;
            foreach (KeyValuePair<string, JToken> parameter in vsoParameters)
            {
                string value = GetValueString(parameter.Value);

                if (firstTime)
                {
                    firstTime = false;
                }
                else
                {
                    builder.Append(", ");
                }
                builder.Append($"'{parameter.Key}': '{value}'");
            }

            builder.Append("}");
            return builder.ToString();
        }

        private static string GetValueString(JToken value)
        {
            return GetValueStringCore(value).Replace("'", @"\'");
        }

        private static string GetValueStringCore(JToken value)
        {
            if (value.Type == JTokenType.Array)
            {
                JArray array = (JArray)value;
                return string.Join(" ", array.Values());
            }
            else
            {
                return value.ToString();
            }
        }
    }
}