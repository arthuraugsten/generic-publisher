using Microsoft.Extensions.DependencyInjection;
using Sample.Publisher.Core.Exceptions;

namespace Sample.Publisher.Core.SystemTests.Exceptions;

public sealed class ExceptionsExtensionsTests
{
    private readonly IServiceCollection _services = new ServiceCollection();

    [Fact]
    public void ShouldConfigureBasicExceptionHandler()
    {
        _services.ConfigureBasicExceptionHandler();

        var serviceProvider = _services.BuildServiceProvider();

        var handler = serviceProvider.GetRequiredService<BaseHandler>();
        handler.Should().BeOfType<GenericHandler>();

        serviceProvider.GetService<ArgumentHandler>().Should().BeOfType<ArgumentHandler>();
        serviceProvider.GetService<NullReferenceHandler>().Should().BeOfType<NullReferenceHandler>();
        serviceProvider.GetService<GenericHandler>().Should().BeOfType<GenericHandler>();
    }

    [Fact]
    public void ShouldConfigureDefaultExceptionHandler()
    {
        _services.ConfigureDefaultExceptionHandler();

        var serviceProvider = _services.BuildServiceProvider();

        var handler = serviceProvider.GetRequiredService<BaseHandler>();
        handler.Should().BeOfType<ArgumentHandler>();
        handler.Next.Should().BeOfType<NullReferenceHandler>();
        handler.Next!.Next.Should().BeOfType<GenericHandler>();
        handler.Next!.Next!.Next.Should().BeNull();

        serviceProvider.GetService<ArgumentHandler>().Should().BeOfType<ArgumentHandler>();
        serviceProvider.GetService<NullReferenceHandler>().Should().BeOfType<NullReferenceHandler>();
        serviceProvider.GetService<GenericHandler>().Should().BeOfType<GenericHandler>();
    }

    [Fact]
    public void ShouldAddGenericHandlerRuleWhenNothingWasConfiguredOnConfigureExceptionHandler()
    {
        _services.ConfigureExceptionHandler(options => { });

        var serviceProvider = _services.BuildServiceProvider();

        var handler = serviceProvider.GetRequiredService<BaseHandler>();
        handler.Should().BeOfType<GenericHandler>();
        handler.Next.Should().BeNull();

        serviceProvider.GetService<ArgumentHandler>().Should().BeOfType<ArgumentHandler>();
        serviceProvider.GetService<NullReferenceHandler>().Should().BeOfType<NullReferenceHandler>();
        serviceProvider.GetService<GenericHandler>().Should().BeOfType<GenericHandler>();
    }

    [Fact]
    public void ShouldSkipGenericHandlerRuleWhenConfigureExceptionHandler()
    {
        _services.ConfigureExceptionHandler(options =>
        {
            options.AddHandler<ArgumentHandler>();
            options.AddHandler<GenericHandler>();
        });

        var serviceProvider = _services.BuildServiceProvider();

        var handler = serviceProvider.GetRequiredService<BaseHandler>();
        handler.Should().BeOfType<ArgumentHandler>();
        handler.Next.Should().BeOfType<GenericHandler>();
        handler.Next!.Next.Should().BeNull();

        serviceProvider.GetService<ArgumentHandler>().Should().BeOfType<ArgumentHandler>();
        serviceProvider.GetService<NullReferenceHandler>().Should().BeOfType<NullReferenceHandler>();
        serviceProvider.GetService<GenericHandler>().Should().BeOfType<GenericHandler>();
    }

    [Fact]
    public void ShouldAddGenericHandlerWhenConfigureExceptionHandler()
    {
        _services.ConfigureExceptionHandler(options =>
        {
            options.AddHandler<ArgumentHandler>();
            options.AddHandler<NullReferenceHandler>();
        });

        var serviceProvider = _services.BuildServiceProvider();

        var handler = serviceProvider.GetRequiredService<BaseHandler>();
        handler.Should().BeOfType<ArgumentHandler>();
        handler.Next.Should().BeOfType<NullReferenceHandler>();
        handler.Next!.Next.Should().BeOfType<GenericHandler>();
        handler.Next!.Next!.Next.Should().BeNull();

        serviceProvider.GetService<ArgumentHandler>().Should().BeOfType<ArgumentHandler>();
        serviceProvider.GetService<NullReferenceHandler>().Should().BeOfType<NullReferenceHandler>();
        serviceProvider.GetService<GenericHandler>().Should().BeOfType<GenericHandler>();
    }
}
