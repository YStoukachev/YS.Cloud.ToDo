using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Services;

namespace YS.Azure.ToDo.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorageService(
            IBlobClientProvider blobClientProvider,
            IOptions<BlobStorageOptions> blobStorageOptions)
        {
            _blobContainerClient = blobClientProvider
                .GetBlobContainerClient(blobStorageOptions.Value.ConnectionString, blobStorageOptions.Value.BlobContainerName);
        }

        public async Task UploadBlobAsync(string fileName, Stream fileContent, CancellationToken cancellationToken = default)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileContent, true, cancellationToken: cancellationToken);
        }

        public async Task DeleteBlobAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var existingBlobs = await _blobContainerClient
                .GetBlobsAsync()
                .ToListAsync(cancellationToken);
            var blobToDelete = existingBlobs.FirstOrDefault(_ => _.Name.Contains(fileName));

            if (blobToDelete != null)
            {
                await _blobContainerClient.DeleteBlobAsync(blobToDelete.Name, cancellationToken: cancellationToken);
            }
        }
    }
}