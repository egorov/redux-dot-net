using System;
using Xunit;
using Redux;

namespace tests
{
    public class MessageValidatorTests
    {
        private MessageValidatorImpl validator;

        public MessageValidatorTests()
        {
            this.validator = new MessageValidatorImpl();
        }

        [Fact]
        public void it_should_implement()
        {
            Assert.IsAssignableFrom<MessageValidator>(this.validator);
        }

        [Fact]
        public void it_should_not_throw()
        {
            this.validator.Validate(new Message("String", "Content"));
        }

        [Fact]
        public void it_should_throw_an_error()
        {
            Assert.Throws<ArgumentNullException>(() => this.validator.Validate(null));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(new Message(null, "x")));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(new Message("", "x")));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(new Message(" ", "x")));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(new Message("Tag", null)));
        }
    }
}