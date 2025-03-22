namespace Data.Models.Reservation
{
    public class ReservationModel
    {
        public Guid Id { get; set; }
        public DateTime ReservationDate { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public Guid SeatId { get; set; }
        public string? FilmName { get; set; }
        public string? HallName { get; set;  }
    }
}