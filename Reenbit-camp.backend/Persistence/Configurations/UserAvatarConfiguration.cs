using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserAvatarConfiguration : IEntityTypeConfiguration<UserAvatar>
{
    public void Configure(EntityTypeBuilder<UserAvatar> builder)
    {
        builder.ToTable("UserAvatars");

        builder.Property(ua => ua.Id)
            .HasColumnName("id");

        builder.HasKey(ua => ua.Id);
        
        builder.Property(ua => ua.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(ua => ua.FileName)
            .HasColumnName("file_name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(ua => ua.FileUrl)
            .HasColumnName("file_url")
            .HasMaxLength(500)
            .IsRequired();
        
        builder.HasOne(ua => ua.User)
            .WithOne(u => u.Avatar)
            .HasForeignKey<UserAvatar>(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(u => u.UserId)
            .IsUnique();
    }
}