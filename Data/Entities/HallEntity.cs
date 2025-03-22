using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class HallEntity
    {
        public Guid Id { get; set; }
        [MaxLength(100)]
        public required string Name { get; set; }

        public int NumberOfSeats { get; set; } 
        public virtual ICollection<SeatEntity>? Seats { get; set; }
        public virtual  ICollection<SessionEntity>? Sessions { get; set; }
    }
}