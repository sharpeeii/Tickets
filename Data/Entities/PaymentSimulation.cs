using System.Collections;

namespace Data.Entities;

public class PaymentSimulation
{
    public Guid PaymentSimulationId { get; set; }
    public DateTime PaymentDate { get; set; }
    public Guid BookingId { get; set; }
    public Booking Booking { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}