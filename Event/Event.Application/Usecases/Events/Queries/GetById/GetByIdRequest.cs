using MediatR;

namespace Event.Application.Usecases.Events.Queries.GetById;

public class GetByIdRequest : IRequest<GetByIdResponse>
{
    public required string UserId { get; set; }
    
    public required Guid EventId { get; set; }
}