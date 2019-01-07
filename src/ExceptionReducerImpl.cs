using System;
using System.Collections.Generic;

namespace Redux
{
    public class ExceptionReducerImpl : XReducer
    {
        private readonly string type = "Exception";
        public object Reduce(object state, Message message)
        {
            this.Validate(message);

            if(message.Type != this.type)
                return state;

            List<Exception> errors = this.GetExceptions(state);

            Exception error = this.GetPayload(message);

            errors.Add(error);

            return errors;                        
        }

        private void Validate(Message message)
        {
            if(message == null)
                throw new ArgumentNullException("message");
        }

        private List<Exception> GetExceptions(object state)
        {
            List<Exception> exceptions = state as List<Exception>;
            
            if(exceptions == null)
                exceptions = new List<Exception>();
            else
                exceptions = new List<Exception>(exceptions);
            
            return exceptions;
        }

        private Exception GetPayload(Message message)
        {              
            Exception error = message.Payload as Exception;

            if(error == null)
                throw new InvalidOperationException("Message Payload should be Exception instance!");

            return error;
        }
    }
}