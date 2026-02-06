using Restoration.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restoration.Function.Data.CONfigurations;

public class CardAttachmentConfiguration : IEntityTypeConfiguration<CardAttachment>
{
    public void Configure(EntityTypeBuilder<CardAttachment> builder)
    {
        builder.ToTable("CardAttachment");

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
            .HasColumnName("id");

        builder.Property(ca => ca.CardId)
            .HasColumnName("card_id")
            .IsRequired();

        builder.Property(ca => ca.FileUrl)
            .HasColumnName("file_url")
            .IsRequired();

        builder.Property(ca => ca.FileName)
            .HasColumnName("file_name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(ca => ca.ContentType)
            .HasColumnName("content_type")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at");
        
        builder.HasOne(ca => ca.Card)
            .WithMany(c => c.Attachments)
            .HasForeignKey(ca => ca.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.CardId);
    }
}