using System;
using System.Collections.Generic;

namespace Redux
{
  public class StoreValueProviderImpl : StoreValueProvider
  {
    private StoreConsumerImpl storeConsumer;
    public StoreValueProviderImpl()
    {
      this.storeConsumer = new StoreConsumerImpl();
    }
    public T get<T>()
    {
      this.storeConsumer.validateStore();
      this.validateKey();

      IDictionary<string, object> state = this.storeConsumer.Store.GetState();

      if(!state.ContainsKey(this.key))
        throw new InvalidOperationException($"There is no key {this.key} in Store!");

      object value = state[this.key];

      if(!(value is T))
        throw new InvalidOperationException($"There is not value of {typeof(T)} in state[{this.key}]!");

      return (T)value;
    }

    private string key;
    public void setKey(string key)
    {
      this.key = key;

      this.validateKey();
    }

    private void validateKey()
    {
      string message = "key can\'t be null, empty or whitespaces!";

      if(string.IsNullOrEmpty(this.key))
        throw new ArgumentNullException(message);

      if(string.IsNullOrWhiteSpace(this.key))
        throw new ArgumentNullException(message);
    }

    public void setStore(Store store)
    {
      this.storeConsumer.setStore(store);
    }
  }
}