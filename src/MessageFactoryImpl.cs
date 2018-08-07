using System;

namespace Redux
{
    public class MessageFactoryImpl
    {
        private PayloadValidators validators;
        
        public MessageFactoryImpl()
            :this(new PayloadValidators()) {}

        public MessageFactoryImpl(PayloadValidators validators){

            if(validators == null)
                throw new ArgumentNullException("validators");
            
            this.validators = validators;
        }
        public Message Make(string type, object payload)
        {
            this.ValidateType(type);
            this.ValidatePayload(type, payload);
            return new Message(type, payload);
        }

        private void ValidateType(string type){
            
            if(string.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");

            if(string.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");
        }

        private void ValidatePayload(string type, object payload){
            
            foreach(ValueValidator validator in this.validators.Get(type)){
                validator.Validate(payload);
            }
        }
    }
}