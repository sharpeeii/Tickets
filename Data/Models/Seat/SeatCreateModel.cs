namespace Data.Models.Seat
{
    public class SeatCreateModel
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public Guid HallId { get; set; }
    }
}