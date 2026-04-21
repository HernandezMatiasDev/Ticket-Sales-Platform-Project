using TicketingSystem.Application.DTOs;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.Queries;

public class GetEventsQuery
{
    private readonly IEventRepository _repository;
    public GetEventsQuery(IEventRepository repository) => _repository = repository;

    public async Task<IEnumerable<EventDto>> ExecuteAsync()
    {
        var events = await _repository.GetAllAsync();
        return events.Select(e => new EventDto(e.Id, e.Name, e.Date, e.Venue));
    }
}