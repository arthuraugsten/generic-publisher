using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Sample.Publisher.WebJob.Services.Exceptions;
using Xunit;

namespace Sample.Publisher.WebJob.Tests.Services.Exceptions;

public sealed class ExceptionHandlerFactoryTests
{
    private readonly Mock<ArgumentHandler> _argumentHandler = new();
    private readonly Mock<NullReferenceHandler> _nullReferenceHandler = new();
    private readonly Mock<GenericHandler> _genericHandler = new();

    private readonly ExceptionHandlerFactory _handler;

    public ExceptionHandlerFactoryTests()
    {
        _handler = new(_argumentHandler.Object, _nullReferenceHandler.Object, _genericHandler.Object);
    }

    public void ShouldConfigureTheOrderProperly()
    {
        var instance = _handler.Create();
        instance.Should().BeOfType<ArgumentHandler>();
    }
}