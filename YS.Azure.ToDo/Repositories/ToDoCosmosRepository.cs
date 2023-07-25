using Microsoft.Extensions.Options;
using YS.Azure.ToDo.Configuration;
using YS.Azure.ToDo.Contracts.Repositories;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class ToDoCosmosRepository : CosmosRepositoryBase<ToDoItemModel>, IToDoCosmosRepository
    {
        public ToDoCosmosRepository(
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