using System.Linq.Expressions;

namespace EL_t3.Core.Interfaces.Repository;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> FindByIdAsync(int id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task InsertManyAsync(IEnumerable<TEntity> entities);
    Task UpsertManyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> match);
}