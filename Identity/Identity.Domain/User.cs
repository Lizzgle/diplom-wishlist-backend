using Microsoft.AspNetCore.Identity;

namespace Identity.Domain;

public class User : IdentityUser
{
    public DateTimeOffset DateOfBirth { get; set; }

    public List<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
}