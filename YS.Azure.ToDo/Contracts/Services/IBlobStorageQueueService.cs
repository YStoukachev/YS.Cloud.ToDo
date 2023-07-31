namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IBlobStorageQueueService
    {
        Task SendMessageAsync(string message, CancellationToken cancellationToken = default);
    }
}