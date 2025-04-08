namespace Data.Entities;

public class BookedSeat
{
    public Guid BookedSeatId { get; set; }
    public Guid SeatId { get; set; }
    public required SeatEntity Seat { get; set; }
    public Guid BookingId { get; set; }
    public required Booking Booking { get; set; }
}