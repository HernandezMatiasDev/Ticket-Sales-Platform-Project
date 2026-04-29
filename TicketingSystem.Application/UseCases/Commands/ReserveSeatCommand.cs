using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Commands;

public record ReserveSeatCommand(int EventId, Guid SeatId, int UserId) : ICommand<Reservation>;
