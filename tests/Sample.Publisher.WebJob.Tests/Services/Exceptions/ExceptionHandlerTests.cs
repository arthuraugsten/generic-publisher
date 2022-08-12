using System;
using System.Threading.Tasks;
using FluentAssertions;
using Sample.Publisher.WebJob.Services.Exceptions;
using Xunit;

namespace Sample.Publisher.WebJob.Tests.Services.Exceptions;

public sealed class ExceptionHandlerTests
{
    private readonly ExceptionHandlerMock _handler = new();
    private readonly NextHandlerMock _nextHandler = new();

    [Fact]
    public async Task ShouldSkipWhenIncorrectTypeAndSkipNullNextInstance()
    {
        await _handler.HandleExceptionAsync(new Exception());
        _handler.Executed.Should().BeFalse();
    }

    [Fact]
    public async Task ShouldSkipWhenIncorrectTypeAndExecuteNextInstance()
    {
        _handler.SetNext(_nextHandler);

        await _handler.HandleExceptionAsync(new NullReferenceException());

        _handler.Executed.Should().BeFalse();
        _nextHandler.Executed.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldExecuteWhenCorrectTypeAndSkipNullNextInstance()
    {
        await _handler.HandleExceptionAsync(new ArgumentException());
        _handler.Executed.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldExecuteWhenCorrectTypeAndSkipNextInstance()
    {
        _handler.SetNext(_nextHandler);

        await _handler.HandleExceptionAsync(new ArgumentException());

        _handler.Executed.Should().BeTrue();
        _nextHandler.Executed.Should().BeFalse();
    }
}

public sealed class ExceptionHandlerMock : ExceptionHandler
{
    public bool Executed { get; private set; } = false;
    public override Type ExceptionType => typeof(ArgumentException);

    protected override Task HandleExceptionInternalAsync(Exception exception)
    {
        Executed = true;
        return Task.CompletedTask;
    }
}

public sealed class NextHandlerMock : ExceptionHandler
{
    public bool Executed { get; private set; } = false;
    public override Type ExceptionType => typeof(NullReferenceException);

    protected override Task HandleExceptionInternalAsync(Exception exception)
    {
        Executed = true;
        return Task.CompletedTask;
    }
}