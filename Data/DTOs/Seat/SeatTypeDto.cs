namespace Data.DTOs.Seat;

public class SeatTypeDto
{
    public int SeatTypeId { get; set; }
    public string Type { get; set; } // default/premium/accessible
    public decimal Coefficient { get; set; }
}
