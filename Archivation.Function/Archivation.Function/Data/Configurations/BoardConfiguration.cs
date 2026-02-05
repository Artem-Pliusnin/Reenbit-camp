using Archivation.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Archivation.Function.Data.CONfigurations;

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
            .IsRequired();

        builder.Property(b => b.CreationDate)
            .HasColumnName("creation_date")
            .IsRequired(); ;

        builder.Property(b => b.LastUpdatedBy)
            .HasColumnName("last_updated_by");

        builder.Property(b => b.LastUpdateDate)
            .HasColumnName("last_update_date");

        builder.Property(b => b.Status)
            .HasColumnName("status_id")
            .HasConversion<int>()
            .IsRequired();
        
        builder.HasIndex(b => b.Status);
    }
}