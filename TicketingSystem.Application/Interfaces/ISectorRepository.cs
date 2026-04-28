using System;
using System.Threading.Tasks;
using TicketingSystem.Domain.Entities; // Asumimos que la entidad Seat está en este namespace

namespace TicketingSystem.Application.Interfaces;

public interface ISectorRepository
{    

    Task<IEnumerable<Sector>> GetByEventIdAsync(int eventId);

}