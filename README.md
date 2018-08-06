# .NET Redux implementation

## Action is Message

Here is no actions in this implementations. Instead I make `Message`-s. You can make it too with:

    MessageFactoryImpl factory = new MessageFactoryImpl();

    string type = "EXCEPTION";
    string payload = "Something goes wrong!";
    Message message = factory.Make(type, payload);

    Assert.Equal(type, message.Type);
    Assert.Equal(payload, message.Payload);


`MessageFactoryImpl` can validate `Message` payload content. Pass `PayloadValidators` to construct it:
    
    MessageFactoryImpl factory = new MessageFactoryImpl(new PayloadValidatorsFactory().Make());

    string type = "EXCEPTION";

    Assert.Throws<ArgumentException>(() => this.factory.Make(type, "Something goes wrong!"));

    Exception payload = new Exception("Something goes wrong!");
    Message message = factory.Make(type, payload);

    Assert.Equal(type, message.Type);
    Assert.Equal(payload, message.Payload);
    
