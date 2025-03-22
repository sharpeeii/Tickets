using Data.Models.Seat;

namespace Data.Models.Hall;

public class HallGetEagerModel
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public ICollection<SeatModel> SeatModels { get; set; }
}