namespace TicketingSystem.Domain.Entities;

public class Sector
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; set; }
    public int EventId { get; private set; }

    public Event Event { get; private set; } = null!;

    public List<Seat> Seats { get; private set; } = new();

    private Sector() { }

    public Sector(string name, int eventId)
    {
        Name = name;
        EventId = eventId;
    }
}