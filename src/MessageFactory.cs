namespace Redux
{
    public interface MessageFactory
    {
        Message Make(string type, object payload);
    }
}