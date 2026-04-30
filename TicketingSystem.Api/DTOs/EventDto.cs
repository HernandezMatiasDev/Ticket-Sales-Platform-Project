using System;

namespace TicketingSystem.Api.Controllers
{
    public record EventDto(int Id, string Name, DateTime EventDate, string Venue);
}