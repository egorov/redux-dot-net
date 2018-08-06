using System;
using System.Collections.Generic;

namespace Redux{
    public class ReducerArgumentsValidator{
        public void ValidateState(IDictionary<string, object> state){
            if(state == null)
                throw new ArgumentNullException("state");
        }

        public void ValidateMessage(Message message){
            if(message == null)
                throw new ArgumentNullException("message");

            if(string.IsNullOrEmpty(message.Type))
                throw new ArgumentException("Message Type can\'t be null, empty or whitespaces!");            

            if(string.IsNullOrWhiteSpace(message.Type))
                throw new ArgumentException("Message Type can\'t be null, empty or whitespaces!");            
        }
    }
}