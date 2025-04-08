using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Data.Entities;

public class BookingEntity
{
    public Guid Id { get; set; }
    public DateTime BookDate { get; set; } = DateTime.UtcNow;
    public Decimal TotalSum { get; set; }
    public bool IsPaid { get; set; } = false; //pending = false, confirmed = true;
    public Guid SessionId { get; set; }
    public SessionEntity? Session { get; set; }
    public Guid UserId { get; set; }
    public UserEntity? User { get; set; }
    public required ICollection<BookedSeatEntity> BookedSeats { get; set; }
}
