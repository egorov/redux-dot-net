using System;

namespace Redux{
    public class StringIsNotNullValidator : ValueValidator
    {
        public void Validate(object value)
        {
            if(value == null)
                throw new ArgumentNullException("value");

            if(!(value is string))
                throw new ArgumentException("value should be a string");

            string content = value as string;

            if(string.IsNullOrEmpty(content))
                throw new ArgumentNullException("value");

            if(string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException("value");
        }
    }
}