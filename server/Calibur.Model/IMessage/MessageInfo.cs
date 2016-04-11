using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calibur.Model.IMessage
{

    public class MessageInfo 
    {
        public string ID { get; set; }

        public ResponseStatus Status { get; set; }

        public MessageMode Type { get; set; }

        public IMessageResponse Body { get; set; }
    }

    public enum MessageMode
    {
        Send=0,
        Shuttle=1,
        Event =2
    }
}
