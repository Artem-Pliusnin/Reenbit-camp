using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class BoardConfiguration : IEntityTypeConfiguration<Board>
{
    public void Configure(EntityTypeBuilder<Board> builder)
    {
        builder.ToTable("Boards");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasColumnName("id");

        builder.Property(b => b.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasColumnName("created_by")
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(b => b.CreationDate)
            .HasColumnName("creation_date")
            .IsRequired()
            .HasDefaultValue(DateTime.UtcNow);

        builder.Property(b => b.LastUpdatedBy)
            .HasColumnName("last_updated_by");

        builder.Property(b => b.LastUpdateDate)
            .HasColumnName("last_update_date");
        
        builder.HasOne(b => b.CreatedByUser)
            .WithMany()
            .HasForeignKey(b => b.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.LastUpdatedByUser)
            .WithMany()
            .HasForeignKey(b => b.LastUpdatedBy)
            .OnDelete(DeleteBehavior.SetNull);
        
    }
}