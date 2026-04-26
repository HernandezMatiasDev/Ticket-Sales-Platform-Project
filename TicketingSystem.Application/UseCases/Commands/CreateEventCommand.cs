using TicketingSystem.Application.Interfaces;
namespace TicketingSystem.Application.UseCases.Commands; 

// Es un registro (record) porque los comandos deben ser inmutables
public record CreateEventCommand(string Name, DateTime EventDate, string Venue) : ICommand<int>;