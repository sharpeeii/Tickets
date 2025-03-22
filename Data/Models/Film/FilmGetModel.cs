namespace Data.Models.Film;

public class FilmGetModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Genre { get; set; }
    public TimeSpan Duration { get; set; }
    public float Rating { get; set; }
    public int RatingAmount { get; set; }
}