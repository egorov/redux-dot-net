using System;

namespace Redux
{
    public interface ValueValidator
    {
        void Validate(object value);
    }
}
