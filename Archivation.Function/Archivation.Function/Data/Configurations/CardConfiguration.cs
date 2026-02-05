using Archivation.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Archivation.Function.Data.CONfigurations;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.ListId)
            .HasColumnName("list_id")
            .IsRequired();
        
        builder.Property(c => c.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(c => c.Description)
            .HasColumnName("description");
        
        builder.Property(c => c.IsCompleted)
            .HasColumnName("is_completed")
            .IsRequired();

        builder.Property(c => c.Position)
            .HasColumnName("position_index")
            .IsRequired();
        
        builder.Property(c => c.StartDate)
            .HasColumnName("start_date");
        
        builder.Property(c => c.DueDate)
            .HasColumnName("due_date");
        
        builder.Property(c => c.LastUpdatedBy)
            .HasColumnName("last_updated_by");

        builder.Property(c => c.LastUpdateDate)
            .HasColumnName("last_update_date")
            .HasDefaultValue(DateTime.UtcNow);
        
        builder.HasOne(c => c.List)
            .WithMany(l => l.Cards)
            .HasForeignKey(c => c.ListId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(l => new { l.ListId, l.Position })
            .IsUnique();
    }
}