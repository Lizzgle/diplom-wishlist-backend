namespace Event.Presentation.Models.Events;

public class GetEventsResponseModel
{
    public List<EventDtoModel> Events { get; set; } = new List<EventDtoModel>();
}

public class EventDtoModel
{
    public Guid Id { get; set; }

    public required DateTimeOffset DateOfEvent { get; set; }
}