namespace Data.DTOs.Seat;

public class SeatDto
{
    public Guid Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public Guid HallId { get; set; }
}