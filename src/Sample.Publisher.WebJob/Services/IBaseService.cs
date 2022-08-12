namespace Sample.Publisher.WebJob.Services;

public interface IBaseService
{
    Task ExecuteProcessAsync(Guid id);
    Task RunAsync(Guid id);
}