using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Infrastructure.Persistence;

namespace TicketingSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio para la entidad Reservation.
    /// Se encarga de la persistencia y recuperación de datos de las reservas en la base de datos.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        // Nota: Asume que "TicketingDbContext" es el nombre de la clase DbContext que creó tu compañero.
        // Ajusta el nombre si él le puso otro (ej. ApplicationDbContext).
        private readonly TicketingDbContext _context; // Asegúrate que el nombre del DbContext sea consistente

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReservationRepository"/>.
        /// </summary>
        /// <param name="context">El contexto de base de datos de Entity Framework.</param>
        public ReservationRepository(TicketingDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Agrega una nueva reserva al contexto de la base de datos de forma asíncrona.
        /// </summary>
        /// <param name="reservation">La reserva a agregar.</param>
        public async Task AddAsync(Reservation reservation)
        {
            await _context.Set<Reservation>().AddAsync(reservation);
        }

        /// <summary>
        /// Obtiene todas las reservas que están en estado pendiente y cuyo tiempo de expiración
        /// es menor o igual al tiempo actual proporcionado.
        /// </summary>
        /// <param name="currentTime">La fecha y hora actual para comparar con la fecha de expiración de la reserva.</param>
        /// <returns>Una colección de reservas pendientes que ya han expirado.</returns>
        public async Task<IEnumerable<Reservation>> GetExpiredPendingReservationsAsync(DateTime currentTime)
        {
            return await _context.Set<Reservation>()
                .Where(r => r.Status == ReservationStatus.Pending && r.ExpiresAt <= currentTime)
                .ToListAsync();
        }

        /// <summary>
        /// Actualiza los datos de una reserva existente en el contexto de la base de datos.
        /// Nota: Para que los cambios se guarden definitivamente en la base de datos, 
        /// se debe invocar el método de guardado en la unidad de trabajo (UnitOfWork).
        /// </summary>
        /// <param name="reservation">La reserva con los datos actualizados.</param>
        /// <returns>Una tarea completada.</returns>
        public Task UpdateAsync(Reservation reservation)
        {
            _context.Set<Reservation>().Update(reservation);
            return Task.CompletedTask;
        }
    }
}