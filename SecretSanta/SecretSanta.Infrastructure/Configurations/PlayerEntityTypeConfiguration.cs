using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretSanta.Domain;

namespace SecretSanta.Infrastructure.Configurations;

public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.GameId).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Card);
        builder.Property(x => x.RecipientId);
    }
}