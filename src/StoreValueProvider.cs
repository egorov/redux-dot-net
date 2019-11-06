using System;

namespace Redux
{
  public interface StoreValueProvider : StoreConsumer, KeyConsumer
  {
    T get<T>();
    bool canGet<T>();
    object get(Type type);
    bool canGet(Type type);    
  }
}