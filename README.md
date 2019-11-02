# Redux mini for .NET

It's simple "application state" container implementation like [Redux for JavaScript](https://redux.js.org/).

## How does it work?

Application state container is just plain `IDictionary<string, object>` instance.

You can manage application state with just three methods of Store instance:

```csharp
public interface Store
{
  void Dispatch(Message message);

  IDictionary<string, object> GetState();

  Action Subscribe(Action<Message> handler);
}
```

"Send" `Message` to `Store` with `Dispatch` method:

```csharp

string type = "User";

User user = new User()
{ 
  Email = "joedoe@mail.local" 
};

Message message = new Message(type, user);

store.Dispatch(message);
```

Get application container state from `Store` with `GetState` method:

```csharp

IDictionary<string, object> state = store.GetState();

User user = state["User"] as User;

```

Know about application state change with `Subscribe` method:

```csharp
public class CustomHandler
{
  private Store store;
    
  public CustomHandler(Store store)
  {
    this.store = store;
  }

  public void Handle(Message message)
  {
    IDictionary<string, object> state = this.store.GetState();

    /// React state change here
  }
}

CustomHandler handler = new CustomHandler(store);

Action unsubscribe = store.Subscribe(handler.Handle);
```

### How to configure own application container?

Make a collection of `Reducer`-s with built-in tools:

```csharp
  
List<Reducer> reducers = new List<Reducer>();

reducers.Add(new ReducerImpl("Order"));
reducers.Add(new ReducerImpl("Payment"));
reducers.Add(new ReducerImpl("Withdrawal"));
reducers.Add(new ExceptionReducerImpl());

/// And pass it to constructor of 
/// built-in Store implementation
Store store = new StoreImpl(reducers);
```
And you are ready to play with your application container. As you can see it knows about four `Message` types:

 - "Order"
 - "Payment"
 - "Withdrawal"
 - "Exception"

It will ignore any other unknown type `Message`-s.

### Tool to get value from state container

There is built-in `StoreValueProvider` utility:

```csharp
Order order = new Order();
Message message = new Message("Order", order);
store.Dispatch(message);

StoreValueProvider provider = new StoreValueProviderImpl();

provider.setStore(store);
provider.setKey("Order");

Assert.Equal(order, provider.get<Order>());
Assert.Equal(order, provider.get(typeof(Order)));

```