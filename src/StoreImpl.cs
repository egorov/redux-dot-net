using System;
using System.Linq;
using System.Collections.Generic;

namespace Redux
{
  public class StoreImpl : Store
  {
    private IDictionary<string, object> state;
    private IEnumerable<Reducer> reducers;
    private MessageValidator messageValidator;
    private HashSet<Action<Message>> subscribers;

    public StoreImpl(IEnumerable<Reducer> reducers)
        : this(reducers,
            new Dictionary<string, object>(),
            new MessageValidatorImpl())
    { }
    public StoreImpl(IEnumerable<Reducer> reducers,
        IDictionary<string, object> state,
        MessageValidator messageValidator)
    {
      this.validate(reducers);

      if (state == null)
        throw new ArgumentNullException("state");

      if (messageValidator == null)
        throw new ArgumentNullException("messageValidator");

      this.state = state;
      this.reducers = reducers;
      this.messageValidator = messageValidator;
      this.subscribers = new HashSet<Action<Message>>();
      this.InitializeState();
    }

    private void validate(IEnumerable<Reducer> reducers)
    {
      if (reducers == null)
        throw new ArgumentNullException("reducers");

      foreach (Reducer reducer in reducers)
      {
        int count = reducers.Count(i => i.Type == reducer.Type);

        if(count == 1)
          continue;

        string message = 
            $"More than one reducers of {reducer.Type} type found!";
        
        throw new InvalidOperationException(message);
      }
    }

    private void InitializeState()
    {
      foreach (Reducer reducer in this.reducers)
      {
        if (this.state.ContainsKey(reducer.Type))
          continue;

        this.state.Add(reducer.Type, null);
      }
    }
    public void Dispatch(Message message)
    {
      this.messageValidator.Validate(message);

      if (!this.state.ContainsKey(message.Type))
        return;

      foreach (Reducer reducer in this.reducers)
      {
        this.state[message.Type] =
            reducer.Reduce(this.state[message.Type], message);
      }

      this.NotifySubscribers(message);
    }

    public IDictionary<string, object> GetState()
    {
      return this.state;
    }

    public Action Subscribe(Action<Message> handler)
    {
      this.subscribers.Add(handler);

      return () =>
      {
        this.subscribers.Remove(handler);
      };
    }

    private void NotifySubscribers(Message message)
    {

      foreach (Action<Message> handle in this.subscribers)
      {
        handle(message);
      }
    }
  }
}