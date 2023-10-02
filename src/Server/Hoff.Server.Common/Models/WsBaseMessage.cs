namespace Hoff.Server.Common.Models
{
    public class WsBaseMessage
    {
        public string MessageType { get; set; }
        public string Message { get; set; }

        public WsBaseMessage(string messageType, string message)
        {
            this.MessageType = messageType;
            this.Message = message;
        }
    }
}
