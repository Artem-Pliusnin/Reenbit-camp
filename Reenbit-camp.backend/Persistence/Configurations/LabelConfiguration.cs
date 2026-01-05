using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.ToTable("Labels");

        builder.HasKey(l => l.Id);
        
        builder.Property(l => l.Id)
            .HasColumnName("id");
        
        builder.Property(l => l.BoardId)
            .HasColumnName("board_id")
            .IsRequired();

        builder.Property(l => l.Text)
            .HasColumnName("text")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.Color)
            .HasColumnName("color")
            .HasMaxLength(7)
            .IsRequired();

        builder.HasOne(l => l.Board)
            .WithMany(b => b.Labels)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}