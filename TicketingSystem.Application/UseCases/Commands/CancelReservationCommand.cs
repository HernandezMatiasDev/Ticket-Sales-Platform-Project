using System;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Commands
{
    public record CancelReservationCommand(Guid ReservationId, int UserId) : ICommand<bool>;
}