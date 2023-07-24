using Azure.Storage.Blobs;

namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IBlobClientProvider
    {
        BlobContainerClient GetBlobContainerClient(string connectionString, string containerName);
    }
}