using System.Reflection.Metadata;
using Restoration.Function.Data.CONfigurations;
using Restoration.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;

namespace Restoration.Function.Data;

public class BoardsDbContext : DbContext
{
    public BoardsDbContext(DbContextOptions<BoardsDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Board> Boards { get; set; }
    
    public DbSet<BoardMember> BoardMembers { get; set; }
    
    public DbSet<List> Lists { get; set; }
    
    public DbSet<Card> Cards { get; set; }
    
    public DbSet<Invitation> Invitations { get; set; }
    
    public DbSet<Label> Labels { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<CardLabel> CardLabels { get; set; }
    
    public DbSet<CardMember> CardMembers { get; set; }
    
    public DbSet<CardAttachment> CardAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new BoardConfiguration());
        modelBuilder.ApplyConfiguration(new BoardMemberConfiguration());
        modelBuilder.ApplyConfiguration(new CardConfiguration());
        modelBuilder.ApplyConfiguration(new CardLabelConfiguration());
        modelBuilder.ApplyConfiguration(new CardMemberConfiguration());
        modelBuilder.ApplyConfiguration(new CardAttachmentConfiguration());
        modelBuilder.ApplyConfiguration(new ListConfiguration());
        modelBuilder.ApplyConfiguration(new CommentConfiguration());
        modelBuilder.ApplyConfiguration(new LabelConfiguration());
        modelBuilder.ApplyConfiguration(new InvitationConfiguration());
    }
}