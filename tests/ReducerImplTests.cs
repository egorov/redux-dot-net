using System;
using System.Collections.Generic;
using Redux;
using Xunit;

namespace tests{
    public class ReducerImplTests{
        private string type;
        private ReducerImpl reducer;
        private Dictionary<string, object> state;

        public ReducerImplTests(){
            this.type = "SAMPLE";
            this.reducer = new ReducerImpl(this.type);
            this.state = new Dictionary<string, object>();
        }

        [Fact]
        public void it_should_implement_Reducer(){

            Assert.True(this.reducer is Reducer);
        }

        [Fact]
        public void it_should_reduce_Message(){
            Message message = new Message(this.type, "Content");
            IDictionary<string, object> after = this.reducer.Reduce(this.state, message);
            int count = after.Count;
            
            Assert.Equal(1, count);
            Assert.Equal(message.Payload, after[message.Type]);
        }

        [Fact]
        public void it_should_not_reduce_Message(){
            Message message = new Message("UNKNOWN", "Content");
            IDictionary<string, object> after = this.reducer.Reduce(this.state, message);
            int count = after.Count;
            
            Assert.Equal(0, count);
        }

        [Fact]
        public void Reduce_should_throw(){
            
            Assert.Throws<ArgumentNullException>(() => this.reducer.Reduce(null, new Message(this.type, "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.state, new Message(null, "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.state, new Message("", "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.state, new Message(" ", "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.state, new Message(string.Empty, "Content")));
        }
    }
}