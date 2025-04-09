using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Hall
    {
        public Guid HallId { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }

        public int NumberOfSeats { get; set; } 
        public virtual ICollection<Seat>? Seats { get; set; }
        public virtual  ICollection<Session>? Sessions { get; set; }
    }
}