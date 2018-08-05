using System;

namespace Redux
{
    public class MessageFactoryImpl
    {
        public Message Make(string type, object payload)
        {
            this.ValidateType(type);
            return new Message(type, payload);
        }

        private void ValidateType(string type){
            
            if(string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");

            if(string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
        }
    }
}