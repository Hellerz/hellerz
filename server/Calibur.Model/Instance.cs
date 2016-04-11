using System.Collections.Generic;
using Calibur.Model.IMessage;

namespace Calibur.Model
{
    public class InstanceRequest : IMessageRequest
    {
        public MessageHead Head { get; set; }

        public string Operate { get; set; }
        public string MemberPath { get; set; }
        public List<object> InstanceParameters { get; set; }
        public List<object> MemberParameters { get; set; }

        public string EventId { get; set; }

        public string PluginKey { get; set; }
        public string JsCallback { get; set; }
        
    }

    public class InstanceResponse : IMessageResponse
    {
        public ResponseStatus Status { get; set; }

        public object Result { get; set; }

    }
}
