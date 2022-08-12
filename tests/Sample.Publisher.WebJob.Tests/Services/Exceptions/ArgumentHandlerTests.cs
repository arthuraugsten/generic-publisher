using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Sample.Publisher.WebJob.Services.Exceptions;
using Xunit;

namespace Sample.Publisher.WebJob.Tests.Services.Exceptions;

public sealed class ArgumentHandlerTests
{
    private readonly Mock<ILogger<ArgumentHandler>> _logger = new();
    private readonly ArgumentHandler _handler;

    public ArgumentHandlerTests()
    {
        _handler = new(_logger.Object);
    }

    [Fact]
    public void ShouldBeExpectedException()
        => _handler.ExceptionType.Should().Be<ArgumentException>();

    [Fact]
    public async Task ShouldNotLogWhenIncorrectType()
    {
        await _handler.HandleExceptionAsync(new Exception());
        _logger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldLogWhenCorrectType()
    {
        await _handler.HandleExceptionAsync(new ArgumentException());

        _logger.Verify(t =>
            t.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
                It.Is<EventId>(eventId => eventId.Id == 0),
                It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Handling Argument Exception" && @type.Name == "FormattedLogValues"),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);

        _logger.VerifyNoOtherCalls();
    }
}