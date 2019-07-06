using System;

namespace Redux
{
  public interface StoreValueProvider : StoreConsumer, KeyConsumer
  {
    T get<T>();
    object get(Type type);
  }
}