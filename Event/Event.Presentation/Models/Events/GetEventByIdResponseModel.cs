using Event.Domain.Enums;

namespace Event.Presentation.Models.Events;

public class GetEventByIdResponseModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTime DateOfEvent { get; set; }
    
    public required string CreatorId { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
     
    public string? Location { get; set; }
     
    public AccessLevel? AccessLevel { get; set; }
     
    public List<string>? ParicipantIds { get; set; } = new List<string>();
}