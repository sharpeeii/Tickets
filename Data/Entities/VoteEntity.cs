using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class VoteEntity
{
    public Guid Id { get; set; }
    [MaxLength(120)]
    public string? FilmName { get; set; }
    public virtual FilmEntity? Film { get; set; }
    public Guid FilmId { get; set; }
    
    public virtual UserEntity? User { get; set; }
    public Guid UserId { get; set; }
    
    [Range(1,10)]
    public int Rating { get; set; }
}