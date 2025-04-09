using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class Vote
{
    public Guid VoteId { get; set; }
    [MaxLength(120)]
    public string? FilmName { get; set; }
    public virtual Film? Film { get; set; }
    public Guid FilmId { get; set; }
    public virtual User? User { get; set; }
    public Guid UserId { get; set; }
    [Range(1,10)]
    public int Rating { get; set; }
}