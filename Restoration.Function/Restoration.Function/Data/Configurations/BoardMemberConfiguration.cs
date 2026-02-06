using Restoration.Function.Models.Etities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restoration.Function.Data.CONfigurations;

public class BoardMemberConfiguration : IEntityTypeConfiguration<BoardMember>
{
    public void Configure(EntityTypeBuilder<BoardMember> builder)
    {
        builder.ToTable("BoardMembers");

        builder.HasKey(bm => bm.Id);

        builder.Property(bm => bm.Id)
            .HasColumnName("id");

        builder.Property(bm => bm.BoardId)
            .HasColumnName("board_id")
            .IsRequired();

        builder.Property(bm => bm.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(bm => bm.Role)
            .HasColumnName("user_role_id")
            .HasConversion<int>()
            .IsRequired();
        
        builder.HasOne(bm => bm.Board)
            .WithMany(b => b.Members)
            .HasForeignKey(bm => bm.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(bm => new { bm.BoardId, bm.UserId })
            .IsUnique();
    }
}