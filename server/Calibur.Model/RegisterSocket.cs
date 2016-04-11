using System;
using System.Collections.Generic;
using Calibur.Model.IMessage;

namespace Calibur.Model
{

    public class RegisterSocketRequest : IMessageRequest
    {
        public MessageHead Head { get; set; }
        public string BrowserId { get; set; }
    }

    public class RegisterSocketResponse : IMessageResponse
    {
        public ResponseStatus Status { get; set; }
    }
}
