using AutoMapper.Execution;
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
    
    DbSet<Session> Sessions { get; set; }
    
    DbSet<Board> Boards { get; set; }
    
    DbSet<BoardMember> BoardMembers { get; set; }
    
    DbSet<List> Lists { get; set; }
    
    DbSet<Card> Cards { get; set; }
    
    DbSet<Invitation> Invitations { get; set; }
    
    DbSet<Label> Labels { get; set; }
    
    DbSet<Comment> Comments { get; set; }
    
    DbSet<CardLabel> CardLabels { get; set; }
    
    DbSet<CardMember> CardMembers { get; set; }
    
    DbSet<CardAttachment> CardAttachments { get; set; }

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