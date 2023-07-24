using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Functions
{
    public class TaskStatusChangerFunction
    {
        private readonly ILogger<TaskStatusChangerFunction> _logger;
        private readonly IToDoService _toDoService;

        public TaskStatusChangerFunction(
            ILogger<TaskStatusChangerFunction> logger, 
            IToDoService toDoService)
        {
            _logger = logger;
            _toDoService = toDoService;
        }

        [Function(nameof(TaskStatusChangerFunction))]
        public async Task RunAsync([TimerTrigger("0 0 0 * * *")] object myTimer)
        {
            _logger.LogInformation("Task status changer job is working.");

            await _toDoService.UpdateTaskStatusesAsync();
            
            _logger.LogInformation("Task status changer finished working.");
        }
    }
}