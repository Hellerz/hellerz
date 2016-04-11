using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CEF.Lib.JavascriptObject
{
    public class DictionaryConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// 
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var map = value as Dictionary<string, string>;
            if (map!=null)
            {
                var header = new JObject();
                map.ForEachOfUnNone(pair =>
                {
                    header.Add(pair.Key, new JValue(pair.Value));
                });
                writer.WriteRawValue(header.ToString());
            }
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// 
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing property value of the JSON that is being converted.</param><param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dictianry = new Dictionary<string,string>();
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    var key = reader.Value.ToString();;
                    var value = string.Empty;
                    reader.Read();
                    if (reader.TokenType == JsonToken.String)
                    {
                        value = reader.Value.ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        if (!dictianry.ContainsKey(key))
                        {
                            dictianry.Add(key, value);
                        }
                        else
                        {
                            dictianry[key] = value;
                        }
                    }
                    reader.Read();
                }
            }
            return dictianry;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (Dictionary<string, string>);
        }
    }
}
