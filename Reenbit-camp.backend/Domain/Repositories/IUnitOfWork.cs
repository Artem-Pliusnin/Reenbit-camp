namespace Domain.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    T GetRepository<T>() where T : class, IBaseRepository;
    void Begin();
    void Commit();
    void Rollback();
}