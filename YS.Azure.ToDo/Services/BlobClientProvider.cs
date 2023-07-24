using Azure.Storage.Blobs;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Services
{
    public class BlobClientProvider : IBlobClientProvider
    {
        public BlobContainerClient GetBlobContainerClient(string connectionString, string containerName)
        {
            var client = new BlobContainerClient(connectionString, containerName);

            return client;
        }
    }
}