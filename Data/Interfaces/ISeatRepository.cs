using Data.Entities;
using Data.Models.Seat;

namespace Data.Interfaces
{
    public interface ISeatRepository
    {
        public Task CreateSeatAsync(SeatEntity seat);
        public Task<ICollection<SeatEntity>> GetAllSeatsAsync(Guid hallId);
        public Task<SeatEntity?> GetSeatAsync(Guid seatId);
        public Task<ICollection<SeatEntity?>> GetMultipleSeatsAsync(ICollection<Guid> seatIds);
        public Task UpdateSeatAsync(Guid seatId, SeatUpdModel model);
        public Task DeleteSeatAsync(Guid seatId);
        public Task<bool> CheckIfDuplicateAsync(Guid hallId, int row, int num);
    }
}