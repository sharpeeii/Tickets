using Newtonsoft.Json;

namespace Data.Entities;

public class SeatType
{
    public int SeatTypeId { get; set; }
    public string Type { get; set; } // default/premium/accessible
    public decimal Coefficient { get; set; }
    [JsonIgnore]
    public virtual ICollection<Seat> Seats { get; set; }
}
