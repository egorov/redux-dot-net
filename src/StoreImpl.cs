using System;
using System.Collections.Generic;

namespace Redux{
    public class StoreImpl : Store
    {
        private IDictionary<string, object> state;
        private IEnumerable<Reducer> reducers;
        public StoreImpl(IDictionary<string, object> state, IEnumerable<Reducer> reducers){

            if(state == null)
                throw new ArgumentNullException("state");
            if(reducers == null)
                throw new ArgumentNullException("reducers");
            
            this.state = state;
            this.reducers = reducers;
        }
        public void Dispatch(Message message)
        {
            foreach(Reducer reducer in this.reducers){
                this.state = reducer.Reduce(this.state, message);
            }
        }

        public IDictionary<string, object> GetState()
        {
            return this.state;
        }
    }
}