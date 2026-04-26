namespace TicketingSystem.Domain.Entities;

public class Event
{
    public int Id { get;  set; }

    public string Name { get; private set; } = string.Empty;

    public DateTime EventDate { get; private set; }
    public string Venue { get; set; } = string.Empty;
    public List<Sector> Sectors { get; set; } = new();
    public string Status { get; set; } = "Active"; 

    private Event() { } // Para EF en el futuro

    public Event(string name, DateTime date, string venue)
    {
        Name = name;
        EventDate = date;
        Venue = venue;
    }
}