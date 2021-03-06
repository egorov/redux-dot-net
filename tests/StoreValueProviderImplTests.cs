using Xunit;
using System;
using System.Collections.Generic;
using Redux;
using System.Linq.Expressions;
using System.Reflection;

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

    public static IList<object[]> typedCanGetTestsData = new List<object[]>()
    {
      new object[] { typeof(string), "This is string" },
      new object[] { typeof(int), 205482 },
      new object[] { typeof(double), 38.95d },
      new object[] { typeof(float), 38.95f },
      new object[] { typeof(decimal), 38.95m },
      new object[] { typeof(bool), false },
      new object[] { typeof(DateTime), DateTime.UtcNow },
      new object[] { typeof(ExceptionValueValidator), new ExceptionValueValidator() },
      new object[] { typeof(IEnumerable<int>), new List<int>() }
    };

    [Theory]
    [MemberData(nameof(typedCanGetTestsData))]
    public void typed_get_should_return(Type type, object value)
    {
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);
      object actual = this.provider.get(type);

      Assert.Equal(value, actual);
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int))]
    [InlineData(typeof(bool))]
    [InlineData(typeof(ExceptionValueValidator))]
    [InlineData(typeof(DateTime))]
    public void typed_get_should_throw_if_in_Store_no_value_of(Type type)
    {
      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Action getValue = () => this.provider.get(type);
      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(getValue);

      string expected = 
        $"There is no value of {type.FullName} type found in cell with {this.key} key!";
      Assert.Equal(expected, error.Message);
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

    [Theory]
    [MemberData(nameof(typedCanGetTestsData))]
    public void typed_canGet_should_return_true(Type type, object value)
    {
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);

      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Assert.True(this.provider.canGet(type));
    }

    [Theory]
    [InlineData(typeof(string))]
    [InlineData(typeof(int))]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(float))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(ExceptionValueValidator))]
    public void typed_canGet_should_return_false(Type type)
    {
      this.provider.setStore(this.store);
      this.provider.setKey(this.key);

      Assert.False(this.provider.canGet(type));
    }

    public static IList<object[]> genericGetTestsData = new List<object[]>()
    {
      new object[] { "This is string", typeof(string) },
      new object[] { 39827, typeof(int) },
      new object[] { 837.228d, typeof(double) },
      new object[] { 2819.372f, typeof(float) },
      new object[] { 1028.32m, typeof(decimal) },
      new object[] { DateTime.UtcNow, typeof(DateTime) },
      new object[] { new ExceptionValueValidator(), typeof(ValueValidator) },
      new object[] { new ExceptionValueValidator(), typeof(ExceptionValueValidator) },
      new object[] { new List<int>(), typeof(IEnumerable<int>) },
      new object[] { new List<int>(), typeof(List<int>) }
    };

    [Theory]
    [MemberData(nameof(genericGetTestsData))]
    public void generic_get_should_return(object value, Type type)
    {
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);
      
      this.provider.setKey(this.key);
      this.provider.setStore(this.store);

      MethodInfo methodInfo = this.getStoreValueProviderGenericGet(type);

      object result = methodInfo.Invoke(this.provider, null);

      Assert.Equal(value, result);
    }

    private MethodInfo getStoreValueProviderGenericGet(Type type)
    {
      MethodInfo method = 
        this.provider.GetType().GetMethod("get", new Type[] {});

      MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { type });

      return genericMethod;
    }

    public static IList<object[]> genericCanGetTestsData = new List<object[]>()
    {
      new object[] { "This is string", (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<string>()) },
      new object[] { 39827, (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<int>()) },
      new object[] { 837.228d, (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<double>()) },
      new object[] { 2819.372f, (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<float>()) },
      new object[] { 1028.32m, (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<decimal>()) },
      new object[] { DateTime.UtcNow, (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<DateTime>()) },
      new object[] { new ExceptionValueValidator(), (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<ValueValidator>()) },
      new object[] { new ExceptionValueValidator(), (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<ExceptionValueValidator>()) },
      new object[] { new List<int>(), (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<IEnumerable<int>>()) },
      new object[] { new List<int>(), (Expression<Func<StoreValueProvider, bool>>)(p => p.canGet<List<int>>()) }
    };

    [Theory]
    [MemberData(nameof(genericCanGetTestsData))]
    public void generic_canGet_should_return_true(
      object value, 
      Expression<Func<StoreValueProvider, bool>> expression
    )
    {
      Message message = new Message(this.key, value);
      this.store.Dispatch(message);
      
      this.provider.setKey(this.key);
      this.provider.setStore(this.store);

      Assert.True(expression.Compile().Invoke(this.provider));
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
    public void typed_get_should_throw_if_store_was_not_set()
    {
      this.provider.setKey(this.key);
      Action notConfigured = () => this.provider.get(typeof(int));

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(notConfigured);
      
      string errorMessage = 
        "Call setStore(Store store) first!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void typed_get_should_throw_if_key_was_not_set()
    {
      this.provider.setStore(this.store);
      Action notConfigured = () => this.provider.get(typeof(int));

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(notConfigured);
      
      string errorMessage = 
        "Call setKey(string key) first!";
      Assert.Equal(errorMessage, error.Message);
    }

    [Fact]
    public void generic_get_should_throw_if_store_was_not_set()
    {
      this.provider.setKey(this.key);
      Action notConfigured = () => this.provider.get<int>();

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(notConfigured);
      
      string errorMessage = 
        "Call setStore(Store store) first!";
      Assert.Equal(errorMessage, error.Message);
    }
  }
}