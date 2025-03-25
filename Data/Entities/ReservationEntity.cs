using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Data.Entities;

public class ReservationEntity
{
    public Guid Id { get; set; }
    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    [MaxLength(10)]
    public string Status { get; set; } = "Pending"; //only Pending/Confirmed. If payment is failed, delete row;
    public Guid SessionId { get; set; }
    public virtual SessionEntity? Session { get; set; }
    public Guid UserId { get; set; }
    public virtual UserEntity? User { get; set; }
    public Guid SeatId { get; set; }
    public virtual SeatEntity? Seat { get; set; }
}
