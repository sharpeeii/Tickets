using System.ComponentModel.DataAnnotations;

namespace Data.DTOs.Vote;

public class VoteDto
{
    public Guid FilmId { get; set; }
    
    [Range(1,10)]
    public int Rating { get; set; }
}