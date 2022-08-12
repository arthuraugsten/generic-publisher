using Microsoft.Extensions.Options;
using Sample.Publisher.Core.Exceptions;

namespace Sample.Publisher.Core.Tests.Exceptions;

public sealed class ExceptionsOptionTests
{
    private readonly CustomOption _options = new();

    [Fact]
    public void ShouldCreateOptionsWithEmptyQueue()
        => _options.Count.Should().Be(0);

    [Fact]
    public void ShouldCreateOptionsWithContainsGenericHandlerEqualFalse()
        => _options.ContainsGenericHandler.Should().BeFalse();

    [Fact]
    public void ShouldIgnoreHandlerWhenQueueAlreadyContains()
    {
        _options.AddHandler<CustomHandler>();

        _options.ContainsGenericHandler.Should().BeFalse();
        _options.ContainsType<CustomHandler>();
        _options.Count.Should().Be(1);

        _options.AddHandler<CustomHandler>();

        _options.ContainsGenericHandler.Should().BeFalse();
        _options.ContainsType<CustomHandler>();
        _options.Count.Should().Be(1);
    }

    [Fact]
    public void ShouldAddMoreThanOnreHandler()
    {
        _options.AddHandler<CustomHandler>();

        _options.ContainsGenericHandler.Should().BeFalse();
        _options.ContainsType<CustomHandler>();
        _options.Count.Should().Be(1);

        _options.AddHandler<GenericHandler>();

        _options.ContainsGenericHandler.Should().BeTrue();
        _options.ContainsType<CustomHandler>();
        _options.ContainsType<GenericHandler>();
        _options.Count.Should().Be(2);
    }

    [Fact]
    public void ShouldChangePropertyWhenTypeIsGenericHandler()
    {
        _options.AddHandler<GenericHandler>();

        _options.ContainsGenericHandler.Should().BeTrue();
        _options.ContainsType<GenericHandler>();
        _options.Count.Should().Be(1);
    }

    [Fact]
    public void ShouldReturnNullWhenQueueIsEmpty()
    {
        _options.Count.Should().Be(0);
        
        var item = _options.GetItem();

        item.Should().BeNull();
        _options.Count.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnAddedTypeAndRemoveFromQueue()
    {
        _options.AddHandler<GenericHandler>();
        var item = _options.GetItem();

        item.Should().Be(typeof(GenericHandler));
        _options.Count.Should().Be(0);
    }

    private sealed class CustomOption : ExceptionsOption
    {
        public int Count => Queue.Count;

        public bool ContainsType<T>() => Queue.Contains(typeof(T));
    }

    private sealed class CustomHandler : BaseHandler
    {
        public override Type ExceptionType => typeof(Exception);

        protected override Task HandleExceptionInternalAsync(Exception exception)
            => Task.CompletedTask;
    }
}
