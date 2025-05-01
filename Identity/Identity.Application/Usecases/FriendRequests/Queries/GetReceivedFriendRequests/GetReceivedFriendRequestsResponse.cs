namespace Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;

public class GetReceivedFriendRequestsResponse
{
    public List<FriendRequestDto> FriendRequests { get; set; } = new List<FriendRequestDto>();
}

public class FriendRequestDto
{
    public required string Id { get; set; }
    
    public required string SenderId { get; set; }
    
    public required string SenderName { get; set; }
    
    public required string SenderEmail { get; set; }
    
    // TODO add avatar
}