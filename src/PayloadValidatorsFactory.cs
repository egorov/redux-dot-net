using System;
using System.Collections.Generic;

namespace Redux{
    public class PayloadValidatorsFactory{
        public PayloadValidators Make(){
            PayloadValidators validators = new PayloadValidators();
            validators.Add("EXCEPTION", new ExceptionValueValidator());
            return validators;
        }
    }
}