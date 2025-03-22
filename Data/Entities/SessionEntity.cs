using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmId { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan SessionDuration { get; set; }
       
        public virtual HallEntity? Hall { get; set; }
        public virtual ICollection<ReservationEntity>? Reservation { get; set; }
        public virtual FilmEntity? Film { get; set; }
    }
}