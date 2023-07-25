using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Contracts.Repositories
{
    public interface IArchivedTasksRepository : ISqlRepository<ToDoItemModel>
    {
        
    }
}