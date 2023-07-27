using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.Contracts.Services;
using YS.Azure.ToDo.Exceptions;
using YS.Azure.ToDo.Models;
using YS.Azure.ToDo.Models.Requests;

namespace YS.Azure.ToDo.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoCosmosRepository _toDoCosmosRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ToDoOptions _toDoOptions;
        private readonly IArchivedTasksRepository _archivedTasksRepository;
        private readonly IMapper _mapper;

        public ToDoService(
            IToDoCosmosRepository toDoCosmosRepository, 
            IBlobStorageService blobStorageService, 
            IOptions<ToDoOptions> toDoOptions, 
            IArchivedTasksRepository archivedTasksRepository, 
            IMapper mapper)
        {
            _toDoCosmosRepository = toDoCosmosRepository;
            _blobStorageService = blobStorageService;
            _archivedTasksRepository = archivedTasksRepository;
            _mapper = mapper;
            _toDoOptions = toDoOptions.Value;
        }

        public async Task<ToDoItemModel> CreateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default)
        {
            return await _toDoCosmosRepository.UpsertAsync(itemModel, cancellationToken);
        }

        public async Task<ToDoItemModel> UpdateToDoItemAsync(ToDoItemModel itemModel, CancellationToken cancellationToken = default)
        {
            return await _toDoCosmosRepository.UpsertAsync(itemModel, cancellationToken);
        }

        public async Task DeleteToDoItemAsync(string itemId, CancellationToken cancellationToken = default)
        {
            await _toDoCosmosRepository.DeleteAsync(itemId, cancellationToken);
        }

        public async Task<IEnumerable<ToDoItemModel>> GetToDosAsync(Expression<Func<ToDoItemModel, bool>> selector = null!, CancellationToken cancellationToken = default)
        {
            return await _toDoCosmosRepository.GetAsync(selector, cancellationToken);
        }

        public async Task UploadFileToTaskAsync(AddFileRequestModel model, CancellationToken cancellationToken = default)
        {
            var fileName = string
                .Format(
                    _toDoOptions.BlobNameTemplate, 
                    model.TaskId,
                    Guid.NewGuid().ToString(),
                    model.File.FileExtension);

            var existingTask = (await _toDoCosmosRepository
                .GetAsync(_ => _.Id == model.TaskId, cancellationToken))
                .FirstOrDefault();

            if (existingTask == null)
            {
                throw new TaskNotFoundException($"Task with {model.TaskId} not found.");
            }

            if (existingTask.FileNames == null)
            {
                existingTask.FileNames = new List<string>
                {
                    fileName
                };
            }
            else
            {
                existingTask.FileNames.Add(fileName);
            }

            await _toDoCosmosRepository.UpsertAsync(existingTask, cancellationToken);
            
            await _blobStorageService.UploadBlobAsync(
                fileName,
                model.File.FileContent, 
                cancellationToken);
        }

        public async Task ArchiveTaskAsync(string taskId, CancellationToken cancellationToken = default)
        {
            var existingTask = (await _toDoCosmosRepository
                    .GetAsync(_ => _.Id == taskId, cancellationToken))
                .FirstOrDefault();

            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found.");
            }

            var mappedTask = _mapper.Map<ToDoEntity>(existingTask);
            mappedTask.Archived = true;

            await _archivedTasksRepository.CreateAsync(mappedTask, cancellationToken);
            await _toDoCosmosRepository.DeleteAsync(taskId, cancellationToken);
        }

        public async Task UnarchiveTaskAsync(string taskId, CancellationToken cancellationToken = default)
        {
            var existingTask = await (await _archivedTasksRepository
                    .GetAsync(_ => _.Id == taskId, cancellationToken))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingTask == null)
            {
                throw new TaskNotFoundException("Task not found.");
            }

            var mappedTask = _mapper.Map<ToDoItemModel>(existingTask);
            mappedTask.Archived = false;

            await _toDoCosmosRepository.UpsertAsync(mappedTask, cancellationToken);
            await _archivedTasksRepository.DeleteAsync(taskId, cancellationToken);
        }

        public async Task<IList<ToDoEntity>> GetArchivedTasksAsync(CancellationToken cancellationToken = default)
        {
            var archivedTasks = (await _archivedTasksRepository
                    .GetAsync(cancellationToken: cancellationToken))
                .ToList();

            return archivedTasks;
        }

        public async Task UpdateTaskStatusesAsync(CancellationToken cancellationToken = default)
        {
            var todoItems = (await _toDoCosmosRepository
                    .GetAsync(_ => _.DueDate != null, cancellationToken: cancellationToken))
                .ToList();

            foreach (var todoItem in todoItems)
            {
                if (todoItem.DueDate < DateTime.Now)
                {
                    todoItem.Status = ToDoStatus.Done;

                    await _toDoCosmosRepository.UpsertAsync(todoItem, cancellationToken);
                }
            }
        }
    }
}