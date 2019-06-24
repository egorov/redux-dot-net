using System;

namespace Redux
{
  public class StoreConsumerImpl : StoreConsumer
  {
    private Store store;
    public Store Store {
      get {
        return this.store;
      }
    }
    public void setStore(Store store)
    {
      if(store == null)
        throw new ArgumentNullException(nameof(store));
      
      this.store = store;
    }

    public void validateStore()
    {
      if(this.store == null)
        throw new InvalidOperationException("Call setStore(Store store) first!");
    }
  }
}