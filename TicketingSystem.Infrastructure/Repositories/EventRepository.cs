using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Persistence;

namespace TicketingSystem.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly TicketingDbContext _context;

    public EventRepository(TicketingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        // Traemos los eventos de la base de datos
        return await _context.Events.ToListAsync();
    }

    public async Task<Event?> GetByIdWithDetailsAsync(int eventId)
    {
        // Aquí usamos Include para traer también los sectores y sus asientos (Eager Loading)
        return await _context.Events
            .Include(e => e.Sectors)
                .ThenInclude(s => s.Seats)
            .FirstOrDefaultAsync(e => e.Id == eventId);
    }
}