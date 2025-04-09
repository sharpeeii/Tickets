namespace Data.Entities;

public class BookedSeat
{
    public Guid BookedSeatId { get; set; }
    public Decimal Price { get; set; }
    public Guid SeatId { get; set; }
    public Seat Seat { get; set; }
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; }
}