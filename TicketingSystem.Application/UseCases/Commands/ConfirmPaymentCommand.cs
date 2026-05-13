using System;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Commands
{
    public record ConfirmPaymentCommand(int UserId) : ICommand<bool>;
}