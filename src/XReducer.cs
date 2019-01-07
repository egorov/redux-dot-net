namespace Redux
{
    public interface XReducer
    {
        object Reduce(object state, Message message);
    }
}