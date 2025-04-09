namespace Data.DTOs.Booking
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public DateTime BookDate { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public Guid SeatId { get; set; }
        public string? FilmName { get; set; }
        public string? HallName { get; set; }
        
    }
}