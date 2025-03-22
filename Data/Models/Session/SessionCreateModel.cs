namespace Data.Models.Session
{
    public class SessionCreateModel
    {
        public DateTime SessionDate { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmId { get; set; }
    }
}