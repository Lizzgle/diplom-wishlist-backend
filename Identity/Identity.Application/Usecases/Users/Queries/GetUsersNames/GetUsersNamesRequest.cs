using MediatR;

namespace Identity.Application.Usecases.Users.Queries.GetUsersNames;

public class GetUsersNamesRequest : IRequest<GetUsersNamesResponse>
{
    public List<string> UserIds { get; set; }
}