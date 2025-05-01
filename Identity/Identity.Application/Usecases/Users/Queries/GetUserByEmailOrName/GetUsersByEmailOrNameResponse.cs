namespace Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;

public class GetUsersByEmailOrNameResponse
{
    public List<UserDto> Users { get; set; } = new List<UserDto>();
}

public class UserDto
{
    public string Id { get; init; }
    public required string Email { get; init; }
    
    public required string UserName { get; init; }
    
    // TODO add avatar
}
