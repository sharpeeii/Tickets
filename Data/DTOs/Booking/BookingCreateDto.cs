namespace Data.DTOs.Booking;

public class BookingCreateDto
{
    public Guid SessionId { get; set; }
    public ICollection<Guid> SeatIds { get; set; }
}
