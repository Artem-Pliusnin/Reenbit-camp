using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Repositories;

public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : class
{
    protected readonly TrelloAppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public BaseRepository(TrelloAppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public async Task<TEntity?> GetByIdAsync(TId id) => 
        await _dbSet.FindAsync(id);

    public async Task<List<TEntity>> GetAllAsync() => 
        await _dbSet.ToListAsync();

    public void Add(TEntity entity) => 
        _dbSet.Add(entity);

    public void Update(TEntity entity) => 
        _dbSet.Update(entity);

    public void Remove(TEntity entity) => 
        _dbSet.Remove(entity);
}