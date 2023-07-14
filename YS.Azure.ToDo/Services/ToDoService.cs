using YS.Azure.ToDo.Contracts;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Services
{
    public class ToDoService : IToDoService
    {
        public async Task<ToDoItem> CreateToDoItemAsync(ToDoItem item, CancellationToken cancellationToken = default)
        {
            // TODO: create logic for saving to DB
            
            return item;
        }

        public async Task<ToDoItem> UpdateToDoItemAsync(Guid itemId, ToDoItem item, CancellationToken cancellationToken = default)
        {
            // TODO: create logic for updating
            
            return item;
        }

        public async Task DeleteToDoItemAsync(Guid itemId, CancellationToken cancellationToken = default)
        {
            // TODO: create logic for deleting
        }
    }
}