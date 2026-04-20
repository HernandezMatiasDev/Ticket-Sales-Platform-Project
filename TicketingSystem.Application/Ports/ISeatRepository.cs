using System;
using System.Threading.Tasks;
using TicketingSystem.Domain.Entities; // Asumimos que la entidad Seat está en este namespace

namespace TicketingSystem.Application.Ports
{
    /// <summary>
    /// Puerto de salida (contrato) para acceder a los datos de la entidad Seat (Butaca).
    /// Este repositorio será implementado por el compañero de proyecto.
    /// </summary>
    public interface ISeatRepository
    {
        /// <summary>
        /// Obtiene una butaca por su identificador único.
        /// </summary>
        /// <param name="seatId">Identificador de la butaca.</param>
        Task<Seat> GetByIdAsync(Guid seatId);

        /// <summary>
        /// Actualiza el estado y la versión (para bloqueo optimista) de una butaca existente.
        /// </summary>
        /// <param name="seat">La instancia de la butaca a actualizar.</param>
        Task UpdateAsync(Seat seat);
    }
}