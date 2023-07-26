using System.Linq.Expressions;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class CosmosRepositoryBase<TEntity> where TEntity : IdEntity
    {
        private readonly Container _container;

        public CosmosRepositoryBase(string dbName, string containerName, string accountEndpoint, string authKey)
        {
            var cosmosClient = new CosmosClient(accountEndpoint, authKey);
            _container = cosmosClient.GetContainer(dbName, containerName);
        }

        public async Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var response = await _container.UpsertItemAsync(entity, new PartitionKey(entity.Id), cancellationToken: cancellationToken);

            return response.Resource;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(id), cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? selector = null, CancellationToken cancellationToken = default)
        {
            var orderedQuery = _container.GetItemLinqQueryable<TEntity>();
            var query = selector == null 
                ? orderedQuery.Where(_ => true) 
                : orderedQuery.Where(selector);
            var feedIterator = query.ToFeedIterator();
            var result = new List<TEntity>();

            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync(cancellationToken);
                
                result.AddRange(response.Resource);
            }
            
            return result;
        }
    }
}