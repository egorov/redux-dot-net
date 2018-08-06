using System;

namespace Redux{
    public class ExceptionValueValidator : ValueValidator
    {
        public void Validate(object value)
        {
            if(value == null)
                throw new ArgumentNullException("value");
            
            if(!(value is Exception))
                throw new ArgumentException("value should be instance of Exception!");
        }
    }
}