namespace Data.Models.Seat;

public class SeatGetSessionModel
{
    public Guid Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public Guid HallId { get; set; }
    public bool IsBooked { get; set; } = false;
}