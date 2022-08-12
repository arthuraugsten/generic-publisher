using Microsoft.Extensions.DependencyInjection;
using Sample.Publisher.Core.Exceptions;

namespace Sample.Publisher.Core.Tests.Exceptions;

public sealed class ExceptionsExtensionsTests
{
    private readonly Mock<IServiceCollection> _services = new();

    private void ValidateDefaultServices()
    {
        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(ArgumentHandler) &&
                    s.ImplementationType == typeof(ArgumentHandler) &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(NullReferenceHandler) &&
                    s.ImplementationType == typeof(NullReferenceHandler) &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(GenericHandler) &&
                    s.ImplementationType == typeof(GenericHandler) &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );
    }

    [Fact]
    public void ShouldThrowExceptionWhenServicesIsNullOnConfigureBasicExceptionHandler()
    {
        IServiceCollection? services = null;

        var exception = Assert.Throws<ArgumentNullException>(() => services!.ConfigureBasicExceptionHandler());
        exception.Message.Should().Be("Value cannot be null. (Parameter 'services')");
    }

    [Fact]
    public void ShouldAddGenericHandlerWhenConfigureBasicExceptionHandler()
    {
        _services.Object.ConfigureBasicExceptionHandler();

        ValidateDefaultServices();

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(BaseHandler) &&
                    s.ImplementationFactory != null &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowExceptionWhenServicesIsNullOnConfigureDefaultExceptionHandler()
    {
        IServiceCollection? services = null;

        var exception = Assert.Throws<ArgumentNullException>(() => services!.ConfigureDefaultExceptionHandler());
        exception.Message.Should().Be("Value cannot be null. (Parameter 'services')");
    }

    [Fact]
    public void ShouldAddGenericHandlerWhenConfigureDefaultExceptionHandler()
    {
        _services.Object.ConfigureDefaultExceptionHandler();

        ValidateDefaultServices();

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(BaseHandler) &&
                    s.ImplementationFactory != null &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldThrowExceptionWhenServicesIsullOnConfigureExceptionHandler()
    {
        IServiceCollection? services = null;

        var exception = Assert.Throws<ArgumentNullException>(() => services!.ConfigureExceptionHandler(options => { }));
        exception.Message.Should().Be("Value cannot be null. (Parameter 'services')");
    }

    [Fact]
    public void ShouldThrowExceptionWhenConfigureStackIsNullOnConfigureExceptionHandler()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _services.Object.ConfigureExceptionHandler(null!));
        exception.Message.Should().Be("Value cannot be null. (Parameter 'configureStack')");
    }

    [Fact]
    public void ShouldAddGenericHandlerWhenOptionsEmptyOnConfigureExceptionHandler()
    {
        _services.Object.ConfigureExceptionHandler(options => { });

        ValidateDefaultServices();

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(BaseHandler) &&
                    s.ImplementationFactory != null &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.VerifyNoOtherCalls();
    }

    [Fact]
    public void ShouldConfigureWhenConfigureExceptionHandler()
    {
        _services.Object.ConfigureExceptionHandler(options =>
        {
            options.AddHandler<ArgumentHandler>();
            options.AddHandler<NullReferenceHandler>();
        });

        ValidateDefaultServices();

        _services.Verify(t =>
            t.Add(
                It.Is<ServiceDescriptor>(s =>
                    s.ServiceType == typeof(BaseHandler) &&
                    s.ImplementationFactory != null &&
                    s.Lifetime == ServiceLifetime.Singleton
                )
            )
        );

        _services.VerifyNoOtherCalls();
    }
}