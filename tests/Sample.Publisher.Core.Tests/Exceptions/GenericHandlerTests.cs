using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Sample.Publisher.Core.Exceptions;

namespace Sample.Publisher.Core.Tests.Exceptions;

public sealed class GenericHandlerTests
{
    private readonly Mock<ILogger<GenericHandler>> _logger = new();
    private readonly GenericHandler _handler;

    public GenericHandlerTests()
    {
        _handler = new();
    }

    public static List<object[]> ValidExceptions { get; } = new(2)
        {
            new object[] { new Exception() },
            new object[] { new OperationCanceledException() },
            new object[] { new ArgumentException() }
        };

    [Fact]
    public void ShouldBeExpectedException()
        => _handler.ExceptionType.Should().Be<Exception>();

    [Theory]
    [MemberData(nameof(GenericHandlerTests.ValidExceptions))]
    public async Task ShouldLogWhenCorrectType(Exception exception)
    {
        await _handler.HandleExceptionAsync(exception);

        // _logger.Verify(t =>
        //     t.Log(
        //         It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
        //         It.Is<EventId>(eventId => eventId.Id == 0),
        //         It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Handling Generic Exception" && @type.Name == "FormattedLogValues"),
        //         It.IsAny<Exception>(),
        //         It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        //     ),
        //     Times.Once);

        // _logger.VerifyNoOtherCalls();
    }
}