using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.UseCases.Handlers;

namespace TicketingSystem.Api.Controllers
{
    /// <summary>
    /// Controlador REST para gestionar la creación de reservas de butacas en eventos.
    /// </summary>
    [ApiController]
    [Authorize]
    // Ruta jerárquica exacta como sugiere el estándar del TP
    [Route("api/v1/events/{eventId}/seats/{seatId}/reservations")]
    public class ReservationsController : ControllerBase
    {
        private readonly ICommandHandler<ReserveSeatCommand, Reservation> _reserveSeatHandler;

        /// <summary>
        /// Inicializa el controlador inyectando el handler correspondiente.
        /// </summary>
        public ReservationsController(ICommandHandler<ReserveSeatCommand, Reservation> reserveSeatHandler)
        {
            _reserveSeatHandler = reserveSeatHandler;
        }

        /// <summary>
        /// Intenta reservar una butaca específica para un evento determinado.
        /// </summary>
        /// <param name="eventId">El identificador único del evento.</param>
        /// <param name="seatId">El identificador único de la butaca.</param>
        /// <returns>El resultado de la operación (200 OK, 400 Bad Request o 409 Conflict).</returns>
        [HttpPost]
        public async Task<IActionResult> Reserve(int eventId, Guid seatId)
        {
            try
            {
                // Extraemos el UserId directo desde el Claim NameIdentifier configurado en el Token
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new { error = "El token es inválido o no contiene un identificador de usuario válido." });
                }

                var command = new ReserveSeatCommand(eventId, seatId, userId);
                var reservation = await _reserveSeatHandler.HandleAsync(command);
                
                // 201 Created si todo fue un éxito
                return Created(string.Empty, new 
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
}