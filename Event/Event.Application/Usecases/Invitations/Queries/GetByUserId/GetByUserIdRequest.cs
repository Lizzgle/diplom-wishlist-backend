using MediatR;

namespace Event.Application.Usecases.Invitations.Queries.GetByUserId;

public class GetByUserIdRequest : IRequest<GetByUserIdResponse>
{
    public required string UserId { get; set; }
}