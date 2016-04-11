using Fleck;

namespace Calibur.Model.IMessage
{
    public interface IMessageRequest
    {
        MessageHead Head { get; set; }
    }
}
