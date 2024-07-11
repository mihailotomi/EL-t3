using System.Linq.Expressions;
using EL_t3.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace EL_t3.Infrastructure.Database.Repository;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>, IDisposable
where TEntity : class
{
    protected readonly AppDatabaseContext _context;

    private DbSet<TEntity>? _dbSet;

    protected DbSet<TEntity> DbSet => _dbSet ??= _context.Set<TEntity>();

    public BaseRepository(AppDatabaseContext context)
    {
        _context = context;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var insertedEntity = await DbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        return insertedEntity.Entity;
    }

    public Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> query)
    {
        return DbSet.FirstOrDefaultAsync(query);
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task UpsertManyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> match)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await DbSet.UpsertRange(entities)
            .On(match)
            .RunAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public void Dispose() => _context.Dispose();
}