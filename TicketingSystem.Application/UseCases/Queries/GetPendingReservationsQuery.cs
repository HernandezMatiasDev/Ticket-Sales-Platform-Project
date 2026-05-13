using System.Collections.Generic;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Queries
{
    public record GetPendingReservationsQuery(int UserId) : IQuery<IEnumerable<PendingReservationDto>>;
}