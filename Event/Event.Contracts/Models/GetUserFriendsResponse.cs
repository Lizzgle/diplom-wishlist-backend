namespace Event.Contracts.Models;

public class GetUserFriendsResponse
{
    public List<UserFriendDto> UserFriends { get; set; }
}

public class UserFriendDto
{
    public required string UserId { get; set; }
    
    public required string UserEmail { get; set; }
}