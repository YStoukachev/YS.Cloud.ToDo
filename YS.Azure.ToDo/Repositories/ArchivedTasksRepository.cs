using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.EntityFramework;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class ArchivedTasksRepository : SqlRepositoryBase<ToDoEntity>, IArchivedTasksRepository
    {
        public ArchivedTasksRepository(ToDoContext dbContext) 
            : base(dbContext)
        {
        }
    }
}