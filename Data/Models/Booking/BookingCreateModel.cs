namespace Data.Models.Reservation;

public class BookingCreateModel
{
    public Guid SessionId { get; set; }
    public ICollection<Guid> SeatIds { get; set; }
}
