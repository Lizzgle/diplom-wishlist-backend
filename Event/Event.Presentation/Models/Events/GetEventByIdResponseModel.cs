using Event.Domain.Enums;

namespace Event.Presentation.Models.Events;

public class GetEventByIdResponseModel
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTimeOffset DateOfEvent { get; set; }
    
    public required CreatorModel Creator { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
     
    public string? Location { get; set; }
     
    public AccessLevel? AccessLevel { get; set; }
     
    public List<ParticipantDtoModel>? ParticipantDtos { get; set; } = new List<ParticipantDtoModel>();
}

public class CreatorModel
{
    public required string CreatorId { get; set; }
    
    public required string Username { get; set; }
}

public class ParticipantDtoModel
{
    public required string CreatorId { get; set; }
    
    public required string Username { get; set; }
}