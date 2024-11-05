using System.Linq.Expressions;

namespace EL_t3.Application.Common.Interfaces.Repository;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> query);
    Task<TEntity> InsertAsync(TEntity entity);
    Task InsertManyAsync(IEnumerable<TEntity> entities);
    Task UpsertManyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> match, Expression<Func<TEntity, TEntity, TEntity>> whenMatched, CancellationToken cancellationToken);
}