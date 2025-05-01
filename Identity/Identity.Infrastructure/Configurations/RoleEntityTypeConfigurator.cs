using Identity.Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class RoleEntityTypeConfigurator : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.ToTable("roles");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name).IsRequired();
        builder.Property(r => r.NormalizedName).IsRequired();

        builder.Ignore(r => r.ConcurrencyStamp);
        
        builder.SeedRoles();
    }
}