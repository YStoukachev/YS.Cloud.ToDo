using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Contracts
{
    public interface IToDoService
    {
        Task<ToDoItem> CreateToDoItemAsync(ToDoItem item, CancellationToken cancellationToken = default);

        Task<ToDoItem> UpdateToDoItemAsync(Guid itemId, ToDoItem item, CancellationToken cancellationToken = default);

        Task DeleteToDoItemAsync(Guid itemId, CancellationToken cancellationToken = default);
    }
}