using Archivation.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Archivation.Function.Data.CONfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.CardId)
            .HasColumnName("card_id")
            .IsRequired();
        
        builder.Property(c => c.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.Property(c => c.Text)
            .HasColumnName("text");
        
        builder.Property(c => c.IsEdited)
            .HasColumnName("is_edited")
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.HasOne(c => c.Card)
            .WithMany(c => c.Comments)
            .HasForeignKey(c => c.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.CardId);
    }
}