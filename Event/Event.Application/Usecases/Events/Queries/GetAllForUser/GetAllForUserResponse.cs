namespace Event.Application.Usecases.Events.Queries.GetAllForUser;

public class GetAllForUserResponse
{
    public List<EventDto> Events { get; set; }
}

public class EventDto
{
    public Guid Id { get; set; }

    public required DateTimeOffset DateOfEvent { get; set; }
}