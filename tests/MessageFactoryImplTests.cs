using Redux;
using System;
using Xunit;

namespace tests
{
    public class MessageFactoryImplTests
    {
        private MessageFactoryImpl factory;

        public MessageFactoryImplTests(){
            this.factory = new MessageFactoryImpl();
        }
    
        [Fact]
        public void it_should_make_a_message(){
            
            string type = "ERROR";
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
        }
    }
}