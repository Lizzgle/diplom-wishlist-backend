using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Seeders;

public static class RoleSeeder
{
    public static void SeedRoles(this EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData([
            new IdentityRole()
            {
                Id = Guid.Parse("45a89411-4ca2-4a3f-a3f4-3c764f4d1904").ToString(),
                Name = "admin",
                NormalizedName = "ADMIN",
            },
            new IdentityRole()
            {
                Id = Guid.Parse("b8f1d6a9-b055-4085-90aa-d785ccdd2913").ToString(),
                Name = "user",
                NormalizedName = "USER"
            }
        ]);
    }
}