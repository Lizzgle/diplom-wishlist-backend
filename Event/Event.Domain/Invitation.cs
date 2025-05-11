using Event.Domain.Enums;

namespace Event.Domain;

public class Invitation : Entity
{
    public required InvitationStatus Status { get; set; }
    
    public required string OrganizerId { get; set; }
    public User Organizer { get; init; }
    
    public required string InvitedId { get; set; }
    public User Invited { get; init; }
    
    public required Guid EventId { get; set; }
    public Event Event { get; set; }
}
