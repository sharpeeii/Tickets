namespace Data.Entities;

public class SeatType
{
    public Guid SeatTypeId { get; set; }
    public string Type { get; set; } = "Default"; // default/premium/accessible
    public decimal Coefficient { get; set; } = 1;
    public virtual ICollection<Seat> Seats { get; set; }
}
