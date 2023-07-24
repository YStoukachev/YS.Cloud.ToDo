using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class ToDoRepository : CosmosRepositoryBase<ToDoItemModel>, IToDoRepository
    {
        public ToDoRepository(
            IOptions<ToDoOptions> toDoOptions,
            IOptions<CosmosDbOptions> cosmosDbOptions)
            : base(
                toDoOptions.Value.CosmosDbName,
                toDoOptions.Value.CosmosContainerName,
                cosmosDbOptions.Value.AccountEndpoint,
                cosmosDbOptions.Value.AuthKey)
        {
        }
    }
}