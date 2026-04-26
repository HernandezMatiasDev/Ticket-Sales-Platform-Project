using TicketingSystem.Domain.Entities;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Application.UseCases.Commands;

namespace TicketingSystem.Application.UseCases.Handlers;

public class CreateEventHandler : ICommandHandler<CreateEventCommand, int>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> HandleAsync(CreateEventCommand command)
    {
        // 1. Mapeo de Command a Entidad de Dominio
        var nuevoEvento = new Event(command.Name, command.EventDate, command.Venue);
        
        await _eventRepository.AddAsync(nuevoEvento);
        
        // 3. Confirmar cambios (Unit of Work)
        await _unitOfWork.SaveChangesAsync();

        return nuevoEvento.Id;
    }
}