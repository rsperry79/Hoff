namespace Hoff.Server.Common.Interfaces
{
    public interface IWsMessage
    {
        //object Payload { get; set; }

        string MessageType { get; set; }
        string Message { get; set; }
    }
}