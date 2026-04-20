using System.Threading.Tasks;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.Ports
{
    /// <summary>
    /// Puerto de salida para el registro de auditoría.
    /// Define el contrato para persistir los eventos importantes del sistema (éxitos y fallos),
    /// permitiendo la trazabilidad sin acoplar el dominio a una base de datos específica.
    /// </summary>
    public interface IAuditLogRepository
    {
        /// <summary>
        /// Registra un nuevo evento de auditoría en el sistema de forma asíncrona.
        /// </summary>
        /// <param name="auditLog">La entidad que contiene los detalles del evento a registrar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        Task AddAsync(AuditLog auditLog);
    }
}
