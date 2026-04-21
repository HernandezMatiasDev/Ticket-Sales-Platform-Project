using System;
using System.Threading.Tasks;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;

namespace TicketingSystem.Application.UseCases
{
    /// <summary>
    /// Caso de uso central de la aplicación: Reserva de butacas.
    /// Orquesta las validaciones de negocio, la actualización de estados, 
    /// la auditoría y el control de transaccionalidad.
    /// </summary>
    public class ReserveSeatUseCase
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa el caso de uso inyectando los repositorios necesarios y la Unidad de Trabajo.
        /// </summary>
        public ReserveSeatUseCase(
            ISeatRepository seatRepository,
            IReservationRepository reservationRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork)
        {
            _seatRepository = seatRepository;
            _reservationRepository = reservationRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Ejecuta de forma asíncrona la lógica de negocio para reservar una butaca.
        /// </summary>
        /// <param name="userId">Identificador del usuario que hace la reserva.</param>
        /// <param name="eventId">Identificador del evento (para validación).</param>
        /// <param name="seatId">Identificador de la butaca deseada.</param>
        /// <returns>La entidad de reserva creada si la operación es exitosa.</returns>
        /// <exception cref="InvalidOperationException">Si la butaca no está disponible.</exception>
        public async Task<Reservation> ExecuteAsync(int userId, int eventId, Guid seatId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1. Obtener y verificar butaca.
                // NOTA: Para que esta validación funcione, la implementación de ISeatRepository.GetByIdAsync
                // DEBE cargar la entidad relacionada 'Sector' (usando .Include(s => s.Sector)).
                var seat = await _seatRepository.GetByIdAsync(seatId);

                // Se valida que la butaca exista, esté disponible y pertenezca al evento correcto.
                // ¡ERROR CRÍTICO CORREGIDO! Se activa la validación del EventId.
                if (seat == null || seat.Status != SeatStatus.Available || seat.Sector.EventId != eventId)
                {
                    throw new InvalidOperationException("La butaca no está disponible.");
                }

                // 2. Modificar butaca
                seat.Reserve(); // Usamos el método del dominio para cambiar el estado
                await _seatRepository.UpdateAsync(seat);

                // 3. Crear Reserva
                var reservation = new Reservation(
                    Guid.NewGuid(),
                    userId,
                    seatId,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(5)
                );
                await _reservationRepository.AddAsync(reservation);

                // 4. Log de intento exitoso (se agrupa en la transacción)
                var successLog = new AuditLog(
                    Guid.NewGuid(),
                    userId,
                    "RESERVE_SUCCESS",
                    "Reservation",
                    reservation.Id.ToString(),
                    $"Reserva bloqueada para butaca {seatId}",
                    DateTime.UtcNow
                );
                await _auditLogRepository.AddAsync(successLog);

                // 5. Confirmar todo (Si hay concurrencia, EF Core lanzará una excepción aquí)
                await _unitOfWork.CommitTransactionAsync();

                return reservation;
            }
            catch (Exception ex)
            {
                // 6. Revertimos la reserva, la butaca y el log de éxito temporal
                await _unitOfWork.RollbackTransactionAsync();

                // 7. Disparamos el registro del fallo independientemente
                await LogFailureAsync(userId, seatId, ex.Message);

                // Relanzamos la excepción para que el Controller devuelva el código HTTP 409 Conflict
                throw; 
            }
        }

        /// <summary>
        /// Método privado auxiliar para registrar un log de auditoría en caso de que 
        /// el intento de reserva fracase.
        /// </summary>
        /// <param name="userId">Identificador del usuario afectado.</param>
        /// <param name="seatId">Identificador de la butaca involucrada.</param>
        /// <param name="reason">Motivo de la falla.</param>
        private async Task LogFailureAsync(int userId, Guid seatId, string reason)
        {
            var errorLog = new AuditLog(
                Guid.NewGuid(),
                userId,
                "RESERVE_FAILED",
                "Seat",
                seatId.ToString(),
                $"Fallo al intentar reservar: {reason}",
                DateTime.UtcNow
            );

            await _auditLogRepository.AddAsync(errorLog);
            
            // Guardamos solo el log. 
            // Gracias a que UnitOfWork limpia el ChangeTracker en el Rollback previo,
            // esta operación aisla correctamente el guardado sin arrastrar errores anteriores.
            try 
            { 
                await _unitOfWork.SaveChangesAsync(); 
            } 
            catch { /* Ignorar errores secundarios de auditoría para no ocultar el error original */ }
        }
    }
}