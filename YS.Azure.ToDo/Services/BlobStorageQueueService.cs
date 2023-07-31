using Azure.Storage.Queues;
using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Services
{
    public class BlobStorageQueueService : IBlobStorageQueueService
    {
        private readonly QueueClient _queueClient;

        public BlobStorageQueueService(
            IOptions<BlobStorageOptions> options,
            IQueueClientProvider queueClientProvider)
        {
            _queueClient = queueClientProvider
                .CreateQueueClient(options.Value.ConnectionString, options.Value.QueueName);
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            await _queueClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            await _queueClient.SendMessageAsync(message, cancellationToken);
        }
    }
}