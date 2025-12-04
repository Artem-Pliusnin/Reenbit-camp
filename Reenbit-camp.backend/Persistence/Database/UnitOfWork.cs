using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TrelloAppDbContext _context;
    private readonly IDictionary<string, object> _repositories = new Dictionary<string, object>();
    
    public UnitOfWork(IServiceProvider serviceProvider, TrelloAppDbContext context)
    {
        _serviceProvider = serviceProvider;
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public T GetRepository<T>() where T : class, IBaseRepository
    {
        var typeName = typeof(T).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            T instance = _serviceProvider.GetService<T>();
            instance.SetContext(_context);
            _repositories.Add(typeName, instance);
        }
        
        return (T)_repositories[typeName];
    }

    public void Begin()
    {
        _context.BeginTransaction();
    }

    public void Commit()
    {
        _context.SaveChanges();
        _context.CommitTransaction();
    }

    public void Rollback()
    {
        _context.RollbackTransaction();
    }
    
    public void Dispose()
    {
        _context.Dispose();
        _repositories.Clear();
    }
}