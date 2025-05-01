using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Seeders;

public static class UserRoleSeeder
{
    public static void SeedUserRoles(this EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData([
            // admin
            new IdentityUserRole<string>()
            {
                UserId = Guid.Parse("aeabe49f-3130-4197-bea5-a80b4b49295c").ToString(),
                RoleId = Guid.Parse("45a89411-4ca2-4a3f-a3f4-3c764f4d1904").ToString(),
            },
            // user
            new IdentityUserRole<string>()
            {
                UserId = Guid.Parse("48ee98e2-8cbc-45ee-86e1-79b74aa8ce9f").ToString(),
                RoleId = Guid.Parse("b8f1d6a9-b055-4085-90aa-d785ccdd2913").ToString(),
            }
        ]);
    }
}