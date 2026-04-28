using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Queries;

public record GetSeatMapQuery(int SectorId) : IQuery<IEnumerable<Seat>>;