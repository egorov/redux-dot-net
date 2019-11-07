using Xunit;
using System;
using System.Collections.Generic;
using Redux;

namespace tests
{
  public class StoreImplTests
  {
    private List<Message> messages;
    private MessageFactoryImpl factory;
    private IDictionary<string, object> state;
    private HashSet<Reducer> reducers;
    private MessageValidator messageValidator;
    private StoreImpl store;

    public StoreImplTests()
    {
      this.messages = new List<Message>();
      this.factory = new MessageFactoryImpl();
      this.state = new Dictionary<string, object>();
      this.messageValidator = new MessageValidatorImpl();
      this.reducers = new HashSet<Reducer>();
      this.reducers.Add(new ReducerImpl("SAMPLE", this.messageValidator));
      this.reducers.Add(new ExceptionReducerImpl());
      this.store = new StoreImpl(this.reducers, this.state, this.messageValidator);
    }

    [Fact]
    public void constructor_should_throw()
    {
      Assert.Throws<ArgumentNullException>(() => new StoreImpl(null));
      Assert.Throws<ArgumentNullException>(() => new StoreImpl(null, this.state, this.messageValidator));
      Assert.Throws<ArgumentNullException>(() => new StoreImpl(this.reducers, null, this.messageValidator));
      Assert.Throws<ArgumentNullException>(() => new StoreImpl(this.reducers, this.state, null));
    }

    [Fact]
    public void it_should_update_initial_state()
    {
      Message message = this.factory.Make("SAMPLE", "Content");
      this.store.Dispatch(message);

      Assert.Same(this.state, this.store.GetState());
    }

    [Fact]
    public void it_should_update_state_with_SAMPLE_message_type()
    {
      Message message = this.factory.Make("SAMPLE", "Content");
      this.store.Dispatch(message);

      Assert.Equal(2, this.state.Count);
      Assert.Equal("Content", this.state["SAMPLE"]);
      Assert.Null(this.state["Exception"]);
    }

    [Fact]
    public void it_should_not_update_state_if_there_is_no_Reducer_for_message_type()
    {
      Message message = this.factory.Make("ERROR", "Something goes wrong!");
      this.store.Dispatch(message);

      Assert.Equal(2, this.state.Count);
      Assert.Null(this.state["SAMPLE"]);
      Assert.Null(this.state["Exception"]);
      Assert.False(this.state.ContainsKey("ERROR"));
    }

    [Fact]
    public void it_should_update_state_with_Exception_message_type()
    {
      Exception payload = new Exception();
      Message message = this.factory.Make("Exception", payload);
      this.store.Dispatch(message);

      Assert.Equal(2, this.state.Count);
      Assert.Null(this.store.GetState()["SAMPLE"]);

      IEnumerable<Exception> exceptions =
          this.store.GetState()["Exception"] as IEnumerable<Exception>;

      Assert.Contains(payload, exceptions);
    }

    [Fact]
    public void it_should_update_state_value_after_Dispatch_same_Message_type()
    {
      Message message = this.factory.Make("SAMPLE", "Content");
      this.TestDispatch(message);

      message = this.factory.Make("SAMPLE", "Another content");
      this.TestDispatch(message);
    }

    private void TestDispatch(Message message)
    {
      this.store.Dispatch(message);

      Assert.Equal(2, this.store.GetState().Count);
      Assert.True(this.store.GetState().ContainsKey(message.Type));
      Assert.Equal(message.Payload, this.store.GetState()[message.Type]);
    }

    [Fact]
    public void it_should_always_return_initial_state_reference()
    {
      Message message = this.factory.Make("SAMPLE", "Content");
      this.store.Dispatch(message);
      IDictionary<string, object> before = this.store.GetState();

      message = this.factory.Make("SAMPLE", "Another content");
      this.store.Dispatch(message);
      IDictionary<string, object> after = this.store.GetState();

      Assert.Same(before, after);
    }

    [Fact]
    public void it_should_throw_on_Dispatch()
    {
      Assert.Throws<ArgumentNullException>(() => this.store.Dispatch(null));
      Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(null, "content")));
      Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message("", "content")));
      Assert.Throws<ArgumentException>(() => this.store.Dispatch(new Message(" ", "content")));
    }

    [Fact]
    public void it_should_not_call_subscriber_Action_if_no_Reducer_for_message_type()
    {
      Action unsubscribe = this.store.Subscribe(this.Handle);
      Message message = this.factory.Make("UNKNOWN", "Content");
      this.store.Dispatch(message);

      Assert.Empty(this.messages);
    }

    private void Handle(Message message)
    {
      this.messages.Add(message);
    }

    [Fact]
    public void it_should_call_subscriber_Action_if_there_is_Reducer_for_message_type()
    {
      Action unsubscribe = this.store.Subscribe(this.Handle);
      Message message = this.factory.Make("SAMPLE", "Content");
      this.TestSubscribersHandle(message);
    }

    private void TestSubscribersHandle(Message message)
    {
      this.store.Dispatch(message);

      Assert.Single(this.messages);
      Assert.Contains(message, this.messages);
    }

    [Fact]
    public void it_should_not_call_subscriber_Action_after_unsubscribe()
    {
      Action unsubscribe = this.store.Subscribe(this.Handle);
      Message message = this.factory.Make("SAMPLE", "Content");
      this.TestSubscribersHandle(message);

      unsubscribe();
      this.TestSubscribersHandle(message);
    }

    [Fact]
    public void constructor_should_throw_same_Type_reducers_passed_in_collection()
    {
      HashSet<Reducer> reducers = new HashSet<Reducer>() 
      {
        new ReducerImpl("Stub"),
        new ReducerImpl("Stub")
      };
      Assert.Equal(2, reducers.Count);

      Action construct = () => new StoreImpl(reducers);

      InvalidOperationException error = 
        Assert.Throws<InvalidOperationException>(construct);

      string message = "More than one reducers of Stub type found!";
      Assert.Equal(message, error.Message);
    }
  }
}