namespace Data.Models.Session;

public class SessionGetAllModel
{
    public Guid Id { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeSpan SessionDuration { get; set; }
    public Guid HallId { get; set; } 
    public Guid FilmId { get; set; }
    public required string FilmName { get; set; }
    public required string HallHame { get; set; }
}