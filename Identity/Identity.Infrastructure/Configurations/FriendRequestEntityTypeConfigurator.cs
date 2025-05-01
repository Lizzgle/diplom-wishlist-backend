using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class FriendRequestEntityTypeConfigurator : IEntityTypeConfiguration<FriendRequest>
{
    public void Configure(EntityTypeBuilder<FriendRequest> builder)
    {
        builder.ToTable("friend_requests");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.SenderId).IsRequired();
        builder.Property(f => f.ReceiverId).IsRequired();
        builder.Property(f => f.Status).IsRequired();
        builder.Property(f => f.CreatedAt).IsRequired();

        builder.HasOne(fr => fr.Sender)
            .WithMany()
            .HasForeignKey(fr => fr.SenderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fr => fr.Receiver)
            .WithMany()
            .HasForeignKey(fr => fr.ReceiverId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}