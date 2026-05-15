using System.Collections.Generic;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Queries
{
    public record GetPaidReservationsQuery(int UserId) : IQuery<IEnumerable<PendingReservationDto>>;
}