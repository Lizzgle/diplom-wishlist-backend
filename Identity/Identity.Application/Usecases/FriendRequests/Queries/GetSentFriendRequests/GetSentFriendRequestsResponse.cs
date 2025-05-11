namespace Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests;

public class GetSentFriendRequestsResponse
{
    public List<FriendRequestDto> FriendRequests { get; set; } = new List<FriendRequestDto>();
}

public class FriendRequestDto
{
    public required string Id { get; set; }
    
    public required string ReceiverId { get; set; }
    
    public required string ReceiverName { get; set; }
    
    public required string ReceiverEmail { get; set; }
    
    // TODO add avatar
}