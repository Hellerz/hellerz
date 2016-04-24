using System;
using System.Collections.Generic;
using System.Reflection;
using Calibur.Model.IMessage;
using CEF.Lib;
using CEF.Lib.Exceptions;
using CEF.Lib.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Calibur.Service
{
    public class WebSocketEntry
    {
        public static void Start()
        {
            WebSocketHelper.OnMessage += (msg, socket) =>
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
                        if (message != null && message["Name"]!=null)
                        {
                            var body = message["Body"] == null ? string.Empty : message["Body"].ToString();
                            var response = CaliburService.SoapMessage(message["Name"].ToString(), body, socket);
                            var duplexResponse = new MessageInfo {ID = messageId,Status = ResponseStatus.Success(),Body = response};
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
                    socket.Send(JsonConvert.SerializeObject(new MessageInfo { ID = messageId,Status = status }, Common.TimeFormat));
                }
            };
        }

        public static string GetErrorCode(Exception ex )
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
        public static readonly Dictionary<Type,string> ErrorCodeMapping = new Dictionary<Type, string>
        {
            {typeof(NoMessageIdException),"NO_MSG_ID"},
            {typeof(ArgumentException),"ARG_ERR"},
            {typeof(NullReferenceException),"NUL_REF"},
            {typeof(JsonSerializationException),"JSON_SERLIZ"},
            {typeof(TargetInvocationException),"REFL_INVOK"},
        }; 
        
    }
}
