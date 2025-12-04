using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasColumnName("firstname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasColumnName("lastname")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Avatar)
            .HasColumnName("avatar")
            .HasMaxLength(255);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .HasColumnName("password")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasColumnName("refreshtoken")
            .HasMaxLength(255);

        builder.Property(u => u.RefreshTokenExpireTime)
            .HasColumnName("refreshtokenexpiretime");
    }
}