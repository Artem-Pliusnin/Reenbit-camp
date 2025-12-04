using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class TrelloAppDbContext : DbContext
{
    public TrelloAppDbContext(DbContextOptions<TrelloAppDbContext> options) 
        : base(options)
    {
    }
    
    DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
    
    public void BeginTransaction()
    {
        Database.BeginTransaction();
    }

    public void CommitTransaction()
    {
        Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        Database.RollbackTransaction();
    }
}