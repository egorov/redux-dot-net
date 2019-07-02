using Xunit;
using System;
using System.Collections.Generic;
using Redux;

namespace tests
{
  public class StoreValueProviderImplTests
  {
    private string key;
    private Store store;
    private StoreValueProviderImpl provider;

    public StoreValueProviderImplTests()
    {
      this.key = "object";
      HashSet<Reducer> reducers = new HashSet<Reducer>();
      reducers.Add(new ReducerImpl(this.key));
      this.store = new StoreImpl(reducers);

      this.provider = new StoreValueProviderImpl();
    }

    [Fact]
    public void should_implement()
    {
      Assert.IsAssignableFrom<StoreConsumer>(this.provider);
      Assert.IsAssignableFrom<KeyConsumer>(this.provider);
      Assert.IsAssignableFrom<StoreValueProvider>(this.provider);
    }

    [Fact]
    public void should_return_default_T_value()
    {
      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Assert.Null(this.provider.get<string>());
      Assert.Equal(0, this.provider.get<int>());
      Assert.False(this.provider.get<bool>());
    }

    [Fact]
    public void should_return_string_value()
    {
      string value = "This is string value";
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      string actual = this.provider.get<string>();

      Assert.Equal(value, actual);
    }

    [Fact]
    public void should_throw_if_value_type_differ()
    {
      string value = "This is string value";
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      Action notInteger = () => this.provider.get<int>();

      Assert.Throws<InvalidOperationException>(notInteger);
    }

    [Fact]
    public void should_throw_if_key_is_missing_in_Store()
    {
      this.provider.setStore(this.store);
      this.provider.setKey("user");
      Action noKey = () => this.provider.get<int>();

      Assert.Throws<InvalidOperationException>(noKey);
    }

    [Fact]
    public void should_return_integer_value()
    {
      int value = 2938;
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      int actual = this.provider.get<int>();

      Assert.Equal(value, actual);
    }

    [Fact]
    public void should_return_boolean_value()
    {
      bool value = true;
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      bool actual = this.provider.get<bool>();

      Assert.Equal(value, actual);
    }

    [Fact]
    public void should_return_DateTime_value()
    {
      DateTime value = DateTime.UtcNow;
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      DateTime actual = this.provider.get<DateTime>();

      Assert.Equal(value, actual);
    }
  }
}