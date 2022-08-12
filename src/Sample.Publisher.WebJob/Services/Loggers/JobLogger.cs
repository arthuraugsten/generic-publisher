using System.Text;
using Microsoft.Extensions.Logging;

namespace Sample.Publisher.WebJob.Services.Loggers;

public sealed class JobLogger<TJob, TEntity> : IJobLogger where TJob : BaseService
{
    private const string MessageTemplate = "{@JobName} - {@Message} - {@JobExecutionId}";
    private readonly ILogger<TJob> _logger;
    private readonly StringBuilder _builder = new(1024);
    private readonly string _typeName;
    private Guid _jobId;

    public JobLogger(ILogger<TJob> logger)
    {
        _logger = logger;
        _typeName = $"{typeof(TEntity).Name}-settings.SourceSystem";
    }

    public void SetJobId(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Invalid Job ID");

        _jobId = id;
    }

    public void LogError(Exception exception, bool fatal = false)
    {
        var originalErrorMessage = $"Integration Exception: {exception}";
        _logger.LogError(exception, $"Base/service - {exception.Message}");

        try
        {
            if (fatal)
            {
                _builder.Append($" FATAL ERROR - {exception.GetType()}");
                _logger.LogCritical(MessageTemplate, _typeName, originalErrorMessage, _jobId);
            }
            else
            {
                // no fatal logic here
            }
        }
        catch
        {
            // exception code
        }
    }

    public void LogInfo(string message, bool appendToJobLog = true)
    {
        _logger.LogInformation(MessageTemplate, _typeName, message, _jobId);
        if (appendToJobLog)
            _builder.Append($" {message} ;");
    }

    public void LogODataError(Exception exception)
    {
        try
        {
            // odata log here
        }
        catch
        {
            // exception code
        }
    }

    public void LogWarning(string message, bool appendToJobLog = true)
    {
        _logger.LogWarning(MessageTemplate, _typeName, message, _jobId);
        if (appendToJobLog)
            _builder.Append($" {message} ;");
    }

    public sealed override string ToString() => _builder.ToString();
}