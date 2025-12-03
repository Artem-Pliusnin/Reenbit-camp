namespace Domain.Repositories;

public interface IRepository<TEntity, TId>
{
    Task<TEntity?> GetByIdAsync(TId id);
    
    Task<List<TEntity>> GetAllAsync();

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}