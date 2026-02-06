using Restoration.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restoration.Function.Data.CONfigurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitations");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id");

        builder.Property(i => i.BoardId)
            .HasColumnName("board_id")
            .IsRequired();
        
        builder.Property(i => i.InvitedUserId)
            .HasColumnName("invited_user_id")
            .IsRequired();
        
        builder.Property(i => i.InvitedByUserId)
            .HasColumnName("invited_by_user_id");
        
        builder.Property(i => i.Status)
            .HasColumnName("status_id")
            .HasConversion<int>()
            .IsRequired();
        
        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(i => i.RespondedAt)
            .HasColumnName("responded_at");
        
        builder.HasOne(I => I.Board)
            .WithMany(b => b.Invitations)
            .HasForeignKey(i => i.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(i => new { i.BoardId, i.InvitedUserId })
            .IsUnique();
    }
}