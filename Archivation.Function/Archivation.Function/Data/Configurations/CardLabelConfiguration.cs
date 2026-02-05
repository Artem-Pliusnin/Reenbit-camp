using Archivation.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Archivation.Function.Data.CONfigurations;

public class CardLabelConfiguration : IEntityTypeConfiguration<CardLabel>
{
    public void Configure(EntityTypeBuilder<CardLabel> builder)
    {
        builder.ToTable("CardLabels");

        builder.HasKey(cl => cl.Id);
        
        builder.Property(cl => cl.Id)
            .HasColumnName("id");
        
        builder.Property(cl => cl.CardId)
            .HasColumnName("card_id")
            .IsRequired();

        builder.Property(cl => cl.LabelId)
            .HasColumnName("label_id")
            .IsRequired();
        
        builder.HasOne(cl => cl.Card)
            .WithMany(c => c.Labels)
            .HasForeignKey(cl => cl.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cl => cl.Label)
            .WithMany(l => l.CardLabels)
            .HasForeignKey(cl => cl.LabelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cl => new { cl.CardId, cl.LabelId })
            .IsUnique();
    }
}