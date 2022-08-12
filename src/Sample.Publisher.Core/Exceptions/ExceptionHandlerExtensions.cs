using Microsoft.Extensions.DependencyInjection;

namespace Sample.Publisher.Core.Exceptions;

public static class ExceptionHandlerExtensions
{
    /// <summary>
    /// Configure the exceptions which the application should handle during the job execution. It should be defined in the order
    /// that you excpect the application should execute the stack execution.
    /// </summary>
    public static void ConfigureExceptionHandler(this IServiceCollection services, Action<ExceptionsOptions> configureStack)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureStack);

        services.AddSingleton<ArgumentHandler>();
        services.AddSingleton<GenericHandler>();
        services.AddSingleton<NullReferenceHandler>();

        var options = new ExceptionsOptions();
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

public sealed record ExceptionsOptions
{
    private readonly Queue<Type> _queue = new();

    internal bool ContainsGenericHandler { get; private set; } = false;

    public void AddHandler<T>() where T : BaseHandler
    {
        var type = typeof(T);

        if (_queue.Contains(type))
        {
            return;
        }

        ContainsGenericHandler = ContainsGenericHandler || type == typeof(GenericHandler);
        _queue.Enqueue(type);
    }

    internal Type? GetItem()
    {
        if (!_queue.TryDequeue(out var item))
        {
            return default;
        }

        return item;
    }
}
