namespace Sample.Publisher.WebJob.Services.Loggers;

public interface IJobLogger
{
    void LogError(Exception exception, bool fatal = false);
    void LogInfo(string message, bool appendToJobLog = true);
    void LogODataError(Exception exception); // ODataException
    void LogWarning(string message, bool appendToJobLog = true);
}