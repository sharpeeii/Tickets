namespace Data.DTOs.Seat;

public class SeatGetSessionDto
{
    public Guid Id { get; set; }
    public int Row { get; set; }
    public int Number { get; set; }
    public Guid HallId { get; set; }
    public bool IsBooked { get; set; } = false;
    public SeatTypeDto SeatTypeDto { get; set; }
}