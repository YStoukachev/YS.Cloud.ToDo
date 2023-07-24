using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;

namespace YS.Azure.ToDo.Repositories
{
    public class CosmosRepositoryBase<TEntity> where TEntity : class
    {
        private readonly Container _container;

        public CosmosRepositoryBase(string dbName, string containerName, string accountEndpoint, string authKey)
        {
            var cosmosClient = new CosmosClient(accountEndpoint, authKey);
            _container = cosmosClient.GetContainer(dbName, containerName);
        }

        public async Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var response = await _container.UpsertItemAsync(entity, cancellationToken: cancellationToken);

            return response.Resource;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _container.DeleteItemAsync<TEntity>(id, PartitionKey.None, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? selector = null, CancellationToken cancellationToken = default)
        {
            var orderedQuery = _container.GetItemLinqQueryable<TEntity>();
            var query = selector == null 
                ? orderedQuery.Where(_ => true) 
                : orderedQuery.Where(selector);
            
            return query.AsEnumerable();
        }
    }
}