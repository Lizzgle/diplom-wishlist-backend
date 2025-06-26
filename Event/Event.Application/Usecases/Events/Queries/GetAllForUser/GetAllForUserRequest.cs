using MediatR;

namespace Event.Application.Usecases.Events.Queries.GetAllForUser;

public class GetAllForUserRequest : IRequest<GetAllForUserResponse>
{
    public required string UserId { get; set; } 
}