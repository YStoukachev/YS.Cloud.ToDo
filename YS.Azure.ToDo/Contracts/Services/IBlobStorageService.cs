namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IBlobStorageService
    {
        Task UploadBlobAsync(string fileName, Stream fileContent, CancellationToken cancellationToken = default);

        Task DeleteBlobAsync(string fileName, CancellationToken cancellationToken = default);
    }
}