namespace Data.DTOs.Session
{
    public class SessionCreateDto
    {
        public DateTime StartDate { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmId { get; set; }
    }
}