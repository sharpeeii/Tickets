using Data.Models.Seat;

namespace Data.Models.Session
{
    public class SessionGetModel
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan SessionDuration { get; set; }
        public Guid HallId { get; set; } 
        public Guid FilmId { get; set; }
        public required string FilmName { get; set; }
        public required string HallHame { get; set; }
        public required ICollection<SeatGetSessionModel> Seats { get; set; }
    }
}