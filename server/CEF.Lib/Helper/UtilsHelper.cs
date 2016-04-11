using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using CEF.Lib.Attributes;
using Newtonsoft.Json;

namespace CEF.Lib.Helper
{
   public static class UtilsHelper
   {
       

       private readonly static Regex ReJson = new Regex(@"^\s*\{");
        private readonly static Regex ReXml = new Regex(@"^\s*\<");

       [JSchema]
        public static string FormatMessage(string unformatted)
        {
            if (!string.IsNullOrWhiteSpace(unformatted))
            {
                try
                {
                    if (ReJson.IsMatch(unformatted))
                    {
                        return FormatJson(unformatted);
                    }
                    if (ReXml.IsMatch(unformatted))
                    {
                        return FormatXml(unformatted);
                    }
                }
                catch
                {
                    return unformatted;
                }
            }
            return unformatted;
        }
       [JSchema]
        public static string Anti_FormatMessage(string formatted)
        {
            if (!string.IsNullOrWhiteSpace(formatted))
            {
                try
                {
                    if (ReJson.IsMatch(formatted))
                    {
                        return Anti_FormatJson(formatted);
                    }
                    if (ReXml.IsMatch(formatted))
                    {
                        return Anti_FormatXml(formatted);
                    }
                }
                catch
                {
                    return formatted;
                }
            }
            return formatted;
        }
        [JSchema]
       public static string FormatJson(string unformattedJson)
        {
            //格式化json字符串
            try
            {
                var serializer = new JsonSerializer();
                var obj = serializer.Deserialize(new JsonTextReader(new StringReader(unformattedJson)));
                if (obj != null)
                {
                    var textWriter = new StringWriter();
                    var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' ',
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {
                return unformattedJson;
            }
            return unformattedJson;

        }
        [JSchema]
        public static string Anti_FormatJson(string unformattedJson)
        {
            //格式化json字符串
            try
            {
                var serializer = new JsonSerializer();
                var obj = serializer.Deserialize(new JsonTextReader(new StringReader(unformattedJson)));
                if (obj != null)
                {
                    var textWriter = new StringWriter();
                    var jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Newtonsoft.Json.Formatting.None,
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
            }
            catch (Exception)
            {
                return unformattedJson;
            }
            return unformattedJson;

        }

        [JSchema]
        public static string FormatXml(string unformattedXml)
        {
            var xd = new XmlDocument();
            xd.LoadXml(unformattedXml);
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw)
                {
                    Formatting = System.Xml.Formatting.Indented,
                    Indentation = 1,
                    IndentChar = '\t'

                };
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                }
            }
            return sb.ToString();
        }

        [JSchema]
        public static string Anti_FormatXml(string unformattedXml)
        {
            var xd = new XmlDocument();
            xd.LoadXml(unformattedXml);
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw)
                {
                    Formatting = System.Xml.Formatting.None,
                };
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                }
            }
            return sb.ToString();
        }
   }
}
