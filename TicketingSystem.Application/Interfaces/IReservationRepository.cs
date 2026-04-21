using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.Interfaces
{
    /// <summary>
    /// Puerto de salida para gestionar la persistencia de las reservas.
    /// Aisla la lógica de negocio de la tecnología de base de datos subyacente.
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Agrega una nueva reserva al repositorio de datos.
        /// </summary>
        /// <param name="reservation">La entidad de reserva a persistir.</param>
        Task AddAsync(Reservation reservation);

        /// <summary>
        /// Obtiene todas las reservas que siguen en estado pendiente pero cuyo
        /// tiempo de expiración ya ha sido superado.
        /// </summary>
        /// <param name="currentTime">La fecha y hora actual para calcular la expiración.</param>
        /// <returns>Una lista de reservas expiradas listas para ser procesadas o canceladas.</returns>
        Task<IEnumerable<Reservation>> GetExpiredPendingReservationsAsync(DateTime currentTime);

        /// <summary>
        /// Actualiza el estado o los datos de una reserva existente.
        /// </summary>
        /// <param name="reservation">La reserva con los cambios a aplicar.</param>
        Task UpdateAsync(Reservation reservation);
    }
}
