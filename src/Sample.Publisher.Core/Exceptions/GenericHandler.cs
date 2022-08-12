namespace Sample.Publisher.Core.Exceptions;

public sealed class GenericHandler : BaseHandler
{
    public override Type ExceptionType => typeof(Exception);

    protected override Task HandleExceptionInternalAsync(Exception exception)
    {
        Console.WriteLine("Handling Generic Exception");

        return Task.CompletedTask;
    }
}