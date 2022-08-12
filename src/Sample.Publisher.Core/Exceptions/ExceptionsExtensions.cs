using Microsoft.Extensions.DependencyInjection;

namespace Sample.Publisher.Core.Exceptions;

public static class ExceptionsExtensions
{
    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddSingleton<ArgumentHandler>();
        services.AddSingleton<NullReferenceHandler>();
        services.AddSingleton<GenericHandler>();
    }

    public static void ConfigureBasicExceptionHandler(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddHandlers();

        services.AddSingleton<BaseHandler>(serviceProvider => serviceProvider.GetRequiredService<GenericHandler>());
    }

    public static void ConfigureDefaultExceptionHandler(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddHandlers();

        services.ConfigureExceptionHandler(options =>
        {
            options.AddHandler<ArgumentHandler>();
            options.AddHandler<NullReferenceHandler>();
            options.AddHandler<GenericHandler>();
        });
    }

    /// <summary>
    /// Configure the exceptions which the application should handle during the job execution. It should be defined in the order
    /// that you excpect the application should execute the stack execution.
    /// </summary>
    public static void ConfigureExceptionHandler(this IServiceCollection services, Action<ExceptionsOption> configureStack)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureStack);

        services.AddHandlers();

        var options = new ExceptionsOption();
        configureStack(options);

        if (options.GetItem() is not Type type)
        {
            services.AddSingleton<BaseHandler>(serviceProvider => serviceProvider.GetRequiredService<GenericHandler>());
            return;
        }

        services.AddSingleton(serviceProvider =>
        {
            BaseHandler firstHandler = (BaseHandler)serviceProvider.GetRequiredService(type);
            var previousHandler = firstHandler;
            Type? nextType;

            while ((nextType = options.GetItem()) != null)
            {
                var nextHandler = (BaseHandler)serviceProvider.GetRequiredService(nextType);
                previousHandler.SetNext(nextHandler);
                previousHandler = nextHandler;
            }

            if (!options.ContainsGenericHandler)
            {
                var genericHandler = serviceProvider.GetRequiredService<GenericHandler>();
                previousHandler.SetNext(genericHandler);
            }

            return firstHandler;
        });

    }
}