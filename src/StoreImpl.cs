using System;
using System.Collections.Generic;

namespace Redux{
    public class StoreImpl : Store
    {
        private IDictionary<string, object> state;
        private IEnumerable<Reducer> reducers;
        private HashSet<Action<Message>> subscribers;
        public StoreImpl(IDictionary<string, object> state, IEnumerable<Reducer> reducers){

            if(state == null)
                throw new ArgumentNullException("state");
            if(reducers == null)
                throw new ArgumentNullException("reducers");
            
            this.state = state;
            this.reducers = reducers;
            this.subscribers = new HashSet<Action<Message>>();
        }
        public void Dispatch(Message message)
        {
            foreach(Reducer reducer in this.reducers){
                this.state = reducer.Reduce(this.state, message);
            }

            this.NotifySubscribers(message);
        }

        public IDictionary<string, object> GetState()
        {
            return this.state;
        }

        public Action Subscribe(Action<Message> handler)
        {
            this.subscribers.Add(handler);

            return () => {
                this.subscribers.Remove(handler);
            };
        }

        private void NotifySubscribers(Message message){

            foreach(Action<Message> handle in this.subscribers){
                handle(message);
            }
        }
    }
}