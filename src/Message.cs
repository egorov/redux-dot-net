using System;

namespace Redux
{
    public class Message
    {
        public string Type { get; private set; }
        public object Payload { get; private set; }
        public Message(string type, object payload)
        {
            this.Type = type;
            this.Payload = payload;
        }
    }
}