using System;

namespace Redux
{
    public class MessageValidatorImpl : MessageValidator
    {
        public void Validate(Message message)
        {
            if(message == null)
                throw new ArgumentNullException("message");

            if(string.IsNullOrEmpty(message.Type))
                throw new ArgumentException("Message Type can\'t be null, empty or whitespaces!");            

            if(string.IsNullOrWhiteSpace(message.Type))
                throw new ArgumentException("Message Type can\'t be null, empty or whitespaces!");            
            
            if(message.Payload == null)
                throw new ArgumentException("Message Payload can\'t be null!");
        }
    }
}