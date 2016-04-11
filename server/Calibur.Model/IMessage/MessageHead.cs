using Fleck;

namespace Calibur.Model.IMessage
{
    public class MessageHead
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public IWebSocketConnection Socket { get; set; }
    }
}
