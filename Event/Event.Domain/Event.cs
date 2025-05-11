using Event.Domain.Enums;

namespace Event.Domain;

public class Event : Entity
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTime DateOfEvent { get; set; }
    
    public required string CreatorId { get; set; }
    
    public AccessLevel AccessLevel { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
    
    public string? Location { get; set; }
    
    public List<Participant> Participants { get; set; } = new List<Participant>();
}