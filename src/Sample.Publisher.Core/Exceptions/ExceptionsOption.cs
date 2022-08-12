namespace Sample.Publisher.Core.Exceptions;

public class ExceptionsOption
{
    protected readonly Queue<Type> Queue = new();
    internal bool ContainsGenericHandler { get; private set; } = false;

    public void AddHandler<T>() where T : BaseHandler
    {
        var type = typeof(T);

        if (Queue.Contains(type))
        {
            return;
        }

        ContainsGenericHandler = ContainsGenericHandler || type == typeof(GenericHandler);
        Queue.Enqueue(type);
    }

    internal Type? GetItem()
    {
        if (!Queue.TryDequeue(out var item))
        {
            return default;
        }

        return item;
    }
}
