using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace YS.Azure.ToDo.Functions
{
    public class TaskStatusNotifyFunction
    {
        private readonly ILogger<TaskStatusNotifyFunction> _logger;

        public TaskStatusNotifyFunction(ILogger<TaskStatusNotifyFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(TaskStatusNotifyFunction))]
        public void RunAsync([QueueTrigger(
                "%BlobStorageOptions:QueueName%",
                Connection = "BlobStorageOptions:ConnectionString")]
            string queueMessage)
        {
            _logger.LogInformation($"Successfully got message: {queueMessage}");
        }
    }
}