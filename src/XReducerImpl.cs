using System;

namespace Redux
{
    public class XReducerImpl : XReducer
    {
        protected readonly string type;

        public string Type { get { return this.type; } }

        protected readonly MessageValidator messageValidator;

        public XReducerImpl(string type, MessageValidator messageValidator)
        {
            if(string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");

            if(string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
            
            if(messageValidator == null)
                throw new ArgumentNullException("messageValidator");
            
            this.type = type;
            this.messageValidator = messageValidator;
        }

        public virtual object Reduce(object state, Message message)
        {
            this.messageValidator.Validate(message);

            if(this.type != message.Type)
                return state;
            
            return message.Payload;            
        }
    }
}