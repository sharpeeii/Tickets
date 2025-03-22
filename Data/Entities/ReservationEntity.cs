using System.Security.Cryptography.X509Certificates;

namespace Data.Entities;

public class ReservationEntity
{
    public Guid Id { get; set; }
    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public Guid SessionId { get; set; }
    public virtual SessionEntity? Session { get; set; }
    public Guid UserId { get; set; }
    public virtual UserEntity? User { get; set; }
    public Guid SeatId { get; set; }
    public virtual SeatEntity? Seat { get; set; }
}
