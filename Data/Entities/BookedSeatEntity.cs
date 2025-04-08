namespace Data.Entities;

public class BookedSeatEntity
{
    public Guid Id { get; set; }
    public Guid SeatId { get; set; }
    public required SeatEntity Seat { get; set; }
    public Guid BookingId { get; set; }
    public required BookingEntity BookingEntity { get; set; }
}