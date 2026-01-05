using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CardMemberConfiguration : IEntityTypeConfiguration<CardMember>
{
    public void Configure(EntityTypeBuilder<CardMember> builder)
    {
        builder.ToTable("CardMembers");

        builder.HasKey(cm => cm.Id);
        
        builder.Property(cm => cm.Id)
            .HasColumnName("id");
        
        builder.Property(cm => cm.CardId)
            .HasColumnName("card_id")
            .IsRequired();

        builder.Property(cm => cm.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.HasOne(cm => cm.Card)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.User)
            .WithMany(u => u.Cards)
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(cm => new { cm.CardId, cm.UserId })
            .IsUnique();
    }
}