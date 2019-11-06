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
      return (T)this.get(typeof(T));
    }

    public object get(Type type)
    {
      object value = this.getValue();

      if(value == null)
        this.throwValueIsNullError(type);

      if(!type.IsAssignableFrom(value.GetType()))
        this.throwWrongValueTypeError(value, type);

      return value;
    }

    public bool canGet<T>()
    {
      return this.canGet(typeof(T));
    }

    public bool canGet(Type type)
    {
      IDictionary<string, object> state = this.storeConsumer.Store.GetState();

      if(!state.ContainsKey(this.keyConsumer.Key))
        return false;

      object value = state[this.keyConsumer.Key];

      if(value == null)
        return false;

      if(!type.IsAssignableFrom(value.GetType()))
        return false;

      return true;
    }

    private object getValue()
    {
      this.storeConsumer.validateStore();
      this.keyConsumer.validateKey();

      IDictionary<string, object> state = this.storeConsumer.Store.GetState();

      this.checkKeyExistsIn(state);

      object value = state[this.keyConsumer.Key];

      return value;
    }

    private void checkKeyExistsIn(IDictionary<string, object> state)
    {
      if(!state.ContainsKey(this.keyConsumer.Key))
      {
        string message = $"The cell with the specified key {this.keyConsumer.Key} is missing in Store!";

        throw new InvalidOperationException(message);
      }
    }

    private void throwValueIsNullError(Type type)
    {
      string message = 
          $"There is no value of {type} type found in cell with {this.keyConsumer.Key} key!";
      throw new InvalidOperationException(message);
    }

    private void throwWrongValueTypeError(object value, Type type)
    {
      string message = 
        $"Expected type of the value is {type}, but actual type is {value.GetType()}, in cell with specified key!";
      throw new InvalidOperationException(message);
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