using Event.Domain.Enums;

namespace Event.Presentation.Models.Invitations;

public class GetByIdResponseModel
{
    public Guid Id { get; set; }
    
    public required InvitationStatus Status { get; set; }
    
    public required string OrganizerId { get; set; }
    
    public required string OrganizerEmail { get; init; }
    
    public required string InvitedId { get; set; }
    
    public required string InvitedEmail { get; init; }
    
    public required Guid EventId { get; set; }
    
    public required string EventName { get; init; }
    
    public required DateTime EventDate { get; init; }
}