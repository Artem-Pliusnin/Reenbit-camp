using Restoration.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restoration.Function.Data.CONfigurations;

public class ListConfiguration : IEntityTypeConfiguration<List>
{
    public void Configure(EntityTypeBuilder<List> builder)
    {
        builder.ToTable("Lists");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasColumnName("id");

        builder.Property(l => l.BoardId)
            .HasColumnName("board_id")
            .IsRequired();
        
        builder.Property(l => l.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(l => l.Position)
            .HasColumnName("position_index")
            .IsRequired();

        builder.Property(l => l.LastUpdatedBy)
            .HasColumnName("last_updated_by");

        builder.Property(l => l.LastUpdateDate)
            .HasColumnName("last_update_date");
        
        builder.HasOne(l => l.Board)
            .WithMany(b => b.Lists)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(l => new { l.BoardId, l.Position })
            .IsUnique();
    }
}