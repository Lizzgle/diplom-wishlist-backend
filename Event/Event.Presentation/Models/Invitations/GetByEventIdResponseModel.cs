using Event.Domain.Enums;

namespace Event.Presentation.Models.Invitations;

public class GetByEventIdResponseModel
{
    public Guid EventId { get; set; }
    
    public List<InvitationByEventIdModel> Invitations { get; set; }
}

public class InvitationByEventIdModel
{
    public Guid Id { get; set; }
    
    public InvitationStatus Status { get; set; }
    
    public required string InvitedId { get; set; }
    
    public required string InvitedEmail { get; set; }
}