namespace Event.Application.Usecases.Events.Queries.GetAllForUser;

public class GetAllForUserResponse
{
    public required string Name { get; set; }

    public required DateTime DateOfEvent { get; set; }
    
    public required string CreatorId { get; set; }
     
    public string? Location { get; set; }
}