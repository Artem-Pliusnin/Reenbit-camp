using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Converters;

namespace Persistence.Configurations;

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
            .HasColumnName("last_update_date")
            .HasDefaultValue(DateTime.UtcNow)
            .HasConversion(UtcDateTimeConverters.Nullable);;
        
        builder.HasOne(l => l.Board)
            .WithMany(b => b.Lists)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.LastUpdatedByUser)
            .WithMany()
            .HasForeignKey(b => b.LastUpdatedBy)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasIndex(l => new { l.BoardId, l.Position })
            .IsUnique();
    }
}