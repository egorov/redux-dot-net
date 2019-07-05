using System;
using System.Collections.Generic;

namespace Redux
{
  public class StoreValueProviderImpl : StoreValueProvider
  {
    private KeyConsumerImpl keyConsumer;
    private StoreConsumerImpl storeConsumer;
    public StoreValueProviderImpl()
    {
      this.keyConsumer = new KeyConsumerImpl();
      this.storeConsumer = new StoreConsumerImpl();
    }
    public T get<T>()
    {
      this.storeConsumer.validateStore();
      this.keyConsumer.validateKey();

      IDictionary<string, object> state = this.storeConsumer.Store.GetState();

      this.checkKeyExistsIn(state);

      object value = state[this.keyConsumer.Key];

      if(value == null)
      {
        string message = 
          $"There is no value of {typeof(T)} type found in cell with {this.keyConsumer.Key} key!";
        throw new InvalidOperationException(message);
      }

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
      if(!state.ContainsKey(this.keyConsumer.Key))
      {
        string message = $"The cell with the specified key {this.keyConsumer.Key} is missing in Store!";

        throw new InvalidOperationException(message);
      }
    }

    public void setKey(string key)
    {
      this.keyConsumer.setKey(key);
    }

    public void setStore(Store store)
    {
      this.storeConsumer.setStore(store);
    }
  }
}