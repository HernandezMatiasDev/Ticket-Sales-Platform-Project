using System;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Commands
{
    public record ConfirmPaymentCommand(Guid ReservationId, int UserId) : ICommand<bool>;
}