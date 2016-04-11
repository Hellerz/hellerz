using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Calibur.Model;
using CEF.Lib;
using CEF.Lib.Attributes;
using CEF.Lib.Helper;
using Fiddler;

namespace Calibur.Business
{
   public  class JSchemaCore
    {
        public static JSchemaResponse GetJSchema(JSchemaRequest request)
        {
            var result =new JSchemaResponse
            {
                Assembly = request.Assembly,
                Class = request.Class
            };
            var type = ReflectHepler.GetClass(request.Assembly,request.Class);
            if (type != null)
            {
                var ctors = type.GetConstructors();
                ctors.ForEachOfUnNone(ctor =>
                {
                    if (ctor.GetCustomAttributes(typeof (JSchemaAttribute)).ToList().CanBeCount())
                    {
                        if (ctor.GetParameters().CanBeCount())
                        {
                            result.HasArgCtor = true;
                        }
                    }
                });
                result.MemberList = GetJsMapInfo(type);
            }
            return result;
        }

        private static List<JSMemberInfo> GetJsMapInfo(IReflect sender)
        {
            var jsMapInfoList = new List<JSMemberInfo>();
            var methods = sender.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod|BindingFlags.Static);
            methods.ForEachOfUnNone(method =>
            {
                var attributes = method.CustomAttributes.ToList();
                if (attributes.CanBeCount())
                {
                    var jsfunction = attributes.FirstOrDefault(attr => attr.AttributeType == typeof(JSchemaAttribute));
                    if (jsfunction != null)
                    {
                        var types = method.GetParameters().ToList().ConvertAll(arg => arg.ParameterType.Name);
                        var jsMapInfo = new JSMemberInfo
                        {
                            Member = method.Name,
                            Types = types,
                            IsStatic = method.IsStatic
                        };
                        jsMapInfoList.Add(jsMapInfo);
                    }
                }
            });
            return jsMapInfoList;
        }
    }
}
