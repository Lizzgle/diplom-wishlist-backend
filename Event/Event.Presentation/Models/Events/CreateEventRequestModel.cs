using Event.Domain.Enums;

namespace Event.Presentation.Models.Events;

public class CreateEventRequestModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTimeOffset DateOfEvent { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
     
    public string? Location { get; set; }
     
    public AccessLevel AccessLevel { get; set; }
     
    public List<string>? UserIds { get; set; }
}