namespace TicketingSystem.Domain.Entities;

public class Seat
{
    public Guid Id { get; private set; }

    public int Number { get; private set; }
    public string Row { get; set; } = string.Empty;

    public bool IsAvailable { get; private set; } = true;

    public Guid SectorId { get; private set; }

    public Sector Sector { get; private set; } = null!;

    private Seat() { }

    public Seat(int number, Guid sectorId)
    {
        Number = number;
        SectorId = sectorId;
    }

    public void MarkAsUnavailable()
    {
        IsAvailable = false;
    }

    public void MarkAsAvailable()
    {
        IsAvailable = true;
    }
}