using System.Diagnostics.Contracts;

namespace Data.Entities
{
    public class SeatEntity
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public Guid HallId { get; set; }
        public virtual HallEntity? Hall { get; set; }
        public virtual ICollection<BookedSeatEntity>? BookedSeats { get; set; } 
    }
}