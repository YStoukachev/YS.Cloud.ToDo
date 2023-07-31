using Azure.Storage.Queues;

namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IQueueClientProvider
    {
        QueueClient CreateQueueClient(string connectionString, string queueName);
    }
}