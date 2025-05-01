using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class FriendshipEntityTypeConfigurator : IEntityTypeConfiguration<Friendship>
{
    public void Configure(EntityTypeBuilder<Friendship> builder)
    {
        builder.ToTable("friendship");
        
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Friend1Id).IsRequired();
        builder.Property(f => f.Friend2Id).IsRequired();
        builder.Property(f => f.FriendsSince).IsRequired();
        
        builder.HasIndex(f => new { f.Friend1Id, f.Friend2Id }).IsUnique();

        builder.HasOne(fr => fr.Friend1)
            .WithMany()
            .HasForeignKey(fr => fr.Friend1Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(fr => fr.Friend2)
            .WithMany()
            .HasForeignKey(fr => fr.Friend2Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}