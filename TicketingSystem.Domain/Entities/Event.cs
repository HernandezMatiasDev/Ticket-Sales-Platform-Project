namespace TicketingSystem.Domain.Entities;

public class Event
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime Date { get; private set; }
    public string Venue { get; set; } = string.Empty;
    public List<Sector> Sectors { get; private set; } = new();

    private Event() { } // Para EF en el futuro

    public Event(string name, DateTime date)
    {
        Name = name;
        Date = date;
    }
}