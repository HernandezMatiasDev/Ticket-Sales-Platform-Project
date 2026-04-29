using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Queries;
using TicketingSystem.Application.Interfaces;
using TicketingSystem.Domain.Entities;

namespace TicketingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IQueryHandler<GetEventsQuery, IEnumerable<Event>> _getEventsHandler;
    private readonly IQueryHandler<GetSeatMapQuery, IEnumerable<Seat>> _getSeatMapHandler;
    private readonly ICommandHandler<CreateEventCommand, int> _createEventHandler;

    public EventsController(
        IQueryHandler<GetEventsQuery, IEnumerable<Event>> getEventsHandler, 
        IQueryHandler<GetSeatMapQuery, IEnumerable<Seat>> getSeatMapHandler,
        ICommandHandler<CreateEventCommand, int> createEventHandler)
    {
        _getEventsHandler = getEventsHandler;
        _getSeatMapHandler = getSeatMapHandler;
        _createEventHandler = createEventHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
    {
        var eventId = await _createEventHandler.HandleAsync(command);
        
        // Se retorna 201 Created con la URL correcta
        return Created($"/api/events/{eventId}", new { id = eventId, message = "Evento creado con éxito" });
    }   

    // GET: api/events
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _getEventsHandler.HandleAsync(new GetEventsQuery());
        
        // Mapeamos las entidades Event de dominio hacia EventDto para el cliente
        var eventDtos = events.Select(e => new EventDto(e.Id, e.Name, e.EventDate, e.Venue));
        return Ok(eventDtos);
    }

    // GET: api/events/{eventId}/sectors/{sectorId}/seats
    [HttpGet("{eventId}/sectors/{sectorId}/seats")]
    public async Task<IActionResult> GetSeatMap(int eventId, int sectorId)
    {
        var seatMap = await _getSeatMapHandler.HandleAsync(new GetSeatMapQuery(sectorId));
        
        if (seatMap == null || !seatMap.Any()) return NotFound("Evento o Sector no encontrado.");
        
        // Mapeamos Seat a SeatMapDto y casteamos el Enum SeatStatus a int (0, 1, 2)
        var seatDtos = seatMap.Select(s => new SeatMapDto(s.Id, s.Number, s.Row, (int)s.Status));
        return Ok(seatDtos);
    }
}