namespace Data.Models.Reservation
{
    public class ReservationCreateModel
    {
        public Guid SessionId { get; set; }
        public Guid SeatId { get; set; }
    }
}