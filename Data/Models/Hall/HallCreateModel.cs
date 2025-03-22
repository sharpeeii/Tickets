namespace Data.Models.Hall
{
    public class HallCreateModel
    {
        public required string Name { get; set; }
        public int NumberOfSeats { get; set; }
        public int Rows { get; set; }
    }
}