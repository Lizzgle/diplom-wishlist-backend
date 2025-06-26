using Event.Domain.Enums;

namespace Event.Application.Usecases.Events.Queries.GetById;

public class GetByIdResponse
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTimeOffset DateOfEvent { get; set; }
    
    public required Creator Creator { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
     
    public string? Location { get; set; }
     
    public AccessLevel? AccessLevel { get; set; }
     
    public List<ParticipantDto>? ParticipantDtos { get; set; } = new List<ParticipantDto>();
}

public class Creator
{
    public required string CreatorId { get; set; }
    
    public required string Username { get; set; }
}

public class ParticipantDto
{
    public required string CreatorId { get; set; }
    
    public required string Username { get; set; }
}