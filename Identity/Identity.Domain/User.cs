using Microsoft.AspNetCore.Identity;

namespace Identity.Domain;

public class User : IdentityUser
{
    public required string DateOfBirth { get; set; }
}