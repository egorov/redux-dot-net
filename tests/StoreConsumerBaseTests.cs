using Xunit;
using Redux;
using System.Collections.Generic;
using System;

namespace tests
{
  public class StoreConsumerSample : StoreConsumerBase
  {
    public Store Store {
      get {
        return base.store;
      }
    }

    public new void validateStore()
    {
      base.validateStore();
    }
  }
  public class StoreConsumerBaseTests
  {
    private StoreConsumerSample consumer;
    private Store store;

    public StoreConsumerBaseTests()
    {
      this.consumer = new StoreConsumerSample();
      this.store = this.makeStore();
    }

    [Fact]
    public void should_set_Store()
    {
      this.consumer.setStore(this.store);

      Assert.Equal(this.store, this.consumer.Store);
    }

    [Fact]
    public void setStore_should_throw()
    {
      Action nullStore = () => this.consumer.setStore(null);

      Assert.Throws<ArgumentNullException>(nullStore);
    }

    [Fact]
    public void validateStore_should_throw()
    {
      Action nullStore = () => this.consumer.validateStore();

      Assert.Throws<InvalidOperationException>(nullStore);
    }

    public Store makeStore()
    {
      HashSet<Reducer> reducers = new HashSet<Reducer>();

      Store store = new StoreImpl(reducers);

      return store;
    }
  }
}