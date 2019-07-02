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
      Assert.Throws<ArgumentNullException>(wrongValue);
    }

    [Fact]
    public void validateKey_should_throw()
    {
      Action keyWasNotSetYet = () => this.consumer.validateKey();
      Assert.Throws<ArgumentNullException>(keyWasNotSetYet);
    }
  }
}