namespace TicketingSystem.Domain.Entities;

public class Sector
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid EventId { get; private set; }

    public Event Event { get; private set; } = null!;

    public List<Seat> Seats { get; private set; } = new();

    private Sector() { }

    public Sector(string name, Guid eventId)
    {
        Name = name;
        EventId = eventId;
    }
}