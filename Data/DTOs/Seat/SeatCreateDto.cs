namespace Data.DTOs.Seat
{
    public class SeatCreateDto
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public Guid HallId { get; set; }
    }
}