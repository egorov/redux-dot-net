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

      this.checkKeyExistsIn(state);

      object value = state[this.key];

      if(value == null)
        return default(T);

      if(!(value is T))
      {
        string message = 
          $"Expected type of the value is {typeof(T)}, but actual type is {value.GetType()}, in cell with specified key!";
        throw new InvalidOperationException(message);
      }

      return (T)value;
    }

    private void checkKeyExistsIn(IDictionary<string, object> state)
    {
      if(!state.ContainsKey(this.key))
      {
        string message = $"The cell with the specified key {this.key} is missing in Store!";

        throw new InvalidOperationException(message);
      }
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