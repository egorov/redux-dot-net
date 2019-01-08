namespace Redux
{
    public interface Reducer : Typed
    {
        object Reduce(object state, Message message);
    }
}