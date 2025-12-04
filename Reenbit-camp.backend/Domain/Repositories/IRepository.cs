namespace Domain.Repositories;

public interface IRepository<TEntity, TId> : IBaseRepository
{
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}