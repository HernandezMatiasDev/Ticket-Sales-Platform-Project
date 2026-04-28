using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Queries;

// Record plano que actúa como DTO de entrada.
// Devuelve una lista de Sectores del dominio.
public record GetEventSectorsQuery(int EventId) : IQuery<IEnumerable<Sector>>;