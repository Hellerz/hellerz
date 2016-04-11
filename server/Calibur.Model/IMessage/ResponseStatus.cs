using System;
using System.Collections.Generic;

namespace Calibur.Model.IMessage
{
    public class ResponseStatus
    {
        public DateTime Timestamp { get; set; }
        public AckCodeType ACK { get; set; }
        public List<ErrorDataType> ErrorDataType { get; set; }

        public static ResponseStatus Success()
        {
            return new ResponseStatus
            {
                ACK = AckCodeType.Successs,
                Timestamp = DateTime.Now
            };
        }
    }

    public class ErrorDataType
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }

    public enum AckCodeType
    {
        Successs,
        Failure,
        Warning
    }
}
