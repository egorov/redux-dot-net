namespace Redux
{
    public interface XReducer : Typed
    {
        object Reduce(object state, Message message);
    }
}