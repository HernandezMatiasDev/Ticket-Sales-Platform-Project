using System;

namespace TicketingSystem.Api.Controllers
{
    // El frontend espera el Status como un número (0 = Disponible, 1 = Reservado, 2 = Vendido)
    public record SeatMapDto(Guid Id, int Number, string Row, int Status);
}