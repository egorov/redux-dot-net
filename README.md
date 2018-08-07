# .NET Redux implementation

## Action is Message

Here is no actions in this implementation. Instead I make `Message`-s. You can make it too with:

    MessageFactoryImpl factory = new MessageFactoryImpl();

    string type = "EXCEPTION";
    string payload = "Something goes wrong!";
    Message message = factory.Make(type, payload);

    Assert.Equal(type, message.Type);
    Assert.Equal(payload, message.Payload);


`MessageFactoryImpl` can validate `Message` payload content. Pass `PayloadValidators` to construct it:
    
    PayloadValidators validators = new PayloadValidatorsFactory().Make();
    MessageFactoryImpl factory = new MessageFactoryImpl(validators);

    string type = "EXCEPTION";

    Assert.Throws<ArgumentException>(() => this.factory.Make(type, "Something goes wrong!"));

    Exception payload = new Exception("Something goes wrong!");
    Message message = factory.Make(type, payload);

    Assert.Equal(type, message.Type);
    Assert.Equal(payload, message.Payload);

## Reducer generates new state

    IDictionary<string, object> before = new Dictionary<string, object>();
    string key = "SAMPLE";
    Reducer reducer = new ReducerImpl(key);
    Message message = new Message(key, "Content");
    IDictionary<string, object> after = reducer.Reduce(before, message);

    Assert.True(after.ContainsKey(key));
    Assert.Equal(message.Payload, after[key]);
    Assert.NotSame(before, after);
    
