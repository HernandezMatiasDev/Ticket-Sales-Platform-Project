using System;

namespace TicketingSystem.Domain.Entities
{
    public enum ReservationStatus
    {
        Pending,
        Paid,
        Expired
    }

    public class Reservation
    {
        public Guid Id { get; private set; }
        public int UserId { get; private set; }
        public Guid SeatId { get; private set; }
        
        public ReservationStatus Status { get; private set; } 
        
        public DateTime ReservedAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        public Reservation(Guid id, int userId, Guid seatId, DateTime reservedAt, DateTime expiresAt)
        {
            Id = id;
            UserId = userId;
            SeatId = seatId;
            Status = ReservationStatus.Pending; // Estado inicial seguro
            ReservedAt = reservedAt;
            ExpiresAt = expiresAt;
        }

        public void MarkAsPaid() => Status = ReservationStatus.Paid;
        public void MarkAsExpired() => Status = ReservationStatus.Expired;
    }
}