using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Application.UseCases.Handlers;

public class ReserveSeatHandler : ICommandHandler<ReserveSeatCommand, bool>
{
private readonly ISeatRepository _seatRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IAuditLogRepository _auditRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Inyectamos absolutamente todo por constructor
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

    public async Task<bool> HandleAsync(ReserveSeatCommand command)
    {
        // 1. Buscar el asiento (validar que exista y esté disponible)
        var seat = await _seatRepository.GetByIdAsync(command.SeatId);
        
        // Asumimos que la entidad Seat tiene una propiedad Status
        if (seat == null || seat.Status != SeatStatus.Available) 
            return false;

        // 2. Cambiar estado de la butaca (Lógica de dominio)
        seat.Reserve();
        await _seatRepository.UpdateAsync(seat);

        // 3. Crear la instancia de Reservation usando su constructor
        // Definimos tiempos: ahora y expiración en 5 minutos
        var now = DateTime.Now;
        var expiration = now.AddMinutes(5);

        var reservation = new Reservation(
            Guid.NewGuid(),      // Generamos el ID de la reserva
            command.UserId, 
            command.SeatId, 
            now, 
            expiration
        );
  
        await _reservationRepository.AddAsync(reservation);

        var auditLog = new AuditLog(
            Guid.NewGuid(),          // id
            command.UserId,          // userId
            "RESERVE_ATTEMPT",       // action
            "Seat",                  // entityType
            seat.Id.ToString(),      // entityId
            $"Reserva de butaca {seat.Number} (ID: {seat.Id}) por usuario {command.UserId}", // details
            now                      // createdAt
        );
        await _auditRepository.AddAsync(auditLog);

        // 5. Persistencia Atómica
        try 
        {
            await _unitOfWork.SaveChangesAsync();
            return true; // Si llega acá, se guardó correctamente
        }
        catch (Exception)
        {
            // Si hay un error de base de datos (ej: FK error, timeout)
            return false;
        }
    }

}