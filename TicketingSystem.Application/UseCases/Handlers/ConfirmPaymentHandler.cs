using System;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers
{
    public class ConfirmPaymentHandler : ICommandHandler<ConfirmPaymentCommand, bool>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmPaymentHandler(
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

        public async Task<bool> HandleAsync(ConfirmPaymentCommand command)
        {
            await _unitOfWork.BeginTransactionAsync();

            var reservation = await _reservationRepository.GetByIdAsync(command.ReservationId);
            if (reservation == null || reservation.UserId != command.UserId || reservation.Status != ReservationStatus.Pending)
                throw new InvalidOperationException("La reserva es inválida, no te pertenece o ya no está pendiente.");

            var seat = await _seatRepository.GetByIdAsync(reservation.SeatId);
            if (seat == null) throw new InvalidOperationException("La butaca asociada no existe.");

            reservation.MarkAsPaid();
            seat.Sell();

            await _reservationRepository.UpdateAsync(reservation);
            await _seatRepository.UpdateAsync(seat);
            await _auditLogRepository.AddAsync(new AuditLog(Guid.NewGuid(), command.UserId, "PAYMENT_SUCCESS", "Reservation", reservation.Id.ToString(), "Pago confirmado. Butaca vendida.", DateTime.UtcNow));

            await _unitOfWork.CommitTransactionAsync();
            return true;
        }
    }
}