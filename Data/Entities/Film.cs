using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Film
{
    public Guid FilmId { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public float Rating { get; set; }
    public int RatingSum { get; set; }
    public int RatingAmount { get; set; }
    public virtual ICollection<Session>? Sessions { get; set; }
    public virtual ICollection<Vote>? Votes { get; set; }
    
}