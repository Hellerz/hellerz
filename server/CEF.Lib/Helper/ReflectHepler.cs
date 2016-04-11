using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CEF.Lib.Helper
{
    public class ReflectHepler
    {
        private static readonly Dictionary<string, Assembly> AssemblyMapping = new Dictionary<string, Assembly>();
        private static readonly Dictionary<string, MemberInfo> MemberInfoMapping = new Dictionary<string, MemberInfo>();

        private static readonly Regex ClassRegex = new Regex(@"(.+)\.([^\.]+)", RegexOptions.Compiled);
        private static readonly Regex MemberRegex = new Regex(@"(.+)\.([^\.]+)\.([^\.]+)", RegexOptions.Compiled);

        public static Assembly GetAssembly(string assemblyName)
        {
            if (!AssemblyMapping.ContainsKey(assemblyName))
            {
                var assembly = Assembly.Load(assemblyName);
                if (assembly == null) throw new ArgumentException(string.Format("Assembly '{0}' cannot find.", assemblyName));
                AssemblyMapping.Add(assemblyName, assembly);
            }
            return AssemblyMapping[assemblyName];
        }

        public static Type GetClass(string assemblyName, string klassName)
        {
            var assembly = GetAssembly(assemblyName);
            var type = assembly.DefinedTypes.FirstOrDefault(t => t.Name == klassName);
            if (type == null) throw new ArgumentException(string.Format("Class '{0}.{1}' cannot find.", assemblyName, klassName));
            return type;
        }
        public static Type GetClass(string klassPathName)
        {
            if (string.IsNullOrWhiteSpace(klassPathName)) throw new ArgumentException("类路径为空");
            var paths = ClassRegex.Match(klassPathName);
            if (!paths.Success) throw new ArgumentException("类路径格式不正确");
            return GetClass(paths.Groups[1].Value, paths.Groups[2].Value);
        }

        public static MemberInfo GetPublicMember(string memberPathName)
        {
            if (string.IsNullOrWhiteSpace(memberPathName)) throw new ArgumentException("成员路径为空");
            var paths = MemberRegex.Match(memberPathName);
            if (!paths.Success) throw new ArgumentException("成员路径格式不正确");
            return GetPublicMember(paths.Groups[1].Value, paths.Groups[2].Value, paths.Groups[3].Value);
        }

        public static MemberInfo GetPublicMember(string assemblyName, string klassName, string memberName)
        {
            var path = string.Format("{0}.{1}.{2}", assemblyName, klassName, memberName);
            if (MemberInfoMapping.ContainsKey(path))
            {
                return MemberInfoMapping[path];
            }
            var kalss = GetClass(assemblyName, klassName);
            var members = kalss.GetMember(memberName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            if (!members.Any() || members[0] == null)
            {
                throw new ArgumentException(string.Format("Member '{0}.{1}.{2}' cannot find.", assemblyName, klassName, memberName));
            }
            MemberInfoMapping.Add(path, members[0]);
            return members[0];
        }

        public static Type[] GetEventTypes(EventInfo @event)
        {
            return GetDelegateTypes(@event.AddMethod);
        }

        public static Type[] GetDelegateTypes(Delegate @delegate)
        {
            return GetDelegateTypes(@delegate.Method);
        }

        private static Type[] GetDelegateTypes(MethodInfo method)
        {
            var invokeMethod = method.GetParameters()[0].ParameterType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public|BindingFlags.Static);
            if (invokeMethod != null)
            {
                return invokeMethod.GetParameters().Select(arg => arg.ParameterType).ToArray();
            }
            return new Type[0];
        }

        public static Type[] GetMethodTypes(MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.CanBeCount())
            {
                return parameters.Select(arg => arg.ParameterType).ToArray();
            }
            return new Type[0];
        }

       

        public static bool IsStaticMember(MemberInfo member)
        {
            var method = member as MethodInfo;
            if (method != null)
            {
                return method.IsStatic;
            }
            var field = member as FieldInfo;
            if (field != null)
            {
                return field.IsStatic;
            }
            var property = member as PropertyInfo;
            if (property != null)
            {
                return property.GetSetMethod().IsStatic;
            }
            var @event = member as EventInfo;
            if (@event != null)
            {
                return @event.GetAddMethod().IsStatic;
            }
            throw new TypeInitializationException(string.Format("{0} cannot be reconglized.",member.GetType().Name),null);
        }

        public static object[] ToArguments(string parameters)
        {
            if (!string.IsNullOrWhiteSpace(parameters))
            {
                var args = new List<object>();
                var array = JArray.Parse(parameters);
                array.ForEachOfUnNone(arr =>
                {
                    if (arr.Type == JTokenType.Integer)
                    {
                        args.Add((int)(long)((JValue)arr).Value);
                    }
                    else
                    {
                        args.Add(((JValue)arr).Value);
                    }
                });
                return args.ToArray();
            }
            return null;
        }

        public static string ToParameters(IEnumerable<object> parameters)
        {
            return parameters == null ? null : JsonConvert.SerializeObject(parameters);
        }
    }
}
