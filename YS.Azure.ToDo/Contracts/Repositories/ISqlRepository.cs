﻿using System.Linq.Expressions;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Contracts.Repositories
{
    public interface ISqlRepository<TEntity> where TEntity : IdEntity
    {
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);
        
        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? selector = null, CancellationToken cancellationToken = default);
    }
}