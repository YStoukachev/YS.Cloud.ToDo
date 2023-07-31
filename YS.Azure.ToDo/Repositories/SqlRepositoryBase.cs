using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YS.Azure.ToDo.EntityFramework;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class SqlRepositoryBase<TEntity> : IDisposable, IAsyncDisposable where TEntity : IdEntity
    {
        private readonly ToDoContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public SqlRepositoryBase(ToDoContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet
                .AddAsync(entity, cancellationToken);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var idEntity = entity as IdEntity;

            var existingEntity = await GetExistingEntity(idEntity.Id, cancellationToken);

            if (existingEntity != null)
            {
                _dbSet.Update(entity);
            }

            return entity;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await GetExistingEntity(id, cancellationToken);

            if (existingEntity != null)
            {
                _dbSet.Remove(existingEntity);
            }
        }

        public async Task<IQueryable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>>? selector = null,
            Expression<Func<TEntity, object>>[]? includeQuery = null,
            CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .AsQueryable();

            if (includeQuery != null)
            {
                foreach (var include in includeQuery)
                {
                    query = query.Include(include);
                }
            }
            
            return selector == null
                ? query
                    .Where(_ => true)
                : query
                    .Where(selector);
        }

        private async Task<TEntity?> GetExistingEntity(string id, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _dbSet
                .FirstOrDefaultAsync(_ => _.Id == id, cancellationToken: cancellationToken);

            return existingEntity;
        }

        public void Dispose()
        {
            _dbContext.SaveChanges();
        }

        public async ValueTask DisposeAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}