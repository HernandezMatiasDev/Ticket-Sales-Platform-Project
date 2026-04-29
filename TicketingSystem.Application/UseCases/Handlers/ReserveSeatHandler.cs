using System;
using System.Threading.Tasks;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers;

public class ReserveSeatHandler : ICommandHandler<ReserveSeatCommand, Reservation>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IAuditLogRepository _auditRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveSeatHandler(
        ISeatRepository seatRepository, 
        IReservationRepository reservationRepository,
        IAuditLogRepository auditRepository,
        IUnitOfWork unitOfWork)
    {
        _seatRepository = seatRepository;
        _reservationRepository = reservationRepository;
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Reservation> HandleAsync(ReserveSeatCommand command)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // 1. Buscar el asiento (validar que exista y esté disponible)
            var seat = await _seatRepository.GetByIdAsync(command.SeatId);
            
            // Validamos que exista, esté disponible y pertenezca al evento
            if (seat == null || seat.Status != SeatStatus.Available || seat.Sector.EventId != command.EventId) 
            {
                throw new InvalidOperationException("La butaca no está disponible o no pertenece al evento especificado.");
            }

            // 2. Cambiar estado de la butaca (Lógica de dominio)
            seat.Reserve();
            await _seatRepository.UpdateAsync(seat);

            // 3. Crear la instancia de Reservation usando su constructor
            var now = DateTime.UtcNow;
            var expiration = now.AddMinutes(5);

            var reservation = new Reservation(
                Guid.NewGuid(),
                command.UserId, 
                command.SeatId, 
                now, 
                expiration
            );
      
            await _reservationRepository.AddAsync(reservation);

            var auditLog = new AuditLog(
                Guid.NewGuid(),
                command.UserId,
                "RESERVE_SUCCESS",
                "Reservation",
                reservation.Id.ToString(),
                $"Reserva de butaca {seat.Number} exitosa.",
                now
            );
            await _auditRepository.AddAsync(auditLog);

            // 5. Persistencia Atómica
            await _unitOfWork.CommitTransactionAsync();
            
            return reservation;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

}