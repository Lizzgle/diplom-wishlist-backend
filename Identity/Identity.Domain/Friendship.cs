namespace Identity.Domain;

public class Friendship : Entity
{
    public required string Friend1Id { get; set; }
    public User Friend1 { get; set; }
    
    public required string Friend2Id { get; set; }
    public User Friend2 { get; set; }
    
    public DateTime FriendsSince { get; set; }
}