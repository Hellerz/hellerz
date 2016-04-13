using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CEF.Lib
{
    public static class Common
    {
        public readonly static IsoDateTimeConverter TimeFormat = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss fffffff" };
        
        /// <summary>
        /// 根据Key获取字典的值，没有的话返回【值类型】的默认值
        /// </summary>
        /// <typeparam name="TKey">Key的类型</typeparam>
        /// <typeparam name="TValue">Value的类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return default(TValue);
        }
        /// <summary>
        /// 根据Key获取字典的值，没有的话返回【值类型】的默认值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            return defaultValue;
        }

        public static Dictionary<TKey, TElement> ToDiffDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, bool cover = false)
        {
            if (source == null) throw new ArgumentNullException("source");
            return source.ToDiffDictionary(keySelector, elementSelector, null, cover);
        }

        public static Dictionary<TKey, TElement> ToDiffDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer,bool cover = false)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (elementSelector == null) throw new ArgumentNullException("elementSelector");
            var d = new Dictionary<TKey, TElement>(comparer);
            foreach (var element in source)
            {
                var key = keySelector(element);
                var value = elementSelector(element);
                if (d.ContainsKey(key))
                {
                    if(cover)
                    {
                        d[key] = value;
                    }
                }
                else
                {
                    d.Add(key, value);
                }
            }
            return d;
        }

        private readonly static Regex ReJson = new Regex(@"^\s*\{");
        private readonly static Regex ReXml = new Regex(@"^\s*\<");
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

        private static string FormatJson(string unformattedJson)
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
        private static string Anti_FormatJson(string unformattedJson)
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

        private static string FormatXml(string unformattedXml)
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

        private static string Anti_FormatXml(string unformattedXml)
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

        /// <summary>
        /// 将字符串转为T类型，转换失败返回默认
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defalut"></param>
        /// <returns></returns>
        public static T ConvertToStruct<T>(string value, T defalut = default(T)) where T : struct, IConvertible
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    var ob = Convert.ChangeType(value, typeof(T));
                    if (ob is T)
                    {
                        return (T)ob;
                    }

                }
                catch
                {
                    return defalut;
                }
            }
            return defalut;
        }

        /// <summary>
        /// 校验集合不为空且Count > count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool CanBeCount<T>(this ICollection<T> collection, int count = 1)
        {
            return collection != null && collection.Count >= count;
        }



        /// <summary>
        /// 校验集合不为空且Count > count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="minCount"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static bool CanBeCount<T>(this ICollection<T> collection, int minCount, int maxCount)
        {
            return collection != null && collection.Count >= minCount && collection.Count <= maxCount;
        }

        public static bool IsBetweenDateTime(this DateTime dateTime, DateTime leftDateTime, DateTime rightDateTime)
        {
            return dateTime > leftDateTime && dateTime < rightDateTime;
        }

        public static bool IsSameDay(this DateTime dateTime, DateTime compareTime)
        {
            return dateTime.Year == compareTime.Year &&
                dateTime.Month == compareTime.Month &&
                dateTime.Day == compareTime.Day;
        }


        /// <summary>
        /// Performs the specified action on each not null element of the <see cref="T:System.Collections.Generic.IEnumerable`1"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void ForEachOfUnNone<T>(this IEnumerable<T> iterator, Action<T> action)
        {
            if (iterator == null) return;
            foreach (var item in iterator)
            {
                if (null == item)
                {
                    continue;
                }
                action(item);
            }
        }

        public static List<Exception> Flatten(Exception exception)
        {
            var list = new List<Exception>();
            if (exception != null)
            {
                var baseExc = exception.GetBaseException();
                while (exception != null)
                {
                    list.Add(exception);
                    exception = exception.InnerException;
                    if (exception == baseExc)
                    {
                        list.Add(exception);
                        break;
                    }
                }
                return list;
            }
            return null;
        }

    }
}
