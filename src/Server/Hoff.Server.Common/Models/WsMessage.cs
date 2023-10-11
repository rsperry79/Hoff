using System;

using Hoff.Server.Common.Interfaces;

using nanoFramework.Json;

namespace Hoff.Server.Common.Models
{
    public class WsMessage : IWsMessage
    {

        public string MessageType { get; set; }

        //private object payload;
        //public object Payload {
        //    get
        //    {
        //        return this.payload;
        //    }
        //    set
        //    {
        //        this.payload = value;
        //        this.MessageType = this.payload.GetType(); 
        //    }
        //}
        public string Message { get; set; }

        public WsMessage()
        {
        }

        public WsMessage(string messageType, string message)
        {
            this.MessageType = messageType;
            this.Message = message;
        }

        public WsMessage(object payload, Type type)
        {
            this.MessageType = type.ToString();
            this.Message = JsonConvert.SerializeObject(payload);

        }

        public WsMessage(string raw)
        {

            Console.WriteLine(raw);
            WsMessage temp = (WsMessage)JsonConvert.DeserializeObject(raw, typeof(WsMessage));
            this.MessageType = temp.MessageType;
            this.Message = raw;
        }

        ~WsMessage()
        {
            this.Message = null;
            this.MessageType = null;
        }
    }
}
