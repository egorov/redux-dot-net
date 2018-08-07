using System;
using System.Collections.Generic;

namespace Redux{
    public interface Store{
        void Dispatch(Message message);
        IDictionary<string, object> GetState();
        Action Subscribe(Action<Message> handler);
    }
}