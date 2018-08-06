using System;
using System.Collections.Generic;

namespace Redux{
    public interface Reducer {
        IDictionary<string, object> Reduce(IDictionary<string, object> state, Message message);
    }
}