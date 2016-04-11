using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace CEF.Lib.Helper
{
    public static class JStringFrtHelper
    {
        private static readonly Regex RToken = new Regex(@"(?<!{)({([^}]+)})", RegexOptions.Compiled);
        public static string Format(string format, JObject json)
        {
            format = RToken.Replace(format, "{0:$2}");
            return string.Format(new JFormatProvider(), format, json);
        }

        public static string Format(string format, object json)
        {
            return Format(format, JObject.FromObject(json));
        }

        public static void Merge(JObject target, JObject bullet)
        {
            foreach (var tar in target)
            {
                var curKey = tar.Key;
                if (bullet[curKey] != null && bullet[tar.Key].Type == target[curKey].Type)
                {
                    if (bullet[curKey].Type == JTokenType.Object)
                    {
                        Merge((JObject)target[curKey], (JObject)bullet[curKey]);
                    }
                    else if (target[curKey].Type == JTokenType.Array)
                    {
                        Merge((JArray)target[curKey], (JArray)bullet[curKey]);
                    }
                    target[tar.Key] = bullet[tar.Key];
                }
            }
        }

        private static void Merge(JArray target, JArray bullet)
        {
            var len = target.Count;
            for (var i = 0; i < len; i++)
            {
                if (bullet.Count <= i) continue;
                var tar = target[i];
                var bul = bullet[i];
                if (tar.Type == bul.Type)
                {
                    if (tar.Type == JTokenType.Object)
                    {
                        Merge((JObject)tar, (JObject)bul);
                    }
                    else if (tar.Type == JTokenType.Array)
                    {
                        Merge((JArray)tar, (JArray)bul);
                    }
                    target[i] = bullet[i];
                }
            }
        }

        private class JFormatProvider : IFormatProvider
        {
            #region IFormatProvider Members

            public object GetFormat(Type formatType)
            {
                return new JFormatter();
            }
            #endregion
        }
        private class JFormatter : ICustomFormatter
        {
            #region ICustomFormatter Members
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg.GetType() != typeof(JObject))
                {
                    throw new ArgumentException(
                        string.Format("Argument type \"{0}\" is not assignable to parameter type \"{1}\"",
                            arg.GetType().FullName, typeof(JObject).FullName));
                }
                try
                {
                    var value = ((JObject)arg).SelectToken(format);
                    return value == null ? string.Empty : value.ToString();
                }
                catch
                {
                    return string.Empty;
                }
            }
            #endregion
        }
    }
}
