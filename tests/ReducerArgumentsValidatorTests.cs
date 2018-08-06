using System;
using System.Collections.Generic;
using Redux;
using Xunit;

namespace tests{
    public class ReducerArgumentsValidatorTests{
        private ReducerArgumentsValidator validator;

        public ReducerArgumentsValidatorTests(){
            this.validator = new ReducerArgumentsValidator();
        }

        [Fact]
        public void ValidateState_should_throw(){
            
            Assert.Throws<ArgumentNullException>(() => this.validator.ValidateState(null));
        }

        [Fact]
        public void ValidateMessage_should_throw(){

            Assert.Throws<ArgumentNullException>(() => this.validator.ValidateMessage(null));
            Assert.Throws<ArgumentException>(() => this.validator.ValidateMessage(new Message(null, "value")));
            Assert.Throws<ArgumentException>(() => this.validator.ValidateMessage(new Message("", "value")));
            Assert.Throws<ArgumentException>(() => this.validator.ValidateMessage(new Message(" ", "value")));
        }

        [Fact]
        public void ValidateMessage_does_not_validate_Message_Payload(){
            
            this.validator.ValidateMessage(new Message("KEY", null));
        }
    }
}