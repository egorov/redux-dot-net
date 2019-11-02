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
    private StoreValueProvider provider;

    public StoreValueProviderImplTests()
    {
      this.key = "object";

      this.store = this.makeStore();

      this.provider = new StoreValueProviderImpl();
    }

    private Store makeStore()
    {
      HashSet<Reducer> reducers = new HashSet<Reducer>();

      reducers.Add(new ReducerImpl(this.key));

      return new StoreImpl(reducers);
    }

    [Fact]
    public void generic_get_should_return_string_value()
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
    public void generic_get_should_return_integer_value()
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
    public void generic_get_should_return_boolean_value()
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
    public void generic_get_should_return_DateTime_value()
    {
      DateTime value = DateTime.UtcNow;
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      DateTime actual = this.provider.get<DateTime>();

      Assert.Equal(value, actual);
    }

    [Fact]
    public void generic_get_should_return_object_instance_value()
    {
      ExceptionValueValidator value = new ExceptionValueValidator();
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      ExceptionValueValidator actual = 
        this.provider.get<ExceptionValueValidator>();

      Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData("This is string value")]
    [InlineData(2098347)]
    [InlineData(38.95d)]
    [InlineData(false)]
    public void typed_get_should_return(object value)
    {
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      object actual = this.provider.get(value.GetType());

      Assert.Equal(value, actual);
    }

    [Fact]
    public void typed_get_should_return_DateTime_value()
    {
      DateTime value = DateTime.UtcNow;
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      object actual = this.provider.get(typeof(DateTime));

      Assert.Equal(value, actual);
    }

    [Fact]
    public void typed_get_should_return_object_instance_value()
    {
      ExceptionValueValidator value = new ExceptionValueValidator();
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      object actual = this.provider.get(typeof(ExceptionValueValidator));

      Assert.Equal(value, actual);
    }

    [Fact]
    public void generic_get_should_throw_if_value_type_differ()
    {
      string value = "This is string value";
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      Action notInteger = () => this.provider.get<int>();

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(notInteger);
      
      string errorMessage = 
        "Expected type of the value is System.Int32, but actual type is System.String, in cell with specified key!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void typed_get_should_throw_if_value_type_differ()
    {
      string value = "This is string value";
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      Action notInteger = () => this.provider.get(typeof(int));

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(notInteger);
      
      string errorMessage = 
        "Expected type of the value is System.Int32, but actual type is System.String, in cell with specified key!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void generic_get_should_throw_if_key_is_missing_in_Store()
    {
      this.provider.setStore(this.store);
      this.provider.setKey("user");
      Action noKey = () => this.provider.get<int>();

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(noKey);
      
      string errorMessage = 
        "The cell with the specified key user is missing in Store!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void typed_get_should_throw_if_key_is_missing_in_Store()
    {
      this.provider.setStore(this.store);
      this.provider.setKey("user");
      Action noKey = () => this.provider.get(typeof(int));

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(noKey);
      
      string errorMessage = 
        "The cell with the specified key user is missing in Store!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void generic_get_should_throw_if_value_is_null()
    {
      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Action getValue = () => this.provider.get<string>();
      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(getValue);
      string expected = 
        $"There is no value of System.String type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);

      getValue = () => this.provider.get<int>();
      error = Assert.Throws<InvalidOperationException>(getValue);
      expected = 
        $"There is no value of System.Int32 type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);

      getValue = () => this.provider.get<bool>();
      error = Assert.Throws<InvalidOperationException>(getValue);
      expected = 
        $"There is no value of System.Boolean type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);
    }

    [Fact]
    public void typed_get_should_throw_if_value_is_null()
    {
      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Action getValue = () => this.provider.get(typeof(string));
      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(getValue);
      string expected = 
        $"There is no value of System.String type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);

      getValue = () => this.provider.get(typeof(int));
      error = Assert.Throws<InvalidOperationException>(getValue);
      expected = 
        $"There is no value of System.Int32 type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);

      getValue = () => this.provider.get(typeof(bool));
      error = Assert.Throws<InvalidOperationException>(getValue);
      expected = 
        $"There is no value of System.Boolean type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);
    }
  }
}