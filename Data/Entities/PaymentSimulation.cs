using System.Collections;

namespace Data.Entities;

public class PaymentSimulation
{
    public Guid PaymentId { get; set; }
    public Guid BookingId { get; set; }
    public required Booking Booking { get; set; }
    public Guid UserId { get; set; }
    public required User User { get; set; }
    public decimal TotalSum { get; set; }
}