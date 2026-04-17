namespace TicketingSystem.Application.DTOs;

public record SeatMapDto(string SectorName, decimal Price, List<SeatDto> Seats);

public record SeatDto(Guid Id, string Row, int Number, bool IsAvailable);