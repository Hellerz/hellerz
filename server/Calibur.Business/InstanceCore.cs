using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calibur.Model;
using CEF.Lib.Helper;

namespace Calibur.Business
{
    public class InstanceCore
    {
        public static InstanceResponse Instance(InstanceRequest request)
        {
            object obj = null;
            var instanceParameters = ReflectHepler.ToParameters(request.InstanceParameters);
            var memberParameters = ReflectHepler.ToParameters(request.MemberParameters);
            switch (request.Operate)
            {
                case "InvokeMethod":
                    obj = InstanceHelper.InvokeMethod(request.MemberPath, instanceParameters, memberParameters);
                    break;
                case "GetProperty":
                    obj = InstanceHelper.GetProperty(request.MemberPath, instanceParameters, memberParameters);
                    break;
                case "SetProperty":
                    InstanceHelper.SetProperty(request.MemberPath, instanceParameters, memberParameters);
                    break;
                case "GetField":
                    obj = InstanceHelper.GetField(request.MemberPath, instanceParameters);
                    break;
                case "SetField":
                    InstanceHelper.SetField(request.MemberPath, instanceParameters, memberParameters);
                    break;
                case "AddEvent":
                    InstanceHelper.AddEvent(request.MemberPath, instanceParameters, request.EventId, request.PluginKey, request.JsCallback);
                    break;
                case "RemoveEvent":
                    InstanceHelper.RemoveEvent(request.MemberPath, instanceParameters, request.EventId);
                    break;
            }
            return new InstanceResponse { Result = obj };
        }
    }
}
