namespace Identity.Application.Usecases.Users.Queries.GetUserInfo;

public class GetUserInfoResponse
{
    // TODO add avatar
    
    public required string Id { get; set; }
    
    public required string Email { get; set; }
    
    public required string UserName { get; set; }
    
    public DateTimeOffset DateOfBirth { get; set; }
}