using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.UseCases.Handlers;
using TicketingSystem.Application.UseCases.Commands;
using TicketingSystem.Application.UseCases.Queries;

namespace TicketingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
// Cambia esto en el constructor y en las variables privadas:
private readonly TicketingSystem.Application.UseCases.Queries.GetEventsQuery _getEventsQuery;
private readonly TicketingSystem.Application.UseCases.Queries.GetSeatMapQuery _getSeatMapQuery;
    public EventsController(GetEventsQuery getEventsQuery, GetSeatMapQuery getSeatMapQuery)
    {
        _getEventsQuery = getEventsQuery;
        _getSeatMapQuery = getSeatMapQuery;
    }

    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventCommand command, [FromServices] CreateEventHandler handler)
    {
        var eventId = await handler.HandleAsync(command);
        
        // Devolvemos un 201 Created según el estándar REST
        return CreatedAtAction(nameof(GetAll), new { id = eventId }, new { id = eventId, message = "Evento creado con éxito" });
    }   

    // GET: api/events
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _getEventsQuery.ExecuteAsync();
        return Ok(events);
    }

    // GET: api/events/{eventId}/sectors/{sectorId}/seats
    [HttpGet("{eventId}/sectors/{sectorId}/seats")]
    public async Task<IActionResult> GetSeatMap(int eventId, int sectorId)
    {
        var seatMap = await _getSeatMapQuery.ExecuteAsync(eventId, sectorId);
        
        if (seatMap == null) return NotFound("Evento o Sector no encontrado.");
        
        return Ok(seatMap);
    }
}