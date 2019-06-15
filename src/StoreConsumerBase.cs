using System;

namespace Redux
{
  public abstract class StoreConsumerBase : StoreConsumer
  {
    protected Store store;
    public void setStore(Store store)
    {
      if(store == null)
        throw new ArgumentNullException(nameof(store));
      
      this.store = store;
    }

    protected void validateStore()
    {
      if(this.store == null)
        throw new InvalidOperationException("Call setStore(Store store) first!");
    }
  }
}