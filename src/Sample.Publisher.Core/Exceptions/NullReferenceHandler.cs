namespace Sample.Publisher.Core.Exceptions;

public sealed class NullReferenceHandler : BaseHandler
{
    public override Type ExceptionType => typeof(NullReferenceException);

    protected override Task HandleExceptionInternalAsync(Exception exception)
    {
        Console.WriteLine("Handling Null Reference Exception");

        return Task.CompletedTask;
    }
}