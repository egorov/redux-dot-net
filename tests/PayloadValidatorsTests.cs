using Redux;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace tests
{
    public class PayloadValidatorsTests
    {
        private PayloadValidators validators;

        public PayloadValidatorsTests(){
            this.validators = new PayloadValidators();
        }

        [Fact]
        public void it_should_Add_validator(){
            string key = "SAMPLE";
            ValueValidator validator = new StringIsNotNullValidator();
            this.validators.Add(key, validator);

            int count = this.validators.Get(key).Count();
            Assert.Equal(1, count);
            Assert.Contains(validator, this.validators.Get(key));
        }

        [Fact]
        public void it_should_not_Add_same_validator_twice(){
            string key = "SAMPLE";
            ValueValidator validator = new StringIsNotNullValidator();
            this.validators.Add(key, validator);
            this.validators.Add(key, validator);

            int count = this.validators.Get(key).Count();
            Assert.Equal(1, count);
            Assert.Contains(validator, this.validators.Get(key));
        }

        [Fact]
        public void it_should_return_an_empty_collection(){

            Assert.Empty(this.validators.Get("FIRST_REQUEST"));
        }

        [Fact]
        public void it_should_Add_throw_an_error(){

            Assert.Throws<ArgumentNullException>(() => this.validators.Add(null, new StringIsNotNullValidator()));
            Assert.Throws<ArgumentNullException>(() => this.validators.Add("", new StringIsNotNullValidator()));        
            Assert.Throws<ArgumentNullException>(() => this.validators.Add(" ", new StringIsNotNullValidator()));        
            Assert.Throws<ArgumentNullException>(() => this.validators.Add("VALID_KEY", null));        
        }
    }
}
