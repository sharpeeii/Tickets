namespace Data.Models.Film;

public class FilmModel
{
    public required string Name { get; set; }
    public required string Genre { get; set; }
    public TimeSpan Duration { get; set; }
}