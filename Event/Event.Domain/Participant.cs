namespace Event.Domain;

public class Participant
{
    public required string UserId { get; set; }
    public User User { get; init; }
    
    public required Guid EventId { get; set; }
    
    public Event Event { get; set; }
}