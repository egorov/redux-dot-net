using Xunit;
using System;
using System.Collections.Generic;
using Redux;

namespace tests{
    public class StoreImplTests{
        private List<Message> messages;
        private MessageFactoryImpl factory;
        private IDictionary<string, object> state;
        private HashSet<Reducer> reducers;
        private StoreImpl store;

        public StoreImplTests(){
            this.messages = new List<Message>();
            this.factory = new MessageFactoryImpl();
            this.state = new Dictionary<string, object>();
            this.reducers = new HashSet<Reducer>();
            this.reducers.Add(new ReducerImpl("SAMPLE"));
            this.store = new StoreImpl(this.reducers, this.state);
        }

        [Fact]
        public void it_should_throw_on_construction(){
            Assert.Throws<ArgumentNullException>(() => new StoreImpl(null));
            Assert.Throws<ArgumentNullException>(() => new StoreImpl(null, this.state));
            Assert.Throws<ArgumentNullException>(() => new StoreImpl(this.reducers, null));
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
            this.TestDispatch(message);
            IDictionary<string, object> before = this.store.GetState();

            message = this.factory.Make("SAMPLE", "Another content");
            this.TestDispatch(message);
            IDictionary<string, object> after = this.store.GetState();            

            Assert.NotSame(before, after);
        }

        private void TestDispatch(Message message){
            this.store.Dispatch(message);            

            Assert.Single(this.store.GetState());
            Assert.True(this.store.GetState().ContainsKey(message.Type));
            Assert.Equal(message.Payload, this.store.GetState()[message.Type]);
        }

        [Fact]
        public void it_should_update_Store_state_after_Dispatch(){

            Message message = this.factory.Make("SAMPLE", "Content");
            this.TestDispatch(message);

            message = this.factory.Make("SAMPLE", "Another content");
            this.TestDispatch(message);
        }

        [Fact]
        public void it_should_throw_on_Dispatch(){

            Assert.Throws<ArgumentNullException>(() => this.store.Dispatch(null));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(null, "content")));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message("", "content")));
            Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(" ", "content")));
        }

        [Fact]
        public void it_should_Handle_message_with_subscriber_Action(){
            
            Action unsubscribe = this.store.Subscribe(this.Handle);
            Message message = this.factory.Make("UNKNOWN", "Content");
            this.TestSubscribersHandle(message);
        }

        private void TestSubscribersHandle(Message message){

            this.store.Dispatch(message);

            Assert.Single(this.messages);
            Assert.Contains(message, this.messages);
        }

        private void Handle(Message message){
            this.messages.Add(message);
        }

        [Fact]
        public void it_should_not_Handle_message_after_unsubscribe_call(){
            
            Action unsubscribe = this.store.Subscribe(this.Handle);
            Message message = this.factory.Make("UNKNOWN", "Content");
            this.TestSubscribersHandle(message);
            
            unsubscribe();
            this.TestSubscribersHandle(message);
        }
    }
}