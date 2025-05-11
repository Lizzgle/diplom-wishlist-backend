using Event.Domain.Enums;

namespace Event.Presentation.Models.Invitations;

public class GetByUserIdResponseModel
{
    public required string UserId { get; set; }
    
    public List<InvitationByUserIdModel> Invitations { get; set; }
}

public class InvitationByUserIdModel
{
    public Guid Id { get; set; }
    
    public InvitationStatus Status { get; set; }
    
    public required string OrganizerId { get; set; }
    
    public required string OrganizerEmail { get; set; }
    
    public required Guid EventId { get; set; }
    
    public required string EventName { get; set; }
    
    public required DateTime EventDate { get; set; }
}