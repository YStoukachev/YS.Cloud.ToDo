using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ToDoOptions _toDoOptions;

        public ToDoService(
            IToDoRepository toDoRepository, 
            IBlobStorageService blobStorageService, 
            IOptions<ToDoOptions> toDoOptions)
        {
            _toDoRepository = toDoRepository;
            _blobStorageService = blobStorageService;
            _toDoOptions = toDoOptions.Value;
        }

        public async Task<ToDoItemModel> CreateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default)
        {
            return await _toDoRepository.UpsertAsync(itemModel, cancellationToken);
        }

        public async Task<ToDoItemModel> UpdateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default)
        {
            return await _toDoRepository.UpsertAsync(itemModel, cancellationToken);
        }

        public async Task DeleteToDoItemAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            await _toDoRepository.DeleteAsync(itemId.ToString(), cancellationToken);
        }

        public async Task<IEnumerable<ToDoItemModel>> GetToDosAsync(Expression<Func<ToDoItemModel, bool>> selector = null!, CancellationToken cancellationToken = default)
        {
            return await _toDoRepository.GetAsync(selector, cancellationToken);
        }

        public async Task UploadFileToTaskAsync(AddFileRequestModel model, CancellationToken cancellationToken = default)
        {
            await _blobStorageService.UploadBlobAsync(
                string.Format(_toDoOptions.BlobNameTemplate, model.TaskId.ToString(), Guid.NewGuid().ToString()),
                model.FileContent, 
                cancellationToken);
        }

        public async Task UpdateTaskStatusesAsync(CancellationToken cancellationToken = default)
        {
            var todoItems = (await _toDoRepository.GetAsync(cancellationToken: cancellationToken))
                .ToList();

            foreach (var todoItem in todoItems)
            {
                if (todoItem.DueDate < DateTime.Now)
                {
                    todoItem.Status = ToDoStatus.Done;

                    await _toDoRepository.UpsertAsync(todoItem, cancellationToken);
                }
            }
        }
    }
}