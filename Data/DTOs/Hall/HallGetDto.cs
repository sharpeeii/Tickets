using Data.DTOs.Seat;

namespace Data.DTOs.Hall;

public class HallGetDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public required ICollection<SeatDto> SeatDtos { get; set; }
}