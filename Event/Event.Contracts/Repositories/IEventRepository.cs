namespace Event.Contracts.Repositories;

public interface IEventRepository : IBaseRepository<Domain.Event>
{
    Task<List<Domain.Event>> GetEventsByIdsAsync(IEnumerable<Guid> eventIds, CancellationToken cancellationToken);
}