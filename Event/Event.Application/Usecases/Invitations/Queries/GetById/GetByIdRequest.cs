using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetById;

public class GetByIdRequest : IRequest<GetByIdResponse>
{
    public required Guid Id { get; init; }

    public required string UserId { get; set; }
}