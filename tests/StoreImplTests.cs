using Xunit;
using System;
using System.Collections.Generic;
using Redux;

namespace tests{
    public class StoreImplTests{
        private MessageFactoryImpl factory;
        private IDictionary<string, object> state;
        private HashSet<Reducer> reducers;
        private StoreImpl store;

        public StoreImplTests(){
            this.factory = new MessageFactoryImpl();
            this.state = new Dictionary<string, object>();
            this.reducers = new HashSet<Reducer>();
            this.reducers.Add(new ReducerImpl("SAMPLE"));
            this.store = new StoreImpl(this.state, this.reducers);
        }

        [Fact]
        public void it_should_not_change_source_state_after_Dispatch(){

            Message message = this.factory.Make("SAMPLE", "Content");
            this.store.Dispatch(message);            

            Assert.Empty(this.state);
        }

        [Fact]
        public void it_should_not_update_state_with_unknown_Message_type(){

            Message message = this.factory.Make("ERROR", "Something goes wrong!");
            this.store.Dispatch(message);            
            Assert.Empty(this.store.GetState());
        }

        [Fact]
        public void it_should_return_another_state_after_Dispatch(){

            Message message = this.factory.Make("SAMPLE", "Content");
            this.store.Dispatch(message);            
            IDictionary<string, object> before = this.store.GetState();

            message = this.factory.Make("SAMPLE", "Another content");
            this.store.Dispatch(message);
            IDictionary<string, object> after = this.store.GetState();            

            Assert.NotSame(before, after);
        }

        [Fact]
        public void it_should_update_Store_state_after_Dispatch(){

            Message message = this.factory.Make("SAMPLE", "Content");
            this.store.Dispatch(message);            

            Assert.Single(this.store.GetState());
            Assert.True(this.store.GetState().ContainsKey("SAMPLE"));
            Assert.Equal("Content", this.store.GetState()["SAMPLE"]);

            message = this.factory.Make("SAMPLE", "Another content");
            this.store.Dispatch(message);

            Assert.Single(this.store.GetState());
            Assert.True(this.store.GetState().ContainsKey("SAMPLE"));
            Assert.Equal("Another content", this.store.GetState()["SAMPLE"]);
        }

        [Fact]
        public void it_should_throw_on_Dispatch(){

            Assert.Throws<ArgumentNullException>(() => this.store.Dispatch(null));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(null, "content")));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message("", "content")));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(" ", "content")));
        }
    }
}