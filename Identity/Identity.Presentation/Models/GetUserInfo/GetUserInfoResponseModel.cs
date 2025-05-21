namespace Identity.Presentation.Models.GetUserInfo;

public class GetUserInfoResponseModel
{
    // TODO add avatar
    
    public required string Id { get; set; }
    
    public required string Email { get; set; }
    
    public required string UserName { get; set; }
    
    public DateTimeOffset DateOfBirth { get; set; }
}