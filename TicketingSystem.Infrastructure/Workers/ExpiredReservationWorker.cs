using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Infrastructure.Workers
{
    /// <summary>
    /// Servicio en segundo plano (Background Worker) que se ejecuta periódicamente 
    /// para buscar reservas que no fueron pagadas a tiempo y liberar las butacas asociadas.
    /// </summary>
    public class ExpiredReservationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ExpiredReservationWorker> _logger;

        // Se inyecta IServiceProvider porque un BackgroundService es un "Singleton" (vive siempre),
        // pero nuestros repositorios son "Scoped" (viven por petición). Necesitamos crear un Scope manual.
        /// <summary>
        /// Inicializa el Worker. Recibe el IServiceProvider para poder crear scopes temporales 
        /// y un ILogger para registrar la actividad del proceso.
        /// </summary>
        /// <param name="serviceProvider">El proveedor de inyección de dependencias.</param>
        /// <param name="logger">El servicio de registro de logs.</param>
        public ExpiredReservationWorker(IServiceProvider serviceProvider, ILogger<ExpiredReservationWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// Método principal del servicio. Se ejecuta en un bucle continuo mientras la API esté viva.
        /// </summary>
        /// <param name="stoppingToken">Token para cancelar la tarea cuando se apaga la API.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Bucle infinito mientras la API esté corriendo
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker revisando reservas expiradas...");

                try
                {
                    // Creamos un entorno (Scope) temporal para inyectar nuestras dependencias
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var reservationRepo = scope.ServiceProvider.GetRequiredService<IReservationRepository>();
                        var seatRepo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();
                        var auditLogRepo = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
                        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                        // Buscamos reservas vencidas (Fecha actual >= ExpiresAt)
                        var expiredReservations = await reservationRepo.GetExpiredPendingReservationsAsync(DateTime.UtcNow);

                        foreach (var reservation in expiredReservations)
                        {
                            await unitOfWork.BeginTransactionAsync();
                            try
                            {
                                // 1. Cambiar estado de la reserva a "Expired"
                                reservation.MarkAsExpired();
                                await reservationRepo.UpdateAsync(reservation);

                                // 2. Liberar la butaca
                                var seat = await seatRepo.GetByIdAsync(reservation.SeatId);
                                if (seat != null)
                                {
                                    seat.MakeAvailable(); // Usamos el método del dominio para liberar la butaca
                                    await seatRepo.UpdateAsync(seat);
                                }

                                // 3. Log de auditoría (UserId = null porque es una acción automática del sistema)
                                var auditLog = new AuditLog(
                                    Guid.NewGuid(),
                                    0, // Usamos 0 (o tu ID para el sistema) ya que int no acepta null.
                                    "RESERVE_EXPIRED",
                                    "Reservation",
                                    reservation.Id.ToString(),
                                    $"Reserva vencida automáticamente. Butaca {reservation.SeatId} liberada.",
                                    DateTime.UtcNow
                                );
                                await auditLogRepo.AddAsync(auditLog);

                                // Guardamos todo atómicamente
                                await unitOfWork.CommitTransactionAsync();
                                _logger.LogInformation($"Reserva {reservation.Id} expirada con éxito.");
                            }
                            catch (Exception ex)
                            {
                                await unitOfWork.RollbackTransactionAsync();
                                _logger.LogError($"Error al expirar reserva {reservation.Id}: {ex.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error crítico en el Worker: {ex.Message}");
                }

                // El proceso duerme 30 segundos antes de volver a escanear la base de datos
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}