using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers;

public class GetEventsHandler : IQueryHandler<GetEventsQuery, IEnumerable<Event>>
{
    private readonly IEventRepository _repository;

    // Inyectado
    public GetEventsHandler(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Event>> HandleAsync(GetEventsQuery query)
    {
        return await _repository.GetAllAsync();
    }
}