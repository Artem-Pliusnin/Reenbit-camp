using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("Session");

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Token)
            .HasColumnName("token")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasIndex(s => s.Token)
            .IsUnique();

        builder.Property(s => s.ExpiresOn)
            .HasColumnName("expires_on")
            .IsRequired();
        
        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Session>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}