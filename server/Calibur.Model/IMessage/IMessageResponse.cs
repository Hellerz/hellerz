namespace Calibur.Model.IMessage
{
    public interface IMessageResponse
    {
        ResponseStatus Status { get; set; }
    }

    public class ResponseBase : IMessageResponse
    {
        public ResponseStatus Status { get; set; }
    }
}
