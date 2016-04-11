using System;
using System.Collections.Generic;
using Calibur.Model.IMessage;

namespace Calibur.Model
{
    public class JSchemaRequest:IMessageRequest
    {
        public MessageHead Head { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }

    }

    public class JSchemaResponse:IMessageResponse
    {
        public ResponseStatus Status { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }
        public bool HasArgCtor { get; set; }
        public List<JSMemberInfo> MemberList { get; set; }
    }

    public class JSMemberInfo
    {
        public string Member { get; set; }
        public bool IsStatic { get; set; }
        public List<string> Types { get; set; }
    }

}
