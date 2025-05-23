using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SecretSanta.Domain;

namespace SecretSanta.Infrastructure.Configurations;

public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description).IsRequired(false);
        builder.Property(x => x.Url).IsRequired(false);
        builder.Property(x => x.MaxAdvance);
        builder.Property(x => x.MinAdvance);
        builder.Property(x => x.DateOfGeneration);
        
        builder.HasMany(x => x.Players).WithOne(x => x.Game).OnDelete(DeleteBehavior.Cascade);
    }
}