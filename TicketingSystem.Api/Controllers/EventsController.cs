using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Application.Queries;

namespace TicketingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
// Cambia esto en el constructor y en las variables privadas:
private readonly TicketingSystem.Application.Queries.GetEventsQuery _getEventsQuery;
private readonly TicketingSystem.Application.Queries.GetSeatMapQuery _getSeatMapQuery;
    public EventsController(GetEventsQuery getEventsQuery, GetSeatMapQuery getSeatMapQuery)
    {
        _getEventsQuery = getEventsQuery;
        _getSeatMapQuery = getSeatMapQuery;
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