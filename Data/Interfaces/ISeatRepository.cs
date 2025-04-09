using Data.Entities;
using Data.DTOs.Seat;

namespace Data.Interfaces
{
    public interface ISeatRepository
    {
        public Task CreateSeatAsync(Seat seat);
        public Task<ICollection<Seat>> GetAllSeatsAsync(Guid hallId);
        public Task<Seat?> GetSeatAsync(Guid seatId);
        public Task<ICollection<Seat?>> GetMultipleSeatsAsync(ICollection<Guid> seatIds);
        public Task UpdateSeatAsync(Guid seatId, SeatUpdDto dto);
        public Task DeleteSeatAsync(Guid seatId);
        public Task<bool> CheckIfDuplicateAsync(Guid hallId, int row, int num);
    }
}