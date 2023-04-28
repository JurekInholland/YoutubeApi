using Microsoft.Extensions.Hosting;

namespace Services.TaskService;

public interface ITaskService : IHostedService
{
    public void ChangeInterval(TimeSpan interval);
}
