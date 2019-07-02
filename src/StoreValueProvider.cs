namespace Redux
{
  public interface StoreValueProvider : StoreConsumer, KeyConsumer
  {
    T get<T>();
  }
}