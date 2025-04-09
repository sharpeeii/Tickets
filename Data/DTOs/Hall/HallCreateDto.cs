namespace Data.DTOs.Hall
{
    public class HallCreateDto
    {
        public required string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public int Rows { get; set; }
    }
}