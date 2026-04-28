using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers;

public class GetEventSectorsHandler : IQueryHandler<GetEventSectorsQuery, IEnumerable<Sector>>
{
    private readonly ISectorRepository _repository;

    // Inyectado
    public GetEventSectorsHandler(ISectorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Sector>> HandleAsync(GetEventSectorsQuery query)
    {
        return await _repository.GetByEventIdAsync(query.EventId);
    }
}