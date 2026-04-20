using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.UseCases;

namespace TicketingSystem.Api.Controllers
{
    /// <summary>
    /// Controlador REST para gestionar la creación de reservas de butacas en eventos.
    /// </summary>
    [ApiController]
    // Ruta jerárquica exacta como sugiere el estándar del TP
    [Route("api/v1/events/{eventId}/seats/{seatId}/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReserveSeatUseCase _reserveSeatUseCase;

        // Inyectamos el Caso de Uso de la capa Application
        /// <summary>
        /// Inicializa el controlador inyectando el caso de uso correspondiente.
        /// </summary>
        public ReservationsController(ReserveSeatUseCase reserveSeatUseCase)
        {
            _reserveSeatUseCase = reserveSeatUseCase;
        }

        /// <summary>
        /// Intenta reservar una butaca específica para un evento determinado.
        /// </summary>
        /// <param name="eventId">El identificador único del evento.</param>
        /// <param name="seatId">El identificador único de la butaca.</param>
        /// <param name="request">El cuerpo de la petición que incluye los datos del usuario.</param>
        /// <returns>El resultado de la operación (200 OK, 400 Bad Request o 409 Conflict).</returns>
        [HttpPost]
        public async Task<IActionResult> Reserve(int eventId, Guid seatId, [FromBody] ReserveRequestDto request)
        {
            try
            {
                // Ejecutamos la transacción
                var reservation = await _reserveSeatUseCase.ExecuteAsync(request.UserId, seatId);
                
                // 200 OK (o 201 Created) si todo fue un éxito
                return Ok(new 
                { 
                    message = "Reserva exitosa", 
                    reservationId = reservation.Id, 
                    expiresAt = reservation.ExpiresAt 
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                // 409 Conflict: Entity Framework detectó que el campo Version cambió. 
                // Alguien más ganó la butaca.
                return Conflict(new { error = "La butaca acaba de ser reservada por otro usuario." });
            }
            catch (InvalidOperationException ex)
            {
                // 400 Bad Request: La butaca no estaba "Available" al momento de consultar.
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                // 500 Internal Server Error para cualquier otra falla imprevista
                return StatusCode(500, new { error = "Ocurrió un error inesperado al procesar la reserva." });
            }
        }
    }

    /// <summary>
    /// DTO (Data Transfer Object) para recibir los datos del cuerpo de la solicitud de reserva.
    /// Nota: Asumimos que el UserId viene en el body temporalmente. Si luego implementan 
    /// autenticación con JWT, el UserId se sacaría de los Claims del token.
    /// </summary>
    public class ReserveRequestDto
    {
        /// <summary>
        /// Identificador del usuario que intenta realizar la reserva.
        /// </summary>
        public int UserId { get; set; }
    }
}