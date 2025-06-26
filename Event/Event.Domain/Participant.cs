namespace Event.Domain;

public class Participant
{
    public required string UserId { get; set; }
    
    public required Guid EventId { get; set; }
    
    public Event Event { get; set; }
}