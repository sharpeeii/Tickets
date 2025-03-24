namespace Data.Models.Reservation;

public class ReservationCreateModel
{
    public Guid SessionId { get; set; }
    public ICollection<Guid> SeatIds { get; set; }
}
