using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YS.Azure.ToDo.EntityFramework;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Repositories
{
    public class SqlRepositoryBase<TEntity> where TEntity : IdEntity
    {
        private readonly ToDoContext _dbContext;

        public SqlRepositoryBase(ToDoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await using (_dbContext)
            {
                await _dbContext
                    .Set<TEntity>()
                    .AddAsync(entity, cancellationToken);
            }

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var idEntity = entity as IdEntity;
            
            await using (_dbContext)
            {
                var dbSet = _dbContext
                    .Set<TEntity>();

                var existingEntity = await GetExistingEntity(idEntity.Id, cancellationToken);

                if (existingEntity != null)
                {
                    dbSet.Update(entity);
                }
            }

            return entity;
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await using (_dbContext)
            {
                var dbSet = _dbContext
                    .Set<TEntity>();

                var existingEntity = await GetExistingEntity(id, cancellationToken);

                if (existingEntity != null)
                {
                    dbSet.Remove(existingEntity);
                }
            }
        }

        public async Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? selector = null, CancellationToken cancellationToken = default)
        {
            await using (_dbContext)
            {
                return selector == null
                    ? _dbContext
                        .Set<TEntity>()
                        .AsQueryable()
                        .Where(_ => true)
                    : _dbContext
                        .Set<TEntity>()
                        .AsQueryable()
                        .Where(selector);
            }
        }

        private async Task<TEntity?> GetExistingEntity(string id, CancellationToken cancellationToken = default)
        {
            var dbSet = _dbContext
                .Set<TEntity>();
                
            var existingEntity = await dbSet
                .FirstOrDefaultAsync(_ => _.Id == id, cancellationToken: cancellationToken);

            return existingEntity;
        }
    }
}