using Event.Domain.Enums;

namespace Event.Application.Usecases.Invitations.Queries.GetByEventId;

public class GetByEventIdResponse
{
    public Guid EventId { get; set; }
    
    public List<InvitationByEventId> Invitations { get; set; }
}

public class InvitationByEventId
{
    public Guid Id { get; set; }
    
    public InvitationStatus Status { get; set; }
    
    public required string InvitedId { get; set; }
    
    public required string InvitedEmail { get; set; }
}