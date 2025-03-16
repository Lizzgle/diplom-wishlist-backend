using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class UserEntityTypeConfigurator : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.NormalizedEmail).IsRequired();
        builder.Property(x => x.NormalizedUserName).IsRequired();
        builder.Property(x => x.DateOfBirth).IsRequired();
        builder.Property(x => x.EmailConfirmed).HasDefaultValue(false);

        builder.Ignore(x => x.PhoneNumber);
        builder.Ignore(x => x.PhoneNumberConfirmed);
        builder.Ignore(x => x.LockoutEnd);
        builder.Ignore(x => x.LockoutEnabled);
    }
}