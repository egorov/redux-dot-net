using System;
using Redux;
using Xunit;

namespace tests
{
  public class KeyConsumerImplTests
  {
    private KeyConsumerImpl consumer;

    public KeyConsumerImplTests()
    {
      this.consumer = new KeyConsumerImpl();
    }

    [Fact]
    public void should_implement()
    {
      Assert.IsAssignableFrom<KeyConsumer>(this.consumer);
    }

    [Fact]
    public void should_set_key()
    {
      string key = "doodle";
      this.consumer.setKey(key);
      this.consumer.validateKey();

      Assert.Equal(key, this.consumer.Key);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void setKey_should_throw(string value)
    {
      Action wrongValue = () => this.consumer.setKey(value);
      
      ArgumentException error = 
        Assert.Throws<ArgumentException>(wrongValue);
      
      string message = "key can\'t be null, empty or whitespaces!";
      Assert.Equal(message, error.Message);
    }

    [Fact]
    public void validateKey_should_throw()
    {
      Action keyWasNotSetYet = () => this.consumer.validateKey();
      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(keyWasNotSetYet);
      
      string message = "Call setKey(string key) first!";
      Assert.Equal(message, error.Message);
    }
  }
}