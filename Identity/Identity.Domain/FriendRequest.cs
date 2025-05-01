using Identity.Domain.Enums;

namespace Identity.Domain;

public class FriendRequest : Entity
{
    public required string SenderId { get; set; }
    public User Sender { get; set; }
    
    public required string ReceiverId { get; set; }
    public User Receiver { get; set; }
    
    public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? RespondedAt { get; set; }
}