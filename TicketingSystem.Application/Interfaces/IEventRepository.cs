using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.Interfaces;

public interface IEventRepository
{
    Task AddAsync(Event newEvent);

    Task<IEnumerable<Event>> GetAllAsync();
    Task<Event?> GetByIdWithDetailsAsync(int eventId);
}