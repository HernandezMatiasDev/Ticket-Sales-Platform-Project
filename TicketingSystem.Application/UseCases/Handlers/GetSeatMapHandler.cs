using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers;

public class GetSeatMapHandler : IQueryHandler<GetSeatMapQuery, IEnumerable<Seat>>
{
    private readonly ISeatRepository _repository;

    // Inyectado
    public GetSeatMapHandler(ISeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Seat>> HandleAsync(GetSeatMapQuery query)
    {
        return await _repository.GetBySectorIdAsync(query.SectorId);
    }
}