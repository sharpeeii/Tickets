namespace Data.Models.Seat;

public class SeatModel
{
    public Guid Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public Guid HallId { get; set; }
}