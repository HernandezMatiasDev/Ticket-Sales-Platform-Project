using System;

namespace TicketingSystem.Application.UseCases.Queries
{
    public record PendingReservationDto(
        Guid ReservationId, 
        Guid SeatId, 
        int SeatNumber, 
        string Row, 
        string SectorName, 
        decimal Price, 
        DateTime ExpiresAt
    );
}