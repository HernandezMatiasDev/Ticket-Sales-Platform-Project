using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Queries;

// Ahora es un record plano que representa la solicitud. 
// Como no devuelve DTO, el tipo de retorno es IEnumerable<Event>.
public record GetEventsQuery() : IQuery<IEnumerable<Event>>;