using Identity.Contracts.Models;

namespace Identity.Application.Usecases.Users.Queries.GetUsersNames;

public class GetUsersNamesResponse
{
    public List<GetUsersNamesByIdsDto> Users { get; set; }
}