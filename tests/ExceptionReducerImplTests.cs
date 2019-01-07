using System;
using System.Collections.Generic;
using Xunit;
using Redux;

namespace tests
{
    public class ExceptionReducerImplTests
    {
        private readonly string type = "Exception";
        private ExceptionReducerImpl reducer;

        public ExceptionReducerImplTests()
        {
            this.reducer = new ExceptionReducerImpl();
        }

        [Fact]
        public void it_should_implement_XReducer()
        {
            Assert.IsAssignableFrom<XReducer>(this.reducer);
        }

        [Fact]
        public void it_should_return_Type()
        {
            Assert.Equal("Exception", this.reducer.Type);
        }

        [Fact]
        public void it_should_Reduce_if_state_is_Empty()
        {
            Exception error = new Exception();            
            Message message = new Message(type, error);
            
            List<Exception> before = new List<Exception>();
            List<Exception> after = 
                this.reducer.Reduce(before, message) as List<Exception>;

            Assert.NotSame(before, after);
            Assert.DoesNotContain<Exception>(error, before);
            Assert.Contains<Exception>(error, after);
            Assert.Single(after);
        }

        [Fact]
        public void it_should_Reduce_if_state_contains_values()
        {
            Exception error = new Exception();            
            Message message = new Message(type, error);

            List<Exception> before = new List<Exception>();
            InvalidOperationException exception = new InvalidOperationException();
            before.Add(exception);
   
            List<Exception> after = 
                this.reducer.Reduce(before, message) as List<Exception>;

            Assert.NotSame(before, after);
            Assert.Contains<Exception>(exception, before);
            Assert.DoesNotContain<Exception>(error, before);

            Assert.Contains<Exception>(error, after);
            Assert.Contains<Exception>(exception, after);
        }

        [Fact]
        public void it_should_Reduce_if_state_is_null()
        {
            Exception error = new Exception();
            
            Message message = new Message(type, error);
            
            List<Exception> state = 
                this.reducer.Reduce(null, message) as List<Exception>;

            Assert.Contains<Exception>(error, state);
        }

        [Fact]
        public void it_should_not_Reduce_when_state_is_null()
        {
            List<Exception> state = 
                this.reducer.Reduce(null, new Message("x", "y")) as List<Exception>;

            Assert.Null(state);
        }

        [Fact]
        public void it_should_not_Reduce_when_state_is_empty()
        {
            List<Exception> before = new List<Exception>();
            List<Exception> after = 
                this.reducer.Reduce(before, new Message("x", "y")) as List<Exception>;

            Assert.Equal(before, after);
        }

        [Fact]
        public void it_should_not_Reduce_when_state_contains_values()
        {
            List<Exception> before = new List<Exception>();
            Exception error = new Exception();
            before.Add(error);

            List<Exception> after = 
                this.reducer.Reduce(before, new Message("x", "y")) as List<Exception>;

            Assert.Equal(before, after);
            Assert.Single(after);
            Assert.Contains<Exception>(error, after);
        }

        [Fact]
        public void it_should_throw_if_Message_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => this.reducer.Reduce(null, null));
            Assert.Throws<InvalidOperationException>(() => this.reducer.Reduce(null, new Message(type, "wrong payload")));
        }
    }
}