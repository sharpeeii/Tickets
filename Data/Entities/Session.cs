using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Hall? Hall { get; set; }
        public virtual ICollection<BookedSeat>? BookedSeats { get; set; }
        public virtual Film? Film { get; set; }
    }
}