namespace TicketingSystem.Application.DTOs;

public record EventDto(Guid Id, string Name, DateTime Date, string Venue);