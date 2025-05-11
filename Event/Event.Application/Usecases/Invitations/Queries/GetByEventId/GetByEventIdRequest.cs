using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetByEventId;

public class GetByEventIdRequest : IRequest<GetByEventIdResponse>
{
    public required Guid EventId { get; init; }

    public required string UserId { get; set; }
}