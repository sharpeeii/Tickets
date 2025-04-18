using System.Diagnostics.Contracts;

namespace Data.Entities
{
    public class Seat
    {
        public Guid SeatId { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public Guid HallId { get; set; }
        public virtual Hall? Hall { get; set; }
        public virtual ICollection<BookedSeat>? BookedSeats { get; set; }
        public int SeatTypeId { get; set; } = 1;
        public virtual SeatType SeatType { get; set; }
    }
}