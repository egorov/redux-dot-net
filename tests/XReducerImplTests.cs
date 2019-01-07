using System;
using Xunit;
using Redux;

namespace tests
{
    public class XReducerImplTests
    {
        private readonly string type = "Sample";
        private XReducerImpl reducer;
        public XReducerImplTests()
        {
            this.reducer = new XReducerImpl(this.type, new MessageValidatorImpl());
        }

        [Fact]
        public void it_should_implement()
        {
            Assert.IsAssignableFrom<XReducer>(this.reducer);
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
            Assert.Throws<ArgumentNullException>(() => new XReducerImpl(null, new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new XReducerImpl("", new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new XReducerImpl(" ", new MessageValidatorImpl()));
            Assert.Throws<ArgumentNullException>(() => new XReducerImpl("Tag", null));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message(null, "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message("", "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message(" ", "x")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(null, new Message("Tag", null)));
        }
    }
}