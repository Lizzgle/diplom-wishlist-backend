using MediatR;

namespace Event.Application.Usecases.Events.Commands.Delete;

public class DeleteEventRequest : IRequest
{
    public required string UserId { get; set; }
    
    public required Guid EventId { get; set; }
}