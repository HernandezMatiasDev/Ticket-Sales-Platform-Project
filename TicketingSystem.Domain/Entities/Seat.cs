namespace TicketingSystem.Domain.Entities;

public enum SeatStatus
{
    Available,
    Reserved,
    Sold
}

public class Seat
{
    public Guid Id { get; private set; }

    public int Number { get; private set; }
    public string Row { get; set; } = string.Empty;

    public SeatStatus Status { get; private set; } = SeatStatus.Available;

    public int SectorId { get; private set; }

    public Sector Sector { get; private set; } = null!;

    private Seat() { }

    public Seat(int number, int sectorId)
    {
        Number = number;
        SectorId = sectorId;
    }

    public void Reserve()
    {
        Status = SeatStatus.Reserved;
    }

    public void MakeAvailable()
    {
        Status = SeatStatus.Available;
    }

    public void Sell()
    {
        Status = SeatStatus.Sold;
    }
}