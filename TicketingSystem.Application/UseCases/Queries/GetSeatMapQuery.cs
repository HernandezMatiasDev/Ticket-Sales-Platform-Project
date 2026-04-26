using TicketingSystem.Application.DTOs;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Queries;

public class GetSeatMapQuery
{
    private readonly IEventRepository _repository;
    public GetSeatMapQuery(IEventRepository repository) => _repository = repository;

    public async Task<SeatMapDto?> ExecuteAsync(int eventId, int sectorId)
    {
        var @event = await _repository.GetByIdWithDetailsAsync(eventId);
        var sector = @event?.Sectors.FirstOrDefault(s => s.Id == sectorId);
        
        if (sector == null) return null;

        var seats = sector.Seats.Select(s => new SeatDto(
            Id: s.Id, 
            Row: s.Row, 
            Number: s.Number, 
            IsAvailable: s.Status == SeatStatus.Available)).ToList();
       return new SeatMapDto(sector.Name, sector.Price, seats);
    }
}