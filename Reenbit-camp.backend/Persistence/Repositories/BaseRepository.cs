using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
{
    protected DbSet<TEntity> _dbSet;
    
    protected DbContext _dbContext;

    public BaseRepository(DbContext context)
    {
        SetContext(context);
    }

    public void SetContext(DbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(TId id, 
        CancellationToken cancellationToken = default) => 
        await _dbSet.FindAsync(id, cancellationToken);

    public async Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default) => 
        await _dbSet.ToListAsync(cancellationToken);

    public void Add(TEntity entity) => 
        _dbSet.Add(entity);

    public void Update(TEntity entity) => 
        _dbSet.Update(entity);

    public void Remove(TEntity entity) => 
        _dbSet.Remove(entity);
}