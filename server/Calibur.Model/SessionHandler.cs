using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calibur.Model.IMessage;

namespace Calibur.Model
{
    public class SessionHandlerRequest : IMessageRequest
    {
        public MessageHead Head { get; set; }
        public EventName EventName { get; set; }
        public EventType EventType { get; set; }

    }

    public class SessionHandlerResponse : IMessageResponse 
    {
        public ResponseStatus Status { get; set; }
    }

    public enum EventType
    {
        Add = 1,
        Remove = 2
    }

    public enum EventName
    {
        BeforeRequest = 1,
        BeforeResponse = 2,
        RequestHeadersAvailable = 3,
        ResponseHeadersAvailable = 4,
        AfterSessionComplete = 5,
        BeforeReturningError = 6
    }

}
