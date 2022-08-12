namespace Sample.Publisher.Core.Exceptions;

public abstract class BaseHandler
{
    internal BaseHandler? Next;

    public abstract Type ExceptionType { get; }

    public void SetNext(BaseHandler successor) => Next = successor;

    protected abstract Task HandleExceptionInternalAsync(Exception exception);

    public async Task HandleExceptionAsync(Exception exception)
    {
        if (IsExpectedException(exception))
            await HandleExceptionInternalAsync(exception);
        else
            await (Next?.HandleExceptionAsync(exception) ?? Task.CompletedTask);
    }

    private bool IsExpectedException(Exception exception)
    {
        var type = exception.GetType();
        return type == ExceptionType || type.IsSubclassOf(ExceptionType);
    }
}