using System.Linq.Expressions;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Contracts.Services
{
    public interface IToDoService
    {
        Task<ToDoItemModel> CreateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default);

        Task<ToDoItemModel> UpdateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default);

        Task DeleteToDoItemAsync(Guid itemId, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<ToDoItemModel>> GetToDosAsync(Expression<Func<ToDoItemModel, bool>> selector = null!, CancellationToken cancellationToken = default);

        Task UpdateTaskStatusesAsync(CancellationToken cancellationToken = default);

        Task UploadFileToTaskAsync(AddFileRequestModel model, CancellationToken cancellationToken = default);
    }
}