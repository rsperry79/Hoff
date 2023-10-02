using Hoff.Server.Common.Models;

namespace Hoff.Server.Common.Interfaces
{
    public interface IWsMessage
    {
        WsBaseMessage Encoded { get; set; }

        object Decode();
    }
}