using Redux;
using System;
using Xunit;

namespace tests
{
    public class MessageFactoryImplTests
    {
        private MessageFactoryImpl factory;

        public MessageFactoryImplTests(){
            PayloadValidators validators = new PayloadValidatorsFactory().Make();            
            this.factory = new MessageFactoryImpl(validators);
        }

        [Fact]
        public void it_should_make_a_message(){
            
            string type = "SAMPLE";
            StringIsNotNullValidator payload = new StringIsNotNullValidator();

            Message message = this.factory.Make(type, payload);
            Assert.Equal(type, message.Type);
            Assert.Equal(payload, message.Payload);
        }

        [Fact]
        public void it_should_make_a_message_with_payload_validation(){
            
            string type = "EXCEPTION";
            Exception payload = new Exception();

            Message message = this.factory.Make(type, payload);
            Assert.Equal(type, message.Type);
            Assert.Equal(payload, message.Payload);
        }

        [Fact]
        public void it_should_throws_if_wrong_type(){

            Assert.Throws<ArgumentNullException>(() => this.factory.Make(null, "value"));
            Assert.Throws<ArgumentNullException>(() => this.factory.Make("", "value"));
            Assert.Throws<ArgumentNullException>(() => this.factory.Make(" ", "value"));
            Assert.Throws<ArgumentNullException>(() => this.factory.Make(string.Empty, "value"));
            Assert.Throws<ArgumentException>(() => this.factory.Make("EXCEPTION", new StringIsNotNullValidator()));
        }
    }
}