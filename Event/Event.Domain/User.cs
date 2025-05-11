namespace Event.Domain;

public class User
{
    public required string Id { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Email { get; init; }
}