using Identity.Domain;

namespace Identity.Application.Usecases.Users.Queries.GetAllUsers;

public class GetAllUsersResponse
{
    public List<AllUserDto> Users { get; set; }
}

public class AllUserDto
{
    public required string Id { get; set; }
    
    public required string Email { get; set; }
    
    public required string UserName { get; set; }
    
}