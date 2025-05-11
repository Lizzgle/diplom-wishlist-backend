using Event.Domain;
using Event.Domain.Enums;
using MediatR;

namespace Event.Application.Usecases.Events.Commands.Update;

public class UpdateEventRequest : IRequest
{
    public required Guid EventId { get; set; }
    
    public required string Name { get; set; }

    public string? Description { get; set; }

    public required DateTime DateOfEvent { get; set; }
    
    public required string UserId { get; set; }
    
    public RecurrenceType Recurrence { get; set; }
    
    public string? Location { get; set; }
    
    public List<string>? UserIds { get; set; }
}