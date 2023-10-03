using System;

using Hoff.Server.Common.Interfaces;

using nanoFramework.Json;

namespace Hoff.Server.Common.Models
{
    public class WsMessage : IWsMessage
    {
        public WsBaseMessage Encoded { get; set; }

        public WsMessage(string type, string message) => this.Encoded = new WsBaseMessage(type, message);

        public WsMessage(string encoded) => this.Encoded = (WsBaseMessage)JsonConvert.DeserializeObject(encoded, typeof(WsBaseMessage));

        public object Decode()
        {
            return JsonConvert.DeserializeObject(this.Encoded.Message, this.GetType(this.Encoded.MessageType)); ;
        }

        private Type GetType(string type)
        {
            return Type.GetType(type);
        }
    }
}
