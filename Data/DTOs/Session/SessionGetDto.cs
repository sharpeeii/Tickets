using Data.DTOs.Seat;

namespace Data.DTOs.Session;
    public class SessionGetDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public Guid HallId { get; set; } 
        public Guid FilmId { get; set; }
        public required string FilmName { get; set; }
        public required string HallHame { get; set; }
        public required ICollection<SeatGetSessionDto> Seats { get; set; }
    }
