using System.ComponentModel.DataAnnotations;

namespace Data.Models.Vote;

public class VoteModel
{
    public Guid FilmId { get; set; }
    
    [Range(1,10)]
    public int Rating { get; set; }
}