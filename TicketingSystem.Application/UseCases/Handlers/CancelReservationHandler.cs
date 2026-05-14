using System;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers
{
    public class CancelReservationHandler : ICommandHandler<CancelReservationCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationHandler(
            IReservationRepository reservationRepository,
            ISeatRepository seatRepository,
            IAuditLogRepository auditLogRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _seatRepository = seatRepository;
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> HandleAsync(CancelReservationCommand command)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var reservation = await _reservationRepository.GetByIdAsync(command.ReservationId);

                if (reservation == null || reservation.UserId != command.UserId || reservation.Status != ReservationStatus.Pending)
                {
                    throw new InvalidOperationException("La reserva no existe, no te pertenece o no está pendiente.");
                }

                reservation.MarkAsExpired();
                await _reservationRepository.UpdateAsync(reservation);

                var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
                if (seat != null)
                {
                    seat.MakeAvailable();
                    await _seatRepository.UpdateAsync(seat);
                }

                var auditLog = new AuditLog(
                    Guid.NewGuid(),
                    command.UserId,
                    "RESERVE_CANCELLED",
                    "Reservation",
                    reservation.Id.ToString(),
                    $"Reserva cancelada manualmente. Butaca {reservation.SeatId} liberada.",
                    DateTime.UtcNow
                );

                await _auditLogRepository.AddAsync(auditLog);

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}