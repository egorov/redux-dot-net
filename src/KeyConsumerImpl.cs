using System;

namespace Redux
{
  public class KeyConsumerImpl : KeyConsumer
  {
    private string key;
    public string Key { 
      get {
        return this.key;
      } 
    }
    public void setKey(string key)
    {
      string message = "key can\'t be null, empty or whitespaces!";

      if(string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
        throw new ArgumentException(message);

      this.key = key;
    }

    public void validateKey()
    {
      string message = "Call setKey(string key) first!";

      if(string.IsNullOrEmpty(this.key) || string.IsNullOrWhiteSpace(this.key))
        throw new InvalidOperationException(message);
    }
  }
}