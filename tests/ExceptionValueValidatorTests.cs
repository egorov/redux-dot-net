using Redux;
using System;
using Xunit;

namespace tests
{
    public class ExceptionValueValidatorTests
    {
        private ExceptionValueValidator validator;

        public ExceptionValueValidatorTests(){
            this.validator = new ExceptionValueValidator();
        }

        [Fact]
        public void it_should_implement_ValueValidator()
        {
            Assert.True(this.validator is ValueValidator);
        }

        [Fact]
        public void Validate_should_throw(){
            
            Assert.Throws<ArgumentNullException>(() => this.validator.Validate(null));
            Assert.Throws<ArgumentException>(() => this.validator.Validate(35));
        }

        [Fact]
        public void Validate_should_not_throw(){
            
            this.validator.Validate(new InvalidOperationException());
        }
    }
}