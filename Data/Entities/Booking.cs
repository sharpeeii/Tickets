namespace Data.Entities;

public class Booking
{
    public Guid BookingId { get; set; }
    public DateTime BookDate { get; set; } = DateTime.UtcNow;
    public Decimal TotalSum { get; set; }
    public bool IsPaid { get; set; } = false; //pending = false, confirmed = true;
    public Guid SessionId { get; set; }
    public Session? Session { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public required ICollection<BookedSeat> BookedSeats { get; set; }
}
