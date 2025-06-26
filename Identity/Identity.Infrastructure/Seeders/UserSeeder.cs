using Identity.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Seeders;

public static class UserSeeder
{
    public static void SeedUsers(this EntityTypeBuilder<User> builder)
    {
        builder.HasData([
            new User()
            {
                Id = Guid.Parse("aeabe49f-3130-4197-bea5-a80b4b49295c").ToString(),
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                DateOfBirth = new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero)
            },
            new User()
            {
                Id = Guid.Parse("48ee98e2-8cbc-45ee-86e1-79b74aa8ce9f").ToString(),
                UserName = "user",
                NormalizedUserName = "USER",
                Email = "user@user.com",
                NormalizedEmail = "USER@USER.COM",
                EmailConfirmed = true,
                DateOfBirth = new DateTimeOffset(new DateTime(2000, 1, 1), TimeSpan.Zero)
            }
        ]);
    }
}