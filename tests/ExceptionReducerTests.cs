using System;
using System.Collections.Generic;
using Redux;
using Xunit;

namespace tests
{
    public class ExceptionReducerTests
    {
        private readonly string type = "Exception";
        private IDictionary<string, object> before;
        private ExceptionReducer reducer;
        
        public ExceptionReducerTests()
        {
            this.before = new Dictionary<string, object>();
            this.reducer = new ExceptionReducer();
        }

        [Fact]
        public void it_should_Reduce_Exception_Message_type()
        {
            Exception payload = new Exception();
            Message message = new Message(type, payload);
            IDictionary<string, object> after = this.reducer.Reduce(this.before, message);

            Assert.True(after.ContainsKey(type));
            Assert.NotSame(before, after);

            IEnumerable<object> errors = after[type] as IEnumerable<object>;
            Assert.Single(errors);
            Assert.Contains(payload, errors);
        }

        [Fact]
        public void it_should_not_Reduce_other_type_Messages()
        {
            Message message = new Message("SAMPLE", "Content");
            IDictionary<string, object> after = this.reducer.Reduce(this.before, message);

            Assert.False(after.ContainsKey(type));
            Assert.Same(before, after);
        }

        [Fact]
        public void Reduce_should_throw()
        {
            
            Assert.Throws<ArgumentNullException>(() => this.reducer.Reduce(null, new Message("MSG", "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.before, new Message(null, "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.before, new Message("", "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.before, new Message(" ", "Content")));
            Assert.Throws<ArgumentException>(() => this.reducer.Reduce(this.before, new Message(string.Empty, "Content")));
        }
    }
}