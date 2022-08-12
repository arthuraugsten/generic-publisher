namespace Sample.Publisher.Core.Exceptions;

public sealed class ArgumentHandler : BaseHandler
{
    public override Type ExceptionType => typeof(ArgumentException);

    protected override Task HandleExceptionInternalAsync(Exception exception)
    {
        Console.WriteLine("Handling Argument Exception");

        return Task.CompletedTask;
    }
}