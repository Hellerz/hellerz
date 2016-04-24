using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Calibur.Business;
using Calibur.Model;
using Calibur.Model.IMessage;
using CEF.Lib.Helper;
using Fleck;
using Newtonsoft.Json;

namespace Calibur.Service
{
    public static class CaliburService
    {
        private static readonly Type WebSocketEntryClass = typeof(CaliburService);

        public static IMessageResponse SoapMessage(string controller, string body, IWebSocketConnection socket)
        {
            if (string.IsNullOrWhiteSpace(controller)) throw new ArgumentException("API cannot be Empty.");

            var method = WebSocketEntryClass.GetMethod(controller, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

            if (method == null) throw new ArgumentException(string.Format("API'{0}' cannot found.", controller));

            var type = ReflectHepler.GetMethodTypes(method).FirstOrDefault();

            IMessageRequest request = null;
            if (body != null)
            {
                request = JsonConvert.DeserializeObject(body, type) as IMessageRequest;
                if (request != null)
                {
                    request.Head = request.Head ?? new MessageHead();
                    request.Head.Socket = socket;
                }
            }
            var response = method.Invoke(null, new object[] { request }) as IMessageResponse;
            if (response != null)
            {
                response.Status = ResponseStatus.Success();
            }
            return response;
        }

        public static InstanceResponse Instance(InstanceRequest request)
        {
            return InstanceCore.Instance(request);
        }

        public static SessionHandlerResponse SessionHandler(SessionHandlerRequest request)
        {
            return SessionCore.SessionHandler(request);
        }

        public static JSchemaResponse JSchema(JSchemaRequest request)
        {
            return JSchemaCore.GetJSchema(request);
        }
    }
}
