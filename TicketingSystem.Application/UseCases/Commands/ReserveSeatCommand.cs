using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases.Commands;

// El record plano que recibe la API
// Retorna bool para indicar si la reserva fue exitosa
public record ReserveSeatCommand(Guid SeatId, int UserId) : ICommand<bool>;