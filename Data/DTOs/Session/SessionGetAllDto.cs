namespace Data.DTOs.Session;

public class SessionGetAllDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public Guid HallId { get; set; } 
    public Guid FilmId { get; set; }
    public required string FilmName { get; set; }
    public required string HallHame { get; set; }
}