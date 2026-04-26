using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Infrastructure.Persistence;

namespace TicketingSystem.Infrastructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly TicketingDbContext _context;

        public SeatRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Seat?> GetByIdAsync(Guid seatId)
        {
            // Es VITAL el .Include(s => s.Sector) para que el ReserveSeatUseCase 
            // pueda validar que la butaca pertenece al evento correcto.
            return await _context.Seats
                .Include(s => s.Sector)
                .FirstOrDefaultAsync(s => s.Id == seatId);
        }

        public Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            return Task.CompletedTask;
        }
    }
}