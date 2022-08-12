using Microsoft.Extensions.DependencyInjection;
using Sample.Publisher.Core.Exceptions;

var builder = new ServiceCollection();

builder.ConfigureExceptionHandler(options =>
{
    options.AddHandler<ArgumentHandler>();
    options.AddHandler<NullReferenceHandler>();
    // options.AddHandler<GenericHandler>();
});

var serviceProvider = builder.BuildServiceProvider();

var handler = serviceProvider.GetRequiredService<BaseHandler>();
var c = 'a';
