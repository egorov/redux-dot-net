using Redux;
using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;

namespace tests
{
    public class PayloadValidatorsTests
    {
        private readonly string sampleKey = "Sample";
        private readonly string exceptionKey = "Exception";
        private PayloadValidators validators;

        public PayloadValidatorsTests()
        {
            this.validators = new PayloadValidators();
        }

        [Fact]
        public void it_should_Add_validator()
        {
            ValueValidator validator = new StringIsNotNullValidator();
            this.validators.Add(sampleKey, validator);

            int count = this.validators.Get(sampleKey).Count();
            Assert.Equal(1, count);
            Assert.Contains(validator, this.validators.Get(sampleKey));

            validator = new ExceptionValueValidator();
            this.validators.Add(sampleKey, validator);
            count = this.validators.Get(sampleKey).Count();
            Assert.Equal(2, count);
            Assert.Contains(validator, this.validators.Get(sampleKey));
        }

        [Fact]
        public void it_should_not_Add_same_validator_twice()
        {
            ValueValidator validator = new StringIsNotNullValidator();
            this.validators.Add(sampleKey, validator);
            this.validators.Add(sampleKey, validator);

            int count = this.validators.Get(sampleKey).Count();
            Assert.Equal(1, count);
            Assert.Contains(validator, this.validators.Get(sampleKey));
        }

        [Fact]
        public void it_should_return_an_empty_collection()
        {
            Assert.Empty(this.validators.Get("FIRST_REQUEST"));
        }

        [Fact]
        public void it_should_return_Keys()
        {
            Assert.Empty(this.validators.Keys);

            this.validators.Add(sampleKey, new StringIsNotNullValidator());
            Assert.Single(this.validators.Keys);
            Assert.Contains(sampleKey, this.validators.Keys);

            this.validators.Add(exceptionKey, new ExceptionValueValidator());
            int count = this.validators.Keys.Count();
            Assert.Equal(2, count);
            Assert.Contains(sampleKey, this.validators.Keys);
            Assert.Contains(exceptionKey, this.validators.Keys);
        }

        [Fact]
        public void it_should_Add_throw_an_error()
        {
            Assert.Throws<ArgumentNullException>(() => this.validators.Add(null, new StringIsNotNullValidator()));
            Assert.Throws<ArgumentNullException>(() => this.validators.Add("", new StringIsNotNullValidator()));        
            Assert.Throws<ArgumentNullException>(() => this.validators.Add(" ", new StringIsNotNullValidator()));        
            Assert.Throws<ArgumentNullException>(() => this.validators.Add("VALID_KEY", null));        
        }
    }
}
