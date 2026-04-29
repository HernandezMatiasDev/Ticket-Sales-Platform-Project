using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Persistence;

namespace TicketingSystem.Infrastructure.Repositories;

public class SectorRepository : ISectorRepository
{
    private readonly TicketingDbContext _context;

    public SectorRepository(TicketingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Sector>> GetByEventIdAsync(int eventId)
    {
        return await _context.Sectors.Where(s => s.EventId == eventId).ToListAsync();
    }
}