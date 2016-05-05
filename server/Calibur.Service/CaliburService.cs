using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Calibur.Business;
using Calibur.Model;
using Calibur.Model.IMessage;
using CEF.Lib;
using CEF.Lib.Exceptions;
using CEF.Lib.Helper;
using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Calibur.Service
{
    public static class CaliburService
    {

        public static void InitialEntry()
        {
            WebSocketHelper.OnMessageHandler += (msg, socket) =>
            {
                var messageId = string.Empty;
                try
                {
                    var iMessage = JObject.Parse(msg);
                    if (iMessage != null)
                    {
                        messageId = iMessage["ID"] != null ? iMessage["ID"].ToString() : "";
                        if (string.IsNullOrWhiteSpace(messageId))
                        {
                            throw new NoMessageIdException("MessageId can not be empty.");
                        }
                        var message = iMessage["Msg"];
                        if (message != null && message["Name"] != null)
                        {
                            var body = message["Body"] == null ? string.Empty : message["Body"].ToString();
                            var response = CaliburService.SoapMessage(message["Name"].ToString(), body, socket);
                            var duplexResponse = new MessageInfo { ID = messageId, Status = ResponseStatus.Success(), Body = response };
                            socket.Send(JsonConvert.SerializeObject(duplexResponse, Common.TimeFormat));
                        }
                    }
                }
                catch (Exception e)
                {
                    var status = new ResponseStatus
                    {
                        ACK = AckCodeType.Failure,
                        Timestamp = DateTime.Now,
                        ErrorDataType = new List<ErrorDataType>()
                    };
                    status.ErrorDataType.Add(new ErrorDataType
                    {
                        ErrorCode = "REQ_BODY",
                        Message = msg
                    });
                    var flatten = Common.Flatten(e);
                    flatten.ForEachOfUnNone(exc => status.ErrorDataType.Add(new ErrorDataType
                    {
                        ErrorCode = GetErrorCode(exc),
                        Message = exc.Message,
                        StackTrace = exc.StackTrace
                    }));
                    socket.Send(JsonConvert.SerializeObject(new MessageInfo { ID = messageId, Status = status }, Common.TimeFormat));
                }
            };
        }

        private static string GetErrorCode(Exception ex)
        {
            foreach (var pair in ErrorCodeMapping)
            {
                if (ex.GetType() == pair.Key)
                {
                    return pair.Value;
                }
            }
            return "UNKNOW";
        }
        private static readonly Dictionary<Type, string> ErrorCodeMapping = new Dictionary<Type, string>
        {
            {typeof(NoMessageIdException),"NO_MSG_ID"},
            {typeof(ArgumentException),"ARG_ERR"},
            {typeof(NullReferenceException),"NUL_REF"},
            {typeof(JsonSerializationException),"JSON_SERLIZ"},
            {typeof(TargetInvocationException),"REFL_INVOK"},
        }; 

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
