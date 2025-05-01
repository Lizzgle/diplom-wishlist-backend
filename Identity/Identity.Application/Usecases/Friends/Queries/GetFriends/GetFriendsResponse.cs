namespace Identity.Application.Usecases.Friends.Queries.GetFriends;

public class GetFriendsResponse
{
    public List<FriendDto> Friends { get; set; } = new List<FriendDto>();
}

public class FriendDto
{
    public required string FriendId { get; set; }
    
    public required string FriendName { get; set; }
    
    public required string FriendEmail { get; set; }
    
    // TODO add avatar
}