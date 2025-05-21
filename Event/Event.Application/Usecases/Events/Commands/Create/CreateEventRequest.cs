using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Events.Commands.Create;

public class CreateEventRequest : IRequest
{
     public required string Name { get; set; }

     public string? Description { get; set; }

     public required DateTimeOffset DateOfEvent { get; set; }
    
     public required string CreatorId { get; set; }
    
     public RecurrenceType Recurrence { get; set; }
     
     public string? Location { get; set; }
     
     public AccessLevel AccessLevel { get; set; }
     
     public List<string>? UserIds { get; set; }
}