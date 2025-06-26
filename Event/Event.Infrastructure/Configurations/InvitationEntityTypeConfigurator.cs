using Event.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Infrastructure.Configurations;

public class InvitationEntityTypeConfigurator : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.EventId).IsRequired();
        builder.Property(i => i.InvitedId).IsRequired();
        builder.Property(i => i.OrganizerId).IsRequired();
        builder.Property(i => i.Status).IsRequired();
    }
}