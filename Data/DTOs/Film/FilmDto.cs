namespace Data.DTOs.Film;

public class FilmDto
{
    public required string Name { get; set; }
    public required string Genre { get; set; }
    public TimeSpan Duration { get; set; }
}