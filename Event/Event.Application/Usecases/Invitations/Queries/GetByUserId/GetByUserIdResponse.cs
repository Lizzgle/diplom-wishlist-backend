using Event.Domain.Enums;

namespace Event.Application.Usecases.Invitations.Queries.GetByUserId;

public class GetByUserIdResponse
{
    public required string UserId { get; set; }
    
    public List<InvitationByUserId> Invitations { get; set; }
}

public class InvitationByUserId
{
    public Guid Id { get; set; }
    
    public InvitationStatus Status { get; set; }
    
    public required string OrganizerId { get; set; }
    
    public required string OrganizerEmail { get; set; }
    
    public required Guid EventId { get; set; }
    
    public required string EventName { get; set; }
    
    public required DateTime EventDate { get; set; }
}