namespace Identity.Contracts.Models;

public class GetUsersNamesByIdsDto
{
    public required string Id { get; set; }
    
    public required string UserName { get; set; } 
}