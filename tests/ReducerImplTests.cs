using System;
using Xunit;
using Redux;

namespace tests
{
    public class ReducerImplTests
    {
        private readonly string type = "Sample";
        private ReducerImpl reducer;
        public ReducerImplTests()
        {
            this.reducer = new ReducerImpl(this.type, new MessageValidatorImpl());
        }

        [Fact]
        public void it_should_implement()
        {
            Assert.IsAssignableFrom<Reducer>(this.reducer);
        }

        [Fact]
        public void it_should_Reduce()
        {
            object state = this.reducer.Reduce(null, new Message(type, "content"));
            Assert.Equal("content", state);

            state = this.reducer.Reduce("before", new Message(type, "after"));
            Assert.Equal("after", state);
        }

        [Fact]
        public void it_should_return_state_untuched()
        {
            object state = this.reducer.Reduce(null, new Message("X", "Y"));
            Assert.Null(state);

            state = this.reducer.Reduce("value", new Message("F", "M"));
            Assert.Equal("value", state);
        }

        [Fact]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentNullException>(() => new ReducerImpl(null, new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new ReducerImpl("", new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new ReducerImpl(" ", new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new ReducerImpl("Tag", null));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message(null, "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message("", "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message(" ", "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message("Tag", null)));
        }
    }
}