using System.Collections;

namespace Data.Entities;

public class PaymentSimulationEntity
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public required BookingEntity Booking { get; set; }
    public Guid UserId { get; set; }
    public required UserEntity User { get; set; }
    public decimal TotalSum { get; set; }
}