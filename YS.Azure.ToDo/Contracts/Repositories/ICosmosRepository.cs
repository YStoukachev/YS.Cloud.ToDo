using System.Linq.Expressions;

namespace YS.Azure.ToDo.Contracts.Repositories
{
    public interface ICosmosRepository<TEntity>
    {
        Task<TEntity> UpsertAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? selector = null, CancellationToken cancellationToken = default);
    }
}