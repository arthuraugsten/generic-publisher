using Sample.Publisher.Core.Exceptions;
using Sample.Publisher.WebJob.Services.Loggers;

namespace Sample.Publisher.WebJob.Services;

public abstract class BaseService : IBaseService
{
    private readonly BaseHandler _exceptionHandler;
    protected readonly IJobLogger _logger;

    protected BaseService(BaseHandler exceptionHandler, IJobLogger logger)
    {
        _exceptionHandler = exceptionHandler;
        _logger = logger;
    }

    public abstract Task ExecuteProcessAsync(Guid id);

    public async Task RunAsync(Guid id)
    {
        try
        {
            // logic before

            await ExecuteProcessAsync(id);

            //logic after
        }
        catch (Exception ex)
        {
            await _exceptionHandler.HandleExceptionAsync(ex);
        }
        finally
        {
            try
            {
                // some code here to post execution
            }
            catch
            {
                // default logger
            }

            // dependency telemetry send
        }
    }
}