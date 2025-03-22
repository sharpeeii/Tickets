using Data.Models.Hall;
using Data.Models.Seat;

namespace Data.Models.Hall
{
    public class HallGetModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int NumberOfSeats { get; set; }
    }
}