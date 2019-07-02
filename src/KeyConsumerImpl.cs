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
      this.key = key;

      this.validateKey();
    }

    public void validateKey()
    {
      string message = "key can\'t be null, empty or whitespaces!";

      if(string.IsNullOrEmpty(this.key))
        throw new ArgumentNullException(message);

      if(string.IsNullOrWhiteSpace(this.key))
        throw new ArgumentNullException(message);
    }
  }
}