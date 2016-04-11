using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CEF.Lib.JavascriptObject;

namespace CEF.Lib.Helper
{
    public static class InstanceHelper
    {
        private readonly static Dictionary<string, Delegate> EventHandlers = new Dictionary<string, Delegate>();

        public static object InvokeMethod(string methodPath, string instanceParameters, string methodParameters)
        {
            return _PreProcess<MethodInfo>(methodPath, instanceParameters, 
                (sender, instance) => sender.Invoke(instance, ReflectHepler.ToArguments(methodParameters))
            );
        }

        public static object InvokeMethod(MethodInfo methodInfo, object instance, string methodParameters)
        {
            return methodInfo.Invoke(instance, ReflectHepler.ToArguments(methodParameters));
        }

        public static object GetProperty(string propertyPath, string instanceParameters, string propertyParameters)
        {
            return _PreProcess<PropertyInfo>(propertyPath, instanceParameters,
                (sender, instanc) => sender.GetValue(instanc, ReflectHepler.ToArguments(propertyParameters))
            );
        }
        public static void SetProperty(string propertyPath, string instanceParameters, string propertyParameters)
        {
            _PreProcess<PropertyInfo>(propertyPath, instanceParameters,
                (sender, instanc) => sender.SetValue(instanc, ReflectHepler.ToArguments(propertyParameters))
            );
        }

        public static object GetField(string fieldPath, string instanceParameters)
        {
            return _PreProcess<FieldInfo>(fieldPath, instanceParameters,
                (sender, instanc) => sender.GetValue(instanc)
            );
        }
        public static void SetField(string fieldPath, string instanceParameters, string propertyParameters)
        {
            _PreProcess<FieldInfo>(fieldPath, instanceParameters,
                (sender, instanc) => sender.SetValue(instanc, ReflectHepler.ToArguments(propertyParameters).First())
            );
        }

        public static void AddEvent(string eventPath, string instanceParameters, string eventId, string pluginKey, string message)
        {
            if (!EventHandlers.ContainsKey(eventId))
            {
                _PreProcess<EventInfo>(eventPath, instanceParameters,(sender, instanc) =>
                {
                    if (!string.IsNullOrWhiteSpace(pluginKey))
                    {
                        var dm = new DynamicMethod(sender.Name + "_" + eventId, null, ReflectHepler.GetEventTypes(sender), sender.DeclaringType, true);
                        var il = dm.GetILGenerator();
                        il.Emit(OpCodes.Ldstr, pluginKey);
                        il.Emit(OpCodes.Ldstr, message);
                        il.Emit(OpCodes.Call, typeof(InstanceHelper).GetMethod("ExecuteScriptAsync"));
                        il.Emit(OpCodes.Ret);
                        var eventHandler = dm.CreateDelegate(sender.EventHandlerType, null);
                        EventHandlers.Add(eventId, eventHandler);
                        sender.AddEventHandler(instanc, eventHandler);
                    }
                });
            }
        }

        public static void RemoveEvent(string eventPath, string instanceParameters, string eventId)
        {
            if (EventHandlers.ContainsKey(eventId))
            {
                _PreProcess<EventInfo>(eventPath, instanceParameters, (sender, instanc) =>
                {
                    sender.RemoveEventHandler(instanc, EventHandlers[eventId]);
                    EventHandlers.Remove(eventId);
                });
            }
        }


        private static object _PreProcess<T>(string memberPath, string instanceParameters, Func<T, object, object> func) where T : MemberInfo
        {
            object value = null;
            var type = typeof(T);
            var oMember = ReflectHepler.GetPublicMember(memberPath);
            if (!(oMember is T)) throw new ArgumentException(string.Format("{0} is not a {1}", memberPath, type.Name));
            var member = oMember as T;
            var klass = member.DeclaringType;
            if (klass != null)
            {
                object instanc = null;
                if (!ReflectHepler.IsStaticMember(member))
                {
                    instanc = Activator.CreateInstance(klass, ReflectHepler.ToArguments(instanceParameters));
                }
                value = func(member, instanc);
                var wapper = instanc as IWapperReturn;
                if (wapper != null)
                {
                    value = wapper.WapperReturn(value);
                }
            }
            return value;
        }

        private static void _PreProcess<T>(string memberPath, string instanceParameters, Action<T, object> func) where T : MemberInfo
        {
            _PreProcess<T>(memberPath, instanceParameters, (sender, instance) =>
            {
                func(sender, instance);
                return null;
            });
        }

        public static void ExecuteScriptAsync(string pluginKey, string message)
        {
            WebSocketHelper.SendMessage( pluginKey,  message);
        }
    }
}
