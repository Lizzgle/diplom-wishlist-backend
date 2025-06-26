using Event.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Infrastructure.Configurations;

public class EventEntityTypeConfigurator : IEntityTypeConfiguration<Domain.Event>
{
    public void Configure(EntityTypeBuilder<Domain.Event> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Description).IsRequired(false);
        builder.Property(e => e.CreatorId).IsRequired();
        builder.Property(e => e.DateOfEvent).IsRequired();
        builder.Property(e => e.Recurrence).HasConversion<string>().IsRequired();
        builder.Property(c => c.Location).IsRequired(false);
        
        builder.HasMany(e => e.Participants)
               .WithOne(ce => ce.Event)
               .HasForeignKey(d => d.EventId);
    }
}