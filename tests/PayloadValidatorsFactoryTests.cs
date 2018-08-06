using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace tests{
    public class PayloadValidatorsFactoryTests{
        private PayloadValidatorsFactory factory;
        public PayloadValidatorsFactoryTests(){
            this.factory = new PayloadValidatorsFactory();
        }

        [Fact]
        public void it_should_contain_ExceptionValueValidator(){

            PayloadValidators payloadValidators = this.factory.Make();

            Assert.Single(payloadValidators.Keys);
            Assert.Contains("EXCEPTION", payloadValidators.Keys);

            IEnumerable<ValueValidator> validators = payloadValidators.Get("EXCEPTION");
            int count = validators.Count();
            Assert.Equal(1, count);

            ValueValidator validator = validators.First();
            Assert.True(validator is ExceptionValueValidator);
        }
    }
}