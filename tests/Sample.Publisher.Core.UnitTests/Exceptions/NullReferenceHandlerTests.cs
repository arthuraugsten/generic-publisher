using Microsoft.Extensions.Logging;
using Sample.Publisher.Core.Exceptions;

namespace Sample.Publisher.Core.Tests.Exceptions;

public sealed class NullReferenceHandlerTests
{
    private readonly Mock<ILogger<NullReferenceHandler>> _logger = new();
    private readonly NullReferenceHandler _handler = new();

    [Fact]
    public void ShouldBeExpectedException()
        => _handler.ExceptionType.Should().Be<NullReferenceException>();

    [Fact]
    public async Task ShouldNotLogWhenIncorrectType()
    {
        await _handler.HandleExceptionAsync(new Exception());
        _logger.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task ShouldLogWhenCorrectType()
    {
        await _handler.HandleExceptionAsync(new NullReferenceException());

        //_logger.Verify(t =>
        //    t.Log(
        //        It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
        //        It.Is<EventId>(eventId => eventId.Id == 0),
        //        It.Is<It.IsAnyType>((@object, @type) => @object.ToString() == "Handling Null Reference Exception" && @type.Name == "FormattedLogValues"),
        //        It.IsAny<Exception>(),
        //        It.IsAny<Func<It.IsAnyType, Exception?, string>>()
        //    ),
        //    Times.Once);

        //_logger.VerifyNoOtherCalls();
    }
}