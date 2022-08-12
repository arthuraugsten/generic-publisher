using System.Diagnostics;

namespace Sample.Publisher.Core.Contexts;

public interface IRequestContext
{
    long GetElapsedTime();
}

public sealed class RequestContext : IRequestContext
{
    private readonly Stopwatch _stopwatch = new();

    public long GetElapsedTime() => _stopwatch.ElapsedMilliseconds;
}