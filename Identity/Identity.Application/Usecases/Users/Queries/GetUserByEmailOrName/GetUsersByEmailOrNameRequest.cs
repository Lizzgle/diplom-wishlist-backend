using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;

public class GetUsersByEmailOrNameRequest : IRequest<GetUsersByEmailOrNameResponse>
{
    public required string Query { get; set; }
}