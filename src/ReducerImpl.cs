using System;
using System.Collections.Generic;

namespace Redux{
    public class ReducerImpl : ReducerArgumentsValidator, Reducer
    {
        private string type;
        public ReducerImpl(string type){

            if(string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            
            if(string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
            
            this.type = type.Substring(0, type.Length);
        }
        public IDictionary<string, object> Reduce(IDictionary<string, object> state, Message message)
        {
            this.ValidateState(state);
            this.ValidateMessage(message);

            if(message.Type != this.type)
                return state;
            
            Dictionary<string, object> after = new Dictionary<string, object>(state);
            after[message.Type] = message.Payload;
            return after;
        }
    }
}