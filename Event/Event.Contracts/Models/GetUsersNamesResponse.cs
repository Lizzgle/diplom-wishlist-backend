namespace Event.Contracts.Models;

public class GetUsersNamesResponse
{
    public required string UserId { get; set; }
    
    public required string UserName { get; set; }
}