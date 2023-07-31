using Azure.Storage.Queues;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Services
{
    public class QueueClientProvider : IQueueClientProvider
    {
        public QueueClient CreateQueueClient(string connectionString, string queueName)
        {
            return new QueueClient(connectionString, queueName);
        }
    }
}