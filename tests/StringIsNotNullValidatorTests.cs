using System;
using Xunit;
using Redux;

namespace tests
{
    public class StringIsNotNullValidatorTests
    {
        private StringIsNotNullValidator validator;

        public StringIsNotNullValidatorTests(){
            this.validator = new StringIsNotNullValidator();
        }

        [Fact]
        public void it_should_implement_ValueValidator()
        {
            Assert.True(this.validator is ValueValidator);
        }

        [Fact]
        public void it_should_Validate_throw()
        {
            Assert.Throws<ArgumentNullException>(() => this.validator.Validate(null));
            Assert.Throws<ArgumentNullException>(() => this.validator.Validate(""));
            Assert.Throws<ArgumentNullException>(() => this.validator.Validate(" "));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(38));
        }
    }
}
